using AKDEMIC.CORE.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace AKDEMIC.REPOSITORY.Data
{
    public class AkdemicContextFactory : IDesignTimeDbContextFactory<AkdemicContext>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AkdemicContextFactory() { }

        public AkdemicContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AkdemicContext>();

            switch (ConstantHelpers.GENERAL.DATABASES.DATABASE)
            {
                case ConstantHelpers.DATABASES.MYSQL:

                    #region Documentation

                    // https://dev.mysql.com/doc/refman/5.6/en/innodb-restrictions.html
                    // https://mathiasbynens.be/notes/mysql-utf8mb4
                    // Query:
                    // mysql: SHOW CHARACTER SET;
                    // mysql: SHOW VARIABLES LIKE "%character%";
                    // mysql: SHOW VARIABLES LIKE "%collation%";
                    // mysql: SHOW VARIABLES LIKE "%innodb%";
                    // mysql: SET default_storage_engine=INNODB;
                    // Default:
                    // my.ini: [mysqld] character-set-server=latin1
                    // my.ini (Unmodifiable): [mysqld] character-set-system=utf8
                    // my.ini (Optional): [mysqld] collation-server=latin1_german1_ci
                    // my.ini (<= 5.6.0): [mysql] default-character-set=latin1
                    // my.ini: [mysqld] default-storage-engine=INNODB
                    // my.ini (>= 5.7.9): [mysqld] innodb_default_row_format=DYNAMIC
                    // my.ini (<= 5.7.6): [mysqld] innodb_file_format=Antelope
                    // my.ini (<= 5.7.6): [mysqld] innodb_file_format_max=Antelope
                    // my.ini (<= 5.7.6): [mysqld] innodb_large_prefix=0
                    // Custom:
                    // my.ini (Optional): [mysqld] character-set-client-handshake=false
                    // my.ini: [mysqld] character-set-server=utf8mb4
                    // my.ini (Optional): [mysqld] collation-server=utf8mb4_general_ci
                    // my.ini (<= 5.6.0): [mysql] default-character-set=utf8mb4
                    // my.ini: [mysqld] default-storage-engine=INNODB
                    // my.ini (>= 5.7.9): [mysqld] innodb_default_row_format=DYNAMIC
                    // my.ini (<= 5.7.6): [mysqld] innodb_file_format=Barracuda
                    // my.ini (<= 5.7.6): [mysqld] innodb_file_format_max=Barracuda
                    // my.ini (<= 5.7.6): [mysqld] innodb_large_prefix=1
                    // my.ini (Optional): [mysqld] skip-character-set-client-handshake

                    #endregion
                    builder.UseMySql(
                        "Server=172.16.10.108;Database=UNAMBA.SIGAU.DB;Uid=remoto;Pwd=Remoto45JK@#ssaa;AllowLoadLocalInfile=true;Connection Timeout=60;Default Command Timeout=300;"
                        , new MySqlServerVersion(ConstantHelpers.DATABASES.VERSIONS.MYSQL.VALUES[ConstantHelpers.DATABASES.VERSIONS.MYSQL.V8021])
                        , opts =>
                        {
                            opts.EnableRetryOnFailure();
                        });
                    break;
                case ConstantHelpers.DATABASES.SQL:
                    builder.UseSqlServer(
                        "Server=localhost;Database=AKDEMIC;Trusted_Connection=True;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=0;"
                        , opts =>
                        {
                            opts.EnableRetryOnFailure();
                        });
                    break;
                case ConstantHelpers.DATABASES.PSQL:
                    builder.UseNpgsql(
                        "Host=localhost;Database=akdemictest;Username=postgres;Password=root;CommandTimeout=600;Timeout=300;Internal Command Timeout=0;Pooling=false;")
                        .UseSnakeCaseNamingConvention();
                    break;
            }

            return new AkdemicContext(builder.Options, _httpContextAccessor);
        }
    }
}
