﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace qr_backend.Helpers
{
    public static class StringHelper
    {
        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str, "[^A-Z_ ]+", "", RegexOptions.Compiled);
        }

        public static string GetJsonFromFile(string fileName)
        {
            return System.IO.File.ReadAllText($"./MockData/{fileName}.json");
        }

        public static bool IsPropertyExist(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            return settings.GetType().GetProperty(name) != null;
        }
    }
}
