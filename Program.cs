using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using OpenSwimScoreboard.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenSwimScoreboard
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            CancellationTokenSource hubCancellationTokenSource = new CancellationTokenSource();
            var hubHost = CreateHostBuilder(args).Build().RunAsync(hubCancellationTokenSource.Token);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            hubCancellationTokenSource.Cancel();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .ConfigureLogging(logging =>
             {
                 logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
                 logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
                 logging.AddEventLog();
             })

            .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:5000");
                });
    }
}
