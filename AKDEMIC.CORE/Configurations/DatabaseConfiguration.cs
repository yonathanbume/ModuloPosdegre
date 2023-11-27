using System;
using System.Collections.Generic;

namespace AKDEMIC.CORE.Configurations
{
    public class DatabaseConfiguration
    {
        public const int MYSQL = 1;
        public const int SQL = 2;

        public static class ConnectionStrings
        {
            public static Dictionary<Tuple<int, int>, string> VALUES = new Dictionary<Tuple<int, int>, string>()
                {
                    { new Tuple<int, int>(MYSQL, MySql.DEFAULT), MySql.VALUES[MySql.DEFAULT] },
                    { new Tuple<int, int>(SQL, MySql.DEFAULT), Sql.VALUES[Sql.DEFAULT] },
                };

            public static class MySql
            {
                public const int DEFAULT = 0;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { DEFAULT, "MySqlDefaultConnection" },
                    };
            }

            public static class Sql
            {
                public const int DEFAULT = 0;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { DEFAULT, "SqlDefaultConnection" },
                    };
            }
        }

        public static class Versions
        {
            public static class MySql
            {
                public const int V5717 = 0;
                public const int V5723 = 1;
                public const int V5726 = 2;

                public static Dictionary<int, Version> VALUES = new Dictionary<int, Version>()
                    {
                        { V5717, new Version(5, 7, 17) },
                        { V5723, new Version(5, 7, 23) },
                        { V5726, new Version(5, 7, 26) },
                    };
            }
        }
    }
}
