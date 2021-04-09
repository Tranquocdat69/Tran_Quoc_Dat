﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilterAndSort.NETCore.Services.Implements
{
    class LogFile : ILogFile
    {
        private readonly ILogger<LogFile> _logger; 
        public LogFile(ILogger<LogFile> logger)
        {
            _logger = logger;
        }
        public void LogErrorExceptionParameter<T>(Exception ex, string param, T value)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " [ERR] =================");
            _logger.LogError("LastContext = {0} = {1}" ,param, value);
            _logger.LogError("Message = " + ex.Message);
            _logger.LogError("StackTrace =" + ex.StackTrace);
            _logger.LogError("TargetSite =" + ex.TargetSite);
            _logger.LogError("********");
        }

        public void LogInformation()
        {
            DateTime startTime = DateTime.Now;
            _logger.LogInformation("begin=" + DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss.fff"));
            _logger.LogInformation("end=" + DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss.fff"));
            DateTime endTime = DateTime.Now;
            //tổng thời gian (ms) chạy chương trình = endTime - startEnd
            double totalMilliseconds = endTime.Subtract(startTime).TotalMilliseconds;
            _logger.LogInformation("ElapsedMilliseconds = {totalMilliseconds}", totalMilliseconds);
        }
    }
}
