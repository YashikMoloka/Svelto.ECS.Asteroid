using System.Collections;
using Code.Others;
using Code.Structs;
using Code.Views;
using Svelto.ECS;
using Svelto.Tasks.Enumerators;
using Svelto.Tasks.ExtraLean;
using UnityEngine;

namespace Code.Engines
{
    public class PlayerShootEngine: IQueryingEntitiesEngine
    {
        private ShootFactory _shootFactory;
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            Shoot().RunOn(StandardSchedulers.updateScheduler);
        }

        public PlayerShootEngine(ShootFactory shootFactory)
        {
            _shootFactory = shootFactory;
        }

        IEnumerator Shoot()
        {
            //wait for the player to spawn
            while (entitiesDB.HasAny<PlayerEntityViewStruct>(ECSGroups.Player) == false || entitiesDB.HasAny<PlayerInputDataEntityStruct>(ECSGroups.Player) == false)
                yield return null; //skip a frame

            var input =
                entitiesDB.QueryEntities<PlayerInputDataEntityStruct>(ECSGroups.Player, out var count);
            while (true)
            {
                while (entitiesDB.QueryUniqueEntity<PlayerLivesEntityStruct>(ECSGroups.Player).IsDead)
                    yield return null;
                if (input[0].IsFire)
                {
                    var player = entitiesDB.QueryUniqueEntity<PlayerEntityViewStruct>(ECSGroups.Player);
                    _shootFactory.Build(player.TransformComponent.position, player.TransformComponent.rotation * Vector3.up);
                    var timer = new WaitForSecondsEnumerator(Consts.PLAYER_SHOOT_COOLDOWN);
                    while (timer.MoveNext())
                    {
                        yield return null;
                    }
                }
                yield return null;
            }
        }
    }
}