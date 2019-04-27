using System;
using Svelto.ECS;
using UnityEngine;

namespace Code.Structs
{
    [Serializable]
    public struct PlayerInputDataEntityStruct : IEntityStruct
    {
        public bool IsRight;
        public bool IsLeft;
        public bool IsForward;
        public bool IsFire;
        public EGID ID { get; set; }
    }
}