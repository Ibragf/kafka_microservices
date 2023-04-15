using mongo.Interfaces;
using mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongo.Properties.Updaters
{
    public class SpecialityUpdater : IMongoUpdater
    {
        public void Execute(string beforeJson, string afterJson, string op)
        {
            if(op == "c")
            {
                var spec = JsonConvert.DeserializeObject<Spec>(afterJson);
                Insert(spec);
            }
        }

        private void Insert(Spec spec)
        {
            var collection = CollectionProvider.GetCollection();

            var filter = Builders<BsonDocument>.Filter.ElemMatch("departments", Builders<BsonDocument>.Filter.Eq("_id", spec.department_fk));

            var update = Builders<BsonDocument>.Update.AddToSet("specs", spec);

            collection.UpdateOne(filter.ToBsonDocument(), update.ToBsonDocument());
        }
    }
}
