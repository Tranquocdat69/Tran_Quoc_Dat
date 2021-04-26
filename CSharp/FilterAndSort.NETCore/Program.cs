using FilterAndSort.NETCore.Services;
using FilterAndSort.NETCore.Services.Implements;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Fluent;
using System;

namespace FilterAndSort.NETCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //Dependency Injection COntainer
            var host = Host.CreateDefaultBuilder().ConfigureServices((context,services) =>
            {
                //khởi tạo những services để dùng
                // service logging để in ra file log
                services.AddLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                    logging.AddNLog();
                });
                //AddSingleton khởi tạo instance 1 lần duy nhất
                services.AddSingleton<IGetData,GetData>();
                services.AddSingleton<IHandleData, HandleData>();
                services.AddSingleton<IRun, Run>();
                services.AddSingleton<ILogFile, LogFile>();
                //NLog.LogManager.Configuration = new NLogLoggingConfiguration(context.Configuration.GetSection("NLog"));
            }).Build();
            //khởi tạo instance Run để chạy chương trình và truyền vào params là những services đã khai báo
            var main = ActivatorUtilities.CreateInstance<Run>(host.Services);
            main.RunApp();
        }
    }
}
