using System;
using System.Collections.Generic;
using ToggleApi.Properties;
using ToggleApi.Utilities;
using static ToggleApi.Utilities.Utils;

namespace ToggleApi.Models
{
    public class Client : IEquatable<Client>, IEqualityComparer<Client>
    {
        public string Version { get; }
        public string Id { get; }

        public Client(string id, string version)
        {
            ThrowOnNullArgument(id, nameof(id));
            ThrowOnNullArgument(version, nameof(version));

            Id = id;
            Version = version;
        }

        public override bool Equals(object obj)
        {
            return obj is Client other && Equals(other);
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

        public int GetHashCode(Client obj)
        {
            if (obj.IsNull())
                return base.GetHashCode();
            //Fix try get value uses both gethashcode and equals
            return obj.Id.GetHashCode();
        }

        public static bool Equals(Client x, Client y)
        {
            return !x.IsNull() && x.Equals(y);
        }

        private bool IsCompatibleVersion(string otherVersion)
        {
            if (IsWildCard(Version) || IsWildCard(otherVersion))
                return true;

            if (Version == otherVersion)
                return true;

            string thisVersion = GetVersionBeforeWildCard(Version);
            string otherVersionSub = GetVersionBeforeWildCard(otherVersion);

            return thisVersion.StartsWith(otherVersionSub)
                || otherVersionSub.StartsWith(thisVersion);
        }

        private static string GetVersionBeforeWildCard(string version)
        {
            int wildCardIndex = version.IndexOf(Resources.Wildcard, StringComparison.Ordinal);

            if (wildCardIndex >= 0)
                return version.Substring(0, wildCardIndex);

            return version;
        }

        private static bool IsWildCard(string version)
        {
            if (version == Resources.Wildcard)
                return true;
            return false;
        }

        public override string ToString()
        {
            return $"{Id}:{Version}";
        }
    }
}
