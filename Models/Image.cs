using Microsoft.AspNetCore.Http;
using System.IO;

namespace DBBMVCWebApp.Models
{
    class Image {

        public byte[] Data { get; set; }

        public Image(IFormFile file)
        {
            Data = GetByteArrayFromImage(file);
        }

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }
    }

}

