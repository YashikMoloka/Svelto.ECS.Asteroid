using Svelto.ECS;
using Svelto.ECS.Unity;
using UnityEngine;

namespace Code.Components.Impl
{
    [System.Serializable]
    public class CollisionImplementor : MonoBehaviour, ICollisionComponent, IImplementor
    {
        public bool IsCollide { get; set; }
        public bool IsCollideLocked { get; set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsCollideLocked)
                IsCollide = true;
        }
    }
}