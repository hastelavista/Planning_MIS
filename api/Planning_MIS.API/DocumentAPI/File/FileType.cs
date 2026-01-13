using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planning_MIS.DocumentAPI.File
{
    public class FileType
    {
        public int Id { get; set; }
        public string FormType { get; set; }
        public string FormSubType { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public bool AllowMultiple { get; set; }
        public int MaxFileSize { get; set; }
        public string Location { get; set; }
        public bool IsRequired { get; set; }

        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }

        public List<FileUploadResult> Files { get; set; } = new List<FileUploadResult>();
    }
}
