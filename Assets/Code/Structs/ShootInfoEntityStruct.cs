using System;
using Svelto.ECS;
using UnityEngine;

namespace Code.Structs
{
    [Serializable]
    public struct ShootInfoEntityStruct : IEntityStruct
    {
        public Vector2 Forward;

        public ShootInfoEntityStruct(Vector2 forward)
        {
            Forward = forward;
        }
    }
}