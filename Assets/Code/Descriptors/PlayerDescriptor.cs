using Code.Structs;
using Code.Views;
using Svelto.ECS;

namespace Code.Descriptors
{
    public class PlayerDescriptor : GenericEntityDescriptor<PlayerInputDataEntityStruct, PlayerEntityViewStruct, 
        CollideViewStruct, PlayerLivesEntityStruct, GameObjectViewStruct>
    {
        
    }
}