using Code.Views;
using Svelto.ECS;
using UnityEngine;

namespace Code.Engines
{
    public class GameObjectDestroyEngine : IReactOnAddAndRemove<GameObjectViewStruct>
    {
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            
        }

        public void Add(ref GameObjectViewStruct entityView)
        {
            
        }

        public void Remove(ref GameObjectViewStruct entityView)
        {
            entityView.GameObjectComponent.DestroyIt = true;
        }
    }
}