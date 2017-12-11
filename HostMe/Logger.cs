// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="imbVeles" >
//
// The MIT Licence
// 
// Copyright (C) 2017 Goran Grubic (HostMe fork) and TheCodeCleaner (HostMe, original project)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// </copyright>
// <summary>
// ------------------------------------------------------------------------------------------------------------------
// GitHub of TheCodeCleaner (original project author): https://github.com/TheCodeCleaner
// GitHub repo of the original project: https://github.com/TheCodeCleaner/HostMe
// 
// ------ Goran Grubic -- imbVeles project ------
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// http://blog.veles.rs/
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using System;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace HostMe
{
    public static class Logger
    {
        public static Boolean doDisableFileCreation = false;

        public static ILog GetLogger()
        {
            return LogManager.GetLogger("logger");
        }

         public static IAppender rollingFileAppender {get;set;}

        static Logger()
        {
            var patternLayout = new PatternLayout { ConversionPattern = "%d [%t] %-5p %m%n" };
            patternLayout.ActivateOptions();

            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;


            rollingFileAppender = CreateRollingFileAppender(patternLayout);

            hierarchy.Root.AddAppender(rollingFileAppender);

            
            

            if (!Environment.UserInteractive)
                return;

            var consoleAppender = CreateColoredConsoleAppender(patternLayout);
            hierarchy.Root.AddAppender(consoleAppender);
        }

        private static IAppender CreateColoredConsoleAppender(ILayout layout)
        {
            var consoleAppender = new ColoredConsoleAppender { Layout = layout };

            consoleAppender.AddMapping(
                new ColoredConsoleAppender.LevelColors
                { Level = Level.Warn, ForeColor = ColoredConsoleAppender.Colors.Yellow });

            consoleAppender.AddMapping(
                new ColoredConsoleAppender.LevelColors
                { Level = Level.Error, ForeColor = ColoredConsoleAppender.Colors.Red });

            consoleAppender.AddMapping(
                new ColoredConsoleAppender.LevelColors
                { Level = Level.Fatal, ForeColor = ColoredConsoleAppender.Colors.Red });

            consoleAppender.ActivateOptions();
            return consoleAppender;
        }

        private static IAppender CreateRollingFileAppender(ILayout layout)
        {
            var logsFolder = PathNormalizer.NormalizePath("logs");

            if (!Directory.Exists(logsFolder))
                Directory.CreateDirectory(logsFolder);

            var rollingFileAppender = new RollingFileAppender
            {
                Layout = layout,
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Size,
                PreserveLogFileNameExtension = true,
                MaxSizeRollBackups = 1,
                MaximumFileSize = "10MB",
                StaticLogFileName = true,
                File = Path.Combine(logsFolder, DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt")
        };
            rollingFileAppender.ActivateOptions();
            return rollingFileAppender;
        }
    }
}
