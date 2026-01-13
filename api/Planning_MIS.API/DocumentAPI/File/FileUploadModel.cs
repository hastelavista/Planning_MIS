using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Planning_MIS.DocumentAPI.File
{
    public class FileUploadModel
    {
        public string MasterId { get; set; }
        public string DataId { get; set; }
        public int FileTypeId { get; set; }
        public IFormFile File { get; set; }
        public string CreatedBy { get; set; }
    }
}
