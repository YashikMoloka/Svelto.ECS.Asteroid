using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Code.Others;
using Svelto.ECS;
using Svelto.Tasks;
using Svelto.Tasks.Enumerators;
using Svelto.Tasks.Lean;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Code.Engines
{
    public class AsteroidSpawnEngine : IQueryingEntitiesEngine
    {
        readonly AsteroidsFactory _enemyFactory;
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            Spawn().RunOn(StandardSchedulers.coroutineScheduler);
        }

        public AsteroidSpawnEngine(AsteroidsFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
        }
        
        IEnumerator<TaskContract> Spawn()
        {
            for (int i = 0; i < Consts.ASTEROID_START_SPAWNS; i++)
            {
                CreateAsteroid();
            }
            while (true)
            {
                yield return new WaitForSecondsEnumerator(Consts.ASTEROID_SECONDS_BETWEEN_SPAWNS).Continue();
                CreateAsteroid();
            }
        }

        private void CreateAsteroid()
        {
            var rect = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            var rects = new List<Vector2>()
            {
                new Vector2(Random.Range(-rect.x - Consts.ASTEROID_SIDE, rect.x + Consts.ASTEROID_SIDE), 
                    rect.y + Consts.ASTEROID_SIDE),
                new Vector2(Random.Range(-rect.x - Consts.ASTEROID_SIDE, rect.x + Consts.ASTEROID_SIDE), 
                    -rect.y - Consts.ASTEROID_SIDE),
                new Vector2(rect.x + Consts.ASTEROID_SIDE,
                    Random.Range(-rect.y - Consts.ASTEROID_SIDE, rect.y + Consts.ASTEROID_SIDE)),
                new Vector2(-rect.x - Consts.ASTEROID_SIDE,
                    Random.Range(-rect.y - Consts.ASTEROID_SIDE, rect.y + Consts.ASTEROID_SIDE)),
            };
            var randomPos = rects[Random.Range(0, 3)];
            var randomforward = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            _enemyFactory.Build(AsteroidType.Big, randomPos, randomforward, Consts.ASTEROID_BIG_SIZE);
        }
        
    }
}