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
        
        public void Build(AsteroidType type, Vector2 randomvector, Vector2 randomforward, Vector2 size, System.Random rnd = null)
        {
            var points = GenerateRandom(size, rnd);
            var cartesian = points
                .Select(x => new Vector2(x.x, x.y))
                .Select(Polar2Cartesian)
                .ToList();
            var gameobject = new GameObject();
            gameobject.name = gameobject.GetInstanceID().ToString();
            gameobject.transform.position = randomvector;
            var col = gameobject.AddComponent<PolygonCollider2D>();
            col.points = cartesian.ToArray();
            col.isTrigger = true;
            gameobject.AddComponent<Rigidbody2D>().isKinematic = true;
            gameobject.layer = LayerMask.NameToLayer("Asteroid");
            var impl1 = (IImplementor)gameobject.AddComponent<PlayerMovementImplementor>();
            var impl2 = (IImplementor)gameobject.AddComponent<GameObjectImplementor>();
            var impl3 = (IImplementor)gameobject.AddComponent<CollisionImplementor>();
            var initializer = _entityFactory
                .BuildEntity<AsteroidDescriptor>(new EGID((uint) gameobject.GetInstanceID(), ECSGroups.Asteroid),
                    new[] {impl1, impl2, impl3});
            initializer.Init(new AsteroidInfoEntityStruct(cartesian.ToArray(), randomforward.normalized, type));
        }
        
        readonly IEntityFactory    _entityFactory;
    }
}