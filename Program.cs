using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TCPIPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient();
            
            tcpClient.Connect("127.0.0.1", 5001);

            Stream stream = tcpClient.GetStream();
            
            byte[] requestBytes = Encoding.ASCII.GetBytes("Hello There");
            stream.Write(requestBytes, 0, requestBytes.Length);
            
            List<byte> bytesRecieved = new List<byte>();
            bool done = false;
            while (!done) 
            {
                int nextByte = stream.ReadByte();
                if(nextByte != -1)
                {
                    bytesRecieved.Add(Convert.ToByte(nextByte));
                }
                else
                {
                    done = true;
                }
            }
            
            foreach (var @byte in bytesRecieved)
            {
                Console.Write(Convert.ToChar(@byte));
            }

            tcpClient.Close();

            Console.WriteLine("Closed");
        }
    }
}
