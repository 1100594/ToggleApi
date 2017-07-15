using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ToggleApi.Models;
using static ToggleApi.Utilities.Utils;

namespace ToggleApi.Utilities
{
    public class ToggleClientParser : IToggleClientParser
    {
        //Example: {a-d1_5:11.*}
        private const string Pattern = @"\{([A-Za-z\d_-]*:[\d|*]+(\.[\d|*]){0,3}[,]?)+}|\[\^([A-Za-z\d_-]*:[\d|*]+(\.[\d|*]){0,3}[,]?)+]";
        private const string OverrideSymbol = "^";
        private const char ClientVersionSeparator = ':';
        private const char ClientsSeparator = ',';

        public string Input { get; set; }

        //TODO: Review this 
        public bool ToggleValue { get; set; }

        public bool IsValid()
        {
            ThrowOnNullArgument(Input, nameof(Input));

            return Regex.IsMatch(Input, Pattern);
        }

        public void Extract(out ICollection<Client> whitelist, out IDictionary<Client, bool> customValues)
        {
            ThrowOnNullArgument(Input, nameof(Input));
            ThrowOnNullArgument(ToggleValue, nameof(ToggleValue));
            whitelist = new List<Client>();
            customValues = new Dictionary<Client, bool>();

            var clientsMatches = Regex.Matches(Input, Pattern);

            foreach (Match match in clientsMatches)
            {
                for (int ctr = 1; ctr <= match.Groups.Count - 1; ctr++)
                {
                    foreach (Capture caputure in match.Groups[ctr].Captures)
                    {
                        //TODO Review this line
                        var clientProperties = caputure.Value.Split(ClientVersionSeparator).ToList().Select(v => v.TrimEnd(ClientsSeparator)).ToList();

                        //TODO Handle errors
                        if (clientProperties == null || clientProperties.Count != 2) continue;

                        var clientApi = new Client(clientProperties.First(), clientProperties.Last());
                        if (match.Value.Contains(OverrideSymbol))
                        {
                            customValues.Add(clientApi, !ToggleValue);
                        }
                        else
                        {
                            whitelist.Add(clientApi);
                        }

                    }
                }
               
            }
        }


    }
}
