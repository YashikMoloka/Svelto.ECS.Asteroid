using System.Collections;
using Code.Descriptors;
using Code.Views;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;
using UnityEngine;

namespace Code.Engines
{
    public class ShootBoundsEngine : IQueryingEntitiesEngine
    {
        readonly IEntityFunctions _entityFunctions;
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            Bounds().RunOn(StandardSchedulers.updateScheduler);
        }

        public ShootBoundsEngine(IEntityFunctions entityFunctions)
        {
            _entityFunctions = entityFunctions;
        }

        IEnumerator Bounds()
        {
            while (true)
            {
                var rect = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
                var shoots = entitiesDB.QueryEntities<TransformEntityViewStruct>(ECSGroups.Shoot);
                foreach (var shoot in shoots)
                {
                    var pos = shoot.TransformComponent.position;

                    if (pos.x < -rect.x - Consts.ASTEROID_SIDE ||
                        pos.x > rect.x + Consts.ASTEROID_SIDE ||
                        pos.y < -rect.y - Consts.ASTEROID_SIDE ||
                        pos.y > rect.y + Consts.ASTEROID_SIDE)
                    {
                        _entityFunctions.RemoveEntity<ShootDescriptor>(shoot.ID);
                    }
                }
                yield return null;
            }
        }
    }
}