using System;
using System.Linq;
using System.ServiceModel;
using OfficeCommunicator.Server.Network;

namespace OfficeCommunicator.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var svcHost = new ServiceHost(typeof(ServerEngine));
            svcHost.Open();
            Console.WriteLine("Available Endpoints :\n");
            svcHost.Description.Endpoints.ToList().ForEach(endpoint => Console.WriteLine(endpoint.Address.ToString()));
            Console.ReadLine();
            svcHost.Close();
        }
    }
}
