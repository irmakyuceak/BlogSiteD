namespace BlogSite.Helpers
{
    public static class FileHelper
    {
        public static async Task<string> UploadFileAsync(IFormFile file, string webRootPath, string folderName = "Articles")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Dosya geçersiz.");

            string uploadPath = Path.Combine(webRootPath, folderName);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }
    }
}
