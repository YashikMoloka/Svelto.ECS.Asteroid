using Svelto.ECS;

namespace Code.Structs
{
    public struct PlayerLivesEntityStruct : IEntityStruct
    {
        public int Lives;
        public bool IsDead;

        public PlayerLivesEntityStruct(int lives)
        {
            Lives = lives;
            IsDead = false;
        }
    }
}