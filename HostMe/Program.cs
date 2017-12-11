using System;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using MicroService4Net;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net;
using System.Collections.Generic;
using System.Threading;

namespace HostMe
{
    class Program
    {
        const int DEFAULT_PORT = 80;
        private const string CONFIG_FILE_NAME = "HostMe.config.json";
        private static readonly ILog Logger = HostMe.Logger.GetLogger();

        private static String[] arguments = new String[] {};
        private static MicroService service = null;
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            WritePassedArgsToLog(args);

            arguments = args;


            var configuration = GetConfiguration() ?? new Configuration { Port = DEFAULT_PORT };

            StaticContentController.SiteRootPath = PathNormalizer.NormalizePath(configuration.Path);
            var port = configuration.Port;

            if (configuration.doAutoSelectUnusedPort)
            {
                port = GetOpenPort(port);
            }



            Logger.InfoFormat("Starting...\r\nPort = {0}\r\nPath = {1}",port,StaticContentController.SiteRootPath);

            var serviceName = Assembly.GetEntryAssembly().GetName().Name + "_Port_" + port;





            service = new MicroService(port, serviceName, serviceName);

            Thread secondThread = new Thread(runService);

            secondThread.Start();

           

            if (configuration.doAutoOpenDefaultBrowser)
            {
                tryOpenUrl("http://localhost:" + port);
            }

        }


        public static void runService()
        {
            service.Run(arguments);

        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        private static Configuration GetConfiguration()
        {
            Configuration configuration = null;
            var configFilePath = PathNormalizer.NormalizePath(CONFIG_FILE_NAME);

            if (!File.Exists(configFilePath))
                return null;

            var jsonConfig = GetConfigAsJson(configFilePath);

            try
            {
                configuration = JsonConvert.DeserializeObject<Configuration>(jsonConfig);
                Logger.Info("Configuration parsed");

                if (configuration.Port == 0)
                {
                    Logger.Info("No configuration port found. using port " + DEFAULT_PORT);
                    configuration.Port = DEFAULT_PORT;
                }
            }
            catch (Exception exception)
            {
                Logger.Error("Exception occured in configuratin parsing", exception);
            }

            return configuration;
        }


        /// <summary>
        /// Gets the next open port
        /// </summary>
        /// <remarks>Based on Andreas Reiff's answer at https://stackoverflow.com/questions/679489/determine-if-port-is-in-use </remarks>
        /// <param name="startPort">The start port.</param>
        /// <returns></returns>
        private static int GetOpenPort(int startPort = 2555)
        {
            int portStartIndex = startPort;
            int count = 99;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();

            List<int> usedPorts = tcpEndPoints.Select(p => p.Port).ToList<int>();
            int unusedPort = 0;

            unusedPort = Enumerable.Range(portStartIndex, 99).Where(port => !usedPorts.Contains(port)).FirstOrDefault();
            return unusedPort;
        }

        /// <summary>
        /// Tries the open URL.
        /// </summary>
        /// <remarks>
        /// <param>Based on Joel Harkers' (Software Developer at Trancon BV, Brussel, België) answer on "How to open in default browser in C#" at StackOverFlow forum</param>
        /// <param>Source: https://stackoverflow.com/questions/4580263/how-to-open-in-default-browser-in-c-sharp </param>
        /// </remarks>
        /// <param name="url">The URL that should be opened (if not the localhosted content)</param>
        /// <returns></returns>
        private static Boolean tryOpenUrl(String url)
        {

            Process proc = null;

                try
                {
                    proc = Process.Start(url);
                
                }
                catch (Exception ex)
                {
                    // hack because of this: https://github.com/dotnet/corefx/issues/10361
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        url = url.Replace("&", "^&");
                        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        Process.Start("xdg-open", url);
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        Process.Start("open", url);
                    }
                    else
                    {
                    return false;
                    }
                }

            return (proc != null);
            
        }


        private static string GetConfigAsJson(string configFilePath)
        {
            Logger.Info("Config found in " + configFilePath);
            var jsonConfig = File.ReadAllText(configFilePath);
            Logger.Info("The Config is: \r\n" + jsonConfig);
            return jsonConfig;
        }

        private static void WritePassedArgsToLog(string[] args)
        {
            if (args.Count() != 0)
            {
                var argsString = args.Aggregate((arg, current) => current + "," + arg);
                Logger.Info("Args = " + argsString);
            }
            else
                Logger.Info("No args passed");
        }
    }
}
