using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using NLog.Config;
using ReportPortal.Logging;

namespace ReportPortalSourceBackTest
{
    [TestClass]
    public class UnitTest1
    {
        private const string LoggerLogLayout = "${longdate}|${level:uppercase=true}| ${message}";
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            ConfigureNLogLogger(LoggerLogLayout);
        }

        [TestMethod]
        public void TestMethod1()
        {
            Logger.Trace("Test message");
            Assert.IsTrue(false);
        }

        /// <summary>
        ///     Configure NLog programmatic https://github.com/NLog/NLog/wiki/Configure-from-code
        /// </summary>
        public static void ConfigureNLogLogger(string logLayout)
        {
            var minNLogLogLevel = LogLevel.Trace;
            var assembly = Assembly.Load("ReportPortal.NLog");
            ConfigurationItemFactory.Default.RegisterItemsFromAssembly(assembly);
            var config = new LoggingConfiguration();
            // Rules for mapping loggers to targets            
            var rpTarget = new ReportPortalTarget
            {
                Layout = logLayout
            };
            config.AddRule(minNLogLogLevel, LogLevel.Fatal, rpTarget);
            // Apply config           
            LogManager.Configuration = config;
        }
    }
}