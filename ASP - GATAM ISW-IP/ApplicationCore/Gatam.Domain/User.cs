using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Domain
{
    public class User
    {
        public string _id { get; set; }
        public User(string id)
            { this._id = id; }

    }
}
