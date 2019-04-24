using UnityEngine;

namespace Code
{
    public static class Consts
    {
        public static readonly Vector2 ASTEROID_BIG_SIZE = new Vector2(0.3f, 1.0f);
        public static readonly Vector2 ASTEROID_SMALL_SIZE = new Vector2(0.05f, 0.1f);
        public static int ASTEROID_BREAK_COUNT = 3;
        public const float SHOOT_SPEED = 7f;
        public const int PLAYER_LIVES = 3;
        public const int ASTEROID_START_SPAWNS = 3;
        public const int ASTEROID_SECONDS_BETWEEN_SPAWNS = 2;
        public const float ASTEROID_SIDE = 1f;
        public const float PLAYER_ROTATE_SPEED = 360f;
        public const float PLAYER_ACCELERATION = 1f;
        public const float PLAYER_ACCELERATION_DRAG = 0.95f;
        public const float PLAYER_SHOOT_COOLDOWN = 0.33f;
        public const float ASTEROID_ROTATE = 160f;
        public const float ASTEROID_SPEED = 1f;
    }
}