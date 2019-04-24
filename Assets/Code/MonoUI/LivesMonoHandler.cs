using System.Collections;
using Code.Structs;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;
using UnityEngine;
using UnityEngine.UI;

namespace Code.MonoUI
{
    public class LivesMonoHandler : MonoBehaviour
    {
        private Consumer<TestConsumerEntityStruct> _consumer;
        private Text _text;
        private void Start()
        {
            _text = GetComponent<Text>();
        }

        public void SetConsumer(Consumer<TestConsumerEntityStruct> consumer)
        {
            _consumer = consumer;
            Test().RunOn(StandardSchedulers.updateScheduler);
        }

        IEnumerator Test()
        {
            while (true)
            {
                if (_consumer.TryDequeue(out var entity))
                {
                    _text.text = entity.TESTVALUE.ToLongDateString();
                }

                yield return null;
            }
        }
    }
}