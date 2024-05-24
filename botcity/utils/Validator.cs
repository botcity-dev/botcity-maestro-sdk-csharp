using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Dev.BotCity.MaestroSdk.Utils {

    public static class Validator {
        public static string ValidateDateTime(DateTime? value, string argumentName){
            if (value.HasValue) {
                if (!(value.Value is DateTime)) {
                    throw new ArgumentException($"Argument '{argumentName}' is not a DateTime. Type: {value.GetType()}");
                }
                return value.Value.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            return null;
        }
    }
    public static class HttpContentFactory {
        public static StringContent CreateJsonContent(object data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            return new StringContent(jsonData, Encoding.UTF8, "application/json");
        }
    }

    public static class HttpClientExtension {
        public static void AddDefaultHeaders(this HttpClient client, string token, string organization, int timeout) {
            {
                client.Timeout = new TimeSpan(0, 0, 0, timeout);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("organization", organization);
            }
        }
    }
}