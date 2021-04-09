﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FilterAndSort.NETCore.Services.Implements
{
    interface ILogFile
    {
        public void LogErrorExceptionParameter<T>(Exception ex, string param, T value);

        public void LogInformation();
    }
}
