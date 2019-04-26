using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Svelto.DataStructures.Experimental;
using Svelto.ECS;
using Svelto.ECS.Internal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Svelto.ECS.Debugger
{
    public class Debugger : MonoBehaviour
    {
        public static Debugger Instance;
        private List<EnginesRootInfo> _infos = new List<EnginesRootInfo>();
        [SerializeField]
        public DebugDicionary DebugInfo = new DebugDicionary();
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
            _infos[0]?.GetInfo();
        }

        public void AddEnginesRoot(EnginesRoot root)
        {
            //DebugInfo = new SerializableDictionary<uint, SerializableDictionary<uint, string>>();
            _infos.Add(new EnginesRootInfo(root, DebugInfo));
        }

        private class EnginesRootInfo
        {
            HashSet<IEngine> _enginesSet;
            DebugDicionary DebugInfo;
            
            FasterDictionary<uint, Dictionary<Type, ITypeSafeDictionary>> _groupEntityDB;
            public EnginesRootInfo(EnginesRoot root, DebugDicionary _debugInfo)
            {
                DebugInfo = _debugInfo;
                
                var typeFields = typeof(EnginesRoot).GetAllFields();
                var enginesSetField = typeFields.First(f => f.Name == "_enginesSet");
                var groupEntityDBField = typeFields.First(f => f.Name == "_groupEntityDB");
                
                _enginesSet = (HashSet<IEngine>) enginesSetField.GetValue(root);
                _groupEntityDB = (FasterDictionary<uint, Dictionary<Type, ITypeSafeDictionary>>) groupEntityDBField.GetValue(root);
            }

            public void GetInfo()
            {
                var settings = new JsonSerializerSettings();
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.Converters.Add(new VectorConverter(true, true, true));
                settings.Converters.Add(new QuaternionConverter());
                settings.Converters.Add(new Matrix4x4Converter());
                settings.Converters.Add(new ResolutionConverter());
                settings.Converters.Add(new ColorConverter());
                settings.Converters.Add(new KeyValuePairConverter());
                settings.Converters.Add(new UnityConverter());
                var dic = DebugInfo;
                using (var enu = _groupEntityDB.GetEnumerator())
                {
                    while (enu.MoveNext())
                    {
                        var current = enu.Current;
                        var val = current.Value;
                        if (!dic.ContainsKey(current.Key))
                            dic[current.Key] = new EntityDictionary();
                        var dictionary = dic[current.Key];
                        foreach (var entityStructs in val)
                        {
                            var type = entityStructs.Key;
                            var valTypeSafe = entityStructs.Value;
                            
                            var fields = valTypeSafe.GetType().GetAllFields();
                            var valuesField = fields.First(s => s.Name == "_values");
                            var valuesInfoField = fields.First(s => s.Name == "_valuesInfo");
                            var values = (Array) valuesField.GetValue(valTypeSafe);
                            var keys = (Array) valuesInfoField.GetValue(valTypeSafe);
                            var temp = valuesInfoField.FieldType.GetElementType().GetAllFields();
                            var nodeKeyField = temp.First(f => f.Name == "key");
                            
                            for (int i = 0; i < values.Length; i++)
                            {
                                var key = (uint)nodeKeyField.GetValue(keys.GetValue(i));
                                if (!dictionary.ContainsKey(key))
                                    dictionary[key] = string.Empty;
                                dictionary[key] += JsonConvert.SerializeObject(values.GetValue(i), settings);
                            }
                        }
                    }
                }
                
                //var str = JsonConvert.SerializeObject(dic, Formatting.Indented);
                //Debug.Log(str);
                
            }
        }
        [Serializable]
        public class DebugDicionary : SerializableDictionary<uint, EntityDictionary>
        {
        }
        [Serializable]
        public class EntityDictionary : SerializableDictionary<uint, string>
        {
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