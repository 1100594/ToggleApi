using System;
using System.Collections.Generic;
using System.Linq;
using ToggleApi.Utilities;

namespace ToggleApi.Models
{
    public class Toggle : IEquatable<Toggle>, IEqualityComparer<Toggle>
    {
        public string Name { get; }
        public bool DefaultValue { get; }
        public IList<Client> Whitelist { get; } = new List<Client>();
        public IDictionary<Client, bool> CustomValues { get; private set; } = new Dictionary<Client, bool>();

        public Toggle(string name, bool value)
        {
            Name = name;
            DefaultValue = value;
        }

        internal void AddToWhitelist(ICollection<Client> clients)
        {
            Whitelist.AddRange(clients.Except(Whitelist));
        }

        private bool IsInWhitelist(Client client)
        {
            return Whitelist.Contains(client);
        }

        private bool WhitelistExists()
        {
            return Whitelist.Count > 0;
        }

        public bool IsApplicableTo(Client client)
        {
            if (!WhitelistExists() || IsInWhitelist(client))
                return true;

            return false;
        }

        public bool ValueFor(Client client)
        {
            Utils.ThrowOnNullArgument(client, nameof(client));

            if (!IsApplicableTo(client))
                throw new ArgumentException(
                    $"Client application \"{client.Id}:{client.Version}\" does not " +
                    $"have permission to access toggle \"{Name}\"");

            if (CustomValues.ContainsKey(client))
                return !DefaultValue;
            return DefaultValue;
        }

        public void OverrideFor(IDictionary<Client, bool> clients)
        {
            var newCustomValues = clients.Where(x => !CustomValues.Keys.Contains(x.Key));
            CustomValues = CustomValues.Concat(newCustomValues).ToDictionary(x => x.Key, x => x.Value);
        }

        public void ClearOverrideFor(Client client)
        {
            Utils.ThrowOnNullArgument(client, nameof(client));

            if (CustomValues.ContainsKey(client))
                CustomValues.Remove(client);
        }

        public void DettachFrom(Client client)
        {
            Utils.ThrowOnNullArgument(client, nameof(client));

            if (Whitelist.Contains(client))
                Whitelist.Remove(client);

            if (CustomValues.ContainsKey(client))
                ClearOverrideFor(client);
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        public override bool Equals(object obj)
        {
            var instance = obj as Toggle;
            return instance != null && Equals(instance);
        }

        public bool Equals(Toggle other)
        {
            return other != null
                && Name == other.Name;
        }

        public bool Equals(Toggle x, Toggle y)
        {
            return x != null && x.Equals(y);
        }

        public int GetHashCode(Toggle obj)
        {
            if (obj?.Name == null) return base.GetHashCode();
            return obj.Name.GetHashCode();
        }
    }
}