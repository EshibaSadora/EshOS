using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

using System.Xml;

namespace Eshiba
{

    class EshHttp
    {
        bool running = true;
        public void Run()
        {
            HttpListener listener = new HttpListener();
            // установка адресов прослушки
            listener.Prefixes.Add("http://192.168.68.111:80/");
            listener.Start();
            



            // метод GetContext блокирует текущий поток, ожидая получение запроса 

            while (running)
            {
                Console.WriteLine("Запущен поток");
                HttpListenerContext context = listener.GetContext();

                Thread requestHandler = new Thread(() =>
                {
                    
                    main(context);


                });

                requestHandler.Start();
            }
            

            /*
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            // получаем объект ответа
            HttpListenerResponse response = context.Response;
            // создаем ответ в виде кода html
            string responseStr = "<html><head><meta charset='utf8'></head><body>Привет мир!</body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseStr);
            // получаем поток ответа и пишем в него ответ
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // закрываем поток
            output.Close();
            // останавливаем прослушивание подключений
             */



            listener.Stop();
            Console.WriteLine("Обработка подключений завершена");
            Console.Read();
        }


        void main(HttpListenerContext context)
        {
            //EshXml xml = new EshXml(GetRequestBody(context));
            //String val = xml.GetXmlElementByPath("//Main/c");

            HttpListenerRequest request = context.Request;
            // получаем объект ответа
            HttpListenerResponse response = context.Response;
            // создаем ответ в виде кода html
            string responseStr = "<html><head><meta charset='utf8'></head><body>Привет мир!</body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseStr);
            // получаем поток ответа и пишем в него ответ
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // закрываем поток
            output.Close();

        }

        public String GetRequestBody(HttpListenerContext context)
        {
            String outbody = "";
            System.IO.Stream body = context.Request.InputStream;
            System.Text.Encoding encoding = context.Request.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
            outbody = reader.ReadToEnd();
            return outbody;
        }

        /*
         *  Console.WriteLine("Подключен клиент");
          
            Thread.Sleep(2000);
            HttpListenerRequest request = context.Request;
            // получаем объект ответа
            HttpListenerResponse response = context.Response;
            // создаем ответ в виде кода html
            string responseStr = "<html><head><meta charset='utf8'></head><body>Привет мир!</body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseStr);
            // получаем поток ответа и пишем в него ответ
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // закрываем поток
            output.Close();

            Console.WriteLine("Отдал данные");
         */

    }
}
