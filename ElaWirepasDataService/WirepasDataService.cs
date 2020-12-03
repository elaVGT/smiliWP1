using ElaWirepasDataService.Grpc;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElaWirepasDataService
{
    internal class WirepasDataService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private WirepasDataServer _DataServer;

        public WirepasDataService(ILogger<WirepasDataService> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting WirepasDataHandler Daemon...");
                _DataServer = new WirepasDataServer();
                _DataServer.Start(); // start EPE config server
                Console.ReadKey();
                Console.CancelKeyPress += delegate { this.StopAsync(cancellationToken); };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "DataSer02");
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {

                _logger.LogInformation("Stopping daemon...");
                _DataServer.StopAsync();
                GrpcEnvironment.ShutdownChannelsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "DataSer01");
            }
            return Task.CompletedTask;

        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing....");
        }
    }

}
