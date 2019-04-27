using System;
using System.Collections;
using Code.Structs;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;
using UnityEngine;

namespace Code.Engines
{
    public class UiTestEngine : IQueryingEntitiesEngine
    {
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            Test().RunOn(StandardSchedulers.updateScheduler);
        }

        IEnumerator Test()
        {
            while (!entitiesDB.HasAny<TestConsumerEntityStruct>(ECSGroups.Test))
                yield return null;
            while (true)
            {

                TestLog();
                yield return null;
            }
        }

        private void TestLog()
        {
            var ui = entitiesDB.QueryEntities<TestConsumerEntityStruct>(ECSGroups.Test, out var count);
            ui[0].TESTVALUE = DateTime.Now;
            entitiesDB.PublishEntityChange<TestConsumerEntityStruct>(ui[0].ID);
//
//            var ui2 = entitiesDB.QueryEntities<TestConsumerEntityStruct>(ECSGroups.Test, out count);
//            Debug.Log(ui2[0].TESTVALUE);
//
//            ref var ui3 = ref entitiesDB.QueryUniqueEntity<TestConsumerEntityStruct>(ECSGroups.Test);
//            ui3.TESTVALUE = DateTime.MinValue;
//
//            ui2 = entitiesDB.QueryEntities<TestConsumerEntityStruct>(ECSGroups.Test, out count);
//            Debug.Log(ui2[0].TESTVALUE);
//            Debug.Log("________________________");
        }
    }
}