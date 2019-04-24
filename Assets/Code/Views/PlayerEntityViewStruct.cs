using Code.Components;
using Svelto.ECS;
using Svelto.ECS.Hybrid;
using UnityEngine;

namespace Code.Views
{
    public struct PlayerEntityViewStruct : IEntityViewStruct
    {
        public ITransformComponent TransformComponent;
        public ISpeedComponent SpeedComponent;
        public EGID ID { get; set; }
    }
}