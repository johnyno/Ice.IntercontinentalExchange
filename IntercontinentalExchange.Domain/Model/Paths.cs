using System;

namespace IntercontinentalExchange.Domain.Model
{
    public class Paths
    {
        public static string ApplicationLogDirectory { get; } = $"{Environment.CurrentDirectory}//Logs";
    }
}
