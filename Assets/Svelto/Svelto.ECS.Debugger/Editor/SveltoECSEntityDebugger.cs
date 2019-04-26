using System;
using System.Collections.Generic;
using Svelto.ECS.Debugger.DebugStructure;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Serialization;

namespace Svelto.ECS.Debugger.Editor
{
    public class SveltoECSEntityDebugger : EditorWindow
    {
        private const float kSystemListWidth = 350f;

        private float CurrentEntityViewWidth =>
            Mathf.Max(100f, position.width - kSystemListWidth);

        [MenuItem("Window/Analysis/Svelto.ECS Debugger", false)]
        private static void OpenWindow()
        {
            GetWindow<SveltoECSEntityDebugger>("Svelto.ECS Entity Debugger");
        }

        private static GUIStyle LabelStyle
        {
            get
            {
                return labelStyle ?? (labelStyle = new GUIStyle(EditorStyles.label)
                {
                    margin = EditorStyles.boldLabel.margin,
                    richText = true
                });
            }
        }

        private static GUIStyle labelStyle;

        private static GUIStyle BoxStyle
        {
            get
            {
                return boxStyle ?? (boxStyle = new GUIStyle(GUI.skin.box)
                {
                    margin = new RectOffset(),
                    padding = new RectOffset(1, 0, 1, 0),
                    overflow = new RectOffset(0, 1, 0, 1)
                });
            }
        }

        private static GUIStyle boxStyle;
        private static SveltoECSEntityDebugger Instance { get; set; }

//        private EntitySelectionProxy selectionProxy;
//
//        [FormerlySerializedAs("componentGroupListStates")]
//        [SerializeField] private List<TreeViewState> entityQueryListStates = new List<TreeViewState>();
//        [FormerlySerializedAs("componentGroupListStateNames")]
//        [SerializeField] private List<string> entityQueryListStateNames = new List<string>();
//        private EntityQueryListView entityQueryListView;
//
        private List<TreeViewState> groupListStates = new List<TreeViewState>();
        private List<string> groupListStateNames = new List<string>();
        public GroupListView groupListView;
        //private DebugRoot rootSelection;
//
        [SerializeField] private TreeViewState entityListState = new TreeViewState();
        private EntityListView entityListView;
//
//        [SerializeField] private ChunkInfoListView.State chunkInfoListState = new ChunkInfoListView.State();
//        private ChunkInfoListView chunkInfoListView;
//
//        //TODO 
//        //internal WorldPopup m_WorldPopup;
//
//        private ComponentTypeFilterUI filterUI;
//
        //public DebugRoot RootSelection => rootSelection;

        [SerializeField] private string lastEditModeWorldSelection = "lastEditModeWorldSelection";//WorldPopup.kNoWorldName;
        [SerializeField] private string lastPlayModeWorldSelection = "lastPlayModeWorldSelection";//WorldPopup.kNoWorldName;
        [SerializeField] private bool showingPlayerLoop;


