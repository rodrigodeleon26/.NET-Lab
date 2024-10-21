namespace Shared
{
    public static class GlobalFunctions
    {
        private static string _connectionString = "Server=sqlserver,1433;Database=HCE;User Id=sa;Password=Abc*123!;Encrypt=False;";

        public static string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
