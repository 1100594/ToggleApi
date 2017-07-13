using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ToggleApi.Models
{
    public class Toggle : IEquatable<Toggle>, IEqualityComparer<Toggle>
    {
        #region Public Variables
        public string Name { get; }
        public bool DefaultValue { get; }
        public IDictionary<Client, bool> Whitelist { get; private set; } = new Dictionary<Client, bool>();
        public IDictionary<Client, bool> CustomValues { get; private set; } = new Dictionary<Client, bool>();
        #endregion

        #region Constructors 
        public Toggle(string name, bool value)
        {
            Name = name;
            DefaultValue = value;
        }
        #endregion

        #region Private & Internal Methods

        internal void AddToWhitelist(IDictionary<Client, bool> clients)
        {
            Whitelist.Concat(clients.Where(x => !Whitelist.Keys.Contains(x.Key)));
        }
        private bool IsInWhitelist(Client client)
        {
            return Whitelist.ContainsKey(client);
        }

        private bool WhitelistExists()
        {
            return Whitelist.Count > 0;
        }
        #endregion

        #region Public Methods
        public bool IsApplicableTo(Client client)
        {
            if (!WhitelistExists() || IsInWhitelist(client))
                return true;

            return false;
        }

        public bool ValueFor(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

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
            AddToWhitelist(clients);
            CustomValues.Concat(clients.Where(x => !CustomValues.Keys.Contains(x.Key)));
        }

        public void ClearOverrideFor(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (CustomValues.ContainsKey(client))
                CustomValues.Remove(client);
        }

        public void DettachFrom(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (Whitelist.ContainsKey(client))
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
            return instance != null && Equals(instance, this);
        }

        public bool Equals(Toggle other)
        {
            return other != null
                && Name == other.Name;
        }

        public bool Equals(Toggle x, Toggle y)
        {
            return x != null && y != null
                && x.Name == y.Name;
        }

        public int GetHashCode(Toggle obj)
        {
            if (obj?.Name == null) return base.GetHashCode();
            return obj.Name.GetHashCode();
        }
        #endregion
    }
}