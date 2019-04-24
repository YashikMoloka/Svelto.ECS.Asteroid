using System.Collections.Generic;
using Svelto.ECS;
using UnityEngine;

namespace Code.Structs
{
    public struct AsteroidInfoEntityStruct : IEntityStruct
    {
        public Vector2[] Points;
        public Vector2 Forward;
        
        public AsteroidInfoEntityStruct(List<Vector2> cartesian, Vector2 forward)
        {
            Points = cartesian.ToArray();
            Forward = forward;
        }
    }
}