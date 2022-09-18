using BotCityMaestroSDK.Dtos.Alert;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCityMaestroSDK.Dtos.Message
{
    public class SendMessage
    {
        [JsonProperty("emails")]
        public List<string> Emails { get; set; }

        [JsonProperty("logins")]
        public List<string> Logins { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        private TypeMail _TypeMail;
        public TypeMail TypeMail
        {

            get { return _TypeMail; }

            set
            {
                _TypeMail = value;
                var enumValue = (TypeMail)value;
                //AlertType = enumValue;
                Type = enumValue.ToString();
            }
        }

        public SendMessage()
        {
            Logins = new List<string>();
            Emails = new List<string>();
        }

    }
    public enum TypeMail
    {
        HTML,
        TEXT
    }
}
