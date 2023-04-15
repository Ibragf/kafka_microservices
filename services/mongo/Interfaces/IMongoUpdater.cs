using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongo.Interfaces
{
    public interface IMongoUpdater
    {
        void Execute(string beforeJson, string afterJson, string op);
    }
}
