using mongo.Interfaces;
using mongo.Models;
using mongo.Properties.Updaters;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongo
{
    public class CollectionProvider
    {
        public static IMongoCollection<Institute> GetCollection()
        {
            var client = new MongoClient("mongodb://mongo:mongo@mongo:27017");
            var db = client.GetDatabase("rtumirea");
            Console.WriteLine("RECEIVED DATABASE");
            return db.GetCollection<Institute>("institutes");
        }

        public static IMongoUpdater GetUpdater(string table)
        {
            switch(table)
            {
                case "institutes":
                    return new InstituteUpdater();
                case "departments":
                    return new DepartmentUpdater();
                case "specialities":
                    return new SpecialityUpdater();
                default: return null!;
            }
        }
    }
}
