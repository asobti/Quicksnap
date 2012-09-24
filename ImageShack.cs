using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bamsak
{
    class ImageShackPlugin : IQuickSnapPlugin
    {
        private const String key = "4CMOPRVXa244eb34ba6c335d785bcf0ff5e6f472";
        
        private String link;

        public ImageShackPlugin()
        {
            this.link = String.Empty;
        }

        public string getUrl()
        {
            return this.link;
        }

        public void upload(CroppedBitmap image)
        {
            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(image));
            String uniqid = Guid.NewGuid().ToString();
            FileStream stream = new FileStream(uniqid, FileMode.Create);
            pngEncoder.Save(stream);
            stream.Close();

            String response = PostToImageShack(uniqid);
            parseResponse(response);
        }

        private String PostToImageShack(String imageFilePath)
        {
            string boundary = "---------FormBoundary-sadkd22dasd---------";

            byte[] imageData;

            FileStream fileStream = File.OpenRead(imageFilePath);
            imageData = new byte[fileStream.Length];
            fileStream.Read(imageData, 0, imageData.Length);
            fileStream.Close();

            const int MAX_URI_LENGTH = 32766;
            string base64img = System.Convert.ToBase64String(imageData);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < base64img.Length; i += MAX_URI_LENGTH)
            {
                sb.Append(Uri.EscapeDataString(base64img.Substring(i, Math.Min(MAX_URI_LENGTH, base64img.Length - i))));
            }
            string uploadRequestString = String.Empty;

            string template = "\r\n\r\n" + boundary + "\r\n" +
                           "Content-Disposition: form-data; name=\"{0}\";" + "\r\n\r\n" +
                           "{1}";

            uploadRequestString += string.Format(template, "key", key);
            uploadRequestString += string.Format(template, "type", "base64");

            uploadRequestString += string.Format("\r\n\r\n" + boundary + "\r\n" +
                        "Content-Disposition: form-data; name=\"{0}\"; filename=\"Icon128.gif\" \r\n" +
                        "Content-Type=image/gif \r\n\r\n" +
                        "{1}", "fileupload", base64img);
            uploadRequestString += "\r\n\r\n" + boundary + "--";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://www.imageshack.us/upload_api.php");
            webRequest.Method = "POST";
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webRequest.ServicePoint.Expect100Continue = false;            

            StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream());
            streamWriter.Write(uploadRequestString);
            streamWriter.Close();

            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseStream);

            string responseString = responseReader.ReadToEnd();
            return responseString;
        }

        private void parseResponse(String response)
        {
            String r = response;
        }
    }
}
