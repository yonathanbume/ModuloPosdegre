using Microsoft.Extensions.Configuration;

namespace AKDEMIC.REPOSITORY
{
    public class DatabaseConfiguration : ConfigurationBase
    {
        private const string DataConnectionKey = "DefaultConnection";

        public string GetDataConnectionString()
        {
            return GetConfiguration().GetConnectionString(DataConnectionKey);
        }
    }
}
