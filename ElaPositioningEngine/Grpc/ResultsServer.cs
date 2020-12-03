using ElaPositioningEngine.Grpc.Services;
using ElaSmili;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElaPositioningEngine.Grpc
{
    public class ResultsServer
    {
        public const int grpcPort = 5002;
        public const string grpcHost = "192.168.1.12";
        private readonly Server m_server;

        public ResultsServer()
        {
            m_server = new Server
            {
                Services =
                {
                   ElaSmiliRequestService.BindService(new LocationResultService())
                },
                Ports =
                {
                    new ServerPort(grpcHost, grpcPort, ServerCredentials.Insecure)
                }
            };
        }

        public void Start()
        {
            m_server.Start();
            Console.WriteLine("EPE Results Server listening on " + grpcHost + ":" + grpcPort);
        }

        public void StopAsync()
        {
            m_server.ShutdownAsync();
        }
    }
}

