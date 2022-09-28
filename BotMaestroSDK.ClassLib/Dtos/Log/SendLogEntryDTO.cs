
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCityMaestroSDK.Dtos.Task
{
    public class SendLogEntryDTO
    {
        public string Page { get; set; }
        public string Size { get; set; }
        public string activityLabel { get; set; }

        public List<string> Sort { get; set; }

        public SendLogEntryDTO()
        {
            Sort = new List<string>();
        }
    }

    
}
