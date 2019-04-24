using Code.Descriptors;
using Code.Structs;
using Svelto.ECS;
using Svelto.ECS.Unity;
using UnityEngine;

namespace Code.Others
{
    public class PlayerFactory
    {
        public PlayerFactory(IEntityFactory entityFactory)
        {
            _entityFactory     = entityFactory;
        }
        
        public void Build()
        {
            var template = Resources.Load<GameObject>("Prefabs/Player");
            
            var player = GameObject.Instantiate(template);
            
            var implementors = player.GetComponentsInChildren<IImplementor>() ?? new object[0];
            
            var initializer = _entityFactory
                .BuildEntity<PlayerDescriptor>(new EGID((uint) player.GetInstanceID(), ECSGroups.Player),
                    implementors);
            initializer.Init(new PlayerLivesEntityStruct(Consts.PLAYER_LIVES));
        }
        
        readonly IEntityFactory    _entityFactory;
    }
}