using System.Globalization;

namespace Eduverse.Backend.WebApi.Services
{
    public class UploadDownloadService
    {

        static string Path = @"./server/";


        public string upload(IFormFile fileItem) {
            string fileName=Guid.NewGuid().ToString();
            string fileType = fileItem.FileName.Split(".")[fileItem.FileName.Split(".").Length-1];
            string path=Path+fileName+"."+fileType;
            using (FileStream fs = new FileStream(path,FileMode.CreateNew)) {
                fileItem.CopyTo(fs);
                fs.Close();
                return path;
            }
        }
        
    }
}
