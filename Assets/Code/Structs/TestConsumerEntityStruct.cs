using System;
using Svelto.ECS;

namespace Code.Structs
{
    [Serializable]
    public struct TestConsumerEntityStruct : IEntityStruct, INeedEGID
    {
        public DateTime TESTVALUE;
        public EGID ID { get; set; }
    }
}