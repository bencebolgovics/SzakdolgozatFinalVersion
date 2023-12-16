using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Szakdolgozat.Services.Interfaces;

namespace Szakdolgozat.Services.Extensions
{
    public abstract class SerializeJsonBase : ISerializeJson
    {
        public string? Target { get; set; }

        public virtual T[] Deserialize<T>()
        {
            if (File.Exists(Target))
            {
                string json = File.ReadAllText(Target);
                return JsonConvert.DeserializeObject<T[]>(json) ?? Array.Empty<T>();
            }

            return Array.Empty<T>();
        }

        public virtual void Serialize<T>(T obj)
        {
            List<T> jsons = Deserialize<T>().ToList();

            if (jsons.Any(x => x.Equals(obj)))
                return;

            jsons.Add(obj);

            File.WriteAllText(Target, JsonConvert.SerializeObject(jsons, Formatting.Indented));
        }
    }
}
