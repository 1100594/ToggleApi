using System;
using System.Collections.Generic;
using System.Linq;
using ToggleApi.Models;
using Xunit;

namespace ToggleApiTest.Models
{
    public class ToggleTest
    {

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void CreateSuccessfull()
        {
            const string name = "myToggle";
            const bool value = true;

            var toggle = new Toggle(name, value);

            Assert.Equal(name, toggle.Name);
            Assert.Equal(value, toggle.DefaultValue);
            Assert.NotNull(toggle.Whitelist);
            Assert.Empty(toggle.Whitelist);
            Assert.NotNull(toggle.CustomValues);
            Assert.Empty(toggle.CustomValues);
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void CreateNullNameThrowsException()
        {
            string name = null;
            bool value = true;

            void Toggle() => new Toggle(name, value);

            Assert.Throws<ArgumentNullException>((Action) Toggle);
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void AddNewWhitelistItems()
        {
            const string name = "myToggle2";
            const bool value = true;

            var toggle = new Toggle(name, value);

            toggle.AddToWhitelist(new[]
            {
                new Client("myClient", "1.1.1.1"),
                new Client("myClient2", "1.1.1.1")
            });

            Assert.NotEmpty(toggle.Whitelist);
            Assert.Equal(2, toggle.Whitelist.Count());
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void AddNullWhitelistItems()
        {
            const string name = "myToggle3";
            const bool value = true;

            var toggle = new Toggle(name, value);

            void Add() => toggle.AddToWhitelist(null);

            Assert.Throws<ArgumentNullException>((Action) Add);
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void AddDuplicateWhitelistItemsThrowsException()
        {
            const string name = "myToggle4";
            const bool value = true;
            var toggle = new Toggle(name, value);

            toggle.AddToWhitelist(new[]
            {
                new Client("myClient", "1.1.1.1"),
                new Client("myClient2", "1.1.1.1")
            });

            void Add() => toggle.AddToWhitelist(new[]
            {
                new Client("myClient", "1.1.1.1")
            });

            Assert.Throws<ArgumentException>((Action) Add);
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void AddNewItemsToCustomValues()
        {
            const string name = "myToggle5";
            const bool value = true;

            var toggle = new Toggle(name, value);

            toggle.AddOrUpdateCustomValues(new[]
            {
                new Client("myClient", "1.1.1.1"),
                new Client("myClient2", "1.1.1.1")
            });

            Assert.Equal(value,toggle.DefaultValue);
            Assert.NotEmpty(toggle.CustomValues);
            Assert.Equal(2, toggle.CustomValues.Count());
            Assert.False(toggle.GetValueFor(new Client("myClient", "1.1.1.1")));
            Assert.False(toggle.GetValueFor(new Client("myClient2", "1.1.1.1")));
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void UpdateItemToCustomValues()
        {
            const string name = "myToggle5";
            const bool value = true;

            var toggle = new Toggle(name, value);
            toggle.AddOrUpdateCustomValues(new[]
            {
                new Client("myClient", "1.1.1.1"),
                new Client("myClient2", "1.1.1.1")
            });
            
            toggle.AddOrUpdateCustomValue(new Client("myClient2", "1.1.1.1"), true);

            Assert.Equal(2, toggle.CustomValues.Count());
            Assert.False(toggle.GetValueFor(new Client("myClient", "1.1.1.1")));
            Assert.True(toggle.GetValueFor(new Client("myClient2", "1.1.1.1")));
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void DettachClientInWhitelist()
        {
            const string name = "myToggle6";
            const bool value = true;

            var toggle = new Toggle(name, value);
            toggle.AddOrUpdateCustomValues(new[] { new Client("myOtherClientToggle", "1.1.1.1") });
            toggle.AddToWhitelist(new[]
            {
                new Client("myClient", "1.1.1.1"),
                new Client("myClient2", "1.1.1.1")
            });

            toggle.DettachFrom(new Client("myClient2", "1.1.1.1"));

            Assert.NotEmpty(toggle.CustomValues);
            Assert.Equal(1, toggle.Whitelist.Count());
            Assert.True(toggle.GetValueFor(new Client("myClient", "1.1.1.1")));
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void DettachClientInCustomValues()
        {
            const string name = "myToggle7";
            const bool value = true;

            var toggle = new Toggle(name, value);
            toggle.AddToWhitelist(new[] { new Client("myOtherToggle", "1.1.1.1") });
            toggle.AddOrUpdateCustomValues(new[]
            {
                new Client("myClient", "1.1.1.1"),
                new Client("myClient2", "1.1.1.1")
            });

            toggle.DettachFrom(new Client("myClient2", "1.1.1.1"));

            Assert.NotEmpty(toggle.Whitelist);
            Assert.Equal(1, toggle.CustomValues.Count());
            Assert.False(toggle.GetValueFor(new Client("myClient", "1.1.1.1")));
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void ValueForNullClientThrowsException()
        {
            const string name = "myToggle8";
            const bool value = true;

            var toggle = new Toggle(name, value);

            void GetValue() => toggle.GetValueFor(null);

            Assert.Throws<ArgumentNullException>((Action) GetValue);
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void ValueForClientNotInWhitelistThrowsException()
        {
            const string name = "myToggle9";
            const bool value = true;
            
            var toggle = new Toggle(name, value);
            toggle.AddToWhitelist(new[] {
                new Client("myClient","*")
            });

            void GetValue() => toggle.GetValueFor(new Client("myToggleTest", "*"));

            Assert.Throws<ArgumentException>((Action) GetValue);
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void ValueForGlobalToggleReturnsDefault()
        {
            const string name = "myToggle10";
            const bool value = true;

            var toggle = new Toggle(name, value);

            var getValue = toggle.GetValueFor(new Client("myToggleTest", "*"));

            Assert.True(getValue);
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void AddNullItemToCustomValuesThrowsException()
        {
            const string name = "myToggle11";
            const bool value = true;

            var toggle = new Toggle(name, value);

            void Add() => toggle.AddToWhitelist(null);

            Assert.Throws<ArgumentNullException>((Action) Add);
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void ToggleEqualsTrue()
        {
            var t1 = new Toggle("t1", true);
            var t2 = new Toggle("T1", false);

            var isEqual = Toggle.Equals(t1, t2);

            Assert.True(isEqual);
        }

        [Fact]
        [Trait("UnitTest", "Toggle")]
        public void Toggle_Equals_False()
        {
            var t1 = new Toggle("t1", true);
            object t2 = new Toggle("t2", true);

            var isEqual = t1.Equals(t2);

            Assert.False(isEqual);
        }

        [Theory]
        [MemberData(nameof(ToggleNullComparison))]
        [Trait("UnitTest", "Toggle")]
        public void ToggleWithNullsEqualsFalse(Toggle t1, Toggle t2)
        {
            var isEqual = Toggle.Equals(t1, t2);

            Assert.False(isEqual);
        }

        private static IEnumerable<object[]> ToggleNullComparison()
        {
            yield return new object[] { null, new Toggle("t2", true) };
            yield return new object[] { new Toggle("t1", true), null };
        }
    }
}
