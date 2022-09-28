
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
    public class ResultLogDTO
    {
        public string id { get; set; }
        public string organizationLabel { get; set; }
        public string activityLabel { get; set; }

        public List<Column> columns { get; set; }

        public ResultLogDTO()
        {
            columns = new List<Column>();
        }

        public void ColumAdd(string Name, string Label, int Width)
        {
            Column param = new Column
            {
                Name = Name,
                Label = Label,
                Width = Width

            };
            columns.Add(param);
        }

        public void ColumnClear(string Name, string Value)
        {
            columns = new List<Column>();
        }

        public void ColumnDelete(string Name)
        {

            foreach (Column param in columns)
            {
                if (param.Name == Name)
                {
                    columns.Remove(param);
                    break;
                }
            }

        }

    }

}
