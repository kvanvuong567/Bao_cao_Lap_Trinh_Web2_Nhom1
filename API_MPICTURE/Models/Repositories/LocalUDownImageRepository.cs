using API_MPICTURE.Data;
using API_MPICTURE.Models.Interfaces;
using System.IO;
namespace API_MPICTURE.Models.Services
{
    public class LocalUDownImageRepository : IUDonwImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;

        public LocalUDownImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public UpDownImage Upload(UpDownImage upDownImage)
        {
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "UpDownImage",
                $"{upDownImage.FileName}{upDownImage.FileExtension}");

            // Upload UpDownImages to Local Path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            upDownImage.File.CopyTo(stream);

            // https://localhost:8080/UpDownImage/UpDownImage.jpg
            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/UpDownImage/{upDownImage.FileName}{upDownImage.FileExtension}";
            upDownImage.FilePath = urlFilePath;

            // Add Image to the UpDownImages table
            _dbContext.UpDownImages.Add(upDownImage);
            _dbContext.SaveChanges();

            return upDownImage;
        }

        public List<UpDownImage> GetAllInfoImages()
        {
            var allImages = _dbContext.UpDownImages.ToList();
            return allImages;
        }
        public (byte[], string, string) DownloadFile(int Id)
        {
            try
            {
                var fileById = _dbContext.UpDownImages.Where(x => x.Id == Id).FirstOrDefault();
                if (fileById == null)
                {
                    throw new FileNotFoundException("File not found.");
                }

                var path = Path.Combine(_webHostEnvironment.ContentRootPath, "UpDownImage", $"{fileById.FileName}{fileById.FileExtension}");
                var stream = File.ReadAllBytes(path);
                var fileName = fileById.FileName + fileById.FileExtension;
                return (stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
