using System.Collections;
using Code.Structs;
using Code.Views;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;

namespace Code.Engines
{
    public class PlayerCollideAsteroidEngine : IQueryingEntitiesEngine
    {
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            Collide().RunOn(StandardSchedulers.lateScheduler);
        }

        IEnumerator Collide()
        {
            while (!entitiesDB.HasAny<CollideViewStruct>(ECSGroups.Player))
                yield return null;
            while (true)
            {
                var player = entitiesDB.QueryEntities<CollideViewStruct, PlayerLivesEntityStruct>(ECSGroups.Player, out var count);

                if (player.Item1[0].CollisionComponent.IsCollide)
                {
                    player.Item2[0].Lives -= 1;
                    player.Item2[0].IsDead = true;
                    player.Item1[0].CollisionComponent.IsCollide = false;
                    player.Item1[0].CollisionComponent.IsCollideLocked = true;
                }

                yield return null;
            }
        }
    }
}