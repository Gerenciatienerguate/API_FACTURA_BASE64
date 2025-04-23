namespace API_CHATBOT_PDF.Interfaces
{
    public interface IFileManagerService
    {

        Task<Byte[]> GetFileBytesArray(string path);

        string GetBase64(byte[] fileBytesArray);
    }
}
