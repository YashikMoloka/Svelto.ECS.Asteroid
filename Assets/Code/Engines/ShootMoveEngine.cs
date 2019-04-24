using System.Collections;
using Code.Structs;
using Code.Views;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;
using UnityEngine;

namespace Code.Engines
{
    public class ShootMoveEngine : IQueryingEntitiesEngine
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
                var shoots = entitiesDB.QueryEntities<TransformEntityViewStruct, ShootInfoEntityStruct>(ECSGroups.Shoot, out var count);
                for (int i = 0; i < count; i++)
                {
                    var sh = shoots.Item2[i];
                    var direction = sh.Forward;
                    shoots.Item1[i].TransformComponent.position += direction * Consts.SHOOT_SPEED * Time.deltaTime;
                }
                yield return null;
            }
        }
    }
}