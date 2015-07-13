using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ColorFlow.Models
{
    public class ImageData
    {
        public byte[] Image { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}