using System;
using System.Collections.Generic;
using Svelto.ECS.Debugger.DebugStructure;
using Unity.Collections;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Svelto.ECS.Debugger.Editor
{

    public delegate void EntitySelectionCallback(DebugEntity selection);
    public delegate DebugGroup GroupSelectionGetter();

    public class EntityListView : TreeView {

        private readonly EntitySelectionCallback setEntitySelection;
        private readonly RootSelectionGetter _getRootSelection;
        private readonly GroupSelectionGetter getSystemSelection;
        private readonly Dictionary<int, DebugEntity> entityById = new Dictionary<int, DebugEntity>();
        private readonly DebugTree Data;
        private readonly List<TreeViewItem> rows = new List<TreeViewItem>();

        public EntityListView(DebugTree tree, TreeViewState state, EntitySelectionCallback entitySelectionCallback, RootSelectionGetter getRootSelection, GroupSelectionGetter getSystemSelection) : base(state)
        {
            Data = tree;
            this.setEntitySelection = entitySelectionCallback;
            this._getRootSelection = getRootSelection;
            this.getSystemSelection = getSystemSelection;
            getNewSelectionOverride = (item, selection, shift) => new List<int>() {item.id};
            Reload();
        }

        internal bool ShowingSomething => _getRootSelection() != null;// &&
                                       //(selectedEntityQuery != null || !(getSystemSelection() is ComponentSystemBase));

        public bool NeedsReload => ShowingSomething;
        
        public void ReloadIfNecessary()
        {
            if (NeedsReload)
                Reload();
        }

        public int EntityCount => rows.Count;

        protected override TreeViewItem BuildRoot()
        {
            var root  = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            root.children = new List<TreeViewItem>();
            entityById.Clear();
            if (Application.isPlaying)
            {
                var group = getSystemSelection();
                var entities = group?.DebugEntities;
                var currentId = 1;
                if (entities != null)
                    foreach (var entity in entities)
                    {
                        var id = currentId++;
                        entityById.Add(id, entity);
                        var node = new TreeViewItem
                            {id = id, displayName = $"Entity {entity.Id}"};
                        //node.Active = true;
                        root.children.Add(node);
                    }
            }
            
            return root;
        }

//        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
//        {
//            if (!ShowingSomething)
//                return new List<TreeViewItem>();
//            
////            var entityManager = _getRootSelection().EntityManager;
////            
////            if (chunkArray.IsCreated)
////                chunkArray.Dispose();
////
////            entityManager.CompleteAllJobs();
////
////            var group = SelectedEntityQuery?.Group;
////
////            if (group == null)
////            {
////                var query = SelectedEntityQuery?.QueryDesc;
////                if (query == null)
////                    group = entityManager.UniversalQuery;
////                else
////                {
////                    group = entityManager.CreateEntityQuery(query);
////                }
////            }
////
////            chunkArray = group.CreateArchetypeChunkArray(Allocator.Persistent);
////
////            rows.SetSource(chunkArray, entityManager, chunkFilter);
////            setChunkArray(chunkArray);
////
////            lastVersion = entityManager.Version;
//
//            return rows;
//        }

        protected override IList<int> GetAncestors(int id)
        {
            return id == 0 ? new List<int>() : new List<int>() {0};
        }

        protected override IList<int> GetDescendantsThatHaveChildren(int id)
        {
            return new List<int>();
        }

        public override void OnGUI(Rect rect)
        {
            //if (_getRootSelection()?.EntityManager.IsCreated == true)
                base.OnGUI(rect);
        }

        public void OnEntitySelected(DebugEntity entity)
        {
            setEntitySelection(entity);
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
//            if (selectedIds.Count > 0)
//            {
//                DebugEntity selectedEntity;
//                if (rows.GetById(selectedIds[0], out selectedEntity))
//                    setEntitySelection(selectedEntity);
//            }
//            else
//            {
//                setEntitySelection(Entity.Null);
//            }
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

//        protected override bool CanRename(TreeViewItem item)
//        {
//            return true;
//        }
        //todo

//        protected override void RenameEnded(RenameEndedArgs args)
//        {
//            if (args.acceptedRename)
//            {
//                var manager = _getRootSelection()?.EntityManager;
//                if (manager != null)
//                {
//                    Entity entity;
//                    if (rows.GetById(args.itemID, out entity))
//                    {
//                        manager.SetName(entity, args.newName);
//                    }
//                }
//            }
//        }

        public void SelectNothing()
        {
            SetSelection(new List<int>());
        }

        public void SetEntitySelection(DebugEntity entitySelection)
        {
//            if (entitySelection != Entity.Null && _getRootSelection().EntityManager.Exists(entitySelection))
//                SetSelection(new List<int>{EntityArrayListAdapter.IndexToItemId(entitySelection.Index)});
        }

        public void TouchSelection()
        {
            SetSelection(
                GetSelection()
                , TreeViewSelectionOptions.RevealAndFrame);
        }

        public void FrameSelection()
        {
            var selection = GetSelection();
            if (selection.Count > 0)
            {
                FrameItem(selection[0]);
            }
        }
    }
}
