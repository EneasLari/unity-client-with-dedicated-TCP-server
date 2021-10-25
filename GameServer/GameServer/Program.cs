using System;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Game Server";
            Server.Start(50, 26956);//check an unused port from wikipedia :
            Console.ReadKey();
        }
    }
}
