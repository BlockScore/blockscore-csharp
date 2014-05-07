using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace BlockScoreAPI
{
    public class Utility
    {
        private static Dictionary<string, object> NvcToDictionary(NameValueCollection nvc, bool handleMultipleValuesPerKey)
        {
            var result = new Dictionary<string, object>();
            foreach (string key in nvc.Keys)
            {
                if (handleMultipleValuesPerKey)
                {
                    string[] values = nvc.GetValues(key);
                    if (values.Length == 1)
                    {
                        result.Add(key, values[0]);
                    }
                    else
                    {
                        result.Add(key, values);
                    }
                }
                else
                {
                    result.Add(key, nvc[key]);
                }
            }

            return result;
        }

        public static string ConvertToJsonRequest(NameValueCollection postData)
        {
            var dictionary = NvcToDictionary(postData, true);
            var jsonRequest = JsonConvert.SerializeObject(dictionary);
            var request = jsonRequest.Replace(@"\", "").Replace(@":""[", @": [").Replace(@"]""", "]");
            return request;
        }

    }
}
