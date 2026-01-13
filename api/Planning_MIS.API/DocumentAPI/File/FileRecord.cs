using System;
using System.Linq;

namespace Planning_MIS.DocumentAPI.File
{
    public class FileRecord
    {
        public Guid Id { get; set; }
        public int FileTypeId { get; set; }
        public string DataId { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileMimeType { get; set; }
        public string Location { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string DisplayCreatedDate
        {
            get
            {
                return CreatedDate.ToString("yyyy-MM-dd");
            }
        }

        public string FileId
        {
            get
            {
                return Id.ToString();
            }
        }

        public string Icon
        {
            get
            {
                string ext = System.IO.Path.GetExtension(FileName);
                string icon = string.Empty;

                if (new string[] { ".pdf" }.Contains(ext))
                {
                    icon = "images/icons/pdf_sm.png";
                }
                else if (new string[] { ".jpg,.png,.jpeg,.bmp,.gif" }.Contains(ext))
                {
                    icon = "images/icons/jpg_sm.png";
                }
                else
                {
                    icon = "images/icons/doc_sm.png";
                }

                return icon;
            }
        }
    }
}
