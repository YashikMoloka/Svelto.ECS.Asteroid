using System.Linq;
using Code.Components.Impl;
using Code.Descriptors;
using Code.Structs;
using Code.Utils;
using Svelto.ECS;
using Svelto.ECS.Unity;
using UnityEngine;
using static Code.Utils.AsteroidSpawnUtil;

namespace Code.Others
{
    public class AsteroidsFactory
    {
        public AsteroidsFactory(IEntityFactory entityFactory)
        {
            _entityFactory     = entityFactory;
        }
        
        public void Build(Vector2 randomvector, Vector2 randomforward)
        {
            var points = GenerateRandom();
            var cartesian = points
                .Select(x => new Vector2(x.x, x.y))
                .Select(Polar2Cartesian)
                .ToList();
            var gameobject = new GameObject();
            gameobject.transform.position = randomvector;
            gameobject.AddComponent<PolygonCollider2D>().points = cartesian.ToArray();
            gameobject.AddComponent<Rigidbody2D>().isKinematic = true;
            gameobject.layer = LayerMask.NameToLayer("Asteroid");
            var impl = gameobject.AddComponent<PlayerMovementImplementor>();

            var initializer = _entityFactory
                .BuildEntity<AsteroidDescriptor>(new EGID((uint) gameobject.GetInstanceID(), ECSGroups.Asteroid),
                    new[] {impl});
            initializer.Init(new AsteroidInfoEntityStruct(cartesian, randomforward.normalized));
        }
        
        readonly IEntityFactory    _entityFactory;
    }
}