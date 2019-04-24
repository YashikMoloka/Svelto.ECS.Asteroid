using Code.Components;
using Svelto.ECS;
using Svelto.ECS.Hybrid;

namespace Code.Views
{
    public struct CollideViewStruct : IEntityViewStruct
    {
        public ICollisionComponent CollisionComponent;
        public EGID ID { get; set; }
    }
}