using System.Net;

namespace AzureDataLake.Authentication
{
    public class Directory
    {
        public string Name;
        public string TenantId;

        private Directory()
        {
            
        }

        public static Directory Resolve(string name)
        {
            var d = new Directory();
            d.Name = name;

            
            string url = "https://login.windows.net/"+ name + "/.well-known/openid-configuration";

            var request = System.Net.WebRequest.Create(url);

            ((HttpWebRequest)request).UserAgent = "Mozilla/5.0 (Windows NT; Windows NT 10.0; en-US) WindowsPowerShell/5.1.14393.0";


            var response = (System.Net.HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new System.ArgumentException();
            }

            var dataStream = response.GetResponseStream();

            var reader = new System.IO.StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();


            reader.Close();
            dataStream.Close();
            response.Close();

            if (response.Headers["Content-Type"] == "text/html")
            {
                throw new System.ArgumentException();
            }

            var j0 = Newtonsoft.Json.Linq.JObject.Parse(responseFromServer);

            var j1 = j0.SelectToken("authorization_endpoint");
            var j2 = j0.SelectToken("token_endpoint");

            var tokens = j2.ToString().Split('/');

            d.TenantId = tokens[3];

            return d;
        }
    }
}