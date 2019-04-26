using Svelto.ECS.Debugger.DebugStructure;
using UnityEditor;
using UnityEngine;

namespace Svelto.ECS.Debugger.Editor
{
    public class EntitySelectionProxy : ScriptableObject
    {
        public delegate void EntityControlDoubleClickHandler(DebugEntity entity);

        public event EntityControlDoubleClickHandler EntityControlDoubleClick;

        //public EntityContainer Container { get; private set; }
        public DebugEntity Entity { get; private set; }
        [SerializeField] private int entityIndex;

        public bool Exists => true;

        public void OnEntityControlDoubleClick(DebugEntity entity)
        {
            EntityControlDoubleClick(entity);
        }

        public void SetEntity(DebugEntity entity)
        {
            this.Entity = entity;
            //this.Container = new EntityContainer(EntityManager, Entity);
            EditorUtility.SetDirty(this);
        }
    }
}