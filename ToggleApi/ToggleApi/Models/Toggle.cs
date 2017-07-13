using System;
using System.Collections.Generic;

namespace ToggleApi.Models
{
    public class Toggle : IEquatable<Toggle>, IEqualityComparer<Toggle>
    {
        public string Name { get; }
        public bool Value { get; }
        public IDictionary<Client, string> Whitelist => new Dictionary<Client, string>();
        public IDictionary<Client, string> CustomValues => new Dictionary<Client, string>();

         public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Toggle other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Toggle x, Toggle y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(Toggle obj)
        {
            throw new NotImplementedException();
        }
    }
}