using System.Collections;
using Code.Descriptors;
using Code.Others;
using Code.Structs;
using Code.Views;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;
using UnityEngine;

namespace Code.Engines
{
    public class AsteroidCollideShootEngine : IQueryingEntitiesEngine
    {
        readonly IEntityFunctions _entityFunctions;
        readonly AsteroidsFactory _asteroidsFactory;
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            Collide().RunOn(StandardSchedulers.updateScheduler);
        }

        public AsteroidCollideShootEngine(IEntityFunctions entityFunctions, AsteroidsFactory asteroidsFactory)
        {
            _entityFunctions = entityFunctions;
            _asteroidsFactory = asteroidsFactory;
        }

        IEnumerator Collide()
        {
            while (true)
            {
                var asteroids = entitiesDB.QueryEntities<CollideViewStruct, TransformEntityViewStruct, AsteroidInfoEntityStruct>(ECSGroups.Asteroid, out var count);
                for (int i = 0; i < count; i++)
                {
                    var shoot = asteroids.Item1[i];
                    if (shoot.CollisionComponent.IsCollide)
                    {
                        shoot.CollisionComponent.IsCollide = false;
                        shoot.CollisionComponent.IsCollideLocked = true;
                        
                        if (asteroids.Item3[i].AsteroidType == AsteroidType.Big)
                            for (int j = 0; j < Consts.ASTEROID_BREAK_COUNT; j++)
                            {
                                var randomforward = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                                _asteroidsFactory.Build(AsteroidType.Small, asteroids.Item2[i].TransformComponent.position, randomforward, Consts.ASTEROID_SMALL_SIZE);
                            }
                        
                        _entityFunctions.RemoveEntity<AsteroidDescriptor>(shoot.ID);
                    }
                }
                yield return null;
            }
        }
    }
}