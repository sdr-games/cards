using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.Localization.Settings;

namespace SDRGames.Whist.LocalizationModule.Models
{
    [Serializable]
    public class LocalizedString
    {
        [field: SerializeField] public UnityEngine.Localization.LocalizedString Entity { get; private set; }

        private Dictionary<string, object> _params;

        public LocalizedString(UnityEngine.Localization.LocalizedString entity)
        {
            Entity = entity;
        }

        public static string GetLocalizedString(string tableName, string indexName)
        {
            string result;
            try
            {
                result = LocalizationSettings.StringDatabase.GetLocalizedString(tableName, indexName);
            }
            catch (Exception ex)
            {
                result = "";
            }
            return result;
        }

        public string GetLocalizedText()
        {
            string result = Entity.GetLocalizedString();
            if(_params != null && _params.Count > 0)
            {
                Match match = Regex.Match(result, @"\{(.*?)\}");
                if(_params.ContainsKey(match.Groups[1].Value))
                {
                    result = result.Replace(match.Groups[0].Value, _params[match.Groups[1].Value].ToString());
                }
            }
            return result;
        }

        public void SetParam(string paramName, object paramValue)
        {
            if(_params == null)
            {
                _params = new Dictionary<string, object>();
            } 

            if(_params.ContainsKey(paramName))
            {
                _params[paramName] = paramValue;
                return;
            }
            _params.Add(paramName, paramValue);
        }
    }
}
