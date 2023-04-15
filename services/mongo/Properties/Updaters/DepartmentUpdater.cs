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
    internal class DepartmentUpdater : IMongoUpdater
    {
        public void Execute(string beforeJson, string afterJson, string op)
        {
            if(op == "c")
            {
                var after = JsonConvert.DeserializeObject<Department>(afterJson);
                Insert(after);
            }
            if(op=="u")
            {
                var before = JsonConvert.DeserializeObject<Department>(beforeJson);
                var after = JsonConvert.DeserializeObject<Department>(afterJson);
                Update(before, after);
            }
            if(op == "d")
            {
                var before = JsonConvert.DeserializeObject<Department>(beforeJson);
                Delete(before);
            }
        }

        private void Insert(Department after)
        {
            var collection = CollectionProvider.GetCollection();
            after.specs = new();
            after.courses = new();

            var filter = Builders<Institute>.Filter.Eq(x => x.id,after.institute_fk);

            var update = Builders<Institute>.Update.Push(x => x.departments, after);

            collection.UpdateOne(filter, update);
        }

        private void Update(Department before, Department after)
        {
            if(before.institute_fk!=after.institute_fk)
            {
                Delete(before);
                Insert(after);
                return;
            }

            var collection = CollectionProvider.GetCollection();


            var arrayFilter = Builders<Institute>.Filter.Eq("_id", after.institute_fk) &
                Builders<Institute>.Filter.Eq("departments.id", after.id);

            var update = Builders<Institute>.Update.Set("depatments.$.name", after.name);

            collection.UpdateOne(arrayFilter, update);
        }

        private void Delete(Department before)
        {
            var collection = CollectionProvider.GetCollection();

            var filter = Builders<Institute>.Filter.Eq("_id", before.institute_fk);
            var update = Builders<Institute>.Update.PullFilter("departments", Builders<Institute>.Filter.Eq(x => x.id, before.id));

            collection.UpdateOne(filter, update);
        }
    }
}
