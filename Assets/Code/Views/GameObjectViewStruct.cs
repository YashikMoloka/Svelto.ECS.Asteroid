using Code.Components;
using Svelto.ECS;
using Svelto.ECS.Hybrid;

namespace Code.Views
{
    public struct GameObjectViewStruct : IEntityViewStruct
    {
        public IGameObjectComponent GameObjectComponent;
        public EGID ID { get; set; }
    }
}