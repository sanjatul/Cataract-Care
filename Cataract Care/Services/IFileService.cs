namespace Cataract_Care.Services
{
    public interface IFileService
    {
        Tuple<int, string> SaveImage(IFormFile imageFile, string imgPath= "Uploads");
        public bool DeleteImage(string imageFileName);
    }
}
