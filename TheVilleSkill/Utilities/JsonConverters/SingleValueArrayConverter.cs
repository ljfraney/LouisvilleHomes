using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheVilleSkill.Utilities.JsonConverters
{
    public class SingleValueArrayConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // The Eventful API response has a property "events" that only ever has one property named "event", which may be a single
            // event, or an array of events. Likewise, an event has a property named "performers" that only ever has one property named
            // "performer", which may be a single performer, or an array of performers. I'm eliminating the "event" and "performer"
            // properties, creating an array when there is only a single item, and moving the array up a level to the "events" and
            // "performers" properties.

            // The "events" or "performers" property is null. Return an empty list.
            if (reader.TokenType == JsonToken.Null)
                return new List<T>();

            // At this point, we have "events" or "performers". Drill down one level.
            reader.Read(); // Read past the property name.
            reader.Read(); // Read to the property value.

            var returnValue = new object();

            // The "event" or "performer" property is null. Return an empty list.
            if (reader.TokenType == JsonToken.Null)
                returnValue = new List<T>();

            // There was a single event or performer. Return the item as a List with one item.
            if (reader.TokenType == JsonToken.StartObject)
            {                
                var instance = (T)serializer.Deserialize(reader, typeof(T));
                returnValue = new List<T>() { instance };
            }

            // There was an array of events or performers. Return the array as a List.
            if (reader.TokenType == JsonToken.StartArray)
                returnValue = serializer.Deserialize(reader, objectType);

            // Read the EndObject token to avoid the "Additional text found in JSON string after finishing deserializing object"
            // Newtonsoft.Json.JsonSerializationException.
            reader.Read();

            return returnValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
