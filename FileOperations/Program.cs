using System;
using FileOperation.Core;
using Microsoft.Extensions.DependencyInjection;

namespace FileOperations
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<FileOperationApp>().Run();
        }

        public static void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<FileOperationApp>();
            services.AddTransient<IFileOperationService, FileOperationService>();
        }
    }
}
