using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToggleApi.Models
{
    public class Client : IEquatable<Client>, IEqualityComparer<Client>
    {
        public string Version { get; }
        public string Id { get; set; }
        public ICollection<Toggle> Toggles { get; set; } = new List<Toggle>();

        public Client()
        {

        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Client other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Client x, Client y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(Client obj)
        {
            throw new NotImplementedException();
        }
    }
}
