using System;
using System.Collections.Generic;
using Code.Others;
using Svelto.ECS;
using UnityEngine;

namespace Code.Structs
{
    [Serializable]
    public struct AsteroidInfoEntityStruct : IEntityStruct
    {
        public Vector2[] Points;
        public Vector2 Forward;
        public AsteroidType AsteroidType;

        public AsteroidInfoEntityStruct(Vector2[] points, Vector2 forward, AsteroidType asteroidType)
        {
            Points = points;
            Forward = forward;
            AsteroidType = asteroidType;
        }
    }
}