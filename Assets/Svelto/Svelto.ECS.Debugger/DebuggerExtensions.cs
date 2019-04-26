using Svelto.ECS;

namespace Code.Svelto.ECS.Debugger
{
    public static class DebuggerExtensions
    {
        public static void AttachDebugger(this EnginesRoot root)
        {
            Debugger.Instance.AddEnginesRoot(root);
        }
    }
}