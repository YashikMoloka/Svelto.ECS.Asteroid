using Svelto.ECS.Unity;
using UnityEngine;

namespace Code.Components.Impl
{
    [System.Serializable]
    public class GameObjectImplementor : MonoBehaviour, IImplementor, IGameObjectComponent
    {
        public bool DestroyIt
        {
            set
            {
                if (value)
                    Destroy(gameObject);
            }
        }
    }
}