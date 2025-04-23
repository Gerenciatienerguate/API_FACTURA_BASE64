using API_CHATBOT_PDF.Interfaces;

namespace API_CHATBOT_PDF.Services
{
    public class FileManagerService : IFileManagerService
    {
        public string GetBase64(byte[] fileBytesArray)
        {
            return Convert.ToBase64String(fileBytesArray);
        }

        public async Task<byte[]> GetFileBytesArray(string path)
        {

            int counter = 0;
            do
            {
                await Task.Delay(1000);
                counter++;
            }
            while (!File.Exists(path) && counter < 5);
            if (counter >= 5) throw new Exception("No fue posible encontrar el archivo indicado.");

            return await File.ReadAllBytesAsync(path);
        }

    }
}
