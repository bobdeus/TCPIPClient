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


            string preRequest = "POST /presentation/ws/jsonBridge.cfm HTTP/1.1\r\n" +
                                "Host: dixond18.mainman.dcs:8080\r\n" +
                                "Content-Type: application/json\r\n";

            string carriageReturn = "\r\n";
            string bodyContent = "{\"serviceName\":\"ConnectionService\",\"methodName\":\"getVersion\"}";

            int lengthContent = Encoding.ASCII.GetBytes(bodyContent).Length;
            string contentLengthWithHeader = $"Content-Length: {lengthContent}\r\n";

            string finalRequest = preRequest + contentLengthWithHeader + carriageReturn + bodyContent;

            byte[] requestBytes = Encoding.ASCII.GetBytes(finalRequest);
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
            
            File.WriteAllBytes("E:\\dump.html", bytesRecieved.ToArray());
            foreach (var @byte in bytesRecieved)
            {
                Console.Write(Convert.ToChar(@byte));
            }

            tcpClient.Close();

            Console.WriteLine();
            Console.WriteLine("Closed");
        }
    }
}