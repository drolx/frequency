//
// NetworkConnection.cs
//
// Author:
//       Godwin peter .O <me@godwin.dev>
//
// Copyright (c) 2020 MIT
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using uhf_rfid_catch.Helpers;

namespace uhf_rfid_catch.Handlers.ReaderConnections
{
    
    // https://stackoverflow.com/questions/19387086/how-to-set-up-tcplistener-to-always-listen-and-accept-multiple-connections
    // Multiple ports java sol => https://stackoverflow.com/questions/5079172/java-server-multiple-ports
    // C# multiple solu => https://social.msdn.microsoft.com/Forums/vstudio/en-US/09828be4-6ac4-45ec-a116-508314dab793/listen-on-multiple-ports?forum=csharpgeneral
    
    public class NetworkConnection
    {
        private static readonly ConfigContext SettingsContext = new ConfigContext();

        public NetworkConnection()
        {
        }

        public void Boot()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse("0.0.0.0");

                Console.WriteLine("Starting TCP listener...");

                TcpListener listener = new TcpListener(ipAddress, 5007);

                listener.Start();

                while (true)
                {
                    Socket client = listener.AcceptSocket();
                    Console.WriteLine("Connection accepted.");

                    var childSocketThread = new Thread(() =>
                    {
                        byte[] data = new byte[100];
                        int size = client.Receive(data);
                        Console.WriteLine("Recieved data: ");
                        for (int i = 0; i < size; i++)
                            Console.Write(Convert.ToChar(data[i]) + "---");

                        Console.WriteLine();

                        client.Close();
                    });
                    childSocketThread.Start();
                }

                listener.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.StackTrace);
                Console.ReadLine();
            }
        }
        
        
    }
}
