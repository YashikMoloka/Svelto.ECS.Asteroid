using Code.Engines;
using Code.Others;
using Svelto.Context;
using Svelto.ECS;
using Svelto.ECS.Schedulers.Unity;
using Svelto.Tasks;

namespace Code
{
    public class MainCompositionRoot : ICompositionRoot
    {
        
        EnginesRoot                    _enginesRoot;
        IEntityFactory                 _entityFactory;
        UnityEntitySubmissionScheduler _unityEntitySubmissionScheduler;
        public void OnContextInitialized<T>(T contextHolder)
        {
            SetupEngines();
            SetupEntities();
        }

        private void SetupEntities()
        {
            BuildPlayer();
        }

        private void BuildPlayer()
        {
            new PlayerFactory(_entityFactory).Build();
        }

        private void SetupEngines()
        {
            _unityEntitySubmissionScheduler = new UnityEntitySubmissionScheduler();
            _enginesRoot                    = new EnginesRoot(_unityEntitySubmissionScheduler);
            _entityFactory = _enginesRoot.GenerateEntityFactory();
            var entityFunctions = _enginesRoot.GenerateEntityFunctions();
            
            _enginesRoot.AddEngine(new PlayerMoveEngine());
            _enginesRoot.AddEngine(new PlayerInputEngine());
            _enginesRoot.AddEngine(new PlayerRessurectEngine());
            _enginesRoot.AddEngine(new PlayerCollideAsteroidEngine());
            _enginesRoot.AddEngine(new PlayerShootEngine(new ShootFactory(_entityFactory)));
            
            _enginesRoot.AddEngine(new AsteroidSpawnEngine(new AsteroidsFactory(_entityFactory)));
            _enginesRoot.AddEngine(new AsteroidRotateEngine());
            _enginesRoot.AddEngine(new AsteroidMoveEngine());
            _enginesRoot.AddEngine(new AsteroidDrawEngine());
            
            _enginesRoot.AddEngine(new ShootMoveEngine());
        }

        public void OnContextDestroyed()
        {
            //final clean up
            _enginesRoot.Dispose();

            //Tasks can run across level loading, so if you don't want that, the runners must be stopped explicitly.
            //careful because if you don't do it and unintentionally leave tasks running, you will cause leaks
            TaskRunner.StopAndCleanupAllDefaultSchedulers();
        }

        public void OnContextCreated<T>(T contextHolder)
        {
        }
    }
}