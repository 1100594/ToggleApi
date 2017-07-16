using System;
using System.Collections.Generic;
using ToggleApi.Properties;
using ToggleApi.Utilities;

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
        bool IEqualityComparer<Client>.Equals(Client x, Client y)
        {
            return Equals(x, y);
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        public  int GetHashCode(Client obj)
        {
            if (obj.IsNull() || obj.Id.IsNull() || obj.Version.IsNull()) return base.GetHashCode();
            return $"{obj.Id}.{obj.Version}".GetHashCode();
        }

        public static bool Equals(Client x, Client y)
        {
            return !x.IsNull() && x.Equals(y);
        }

        private bool IsCompatibleVersion(string otherVersion)
        {
            //TODO The 1.1.* scenario is missing
            return Version.Equals(otherVersion) || Version.Equals(Resources.Wildcard);
        }

        public override string ToString()
        {
            return $"{Id}:{Version}";
        }

    }
}
