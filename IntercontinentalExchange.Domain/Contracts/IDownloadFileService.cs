using System.Threading.Tasks;

namespace IntercontinentalExchange.Domain.Contracts
{
    public interface IDownloadFileService
    {
        Task DownloadFile(string url, string folder);

        public bool IsFileExist(string path);

    }
}