        public DebugGroup GroupSelection { get; private set; }
//
        public DebugRoot RootSelection { get; set; }
//
        public void SetGroupSelection(DebugGroup manager, DebugRoot root, bool updateList, bool propagate)
        {
            if (manager != null && root == null)
                throw new ArgumentNullException("System cannot have null world");
            GroupSelection = manager;
            RootSelection = root;
            if (updateList)
                groupListView.SetSystemSelection(manager, root);
//            CreateEntityQueryListView();
//            if (propagate)
//            {
//                entityQueryListView.TouchSelection();
//            }
        }
//
//        public EntityListQuery EntityListQuerySelection { get; private set; }
//
//        public void SetEntityListSelection(EntityListQuery newSelection, bool updateList, bool propagate)
//        {
//            chunkInfoListView.ClearSelection();
//            EntityListQuerySelection = newSelection;
//            if (updateList)
//                entityQueryListView.SetEntityListSelection(newSelection);
//            entityListView.SelectedEntityQuery = newSelection;
//            if (propagate)
//                entityListView.TouchSelection();
//        }
//
//        public Entity EntitySelection => selectionProxy.Entity;
//
        internal void SetEntitySelection(Entity newSelection, bool updateList)
        {
            if (updateList)
                entityListView.SetEntitySelection(newSelection);

            var world = WorldSelection ?? (SystemSelection as ComponentSystemBase)?.World;
            if (world != null && newSelection != Entity.Null)
            {
                selectionProxy.SetEntity(world, newSelection);
                Selection.activeObject = selectionProxy;
            }
            else if (Selection.activeObject == selectionProxy)
            {
                Selection.activeObject = null;
            }
        }
//
//        internal static void SetAllSelections(World world, ComponentSystemBase system, EntityListQuery entityQuery,
//            Entity entity)
//        {
//            if (Instance == null)
//                OpenWindow();
//            Instance.SetWorldSelection(world, false);
//            Instance.SetSystemSelection(system, world, true, false);
//            Instance.SetEntityListSelection(entityQuery, true, false);
//            Instance.SetEntitySelection(entity, true);
//            Instance.entityListView.FrameSelection();
//        }


//        public void SetWorldSelection(World selection, bool propagate)
//        {
//            if (worldSelection != selection)
//            {
//                worldSelection = selection;
//                showingPlayerLoop = worldSelection == null;
//                if (worldSelection != null)
//                {
//                    if (EditorApplication.isPlaying)
//                        lastPlayModeWorldSelection = worldSelection.Name;
//                    else
//                        lastEditModeWorldSelection = worldSelection.Name;
//                }
//
//                CreateSystemListView();
//                if (propagate)
//                    systemListView.TouchSelection();
//            }
//        }
//
//        public void SetEntityListChunkFilter(ChunkFilter filter)
//        {
//            entityListView.SetFilter(filter);
//        }
//
        private void CreateEntityListView()
        {
            entityListView?.Dispose();

            entityListView = new EntityListView(
                entityListState,
                EntityListQuerySelection,
                x => SetEntitySelection(x, false),
                () => SystemSelectionWorld ?? WorldSelection,
                () => SystemSelection,
                x => chunkInfoListView.SetChunkArray(x)
                );
        }
//
        private void CreateGroupListView()
        {
            groupListView = GroupListView.CreateList(Data, groupListStates, groupListStateNames, 
                (group, root) => SetGroupSelection(group, root, false, true), () => RootSelection);
            groupListView.multiColumnHeader.ResizeToFit();
        }
//
//        private void CreateEntityQueryListView()
//        {
//            entityQueryListView = EntityQueryListView.CreateList(groupSelection as ComponentSystemBase, entityQueryListStates, entityQueryListStateNames, x => SetEntityListSelection(x, false, true), () => SystemSelectionWorld);
//        }
//
//        [SerializeField] private bool ShowInactiveSystems;
//
//        private void CreateWorldPopup()
//        {
//            m_WorldPopup = new WorldPopup(
//                () => WorldSelection,
//                x => SetWorldSelection(x, true),
//                () => ShowInactiveSystems,
//                () =>
//                {
//                    ShowInactiveSystems = !ShowInactiveSystems;
//                    systemListView.Reload();
//                }
//                );
//        }
//
//
        public DebugTree Data;
        private void OnEnable()
        {
            Instance = this;
            UpdateData();
//            CreateEntitySelectionProxy();
//            CreateWorldPopup();
            CreateGroupListView();
//            CreateEntityQueryListView();
//            CreateEntityListView();
//            groupListView.TouchSelection();

            EditorApplication.playModeStateChanged += OnPlayModeStateChange;
        }

        private void CreateEntitySelectionProxy()
        {
            selectionProxy = ScriptableObject.CreateInstance<EntitySelectionProxy>();
            selectionProxy.hideFlags = HideFlags.HideAndDontSave;

            selectionProxy.EntityControlDoubleClick += entity =>
            {
                entityListView?.OnEntitySelected(entity);
            };
        }
//
        private void OnDestroy()
        {
            entityListView?.Dispose();
        }

        private void OnDisable()
        {
            entityListView?.Dispose();
            if (Instance == this)
                Instance = null;
//            if (selectionProxy)
//                DestroyImmediate(selectionProxy);

            EditorApplication.playModeStateChanged -= OnPlayModeStateChange;
        }

        private void OnPlayModeStateChange(PlayModeStateChange change)
        {
            //if (change == PlayModeStateChange.EnteredPlayMode)
//            if (change == PlayModeStateChange.ExitingPlayMode)
//                SetAllEntitiesFilter(null);
            if (change == PlayModeStateChange.ExitingPlayMode /*&& Selection.activeObject == selectionProxy*/)
                Selection.activeObject = null;
        }

        private readonly RepaintLimiter repaintLimiter = new RepaintLimiter();

        private void Update()
        {
            if (repaintLimiter.SimulationAdvanced())
            {
                UpdateData();
                Repaint();
            }
//            else if (!Application.isPlaying)
//            {
////                if (systemListView.NeedsReload || entityQueryListView.NeedsReload || entityListView.NeedsReload || !filterUI.TypeListValid())
////                    Repaint();
//            }
        }

