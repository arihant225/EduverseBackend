using Azure;
using Azure.Core;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Reflection.PortableExecutable;

namespace Eduverse.Backend.WebApi.Services
{
    public class UploadDownloadService
    {

        static string Path = @"./server/";


        public static string upload(IFormFile fileItem) {
            string fileName=Guid.NewGuid().ToString();
            string fileType = fileItem.FileName.Split(".")[fileItem.FileName.Split(".").Length-1];
            string path=Path+fileName+"."+fileType;
            using (FileStream fs = new FileStream(path,FileMode.CreateNew)) {
                fileItem.CopyTo(fs);
                fs.Close();
                return fileName + "." + fileType;
            }
        }
      
        public static MemoryStream? GetDocument(string path) { 
            try
            {
            string[] name = path.Split(".");
                path = Path + path;
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                    return null; // Additional check for invalid paths

                byte[] bytes= File.ReadAllBytes(path);
                MemoryStream stream = new MemoryStream(bytes);
                return stream;
            }
            catch (Exception ex)
            {
                // Log or handle the exception for debugging purposes
                Console.WriteLine($"Exception: {ex}");
                return null;
            }
        }


        public static string GetContentType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".txt":
                    return "text/plain";
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                // Add more cases for other file extensions as needed

                default:
                    return "application/octet-stream"; // Default content type for unknown file types
            }
        }

    }
   
}
