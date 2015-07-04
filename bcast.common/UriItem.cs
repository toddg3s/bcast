using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    public class UriItem : Item
    {
        protected bool _valid = true;
        private Uri _uri;
        private string _contenttype;
        private byte[] _uricontents;

        public UriItem()
        {
            this.DataType = "uri";
        }

        public Uri Uri
        {
            get { return _uri ?? GetUriValue(); }
        }

        public string ContentType
        {
            get
            {
                if (_uricontents == null) GetContents(false);
                return _contenttype;
            }
            set { _contenttype = value; }
        }

        private System.Uri GetUriValue()
        {
            if (!_valid) return null;
            if (_uri != null) return _uri;
            _valid = Uri.TryCreate(this.Data, UriKind.RelativeOrAbsolute, out _uri);
            return _uri;
        }

        public void Navigate()
        {
            GetUriValue();
            if (_valid)
                Process.Start(this.Data);
        }
        public byte[] GetContents(bool force)
        {
            if (_uricontents != null && !force) return _uricontents;

            GetUriValue();
            if (!_valid) return null;

            var content = new MemoryStream();
            var webReq = (HttpWebRequest)WebRequest.Create(this.Data);
            using (WebResponse response = webReq.GetResponse())
            {
                _contenttype = response.ContentType;
                using (Stream responseStream = response.GetResponseStream())
                {
                    responseStream.CopyTo(content);
                }
            }
            _uricontents = content.ToArray();

            return _uricontents;
        }

    }
}
