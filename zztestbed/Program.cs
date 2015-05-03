using bcast.authclient;
using bcast.common;
using bcast.data.sqlserver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zztestbed
{
    class Program
    {
        static void Main(string[] args)
        {
            //bcastdata ctx = new bcastdata();
            //ctx.Account.Add(new bcast.common.Account() { Name = "blah", Locked = false });
            //ctx.SaveChanges();

            //var da = new DataAccessImpl();

            //var endp = new Endpoint()
            //{
            //    Name = "blah.blee",
            //    Type = EndpointType.Android,
            //    Location = "mail://4252468456@tmomail.com"
            //};

            //var endp = da.getEndpoint("blah.blee");
            //endp.Enabled = true;


            //da.saveEndpoint(endp);

            //var token = Auth.Create("blah2", "blee");

            //var token2 = Auth.Check("blah", "blee");

            //Random r = new Random((DateTime.Now - new DateTime(2015, 04, 18)).Seconds);

            //var sb = new StringBuilder();

            //for (var i = 0; i < 10; i++)
            //{
            //    sb.Append((Char)r.Next(97, 122));
            //}
            //Console.WriteLine(sb.ToString());

            Uri pull = new Uri("pull://");

            Uri pull2 = (new UriBuilder("pull", "")).Uri;

            Console.ReadLine();
        }
    }
}
