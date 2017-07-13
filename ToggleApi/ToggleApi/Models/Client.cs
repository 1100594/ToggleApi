using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToggleApi.Properties;

namespace ToggleApi.Models
{
    public class Client : IEquatable<Client>, IEqualityComparer<Client>
    {
        #region Public Variables
        public string Version { get; }
        public string Id { get; set; }
        public ICollection<Toggle> Toggles { get; set; } = new List<Toggle>();
        #endregion

        #region Constructors 
        public Client(string id, string version)
        {
            Id = id;
            Version = version;
        }
        #endregion

        #region Public Methods
        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        public override bool Equals(object obj)
        {
            var instance = obj as Client;
            return instance != null && Equals(instance, this);
        }

        public bool Equals(Client other)
        {
            return other != null
                && Id == other.Id
                && (Version == other.Version || Version == Resources.Wildcard);
        }

        public bool Equals(Client x, Client y)
        {
            return x != null && y != null
                && x.Id == y.Id
                && (x.Version == y.Version || x.Version == Resources.Wildcard);
        }

        public int GetHashCode(Client obj)
        {
            if (obj?.Id == null || obj?.Version == null) return base.GetHashCode();
            return $"{obj.Id}.{obj.Version}".GetHashCode();
        }
        #endregion
    }
}
