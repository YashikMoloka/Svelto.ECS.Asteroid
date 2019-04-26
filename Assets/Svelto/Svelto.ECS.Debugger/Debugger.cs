using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Svelto.ECS.Debugger.DebugStructure;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Svelto.ECS.Debugger
{
    public class Debugger : MonoBehaviour
    {
        public static Debugger Instance;
        private static Dictionary<uint, string> GroupDebugNames = new Dictionary<uint, string>();
        [NonSerialized]
        public DebugTree DebugInfo = new DebugTree();
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
            DebugInfo.Update();
        }

        public void AddEnginesRoot(EnginesRoot root)
        {
            DebugInfo.AddRootToTree(root);
        }

        public static void RegisterNameGroup(uint id, string name)
        {
            GroupDebugNames[id] = name;
        }
        public static string GetNameGroup(uint id)
        {
            if (!GroupDebugNames.ContainsKey(id))
                return $"Unknown group: {id}";
            return GroupDebugNames[id];
        }
    }

    public class UnityConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Object resolution = (Object) value;
            writer.WriteStartObject();
            writer.WritePropertyName("name");
            writer.WriteValue(resolution.name);
            writer.WriteEndObject();
        }

        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof (Object) || objectType.IsSubclassOf(typeof (Object));
        }

        public override object ReadJson(
            JsonReader reader,
            System.Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            return (object) null;
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }
    }
}