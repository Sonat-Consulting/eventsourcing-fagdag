using MongoDB.Bson;
using MongoDB.Bson.IO;
using Newtonsoft.Json;

namespace Clippers.Infrastructure.EventStore
{
    public static class MongoDynamic
    {
        private static System.Text.RegularExpressions.Regex objectIdReplace = new System.Text.RegularExpressions.Regex(@"ObjectId\((.[a-f0-9]{24}.)\)", System.Text.RegularExpressions.RegexOptions.Compiled);
        /// <summary>
        /// deserializes this bson doc to a .net dynamic object
        /// </summary>
        /// <param name="bson">bson doc to convert to dynamic</param>
        public static dynamic ToDynamic(this BsonDocument bson)
        {
            //var json = objectIdReplace.Replace(bson.ToJson(), (s) => s.Groups[1].Value);
            //return Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);

            //var json = bson.ToJson(new MongoDB.Bson.IO.JsonWriterSettings { OutputMode = JsonOutputMode.Strict });
            //dynamic e = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(json);
            //return e;
            var jsonSettings = new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.IsoDateFormat };
            var jsonString = bson.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.Strict });
            //var dyn2 = JObject.Parse(jsonString);
            var dyn = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonString);

            return dyn;
        }
    }
}
