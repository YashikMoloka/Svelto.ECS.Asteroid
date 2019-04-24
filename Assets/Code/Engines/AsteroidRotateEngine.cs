using System.Collections;
using Code.Game.Others.Extensions;
using Code.Views;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;
using UnityEngine;

namespace Code.Engines
{
    public class AsteroidRotateEngine : IQueryingEntitiesEngine
    {
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            Rotate().RunOn(StandardSchedulers.updateScheduler);
        }

        IEnumerator Rotate()
        {
            while (true)
            {
                var ast = entitiesDB.QueryEntitiesSegment<TransformEntityViewStruct>(ECSGroups.Asteroid);
                foreach (var asteroid in ast)
                {
                    asteroid.TransformComponent.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * Consts.ASTEROID_ROTATE);
                }

                yield return null;
            }
        }
    }
}