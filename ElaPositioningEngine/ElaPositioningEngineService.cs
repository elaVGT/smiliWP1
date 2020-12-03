using ElaPositioningEngine.Grpc;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElaPositioningEngine
{
    internal class ElaPositioningEngineService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private ResultsServer _ResultsServer;

        public ElaPositioningEngineService(ILogger<ElaPositioningEngineService> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting EPE Daemon...");
                _ResultsServer = new ResultsServer();
                _ResultsServer.Start(); // start EPE config server
                Console.ReadKey();
                Console.CancelKeyPress += delegate { this.StopAsync(cancellationToken); };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "EPESer02");
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {

                _logger.LogInformation("Stopping daemon...");
                _ResultsServer.StopAsync();
                GrpcEnvironment.ShutdownChannelsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "EPESer01");
            }
            return Task.CompletedTask;

        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing....");
        }
    }

}
