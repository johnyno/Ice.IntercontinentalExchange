using IntercontinentalExchange.Domain.Contracts;
using IntercontinentalExchange.Domain.Contracts.Configurations;
using IntercontinentalExchange.Infrastructure.Services;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace IntercontinentalExchange.UnitTests
{
    public class DownloadNoaaFileServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task DownloadNoaaFileService_FileNotExistNotReuse_CallDownloadingServiceWithCorrectParameters()
        {
            var rootUrl = @"https://noaa-gfs-bdp-pds.s3.amazonaws.com";
            var fileUrlPattern = @"gfs.{0}{1}{2}/00/atmos";
            var fileNamePattern = "gfs.t00z.pgrb2.0p25.f{0}";
            var date = new DateTime(2021, 10, 7, 3, 0, 0);
            var folderPath = "c:\\temp";
            var reuseDownloadedFiles = false;

            var expectedFullFileUrl =
                "https://noaa-gfs-bdp-pds.s3.amazonaws.com/gfs.20211007/00/atmos/gfs.t00z.pgrb2.0p25.f003";
            var expectedFullFileName = "c:\\temp\\gfs.t00z.pgrb2.0p25.f003";

            var autoMocker = new AutoMocker();
            var configMock = autoMocker.GetMock<IDownloadNoaaFileServiceConfig>();
            configMock.Setup(x => x.RootUtl).Returns(rootUrl);
            configMock.Setup(x => x.FileUrlPattern).Returns(fileUrlPattern);
            configMock.Setup(x => x.FileNamePattern).Returns(fileNamePattern);

            var downloaderMock = autoMocker.GetMock<IDownloadFileService>();
            downloaderMock.Setup(x => x.IsFileExist(It.IsAny<string>())).Returns(false);

            var downloadNoaaFileService = autoMocker.CreateInstance<DownloadNoaaFileService>();
            await downloadNoaaFileService.DownloadFile(date, folderPath, reuseDownloadedFiles);

            autoMocker.GetMock<IDownloadFileService>().Verify(x=>x.DownloadFile(expectedFullFileUrl,expectedFullFileName),Times.Once);
        }

        [Test]
        public async Task DownloadNoaaFileService_FileExistNotReuse_NotCallDownloadingService()
        {
            var rootUrl = @"https://noaa-gfs-bdp-pds.s3.amazonaws.com";
            var fileUrlPattern = @"gfs.{0}{1}{2}/00/atmos";
            var fileNamePattern = "gfs.t00z.pgrb2.0p25.f{0}";
            var date = new DateTime(2021, 10, 7, 3, 0, 0);
            var folderPath = "c:\\temp";
            var reuseDownloadedFiles = false;

            var expectedFullFileUrl =
                "https://noaa-gfs-bdp-pds.s3.amazonaws.com/gfs.20211007/00/atmos/gfs.t00z.pgrb2.0p25.f003";
            var expectedFullFileName = "c:\\temp\\gfs.t00z.pgrb2.0p25.f003";

            var autoMocker = new AutoMocker();
            var configMock = autoMocker.GetMock<IDownloadNoaaFileServiceConfig>();
            configMock.Setup(x => x.RootUtl).Returns(rootUrl);
            configMock.Setup(x => x.FileUrlPattern).Returns(fileUrlPattern);
            configMock.Setup(x => x.FileNamePattern).Returns(fileNamePattern);

            var downloaderMock = autoMocker.GetMock<IDownloadFileService>();
            downloaderMock.Setup(x => x.IsFileExist(It.IsAny<string>())).Returns(true);

            


            var downloadNoaaFileService = autoMocker.CreateInstance<DownloadNoaaFileService>();
            await downloadNoaaFileService.DownloadFile(date, folderPath, reuseDownloadedFiles);

            autoMocker.GetMock<IDownloadFileService>().Verify(x => x.DownloadFile(expectedFullFileUrl, expectedFullFileName), Times.Once);
        }

        [Test]
        public async Task DownloadNoaaFileService_FileExistReuse_NotCallDownloadingService()
        {
            var rootUrl = @"https://noaa-gfs-bdp-pds.s3.amazonaws.com";
            var fileUrlPattern = @"gfs.{0}{1}{2}/00/atmos";
            var fileNamePattern = "gfs.t00z.pgrb2.0p25.f{0}";
            var date = new DateTime(2021, 10, 7, 3, 0, 0);
            var folderPath = "c:\\temp";
            var reuseDownloadedFiles = true;

            var expectedFullFileUrl =
                "https://noaa-gfs-bdp-pds.s3.amazonaws.com/gfs.20211007/00/atmos/gfs.t00z.pgrb2.0p25.f003";
            var expectedFullFileName = "c:\\temp\\gfs.t00z.pgrb2.0p25.f003";

            var autoMocker = new AutoMocker();
            var configMock = autoMocker.GetMock<IDownloadNoaaFileServiceConfig>();
            configMock.Setup(x => x.RootUtl).Returns(rootUrl);
            configMock.Setup(x => x.FileUrlPattern).Returns(fileUrlPattern);
            configMock.Setup(x => x.FileNamePattern).Returns(fileNamePattern);

            var downloaderMock = autoMocker.GetMock<IDownloadFileService>();
            downloaderMock.Setup(x => x.IsFileExist(It.IsAny<string>())).Returns(true);




            var downloadNoaaFileService = autoMocker.CreateInstance<DownloadNoaaFileService>();
            await downloadNoaaFileService.DownloadFile(date, folderPath, reuseDownloadedFiles);

            autoMocker.GetMock<IDownloadFileService>().Verify(x => x.DownloadFile(expectedFullFileUrl, expectedFullFileName), Times.Never);
        }
    }
}