using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.DTOs.ImportDTOs
{
    public class ImportInfo
    {
        public ImportInfo(string importKey, object importData)
        {
            ImportKey = importKey;
            ImportData = importData;
        }
        public string ImportKey { get; set; }
        public object ImportData { get; set; }
    }
}