        private void UpdateData()
        {
            if (Data == null)
            {
                if (Application.isPlaying)
                {
                    Data = Debugger.Instance.DebugInfo;
                    if (Data.DebugRoots.Count > 0)
                        RootSelection = Data.DebugRoots[0];
                    CreateGroupListView();
                }
            }
        }

        private void ShowWorldPopup()
        {
            //m_WorldPopup.OnGUI(showingPlayerLoop, EditorApplication.isPlaying ? lastPlayModeWorldSelection : lastEditModeWorldSelection);
        }

        private void GroupList()
        {
            var rect = GUIHelpers.GetExpandingRect();
//            if (World.AllWorlds.Count != 0)
//            {
                groupListView.OnGUI(rect);
//            }
//            else
//            {
//                GUIHelpers.ShowCenteredNotification(rect, "No systems (Try pushing Play)");
//            }
        }

        private void GroupsHeader()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Groups", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            ShowWorldPopup();
            GUILayout.EndHorizontal();
        }

        const float kChunkInfoButtonWidth = 60f;

        private void EntityHeader()
        {
            if (RootSelection != null || RootSelection != null)
            {
                var rect = new Rect(kSystemListWidth, 3f, CurrentEntityViewWidth, kLineHeight);
                if (GroupSelection == null)
                {
                    GUI.Label(rect, "All Entities", EditorStyles.boldLabel);
                }
                else
                {
                    //var type = GroupSelection.GetType();
                    GUI.Label(rect, Debugger.GetNameGroup(GroupSelection.Id), LabelStyle);
                }
            }
        }

//        private void EntityQueryList()
//        {
//            if (SystemSelection != null)
//            {
//                entityQueryListView.SetWidth(CurrentEntityViewWidth);
//                var height = Mathf.Min(entityQueryListView.Height + BoxStyle.padding.vertical, position.height*0.5f);
//                GUILayout.BeginVertical(BoxStyle, GUILayout.Height(height));
//
//                entityQueryListView.OnGUI(GUIHelpers.GetExpandingRect());
//                GUILayout.EndVertical();
//            }
//            else if (WorldSelection != null)
//            {
//                GUILayout.BeginHorizontal();
//                filterUI.OnGUI();
//                GUILayout.FlexibleSpace();
//                GUILayout.Label("entityListView.EntityCount.ToString()");
//                GUILayout.EndHorizontal();
//            }
//        }

//        private EntityListQuery filterQuery;
//        private World systemSelectionWorld;
        private const float kLineHeight = 18f;
//
//        public void SetAllEntitiesFilter(EntityListQuery entityQuery)
//        {
//            filterQuery = entityQuery;
//            if (WorldSelection == null || SystemSelection != null)
//                return;
//            ApplyAllEntitiesFilter();
//        }
//
//        private void ApplyAllEntitiesFilter()
//        {
//            SetEntityListSelection(filterQuery, false, true);
//        }

        void EntityList()
        {
            GUILayout.BeginVertical(BoxStyle);
            entityListView.OnGUI(GUIHelpers.GetExpandingRect());
            GUILayout.EndVertical();
        }

        private void OnSelectionChange()
        {
//            if (Selection.activeObject != selectionProxy)
//            {
//                entityListView.SelectNothing();
//            }
        }

        private void OnGUI()
        {
//            if (Event.current.type == EventType.Layout)
//            {
//                systemListView.ReloadIfNecessary();
//                filterUI.GetTypes();
//                entityQueryListView.ReloadIfNecessary();
//                entityListView.ReloadIfNecessary();
//            }
//
//            if (Selection.activeObject == selectionProxy)
//            {
//                if (!selectionProxy.Exists)
//                {
//                    Selection.activeObject = null;
//                    entityListView.SelectNothing();
//                }
//            }
            GUILayout.BeginArea(new Rect(0f, 0f, kSystemListWidth, position.height)); // begin System side
            GroupsHeader();

            GUILayout.BeginVertical(BoxStyle);
            GroupList();
            GUILayout.EndVertical();

            GUILayout.EndArea(); // end System side

            EntityHeader();

            GUILayout.BeginArea(new Rect(kSystemListWidth, kLineHeight, CurrentEntityViewWidth, position.height - kLineHeight));
            //EntityQueryList();
            EntityList();
            GUILayout.EndArea();

            repaintLimiter.RecordRepaint();
        }
    }
}
