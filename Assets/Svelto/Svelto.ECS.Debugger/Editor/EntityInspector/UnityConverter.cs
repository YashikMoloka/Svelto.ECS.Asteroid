using Newtonsoft.Json;
using UnityEngine;

namespace Svelto.ECS.Debugger.Editor.EntityInspector
{
    //todo normal Unity.Object serialize
    public class UnityConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Object obj = (Object) value;
            writer.WriteStartObject();
            writer.WritePropertyName("MonoBehaviour");
            writer.WriteValue(JsonUtility.ToJson(obj, true));
            writer.WriteEndObject();
        }

        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof(Object) || objectType.IsSubclassOf(typeof (Object));
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