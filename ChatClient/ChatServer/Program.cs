using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(ChatWCF.ChatService)))
            {
                host.Open();
                Console.WriteLine("Host begin");
                Console.ReadKey();
            }
        }
    }
}
