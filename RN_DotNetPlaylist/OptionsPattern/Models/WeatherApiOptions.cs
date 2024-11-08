using System.ComponentModel.DataAnnotations;

namespace OptionsPattern.Models
{
    public class WeatherApiOptions
    {
        
        [Required]
        public string Url { get; set; }
        
        [Required]
        public string ApiKey { get; set; }
        
    }
}