using CliWrap;
using CliWrap.Buffered;
using diffuisiondashboard.Repository;
using Diffusion.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace diffuisiondashboard.Connectify
{
    public class ScrapperService : IHostedService
    {
        public Task StartAsync(CancellationToken stoppingToken)
        {
            //Health Monitor
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    int counter = 0;
                    while (true)
                    {
                        Console.WriteLine("Reading GPU Info...");
                        var result = await Cli.Wrap("nvidia-smi").ExecuteBufferedAsync();
                        var stdout = result.StandardOutput;
                        MatchCollection matches = Regex.Matches(stdout, "[0-9a-zA-Z]*MiB[0-9a-zA-Z]*");
                        if (matches.Count >= 2)
                        {
                            Console.WriteLine("Got GPU metrix");
                            var healthInfo = new HealthInfo
                            {
                                GPU = new GPUMetrix
                                {
                                    Used = matches[0].Value.Replace("MiB", string.Empty),
                                    Total = matches[1].Value.Replace("MiB", string.Empty),
                                }
                            };
                            Console.WriteLine("Adding GPU metrix...");
                            new HealthRepo("Health").Add(healthInfo);
                            counter++;
                            if (counter > 60)
                            {
                                new HealthRepo("Health").Truncate();
                                counter = 0;
                            }
                            Console.WriteLine(healthInfo.GPU.Used + "\n" + healthInfo.GPU.Total);
                            Debug.WriteLine(healthInfo.GPU.Used + "\n" + healthInfo.GPU.Total);
                        }
                        Thread.Sleep(2500);
                    }
                }
                catch (Exception)
                {
                }
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
