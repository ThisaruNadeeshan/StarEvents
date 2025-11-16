using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;

namespace StarEvents.Helpers
{
    public static class ImageStorage
    {
        private static readonly string _privateKey;
        private static readonly string _urlEndpoint;
        private static readonly string _defaultFolder;

        static ImageStorage()
        {
            _privateKey = ConfigurationManager.AppSettings["ImageKit.PrivateKey"];
            _urlEndpoint = ConfigurationManager.AppSettings["ImageKit.UrlEndpoint"]?.TrimEnd('/');
            _defaultFolder = ConfigurationManager.AppSettings["ImageKit.UploadFolder"] ?? "/starevents";
        }

        // Public helper used by controllers for HttpPostedFileBase
        public static string UploadHttpFile(HttpPostedFileBase file, string folder)
        {
            if (file == null || file.ContentLength <= 0) return null;

            using (var ms = new MemoryStream())
            {
                file.InputStream.CopyTo(ms);
                var bytes = ms.ToArray();
                var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                return UploadBytes(bytes, fileName, folder);
            }
        }

        // Public helper used for QR code bitmaps etc.
        public static string UploadBytes(byte[] bytes, string fileName, string folder)
        {
            if (bytes == null || bytes.Length == 0) return null;

            var targetFolder = string.IsNullOrWhiteSpace(folder) ? _defaultFolder : folder;

            // ImageKit upload endpoint (not the URL endpoint/CDN)
            var uploadUrl = "https://upload.imagekit.io/api/v1/files/upload";

            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                // Basic auth: privateKey:
                var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(_privateKey + ":"));
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", authToken);

                // ImageKit expects "file" as base64 or URL; here we send base64
                var base64 = Convert.ToBase64String(bytes);

                content.Add(new StringContent(base64), "file");
                content.Add(new StringContent(fileName), "fileName");
                content.Add(new StringContent(targetFolder), "folder");

                var response = client.PostAsync(uploadUrl, content).Result;
                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().Result;
                var obj = JObject.Parse(json);

                // Response has "url" (full CDN URL) and "filePath"
                var url = (string)obj["url"];
                return url;
            }
        }
    }
}