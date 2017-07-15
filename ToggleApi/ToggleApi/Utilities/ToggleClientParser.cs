using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ToggleApi.Models;

namespace ToggleApi.Utilities
{
    public class ToggleClientParser : IToggleClientParser
    {
        private const string ValidationPattern = @"\{([A-Za-z\d_-]*:[\d|*]+(\.[\d|*]){0,3}[,]?)+}|\[\^[^]]([,]?[A-Za-z\d_-]*:[\d|*]+(\.[\d|*]){0,3})+]";
        private const string ExtractionPattern = @"^(\{([^}]*)})*\[(\^)([^]]*)]";

        public string Input { get; set; }

        public bool IsValid()
        {
           Utils.ThrowOnNullArgument(Input, nameof(Input));

           return Regex.IsMatch(Input, ValidationPattern);
        }

        public void Extract(out ICollection<Client> whitelist, out IDictionary<Client, bool> customValues)
        {
            Utils.ThrowOnNullArgument(Input, nameof(Input));

            whitelist = new List<Client>();
            customValues = new Dictionary<Client, bool>();

            //TODO try to extract the whitelist and custom value
            var lists = Regex.Split(Input, ValidationPattern);
            //foreach (Match list in lists)
            //{
            //    var service = Regex.Split(list.ToString(), ExtractionPattern);
            //}
        }


    }
}
