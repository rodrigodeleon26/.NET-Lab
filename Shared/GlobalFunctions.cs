using Microsoft.Extensions.Configuration;

namespace Shared
{
    public static class GlobalFunctions
    {
        private static string _connectionString = "Server=sqlserver,1433;Database=HCE;User Id=sa;Password=Abc*123!;Encrypt=False;";
        private static string _secretKey = "Se_viene_la_Sexta_de_la_mano_de_la_Fiera_y_el_Leo"; 
        private static string[] _allowedOrigins =
        {
            "https://localhost:5010",
            "https://localhost:5011",
            "https://localhost:5012",
            "http://localhost:4200"
        };

        // Configuración SMTP
        private static string _smtpHost = "smtp.gmail.com";
        private static int _smtpPort = 587;
        private static string _smtpUserName = "hcesistema@gmail.com";
        private static string _smtpPassword = "ftsv cbhn aspv amdx";
        private static bool _smtpEnableSsl = true;

        public static string GetConnectionString()
        {

            return _connectionString;
        }

        public static string GetSecretKey()
        {
            return _secretKey;
        }

        public static string[] GetAllowedOrigins()
        {
            return _allowedOrigins;
        }

        public static Dictionary<string, string> GetSmtpConfig()
        {
            return new Dictionary<string, string>
            {
                { "Host", _smtpHost },
                { "Port", _smtpPort.ToString() },
                { "UserName", _smtpUserName },
                { "Password", _smtpPassword },
                { "EnableSsl", _smtpEnableSsl.ToString() }
            };
        }
    }
}
