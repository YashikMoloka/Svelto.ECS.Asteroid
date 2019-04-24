using Svelto.ECS;

namespace Code
{
    public static class ECSGroups
    {
        public static ExclusiveGroup Player = new ExclusiveGroup();
        public static ExclusiveGroup Asteroid = new ExclusiveGroup();
        public static ExclusiveGroup Shoot = new ExclusiveGroup();
        public static ExclusiveGroup Test = new ExclusiveGroup();
    }
}