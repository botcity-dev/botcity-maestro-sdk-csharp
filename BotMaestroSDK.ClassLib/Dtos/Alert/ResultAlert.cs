using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCityMaestroSDK.Dtos.Alert
{
    public class ResultAlert
    {
        
        [JsonProperty("id")]
        public string Id { get; set; }
        public string OrganizationLabel { get; set; }
        public string ActivityName { get; set; }
        public string BotId { get; set; }
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int? Days { get; set; }

        private AlertType _AlertType;
        public AlertType AlertType
        {

            get { return _AlertType; }

            set
            {
                _AlertType = value;
                var enumValue = (AlertType)value;
                //AlertType = enumValue;
                Type = enumValue.ToString();
            }
        }
    }
}
