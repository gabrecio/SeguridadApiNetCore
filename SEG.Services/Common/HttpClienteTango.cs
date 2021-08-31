using Microsoft.Extensions.Configuration;
using SEG.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Services.Common
{
    public class HttpClienteTango
    {
        private IConfigurationRoot _configuration;
        public HttpClienteTango()
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();
            _configuration = configuration;


        }

        public string GET(string endPoint)
        {
            var urlTango = _configuration.GetSection("AppSettings").Get<AppSettings>().ApiTango;
          //  var urlTango = ConfigurationManager.AppSettings["ApiTango"];
            Uri clientSecurity = new Uri(urlTango + endPoint);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(clientSecurity);
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";

            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

        // POST a JSON string
        public string POST(string endPoint, string jsonContent)
        {
            var urlTango = _configuration.GetSection("AppSettings").Get<AppSettings>().ApiTango;
            // var urlTango = ConfigurationManager.AppSettings["ApiTango"];
            Uri clientSecurity = new Uri(urlTango + endPoint);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(clientSecurity);
            request.ContentType = @"application/json";
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = jsonContent;

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            long length = 0;
            try
            {
                var httpResponse = (HttpWebResponse)request.GetResponse();
                var result = "0";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                
                return result; 
            }
            catch (WebException ex)
            {
                return "0";
            }
        }
    }
}
