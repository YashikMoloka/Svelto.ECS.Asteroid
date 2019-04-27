using Code.Descriptors;
using Code.Structs;
using Svelto.ECS;
using Svelto.ECS.Unity;
using UnityEngine;

namespace Code.Others
{
    public class ShootFactory
    {
        public ShootFactory(IEntityFactory entityFactory)
        {
            _entityFactory     = entityFactory;
        }

        public void Build(Vector2 pos, Vector2 forward)
        {
            var template = Resources.Load<GameObject>("Prefabs/Shoot");
            
            var shoot = GameObject.Instantiate(template);
            shoot.name = shoot.GetInstanceID().ToString();
            shoot.transform.position = pos;
            shoot.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, forward));
            var implementors = shoot.GetComponentsInChildren<IImplementor>() ?? new object[0];
            
            var initializer = _entityFactory
                .BuildEntity<ShootDescriptor>(new EGID((uint) shoot.GetInstanceID(), ECSGroups.Shoot),
                    implementors);
            initializer.Init(new ShootInfoEntityStruct(forward));
        }
        
        readonly IEntityFactory _entityFactory;
    }
}