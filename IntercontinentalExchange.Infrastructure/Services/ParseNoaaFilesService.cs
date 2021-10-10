using System;
using System.Diagnostics;
using System.Threading.Tasks;
using IntercontinentalExchange.Domain.Contracts;
using IntercontinentalExchange.Domain.Contracts.Configurations;

namespace IntercontinentalExchange.Infrastructure.Services
{
    internal class ParseNoaaFilesService : IParseNoaaFilesService
    {
        private readonly IAppLogger _logger;

        private readonly string _argumentsPattern;


        public ParseNoaaFilesService(IAppLogger logger, IParseNoaaFilesServiceConfig config)
        {
            _logger = logger;
            _argumentsPattern = config.ArgumentsPattern;
        }
        public Task<double> GetTemperature(double lon, double lat, string filePath, string parserLocation)
        {
            try
            {
                var arguments = string.Format(_argumentsPattern,filePath,lon,lat);
                using var process = new Process
                {
                    StartInfo =
                    {

                        FileName = parserLocation,
                        Arguments =
                            arguments,
                        CreateNoWindow = false,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                process.Start();

                process.WaitForExit(1000 * 10);     // (optional) wait up to 10 seconds

                var output = process.StandardOutput.ReadLine();
                if (output == null)
                    throw new Exception("Parsing output is null");

                var temperature = output.Split("val=")[1];
                
                if (double.TryParse(temperature, out var temperatureToRet))
                    return Task.FromResult(temperatureToRet);

                throw new Exception($"Parsing output cant be parsed. Output: {temperature}. Full output: {output}");
            }
            catch (Exception e)
            {
                _logger.Error("Noaa file parsing error", e);
                throw;
            }

        }
    }

}
