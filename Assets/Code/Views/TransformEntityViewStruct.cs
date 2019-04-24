using Code.Components;
using Svelto.ECS;
using Svelto.ECS.Hybrid;
using UnityEngine;

namespace Code.Views
{
    public struct TransformEntityViewStruct : IEntityViewStruct
    {
        public ITransformComponent TransformComponent;
        public EGID ID { get; set; }
    }
}