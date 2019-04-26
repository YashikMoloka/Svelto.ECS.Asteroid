namespace Svelto.ECS.Debugger
{
    public class ExclusiveGroupNamed : ExclusiveGroup
    {
        public ExclusiveGroupNamed(string name) : base()
        {
            var id = (uint) this;
            Debugger.RegisterNameGroup(id, name);
        }
    }
}