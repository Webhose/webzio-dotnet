namespace webzio
{
    using System.Configuration;

    public class WebzOptions
    {
        public string Token { get; set; } = ConfigurationManager.AppSettings["webzio:token"];
        public string Format => "json";
    }
}