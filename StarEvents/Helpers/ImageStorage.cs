using System;
using System.Configuration;
using System.IO;
using System.Web;
using Imagekit;

namespace StarEvents.Helpers
{
    public static class ImageStorage
    {
        private static readonly Imagekit.Imagekit _client;

        static ImageStorage()
        {
            var publicKey = ConfigurationManager.AppSettings["ImageKit.PublicKey"];
            var privateKey = ConfigurationManager.AppSettings["ImageKit.PrivateKey"];
            var urlEndpoint = ConfigurationManager.AppSettings["ImageKit.UrlEndpoint"];
            var uploadFolder = ConfigurationManager.AppSettings["ImageKit.UploadFolder"] ?? "/";

            _client = new Imagekit.Imagekit(publicKey, privateKey, urlEndpoint, uploadFolder);
        }

        public static string UploadHttpFile(HttpPostedFileBase file, string folder)
        {
            if (file == null || file.ContentLength <= 0) return null;

            using (var ms = new MemoryStream())
            {
                file.InputStream.CopyTo(ms);
                var bytes = ms.ToArray();
                var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);

                var result = _client.Upload(new FileCreateRequest
                {
                    file = Convert.ToBase64String(bytes),
                    fileName = fileName,
                    folder = folder
                });

                return result.url;
            }
        }

        public static string UploadBytes(byte[] bytes, string fileName, string folder)
        {
            if (bytes == null || bytes.Length == 0) return null;

            var result = _client.Upload(new FileCreateRequest
            {
                file = Convert.ToBase64String(bytes),
                fileName = fileName,
                folder = folder
            });

            return result.url;
        }
    }
}


