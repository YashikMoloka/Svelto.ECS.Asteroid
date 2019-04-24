using Svelto.ECS.Unity;
using UnityEngine;

namespace Code.Components.Impl
{
    public class PlayerMovementImplementor
        : MonoBehaviour, IImplementor, ISpeedComponent, ITransformComponent
    {
        Rigidbody2D    _playerRigidbody; 
        Transform    _playerTransform;

        public Vector2 Velocity { get; set; }
        public bool IsAccelerating { get; set; }

        public Vector2 position
        {
            get => _playerTransform.position;
            set => _playerRigidbody.MovePosition(value);
        }
        public Quaternion rotation
        {
            get => _playerTransform.rotation;
            set => _playerRigidbody.MoveRotation(value);
        }

        public Matrix4x4 matrix { get => transform.localToWorldMatrix; }

        void Awake()
        {
            _playerRigidbody = GetComponent<Rigidbody2D>();
            _playerTransform = transform;
        }
    }
}