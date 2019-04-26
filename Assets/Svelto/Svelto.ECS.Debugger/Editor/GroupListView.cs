using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;
using System.Linq;
using Svelto.ECS.Debugger.DebugStructure;
using UnityEngine;
using UnityEngine.Experimental.LowLevel;
using UnityEngine.Profiling;

namespace Svelto.ECS.Debugger.Editor
{

    public delegate void SystemSelectionCallback(DebugGroup manager, DebugRoot world);
    public delegate DebugRoot WorldSelectionGetter();

    public class GroupListView : TreeView
    {
        internal readonly Dictionary<int, DebugGroup> managersById = new Dictionary<int, DebugGroup>();
        private readonly Dictionary<int, DebugRoot> worldsById = new Dictionary<int, DebugRoot>();
        private readonly Dictionary<int, HideNode> hideNodesById = new Dictionary<int, HideNode>();

        private const float kToggleWidth = 22f;
        private const float kTimingWidth = 70f;
        private const int kAllEntitiesItemId = 0;

        private readonly SystemSelectionCallback systemSelectionCallback;
        private readonly WorldSelectionGetter getWorldSelection;

        private static GUIStyle RightAlignedLabel
        {
            get
            {
                if (rightAlignedText == null)
                {
                    rightAlignedText = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleRight
                    };
                }

                return rightAlignedText;
            }
        }

        private static GUIStyle rightAlignedText;

        internal static MultiColumnHeaderState GetHeaderState()
        {
            var columns = new[]
            {
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Group Name"),
                    headerTextAlignment = TextAlignment.Left,
                    sortingArrowAlignment = TextAlignment.Right,
                    canSort = true,
                    sortedAscending = true,
                    width = 100,
                    minWidth = 100,
                    maxWidth = 2000,
                    autoResize = true,
                    allowToggleVisibility = false
                }
            };

            return new MultiColumnHeaderState(columns);
        }

        private static TreeViewState GetStateForWorld(DebugRoot world, List<TreeViewState> states, List<string> stateNames)
        {
            if (world == null)
                return new TreeViewState();

            var currentWorldName = "Default";//world.Name;

            var stateForCurrentWorld = states.Where((t, i) => stateNames[i] == currentWorldName).FirstOrDefault();
            if (stateForCurrentWorld != null)
                return stateForCurrentWorld;

            stateForCurrentWorld = new TreeViewState();
            states.Add(stateForCurrentWorld);
            stateNames.Add(currentWorldName);
            return stateForCurrentWorld;
        }

        public static GroupListView CreateList(DebugTree tree, List<TreeViewState> states, List<string> stateNames, SystemSelectionCallback systemSelectionCallback, WorldSelectionGetter worldSelectionGetter)
        {
            var state = GetStateForWorld(worldSelectionGetter(), states, stateNames);
            var header = new MultiColumnHeader(GetHeaderState());
            return new GroupListView(tree, state, header, systemSelectionCallback, worldSelectionGetter);
        }

        public DebugTree Data;
        internal GroupListView(DebugTree tree, TreeViewState state, MultiColumnHeader header, SystemSelectionCallback systemSelectionCallback, WorldSelectionGetter worldSelectionGetter) : base(state, header)
        {
            this.getWorldSelection = worldSelectionGetter;
            this.systemSelectionCallback = systemSelectionCallback;
            columnIndexForTreeFoldouts = 0;
            Data = tree;
            RebuildNodes();
        }

