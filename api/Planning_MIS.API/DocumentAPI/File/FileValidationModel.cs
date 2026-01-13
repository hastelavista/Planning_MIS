using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planning_MIS.DocumentAPI.File
{
    public class FileValidationModel
    {
        public int ApplicationId { get; set; }
        public string DataId { get; set; }
        public List<string> FormTypes { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
    }
}
