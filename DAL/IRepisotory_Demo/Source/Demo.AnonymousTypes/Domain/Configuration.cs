using System;
using System.Configuration;

namespace Demo.AnonymousTypes.Domain
{
    public class Configuration
    {
        public static string GetConnectionString()
        {
            //return ConfigurationManager.ConnectionStrings["ActiveConnection"].ConnectionString;
            return "Server=127.0.0.1;Database=AppTestWeb;User Id=sa;Password=temporal;";
        }
    }
}