//        private HideNode CreateNodeForManager(int id, DebugGroup system)
//        {
//            var active = true;
//            if (!(system is ComponentSystemGroup))
//            {
//                managersById.Add(id, system);
//                worldsById.Add(id, system.Parent);
//                active = false;
//            }
//            var name = getWorldSelection() == null ? $"{system.GetType().Name} ({system.World.Name})" : system.GetType().Name;
//            var item = new TreeViewItem { id = id, displayName = name };
//
//            var hideNode = new HideNode(item) { Active = active };
//            hideNodesById.Add(id, hideNode);
//            return hideNode;
//        }

        private PlayerLoopSystem lastPlayerLoop;

        private class HideNode
        {
            public readonly TreeViewItem Item;
            public bool Active = true;
            public List<HideNode> Children;

            public HideNode(TreeViewItem item)
            {
                Item = item;
            }

            public void AddChild(HideNode child)
            {
                if (Children == null)
                    Children = new List<HideNode>();
                Children.Add(child);
            }

            public TreeViewItem BuildList()
            {
                if (Active)
                {
                    Item.children = null;
                    if (Children != null)
                    {
                        Item.children = new List<TreeViewItem>();
                        foreach (var child in Children)
                        {
                            var childItem = child.BuildList();
                            if (childItem != null)
                                Item.children.Add(childItem);
                        }
                    }
                    return Item;

                }
                else
                {
                    return null;
                }
            }
        }

        private HideNode rootNode;

        private void RebuildNodes()
        {
            rootNode = null;
            Reload();
        }

        private HideNode BuildNodesForPlayerLoopSystem(ref int currentId)
        {
            List<HideNode> children = new List<HideNode>();

            if (Application.isPlaying)
            {
                var root = getWorldSelection();
                //todo
                var groups = Data.DebugRoots.Find(r => r == root).DebugGroups;
                foreach (var debugGroup in groups)
                {
                    var id = currentId++;
                    managersById.Add(id, debugGroup);
                    worldsById.Add(id, debugGroup.Parent);
                    var node = new HideNode(new TreeViewItem
                        {id = id, displayName = Debugger.GetNameGroup(debugGroup.Id)});
                    node.Active = true;
                    children.Add(node);
                }
            }
            if (children != null || getWorldSelection() == null)
            {
                var systemNode = new HideNode(new TreeViewItem() {id = currentId++, displayName = "Def"});
                systemNode.Children = children;
                return systemNode;
            }

            return null;
        }

        private void BuildNodeTree()
        {
            managersById.Clear();
            worldsById.Clear();
            hideNodesById.Clear();

            var currentID = kAllEntitiesItemId + 1;

            rootNode = BuildNodesForPlayerLoopSystem(ref currentID)
                       ?? new HideNode(new TreeViewItem {id = currentID, displayName = "Root"});
            return;
        }

        private bool GetDefaultExpandedIds(HideNode parent, List<int> ids)
        {
            var shouldExpand = managersById.ContainsKey(parent.Item.id);
            if (parent.Children != null)
            {
                foreach (var child in parent.Children)
                {
                    shouldExpand |= GetDefaultExpandedIds(child, ids);
                }

                if (shouldExpand)
                {
                    ids.Add(parent.Item.id);
                }
            }
            
            return shouldExpand;
        }

        protected override TreeViewItem BuildRoot()
        {
            if (rootNode == null)
            {
                BuildNodeTree(); 
                var expanded = new List<int>();
                GetDefaultExpandedIds(rootNode, expanded);
                expanded.Sort();
                state.expandedIDs = expanded;
            }

            var root = rootNode.BuildList();

            if (!root.hasChildren)
                root.children = new List<TreeViewItem>(0);

            if (getWorldSelection() != null)
            {
                //root.children.Insert(0, new TreeViewItem(kAllEntitiesItemId, 0, $"All Entities (Default)"));
            }
            
            root.depth = -1;
            
            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected override void BeforeRowsGUI()
        {
//            var becameVisible = false;
//            foreach (var idManagerPair in managersById)
//            {
//                var componentSystemBase = idManagerPair.Value;
//                var hideNode = hideNodesById[idManagerPair.Key];
////                if (componentSystemBase.LastSystemVersion != 0 && !hideNode.Active)
////                {
////                    hideNode.Active = true;
////                    becameVisible = true;
////                }
//            }
//            if (becameVisible)
//                Reload();
            base.BeforeRowsGUI();
        }

        protected override void RowGUI (RowGUIArgs args)
        {
            if (args.item.depth == -1)
                return;
            var item = args.item;

            var enabled = GUI.enabled;

            if (managersById.ContainsKey(item.id))
            {
            }
            else if (args.item.id == kAllEntitiesItemId)
            {
                
            }
            else
            {
                GUI.enabled = false;
            }

            var indent = GetContentIndent(item);
            var nameRect = args.GetCellRect(0);
            nameRect.xMin = nameRect.xMin + indent;
            GUI.Label(nameRect, item.displayName);
            GUI.enabled = enabled;
        }

        protected override void AfterRowsGUI()
        {
            base.AfterRowsGUI();
            if (Event.current.type == EventType.MouseDown)
            {
                SetSelection(new List<int>());
            }
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            if (selectedIds.Count > 0 && managersById.ContainsKey(selectedIds[0]))
            {
                systemSelectionCallback(managersById[selectedIds[0]], worldsById[selectedIds[0]]);
            }
            else
            {
                systemSelectionCallback(null, null);
                SetSelection(getWorldSelection() == null ? new List<int>() : new List<int> {kAllEntitiesItemId});
            }
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        public void TouchSelection()
        {
            SetSelection(GetSelection(), TreeViewSelectionOptions.FireSelectionChanged);
        }

//        private bool PlayerLoopsMatch(PlayerLoopSystem a, PlayerLoopSystem b)
//        {
//            if (a.type != b.type)
//                return false;
//            if (a.subSystemList == b.subSystemList)
//                return true;
//            if (a.subSystemList == null || b.subSystemList == null)
//                return false;
//            if (a.subSystemList.Length != b.subSystemList.Length)
//                return false;
//            for (var i = 0; i < a.subSystemList.Length; ++i)
//            {
//                if (!PlayerLoopsMatch(a.subSystemList[i], b.subSystemList[i]))
//                    return false;
//            }
//
//            return true;
//        }

        public bool NeedsReload
        {
            get
            {
//                if (!PlayerLoopsMatch(lastPlayerLoop, ScriptBehaviourUpdateOrder.CurrentPlayerLoop))
//                    return true;
//
//                foreach (var world in worldsById.Values)
//                {
//                    if (!world.IsCreated)
//                        return true;
//                }
//
//                foreach (var manager in managersById.Values)
//                {
//                    if (manager is ComponentSystemBase system)
//                    {
//                        if (system.World == null || !system.World.Systems.Contains(manager))
//                            return true;
//                    }
//                }

                return true;
            }
        }

        public void ReloadIfNecessary()
        {
            if (NeedsReload)
                RebuildNodes();
        }

        private int lastTimedFrame;

//        public void UpdateTimings()
//        {
//            if (Time.frameCount == lastTimedFrame)
//                return;
//
//            foreach (var recorder in recordersByManager.Values)
//            {
//                recorder.Update();
//            }
//
//            lastTimedFrame = Time.frameCount;
//        }

        public void SetSystemSelection(DebugGroup manager, DebugRoot world)
        {
            foreach (var pair in managersById)
            {
                if (pair.Value == manager)
                {
                    SetSelection(new List<int> {pair.Key});
                    return;
                }
            }
            SetSelection(new List<int>());
        }
    }
}
