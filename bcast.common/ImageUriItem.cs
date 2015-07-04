using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    public class ImageUriItem : UriItem
    {
        public ImageUriItem()
        {
            this.DataType = "imageuri";
        }

        public Image GetImage()
        {
            var bytes = GetContents(false);
            if (bytes==null || bytes.Length==0) return null;

            using(var s = new MemoryStream(bytes))
            {
                return Image.FromStream(s);
            }
        }
    }
}
