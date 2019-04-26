using Svelto.ECS;
using Svelto.ECS.Debugger;

namespace Code
{
    public static class ECSGroups
    {
        public static ExclusiveGroup Player = new ExclusiveGroupNamed("Player");
        public static ExclusiveGroup Asteroid = new ExclusiveGroupNamed("Asteroid");
        public static ExclusiveGroup Shoot = new ExclusiveGroupNamed("Shoot");
        public static ExclusiveGroup Test = new ExclusiveGroupNamed("Test");
    }
}