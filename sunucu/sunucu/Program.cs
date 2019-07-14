using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using GMap.NET;
using GMap.NET.WindowsForms;
namespace sunucu
{
    public class Program
    {
        public static QuadTree quadTree = new QuadTree();
        public static Reduction reducTion = new Reduction();
        public static List<double> sorgux = new List<double>();
        public static List<double> sorguy = new List<double>();

        public static void Main(string[] args)
        {
            List<PointLatLng> points = new List<PointLatLng>();
            List<string> liste = new List<string>();
            TcpListener listen = new TcpListener(IPAddress.Any, 1200);
            Console.WriteLine("[Listenning...");
            listen.Start();
            TcpClient client = listen.AcceptTcpClient();
            Console.WriteLine("[Client connected]");
            NetworkStream stream = client.GetStream();
            byte[] buffer = null;
            byte[] bos = null;
            int data;
            string ch = null;
            while (client.Connected)
            {
                buffer = new byte[client.ReceiveBufferSize];
                data = stream.Read(buffer, 0, client.ReceiveBufferSize);
                if (data != 0)
                {
                    ch = Encoding.Unicode.GetString(buffer, 0, data);
                    liste.Add(ch);

                }
                else
                    break;
                bos = Encoding.Unicode.GetBytes("a");
                stream.Write(bos, 0, bos.Length);
                stream.Flush();
                buffer = null;
            }
            client.Close();
            double a, b;
            for (int i = 0; i < liste.Count / 2; i++)
            {
                a = Convert.ToDouble(liste[i]);
                b = Convert.ToDouble(liste[(liste.Count / 2) + i]);
                points.Add(new PointLatLng(a, b));

            }
            points = reduction(points);
            quadtree(points);
            veriGonder(points);
            Console.WriteLine(reducTion.sure);
            sureGonder(reducTion.sure);
            veriAl();

            Console.ReadKey();


        }
        public static void quadtree(List<PointLatLng> points)
        {
            double x, y;
            for (int i = 0; i < points.Count; i++)
            {
                x = points[i].Lat;
                y = points[i].Lng;
                quadTree.insert(x, y);
            }
        }
        public static List<PointLatLng> reduction(List<PointLatLng> points)
        {
            points = reducTion.ReductionFunction(points, 35);

            return points;
        }
        public static void veriAl()
        {
            List<string> liste = new List<string>();
            TcpListener listen = new TcpListener(IPAddress.Any, 1202);
            Console.WriteLine("[Listenning...");
            listen.Start();
            TcpClient client = listen.AcceptTcpClient();
            Console.WriteLine("[Client connected]");
            NetworkStream stream = client.GetStream();
            byte[] buffer = null;
            byte[] bos = null;
            int data;
            string ch = null;
            while (client.Connected)
            {
                buffer = new byte[client.ReceiveBufferSize];
                data = stream.Read(buffer, 0, client.ReceiveBufferSize);
                if (data != 0)
                {
                    ch = Encoding.Unicode.GetString(buffer, 0, data);
                    liste.Add(ch);

                }
                else
                    break;
                bos = Encoding.Unicode.GetBytes("a");
                stream.Write(bos, 0, bos.Length);
                stream.Flush();
                buffer = null;
            }
            client.Close();
            double x1, x2, y1, y2;
            x1 = Convert.ToDouble(liste[0]);
            x2 = Convert.ToDouble(liste[1]);
            y1 = Convert.ToDouble(liste[2]);
            y2 = Convert.ToDouble(liste[3]);
            quadTree.ara(x1, x2, y1, y2);
            for (int i = 0; i < sorgux.Count; i++)
            {
                Console.WriteLine("" + sorgux[i] + "," + "" + sorguy[i]);
            }
            sorguVerisi();
        }
        public static void veriGonder(List<PointLatLng> points)
        {
            List<string> liste = new List<string>();
            TcpListener listen = new TcpListener(IPAddress.Any, 1201);
            Console.WriteLine("[Listenning...");
            listen.Start();
            TcpClient client = listen.AcceptTcpClient();
            Console.WriteLine("[Client connected]");
            NetworkStream stream = client.GetStream();
            byte[] bos = null;
            byte[] message = null;
            for (int i = 0; i < points.Count; i++)
            {
                var lat = ((decimal)points[i].Lat).ToString();
                message = Encoding.Unicode.GetBytes(lat);
                stream.Write(message, 0, message.Length);
                bos = Encoding.Unicode.GetBytes("a");
                stream.Read(bos, 0, bos.Length);
            }
            for (int i = 0; i < points.Count; i++)
            {
                var lng = ((decimal)points[i].Lng).ToString();
                message = Encoding.Unicode.GetBytes(lng);
                stream.Write(message, 0, message.Length);
                bos = Encoding.Unicode.GetBytes("a");
                stream.Read(bos, 0, bos.Length);

            }
            Console.WriteLine("-----------Sent------------");
            client.Close();

        }

        public static void sorguVerisi()
        {
            TcpListener listen = new TcpListener(IPAddress.Any, 1203);
            Console.WriteLine("[Listenning...");
            listen.Start();
            TcpClient client = listen.AcceptTcpClient();
            Console.WriteLine("[Client connected]");
            NetworkStream stream = client.GetStream();
            byte[] bos = null;
            byte[] message = null;
            for (int i = 0; i < sorgux.Count; i++)
            {
                var lat = ((decimal)sorgux[i]).ToString();
                message = Encoding.Unicode.GetBytes(lat);
                stream.Write(message, 0, message.Length);
                bos = Encoding.Unicode.GetBytes("a");
                stream.Read(bos, 0, bos.Length);
            }
            for (int i = 0; i < sorgux.Count; i++)
            {
                var lng = ((decimal)sorguy[i]).ToString();
                message = Encoding.Unicode.GetBytes(lng);
                stream.Write(message, 0, message.Length);
                bos = Encoding.Unicode.GetBytes("a");
                stream.Read(bos, 0, bos.Length);

            }
            Console.WriteLine("-----------Sent------------");
            client.Close();
        }
        public static void sureGonder(TimeSpan sure)
        {
            TcpListener listen = new TcpListener(IPAddress.Any, 1204);
            Console.WriteLine("[Listenning...");
            listen.Start();
            TcpClient client = listen.AcceptTcpClient();
            Console.WriteLine("[Client connected]");
            NetworkStream stream = client.GetStream();
            byte[] message = null;

            var lat = ((TimeSpan)sure).ToString();
            message = Encoding.Unicode.GetBytes(lat);
            stream.Write(message, 0, message.Length);

            Console.WriteLine("-----------Sent------------");
            client.Close();
        }
    }
}