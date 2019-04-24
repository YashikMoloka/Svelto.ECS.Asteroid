#if UNITY_5 || UNITY_5_3_OR_NEWER
using System.Collections;
using System.Collections.Generic;
using Svelto.Common;
using Svelto.Tasks.Internal;
using Svelto.Tasks.Unity.Internal;

namespace Svelto.Tasks
{
    namespace Lean.Unity
    {
        public class RenderMonoRunner:RenderMonoRunner<IEnumerator<TaskContract>>
        {
            public RenderMonoRunner(string name) : base(name)
            {
            }
        }
        
        public class RenderMonoRunner<T> : Svelto.Tasks.Unity.RenderMonoRunner<SveltoTask<T>> where T : IEnumerator<TaskContract>
        {
            public RenderMonoRunner(string name) : base(name)
            {
            }
        }
    }
    
    namespace ExtraLean.Unity
    {
        public class RenderMonoRunner:RenderMonoRunner<IEnumerator>
        {
            public RenderMonoRunner(string name) : base(name)
            {
            }
        }
        
        public class RenderMonoRunner<T> : Svelto.Tasks.Unity.RenderMonoRunner<SveltoTask<T>> where T : IEnumerator
        {
            public RenderMonoRunner(string name) : base(name)
            {
            }
        }
    }

    namespace Unity
    {
        public class RenderMonoRunner<T> : RenderMonoRunner<T, StandardRunningTasksInfo> where T : ISveltoTask
        {
            public RenderMonoRunner(string name) : base(name, new StandardRunningTasksInfo())
            {
            }
        }

        public class RenderMonoRunner<T, TFlowModifier> : BaseRunner<T> where T : ISveltoTask
                                                                        where TFlowModifier : IRunningTasksInfo
        {
            public RenderMonoRunner(string name, TFlowModifier modifier) : base(name)
            {
                modifier.runnerName = name;

                _processEnumerator =
                    new CoroutineRunner<T>.Process<TFlowModifier, PlatformProfiler>
                        (_newTaskRoutines, _coroutines, _flushingOperation, modifier);

                UnityCoroutineRunner.StartRenderCoroutine(_processEnumerator);
            }
        }
    }
}
#endif
