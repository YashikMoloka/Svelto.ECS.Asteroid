using System;
using System.Collections;
using Code.Structs;
using Code.Views;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Engines
{
    public class PlayerMoveEngine : IQueryingEntitiesEngine
    {
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            Move().RunOn(StandardSchedulers.updateScheduler);
        }

        IEnumerator Move()
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
                
                var player = entitiesDB.QueryUniqueEntity<PlayerEntityViewStruct>(ECSGroups.Player);
                var inputNow = input[0];
                
                //input
                if (inputNow.IsRight)
                    player.TransformComponent.rotation = Quaternion.Euler(player.TransformComponent.rotation.eulerAngles - Vector3.forward * Consts.PLAYER_ROTATE_SPEED * Time.deltaTime);

                if (inputNow.IsLeft)
                    player.TransformComponent.rotation = Quaternion.Euler(player.TransformComponent.rotation.eulerAngles + Vector3.forward * Consts.PLAYER_ROTATE_SPEED * Time.deltaTime);

                player.SpeedComponent.IsAccelerating = inputNow.IsForward;
                //endinput
                player.SpeedComponent.Velocity *= Consts.PLAYER_ACCELERATION_DRAG;
                
                var acceleration = Vector2.zero;
                var angle = player.TransformComponent.rotation.eulerAngles.z;
                var forward = (Vector2)(player.TransformComponent.rotation * Vector2.up);
                if (player.SpeedComponent.IsAccelerating)
                {
                    acceleration += (forward * Consts.PLAYER_ACCELERATION * Time.deltaTime);
                }

                player.SpeedComponent.Velocity += acceleration;
                var endpos = player.TransformComponent.position + player.SpeedComponent.Velocity;
                
                var screenWidth = Screen.width;
                var screenHeight = Screen.height;
                var rect = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, 0));

                while (endpos.x < -rect.x)
                    endpos.x += rect.x*2;

                while (endpos.x > rect.x)
                    endpos.x = endpos.x - rect.x * 2;

                while (endpos.y < -rect.y)
                    endpos.y += rect.y * 2;

                while (endpos.y > rect.y)
                    endpos.y = endpos.y - rect.y * 2;

                player.TransformComponent.position = endpos;
                yield return null;
            }
        }
    }
}