//Версия 1.0.1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Eshiba
{
    class NET
    {
        public class Server
        {          
            static void SetAdr(string _ip,int _port)
            {
                ip = _ip; port = _port;
            }

            static string ip; static int port;

            Thread ServTherd = new Thread(CreateServer);

            static IPHostEntry ipHost;
            static IPAddress ipAddr;
            static IPEndPoint ipEndPoint;
            static Socket socket;

            public static void CreateServer()
            {
                IPAddress localAddress = IPAddress.Parse(ip);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipEndpoint = new IPEndPoint(localAddress, port);

                try
                {
                    socket.Bind(ipEndpoint);
                    socket.Listen(1);


                    while (true)
                    {
                        Socket handler = socket.Accept();

                        Main(handler);

                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    Console.ReadLine();
                }
            } 

            static void Main(Socket handler)
            {

            }

            static void SendInt32(Socket handler, int var)
            {
                handler.Send(BitConverter.GetBytes(var));
            }

            static void SendInt16(Socket handler, short var)
            {
                handler.Send(BitConverter.GetBytes(var));
            }

            static void SendByte(Socket handler, byte var)
            {
                handler.Send(new byte[] { var });
            }

            static  public Byte GetByte(Socket handler)
            {
                byte[] data = new byte[1];
                handler.Receive(data);
                return data[0];
            }

            static public short GetInt16(Socket handler)
            {
                byte[] data = new byte[2];
                handler.Receive(data);
                return (short)DataConverter.Byte2ToInt16(data);
            }

            static public int GetInt32(Socket handler)
            {
                byte[] data = new byte[4];
                handler.Receive(data);
                return DataConverter.Byte4toInt32(data);
            }

            static public void SendString(Socket handler, string var)
            {
                byte[] str = DataConverter.StrToByte(var);
                handler.Send(BitConverter.GetBytes(str.Length));
                handler.Send(str);
            }

            static public string GetString(Socket handler)
            {
                byte[] data = new byte[4];
                handler.Receive(data);
                byte[] str = new byte[DataConverter.Byte4toInt32(data)];
                handler.Receive(str);
                return DataConverter.ByteToStr(str);
            }

        }


        public class Client
        {
            IPHostEntry ipHost;
            IPAddress ipAddr;
            IPEndPoint ipEndPoint;
            Socket socket;

            public void CreateClient(string ip, int port)
            {
                ipHost = Dns.GetHostEntry(ip);
                ipAddr = ipHost.AddressList[0];
                ipEndPoint = new IPEndPoint(ipAddr, port);
                socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }

            public Byte GetByte()
            {
                socket.Connect(ipEndPoint);
                byte[] data = new byte[1];
                socket.Receive(data);
                socket.Close();
                return data[0];
            }

            public short GetInt16()
            {
                socket.Connect(ipEndPoint);
                byte[] data = new byte[2];
                socket.Receive(data);
                socket.Close();
                return (short)DataConverter.Byte2ToInt16(data);
            }

            public int GetInt32()
            {
                socket.Connect(ipEndPoint);
                byte[] data = new byte[4];
                socket.Receive(data);
                socket.Close();
                return DataConverter.Byte4toInt32(data);
            }

            public void SendInt32(int var)
            {
                socket.Connect(ipEndPoint);
                socket.Send(BitConverter.GetBytes(var));
                socket.Close();
            }

            public void SendInt16(short var)
            {
                socket.Connect(ipEndPoint);
                socket.Send(BitConverter.GetBytes(var));
                socket.Close();
            }

            public void SendByte(byte var)
            {
                socket.Connect(ipEndPoint);
                socket.Send(new byte[] { var });
                socket.Close();
            }

            public void SendString(string var)
            {
                socket.Connect(ipEndPoint);
                byte[] str = DataConverter.StrToByte(var);
                socket.Send(BitConverter.GetBytes(str.Length));
                socket.Send(str);
                socket.Close();
            }

            public string GetString()
            {
                socket.Connect(ipEndPoint);
                byte[] data = new byte[4];
                socket.Receive(data);
                byte[] str = new byte[DataConverter.Byte4toInt32(data)];
                socket.Receive(str);
                socket.Close();
                return DataConverter.ByteToStr(str);           
            }

        }
    }
}
