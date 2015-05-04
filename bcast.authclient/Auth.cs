using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace bcast.authclient
{
    public static class Auth
    {
        public static string Check(string Name, string Password)
        {
            try
            {
                return CallAuth("check", Name, Password);
            }
            catch(WebException wex)
            {
                if(wex.Status == WebExceptionStatus.ProtocolError)
                {
                    var resp = wex.Response as HttpWebResponse;
                    if(resp!=null && (resp.StatusCode == HttpStatusCode.Unauthorized || resp.StatusCode == HttpStatusCode.Forbidden))
                    {
                        return null;
                    }
                }
                throw new Exception("Error while authenticating user", wex);
            }
            catch(Exception ex)
            {
                throw new Exception("Error while authenticating user", ex);
            }
        }

        public static string Create(string Name, string Password)
        {
            return CallAuth("create", Name, Password);
        }

        public static string Reset(string Name, string Password)
        {
            return CallAuth("reset", Name, Password);
        }

        private static string CallAuth(string method, string name, string password)
        {
            var url = ConfigurationManager.AppSettings["bcast.authclient.authserver"];
            if (String.IsNullOrWhiteSpace(url))
                throw new Exception("Auth server not configured");
            url += (url.EndsWith("/") ? "" : "/") + method;
            var creds = "{ " + String.Format("\"Name\" : \"{0}\", \"Password\" : \"{1}\"", name, password) + " }";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            var s = request.GetRequestStream();
            var body = new ASCIIEncoding().GetBytes(creds);
            s.Write(body, 0, body.Length);
            var resp = request.GetResponse();
            var rs = resp.GetResponseStream();
            var rsbody = (new StreamReader(rs)).ReadToEnd();
            rs.Close();
            return rsbody.Replace("\"", "");
        }

    }
}
