using System.Collections;
using Code.Structs;
using Code.Views;
using Svelto.ECS;
using Svelto.Tasks.Enumerators;
using Svelto.Tasks.ExtraLean;
using UnityEngine;

namespace Code.Engines
{
    public class PlayerRessurectEngine :IQueryingEntitiesEngine
    {
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            Ressurect().RunOn(StandardSchedulers.earlyScheduler);
        }

        IEnumerator Ressurect()
        {
            while (!entitiesDB.HasAny<CollideViewStruct>(ECSGroups.Player))
            {
                yield return null;
            }
            while (true)
            {
                var player = entitiesDB.QueryEntities<CollideViewStruct, PlayerLivesEntityStruct, PlayerEntityViewStruct>(ECSGroups.Player, out var count);
                if (player.Item2[0].IsDead)
                {
                    var timer = new WaitForSecondsEnumerator(3);
                    while (timer.MoveNext())
                    {
                        yield return null;
                    }
                    player.Item2[0].IsDead = false;
                    player.Item3[0].TransformComponent.position = Vector2.zero;
                    
                    timer = new WaitForSecondsEnumerator(5);
                    while (timer.MoveNext())
                    {
                        yield return null;
                    }
                    player.Item1[0].CollisionComponent.IsCollideLocked = false;
                }

                yield return null;
            }
        }
    }
}