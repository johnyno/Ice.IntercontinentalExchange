using System;
using System.ComponentModel.DataAnnotations;

namespace IntercontinentalExchange.Host.Models.Requests
{
    public class GetForecastRequest
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public double Lat { get; set; }
        [Required]
        public double Lon { get; set; }
    }
}
