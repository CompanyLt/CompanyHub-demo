using Darbu_records.Formos;
using Darbu_records.Models;

namespace Darbu_records.DAL
{
    public class FileCommunicationService
    {

        string[] allowedPhotoExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        string allowedPdfExtensions =".pdf"; 

   



        public bool checkPdfExtention(string fileName)
        {
           // string[] allowedPhotoExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
          
          //  string[] allowedPhotoExtensions = { ".pdf"};
            string ext = Path.GetExtension(fileName).ToLowerInvariant();         
            return allowedPdfExtensions.Contains(ext);

           // return Path.GetExtension(fileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase);
            
          
        }

        public bool checkImageExtention(string fileName)
        {
          
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            return allowedPhotoExtensions.Contains(ext);

            // return Path.GetExtension(fileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase);


        }

        public void checkDirection(string path)
        {

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

        }

        public async Task filesSorter(IEnumerable<IFormFile>? filesCollection,FileForm fileform,string fileDirectory)
        {           
            foreach (var temp in filesCollection)
            {
                if (checkImageExtention(temp.FileName))
                {
                   

                    string name = (Guid.NewGuid().ToString() + temp.FileName);
                    string originalName = Path.GetFileNameWithoutExtension(temp.FileName);
                    RecordFile recordFile = new RecordFile()
                    {
                      name=name,
                      originalName=originalName,
                      directory = fileDirectory                     
                    };

                    //cia sujungiam i kelia
                    string kelias_image = Path.Combine(fileDirectory, name);

                    fileform.photosCollection.Add(recordFile);
                  //  images.Add(name);
                    //sukuriam filestream sukuriam faila 
                    using (FileStream file_create = new FileStream(kelias_image, FileMode.Create))
                    {

                        await temp.CopyToAsync(file_create);

                    }

                }
                else
                {
                    string name = (Guid.NewGuid().ToString() + temp.FileName);
                    string originalName = Path.GetFileNameWithoutExtension (temp.FileName);
                    RecordFile recordFile = new RecordFile()
                    {
                        name = name,
                        originalName=originalName,
                        directory = fileDirectory

                    };




                    //cia sujungiam i kelia
                    string kelias_image = Path.Combine(fileDirectory, name);
                    fileform.filesCollection.Add(recordFile);
                    //sukuriam filestream sukuriam faila 
                    using (FileStream file_create = new FileStream(kelias_image, FileMode.Create))
                    {

                        await temp.CopyToAsync(file_create);

                    }





                }

            }
        }


        public async Task<RecordFile> CreateFile(IFormFile? formFile, string fileDirectory)
        {

            if (!checkImageExtention(formFile.FileName))
            {
                return new RecordFile();
            }


                    string name = (Guid.NewGuid().ToString() + formFile.FileName);
                    string originalName = Path.GetFileNameWithoutExtension(formFile.FileName);
                    RecordFile recordFile = new RecordFile()
                    {
                        name = name,
                        originalName = originalName,
                        directory = fileDirectory
                    };

                    //cia sujungiam i kelia
                    string kelias_image = Path.Combine(fileDirectory, name);
                 
                    using (FileStream file_create = new FileStream(kelias_image, FileMode.Create))
                    {

                        await formFile.CopyToAsync(file_create);

                    }

               
                    return recordFile;
            
        }












    }
}
