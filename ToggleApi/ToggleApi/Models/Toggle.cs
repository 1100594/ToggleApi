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
			ThrowOnNullArgument(name, nameof(name));

			Name = name;
			DefaultValue = value;
			_whitelist = new List<Client>();
			_customValues = new Dictionary<Client, bool>();
		}

		public void AddToWhitelist(IEnumerable<Client> clients)
		{
			ThrowOnNullArgument(clients, nameof(clients));

			if (clients.Any(client => IsInWhitelist(client)))
			{
				var client = clients.First(cl => IsInWhitelist(cl));
				throw new ArgumentException(
					$"Client application \"{client.Id}:{client.Version}\" already " +
					$"have permission to access toggle \"{Name}\"");
			}

			_whitelist.AddRange(clients);
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

		public void AddOrUpdateCustomValues(IEnumerable<Client> clients)
		{
			ThrowOnNullArgument(clients, nameof(clients));

			foreach (var client in clients)
			{
				AddOrUpdateCustomValue(client, !DefaultValue);
			}
		}

		public void AddOrUpdateCustomValue(Client client, bool toggleValue)
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

		public void RemoveCustomValueFor(Client client)
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
				RemoveCustomValueFor(client);
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
			return obj is Toggle other 
				&& Equals(other);
		}

		public bool Equals(Toggle other)
		{
			return !other.IsNull()
				&& string.Equals(Name, other.Name, StringComparison.CurrentCultureIgnoreCase);
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
	}
}