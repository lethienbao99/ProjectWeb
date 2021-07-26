using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.CommonModels
{
    public class ImageModel: BaseModel
    {
        public Guid? ProductID { get; set; }
        public string Caption { get; set; }
        public string FilePath { get; set; }
        public bool IsDefault { get; set; }
        public long FileSize { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
