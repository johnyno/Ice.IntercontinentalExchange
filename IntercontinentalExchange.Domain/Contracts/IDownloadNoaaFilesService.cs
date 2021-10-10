using System;
using System.Threading.Tasks;

namespace IntercontinentalExchange.Domain.Contracts
{
    public interface IDownloadNoaaFilesService
    {
        public Task<string> DownloadFile(DateTime date,string folderPath, bool reuseDownloadedFiles = true);
    }
}
