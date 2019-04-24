using System.Collections;
using Code.Game.Others.Extensions;
using Code.Structs;
using Code.Views;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;
using UnityEngine;

namespace Code.Engines
{
    public class AsteroidMoveEngine : IQueryingEntitiesEngine
    {
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            Move().RunOn(StandardSchedulers.updateScheduler);
        }

        IEnumerator Move()
        {
            while (true)
            {
                var asteroids = entitiesDB.QueryEntities<TransformEntityViewStruct, AsteroidInfoEntityStruct>(ECSGroups.Asteroid, out var count);
                var rect = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

                for (int i = 0; i < count; i++)
                {
                    var asteroid = asteroids.Item2[i];
                    var nowPosition = asteroids.Item1[i];
                    var vec = asteroid.Forward * Consts.ASTEROID_SPEED * Time.deltaTime;
                    
                    var endpos = nowPosition.TransformComponent.position + vec;
                    while (endpos.x < -rect.x - Consts.ASTEROID_SIDE)
                        endpos.x += rect.x*2 + Consts.ASTEROID_SIDE;

                    while (endpos.x > rect.x + Consts.ASTEROID_SIDE)
                        endpos.x = endpos.x - rect.x * 2 - Consts.ASTEROID_SIDE;

                    while (endpos.y < -rect.y - Consts.ASTEROID_SIDE)
                        endpos.y += rect.y * 2 + Consts.ASTEROID_SIDE;

                    while (endpos.y > rect.y + Consts.ASTEROID_SIDE)
                        endpos.y = endpos.y - rect.y * 2 - Consts.ASTEROID_SIDE;

                    nowPosition.TransformComponent.position = endpos;
                }
                yield return null;
            }
        }
    }
}