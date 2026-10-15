using System.Configuration;

namespace POD.DAL
{
    public static class DbConnection
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager
                    .ConnectionStrings["PODConf"]
                    .ConnectionString;
            }
        }
    }
}