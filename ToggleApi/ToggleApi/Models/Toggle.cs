using System;
using System.Collections.Generic;
using System.Linq;
using ToggleApi.Utilities;
using static ToggleApi.Utilities.Utils;

namespace ToggleApi.Models
{
    public class Toggle : IEquatable<Toggle>, IEqualityComparer<Toggle>
    {
        private List<Client> _whitelist;
        private Dictionary<Client, bool> _customValues;

        public string Name { get; }
        public bool DefaultValue { get; internal set; }

        public IEnumerable<Client> Whitelist => _whitelist.AsReadOnly();
        public IEnumerable<Client> CustomValues => _customValues.Keys;

        public Toggle(string name, bool value)
        {
            Name = name;
            DefaultValue = value;
            _whitelist = new List<Client>();
            _customValues = new Dictionary<Client, bool>();
        }

        public void AddToWhitelist(ICollection<Client> clients)
        {
            ThrowOnNullArgument(clients, nameof(clients));

            _whitelist.AddRange(clients.Except(_whitelist));
        }

        public void UpdateWhitelist(Client client)
        {
            ThrowOnNullArgument(client, nameof(client));

            if (IsApplicableTo(client))
                throw new ArgumentException(
                    $"Client application \"{client.Id}:{client.Version}\" already " +
                    $"have permission to access toggle \"{Name}\"");

            _whitelist.Add(client);
        }

        public void UpdateCustomValue(Client client, bool toggleValue)
        {
            ThrowOnNullArgument(client, nameof(client));
            if (IsInCustomValues(client))
            {
                _customValues[client] = toggleValue;
            }
            else
            {
               _customValues.Add(client, toggleValue);
            }
        }

        private bool WhitelistExists()
        {
            return _whitelist.Count > 0;
        }
        private bool IsInWhitelist(Client client)
        {
            return _whitelist.Contains(client);
        }

        private bool IsInCustomValues(Client client)
        {
            return _customValues.ContainsKey(client);
        }

        public bool IsApplicableTo(Client client)
        {
            if (!WhitelistExists())
                return true;

            return IsInWhitelist(client) || IsInCustomValues(client);
        }

        public bool GetValueFor(Client client)
        {
            ThrowOnNullArgument(client, nameof(client));

            if (!IsApplicableTo(client))
                throw new ArgumentException(
                    $"Client application \"{client.Id}:{client.Version}\" does not " +
                    $"have permission to access toggle \"{Name}\"");

            if (_customValues.TryGetValue(client, out bool customValue))
                return customValue;
            return DefaultValue;
        }

        public void AddToCustomValues(IEnumerable<Client> clients)
        {
            ThrowOnNullArgument(clients, nameof(clients));

            var newCustomValues = clients.Where(c => !_customValues.Keys.Contains(c)).ToList();

            foreach (var value in newCustomValues)
            {
                _customValues.Add(value, !DefaultValue);
            }
        }

        public void ClearOverrideFor(Client client)
        {
            ThrowOnNullArgument(client, nameof(client));

            if (_customValues.ContainsKey(client))
                _customValues.Remove(client);
        }

        public void DettachFrom(Client client)
        {
            ThrowOnNullArgument(client, nameof(client));

            if (_whitelist.Contains(client))
            {
                _whitelist.Remove(client);
            }
            else if (_customValues.ContainsKey(client))
            {
                ClearOverrideFor(client);
            }
            else
            {
                throw new ArgumentException($"Client application \"{client.Id}:{client.Version}\" " +
                                            $"does not have permission to access toggle \"{Name}\"");
            }

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
            return !other.IsNull()
                && Name == other.Name;
        }

        bool IEqualityComparer<Toggle>.Equals(Toggle x, Toggle y)
        {
            return Equals(x, y);
        }

        public static bool Equals(Toggle x, Toggle y)
        {
            return !x.IsNull() && x.Equals(y);
        }


        public int GetHashCode(Toggle obj)
        {
            return obj?.Name == null ? base.GetHashCode() : obj.Name.GetHashCode();
        }
        public override string ToString()
        {
            return $"{Name}:{DefaultValue}";
        }

    }
}