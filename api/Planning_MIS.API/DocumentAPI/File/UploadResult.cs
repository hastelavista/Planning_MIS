using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planning_MIS.DocumentAPI.File
{
    public class UploadResult
    {
        public string Message { get; set; }
        public List<FileUploadResult> Results { get; set; } = new List<FileUploadResult>();
    }
}
