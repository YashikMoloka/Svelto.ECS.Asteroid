using System.Collections;
using Code.Game.Others.Extensions;
using Code.Structs;
using Code.Views;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;
using UnityEngine;

namespace Code.Engines
{
    public class PlayerInputEngine : IQueryingEntitiesEngine
    {
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            ReadInput().RunOn(StandardSchedulers.updateScheduler);
        }
        
        IEnumerator ReadInput()
        {
            //wait for the player to spawn
            while (entitiesDB.HasAny<PlayerEntityViewStruct>(ECSGroups.Player) == false)
                yield return null; //skip a frame

            var playerEntityViews =
                entitiesDB.QueryEntities<PlayerInputDataEntityStruct>(ECSGroups.Player, out var count);

            while (true)
            {
                playerEntityViews[0].IsFire  = Input.GetKey(KeyCode.Space);
                playerEntityViews[0].IsLeft = Input.GetKey(KeyCode.LeftArrow);
                playerEntityViews[0].IsRight = Input.GetKey(KeyCode.RightArrow);
                playerEntityViews[0].IsForward = Input.GetKey(KeyCode.UpArrow);
                yield return null;
            }
        }
    }
}