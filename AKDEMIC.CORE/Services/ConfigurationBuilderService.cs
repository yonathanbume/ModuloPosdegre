using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace AKDEMIC.CORE.Services
{
    public class ConfigurationBuilderService
    {
        private readonly ConfigurationBuilder _configurationBuilder;
        private IConfigurationRoot _configurationRoot;

        public ConfigurationBuilderService()
        {
            _configurationBuilder = new ConfigurationBuilder();
        }

        public ConfigurationBuilderService(ConfigurationBuilder configurationBuilder)
        {
            _configurationBuilder = configurationBuilder;
            _configurationRoot = _configurationBuilder.Build();
        }

        public ConfigurationBuilderService(Func<ConfigurationBuilder, IConfigurationBuilder> func)
        {
            _configurationBuilder = new ConfigurationBuilder();
            var configurationBuilder = func(_configurationBuilder);

            _configurationRoot = _configurationBuilder.Build();
        }

        public IConfigurationRoot GetConfigurationRoot()
        {
            return _configurationRoot;
        }

        public void AddEnvironmentJsonFile(string path)
        {
            var configurationBuilder = _configurationBuilder.AddJsonFile($"{path}/environment.json");

            if (Debugger.IsAttached)
            {
                configurationBuilder = _configurationBuilder.AddJsonFile($"{path}/environment.debug.json");
            }
            else
            {
                configurationBuilder = _configurationBuilder.AddJsonFile($"{path}/environment.production.json");
            }

            _configurationRoot = configurationBuilder.Build();
        }
    }
}
