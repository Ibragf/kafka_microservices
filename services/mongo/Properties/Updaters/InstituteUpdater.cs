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
    public class InstituteUpdater : IMongoUpdater
    {
        public void Execute(string beforeAfter, string afterJson, string op)
        {
            var institute = JsonConvert.DeserializeObject<Institute>(afterJson);
            if (institute == null) throw new NullReferenceException("Institute instance is null");
            institute.departments = new();

            if(op == "c")
            {
                var collection = CollectionProvider.GetCollection();

                collection.InsertOne(institute);
            }

            if (op == "u") Update(institute);

            if (op == "d")
            {
                var before = JsonConvert.DeserializeObject<Institute>(afterJson);
                Delete(before);
            }
        }

        private void Update(Institute institute)
        {
            var collection = CollectionProvider.GetCollection();

            var filter = Builders<Institute>.Filter.Eq("_id", institute.id);

            var update = Builders<Institute>.Update.Set(x => x.name, institute.name);

            collection.UpdateOne(filter, update);
        }

        private void Delete(Institute institute)
        {
            var collection = CollectionProvider.GetCollection();

            var filter = Builders<Institute>.Filter.Eq("_id", institute.id);

            collection.DeleteOne(filter);
        }
    }
}
