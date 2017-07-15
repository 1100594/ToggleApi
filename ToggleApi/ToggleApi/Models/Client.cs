using System;
using System.Collections.Generic;
using ToggleApi.Properties;

namespace ToggleApi.Models
{
    public class Client : IEquatable<Client>, IEqualityComparer<Client>
    {
        public string Version { get; }
        public string Id { get; }

        public Client(string id, string version)
        {
            Id = id;
            Version = version;
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        public override bool Equals(object obj)
        {
            return obj is Client && Equals(obj);
        }

        public bool Equals(Client other)
        {
            return other != null
                && Id == other.Id
                && IsCompatibleVersion(other.Version);
        }

        public bool Equals(Client x, Client y)
        {
            return x != null && x.Equals(y);
        }

        public int GetHashCode(Client obj)
        {
            if (obj?.Id == null || obj.Version == null) return base.GetHashCode();
            return $"{obj.Id}.{obj.Version}".GetHashCode();
        }


        private bool IsCompatibleVersion(string otherVersion)
        {
            return Version == otherVersion || Version == Resources.Wildcard;
        }

    }
}
