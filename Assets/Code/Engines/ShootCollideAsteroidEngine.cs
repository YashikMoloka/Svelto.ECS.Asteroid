using System.Collections;
using Code.Descriptors;
using Code.Structs;
using Code.Views;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;

namespace Code.Engines
{
    public class ShootCollideAsteroidEngine : IQueryingEntitiesEngine
    {
        readonly IEntityFunctions _entityFunctions;
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            Collide().RunOn(StandardSchedulers.updateScheduler);
        }

        public ShootCollideAsteroidEngine(IEntityFunctions entityFunctions)
        {
            _entityFunctions = entityFunctions;
        }
        
        IEnumerator Collide()
        {
            while (!entitiesDB.HasAny<CollideViewStruct>(ECSGroups.Shoot))
                yield return null;
            while (true)
            {
                var shoots = entitiesDB.QueryEntities<CollideViewStruct>(ECSGroups.Shoot);
                foreach (var shoot in shoots)
                {
                    if (shoot.CollisionComponent.IsCollide)
                    {
                        shoot.CollisionComponent.IsCollide = false;
                        shoot.CollisionComponent.IsCollideLocked = true;
                        _entityFunctions.RemoveEntity<ShootDescriptor>(shoot.ID);
                    }
                }
                yield return null;
            }
        }
    }
}