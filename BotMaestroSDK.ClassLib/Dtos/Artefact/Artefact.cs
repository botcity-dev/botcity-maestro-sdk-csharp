using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCityMaestroSDK.Dtos.Artefact
{
    public class Artefact
    {
        public int id { get; set; }
        public string Type { get; set; }
        public int?  taskId { get; set; }
        public string Name { get; set; }
        public string fileName { get; set; }
        public string storageFileName { get; set; }
        public string storageFilePath { get; set; }
        public int? organizationId { get; set; }
        public int? userId { get; set; }
        public DateTime? dateCreation { get; set; }
        public string taskName { get; set; }
        public int? days { get; set; }

    }
}
