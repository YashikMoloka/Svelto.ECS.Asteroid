using UnityEngine;

namespace Code.Components
{
    public interface IComponent
    {
    }

    public interface IAnimationComponent : IComponent
    {
        string         playAnimation  { set; get; }
        AnimationState animationState { set; }
        bool           reset          { set; }
    }

    public interface IPositionComponent : IComponent
    {
        Vector2 position { get; }
    }

    public interface ITransformComponent : IPositionComponent
    {
        new Vector2 position { get; set; }
        Quaternion  rotation { get; set; }
        Matrix4x4  matrix { get; }
    }

    public interface ILayerComponent
    {
        int layer { set; }
    }

    public interface IRigidBodyComponent : IComponent
    {
        bool isKinematic { set; }
    }

    public interface ISpeedComponent : IComponent
    {
        Vector2 Velocity { get; set; }
        bool IsAccelerating { get; set; }
    }

    public interface IDamageSoundComponent : IComponent
    {
        AudioType playOneShot { set; }
    }
    public interface ICollisionComponent : IComponent
    {
        bool IsCollide { get; set; }
        bool IsCollideLocked { get; set; }
    }
    public interface IGameObjectComponent : IComponent
    {
        bool DestroyIt { set; }
    }
}