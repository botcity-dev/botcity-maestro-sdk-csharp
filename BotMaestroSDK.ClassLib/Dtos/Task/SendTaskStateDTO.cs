using BotCityMaestroSDK.Dtos.Task;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCityMaestroSDK.Dtos.Task
{
    public class SendTaskStateDTO
    {
        public string state { get; set; }
        public string FinishStatus { get; set; }
        public string finishMessage { get; set; }
        public FinishedStatus SendStatus { get; set;}

        

    }

    public enum FinishedStatus
    {
        SUCCESS,
        FAILED,
        PARTIALLY_COMPLETED

    }
}
