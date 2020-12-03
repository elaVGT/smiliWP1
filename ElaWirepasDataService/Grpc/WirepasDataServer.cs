using ElaSmili;
using ElaWirepas;
using ElaWirepasDataService.Grpc.Services;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElaWirepasDataService.Grpc
{
    public class WirepasDataServer
    {
        public const int grpcPort = 5001;
        public const string grpcHost = "192.168.1.12";
        private readonly Server m_server;

        public WirepasDataServer()
        {
            m_server = new Server
            {
                Services =
                {
                   ElaSmiliRequestService.BindService(new SmiliDataFlowService())
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
            Console.WriteLine("Wirepas Data Server listening on " + grpcHost + ":" + grpcPort);
        }

        public void StopAsync()
        {
            m_server.ShutdownAsync();
        }
    }
}

