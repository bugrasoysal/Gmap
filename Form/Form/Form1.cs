using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Net;
using System.Net.Sockets;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;

namespace FormApp
{
    public partial class Form1
    {
        GMapOverlay overlay = new GMapOverlay("Overlay");
        List<PointLatLng> points = new List<PointLatLng>();
        string dosya_yolu;
        int a = 0;
        double[] lat = new double[2];
        double[] lng = new double[2];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            file.FilterIndex = 2;
            file.Title = "Text dosyası seçiniz..";
            file.ShowDialog();
            dosya_yolu = file.FileName;

            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;

            FileStream fs = new FileStream(dosya_yolu, FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);

            string[] a = null;
            double b, c;

            string yazi = sw.ReadLine();
            while (yazi != null)
            {
                a = yazi.Split(' ');
                b = Convert.ToDouble(a[0]);
                c = Convert.ToDouble(a[1]);
                Console.WriteLine("" + b + "   " + "" + c);
                points.Add(new PointLatLng(b, c));
                a = null;
                yazi = sw.ReadLine();

            }
            sw.Close();
            fs.Close();

            GMapRoute route = new GMapRoute(points, "A walk in the park");
            route.Stroke = new Pen(Color.Blue, 2);
            overlay.Routes.Add(route);
            gMapControl1.Overlays.Add(overlay);
            gMapControl1.Position = new PointLatLng(points[0].Lat, points[0].Lng);
            gMapControl1.MinZoom = 1;
            gMapControl1.MaxZoom = 100;
            gMapControl1.Zoom = 10;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            byte[] message = null;
            byte[] bos = null;

            TcpClient client = new TcpClient("127.0.0.1", 1200);
            Console.WriteLine("[Try to connect to server...]");
            NetworkStream n = client.GetStream();
            Console.WriteLine("[Connected]");
            for (int i = 0; i < points.Count; i++)
            {
                var lat = ((decimal)points[i].Lat).ToString();
                message = Encoding.Unicode.GetBytes(lat);
                n.Write(message, 0, message.Length);
                bos = Encoding.Unicode.GetBytes("a");
                n.Read(bos, 0, bos.Length);
            }
            for (int i = 0; i < points.Count; i++)
            {
                var lng = ((decimal)points[i].Lng).ToString();
                message = Encoding.Unicode.GetBytes(lng);
                n.Write(message, 0, message.Length);
                bos = Encoding.Unicode.GetBytes("a");
                n.Read(bos, 0, bos.Length);

            }
            Console.WriteLine("-----------Sent------------");
            client.Close();
            veriAl();
            sureAl();

        }
        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                lat[a] = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
                lng[a] = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
                Console.WriteLine("" + lat + "" + lng);
                a++;
            }
            if (a == 2)
            {
                polyCiz(lat[0], lat[1], lng[0], lng[1]);
                sorguVerisi();
                a = 0;
            }
        }
        public void routeCiz(List<PointLatLng> point)
        {
            GMapRoute route = new GMapRoute(point, "A walk in the park");
            route.Stroke = new Pen(Color.Red, 2);
            overlay.Routes.Add(route);
            gMapControl1.Overlays.Add(overlay);
            gMapControl1.Invalidate();


        }
        public void markerCiz(List<PointLatLng> point)
        {
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gMapControl1.ShowCenter = false;

            for (int i = 0; i < point.Count; i++)
            {
                GMapMarker marker = new GMarkerGoogle(
                new PointLatLng(point[i].Lat, point[i].Lng),
                GMarkerGoogleType.green);
                overlay.Markers.Add(marker);

            }
            gMapControl1.Overlays.Add(overlay);
        }
        public void polyCiz(double x1, double x2, double y1, double y2)
        {
            List<double> liste = new List<double>();
            if (x1 < x2)
            {
                liste.Add(x1);
                liste.Add(x2);
            }
            else
            {
                liste.Add(x2);
                liste.Add(x1);
            }
            if (y1 < y2)
            {
                liste.Add(y1);
                liste.Add(y2);
            }
            else
            {
                liste.Add(y2);
                liste.Add(y1);
            }
            List<PointLatLng> points2 = new List<PointLatLng>();
            points2.Add(new PointLatLng(x1, y2));
            points2.Add(new PointLatLng(x1, y1));
            points2.Add(new PointLatLng(x2, y1));
            points2.Add(new PointLatLng(x2, y2));

            GMapPolygon polygon = new GMapPolygon(points2, "Jardin des Tuileries");
            overlay.Polygons.Add(polygon);
            gMapControl1.Overlays.Add(overlay);

            polygon.Fill = new SolidBrush(Color.FromArgb(20, Color.White));
            polygon.Stroke = new Pen(Color.Green, 3);
            byte[] message = null;
            byte[] bos = null;

            TcpClient client = new TcpClient("127.0.0.1", 1202);
            Console.WriteLine("[Try to connect to server...]");
            NetworkStream n = client.GetStream();
            Console.WriteLine("[Connected]");
            for (int i = 0; i < liste.Count; i++)
            {
                var lat = ((decimal)liste[i]).ToString();
                message = Encoding.Unicode.GetBytes(lat);
                n.Write(message, 0, message.Length);
                bos = Encoding.Unicode.GetBytes("a");
                n.Read(bos, 0, bos.Length);
            }
            Console.WriteLine("-----------Sent------------");
            client.Close();
        }

        public void veriAl()
        {
            List<PointLatLng> points2 = new List<PointLatLng>();
            List<string> liste = new List<string>();
            TcpClient client2 = new TcpClient("127.0.0.1", 1201);
            Console.WriteLine("[Try to connect to server...]");
            NetworkStream n2 = client2.GetStream();
            Console.WriteLine("[Connected]");
            byte[] buffer = null;
            byte[] bos2 = null;
            int data;
            string ch = null;
            while (client2.Connected)
            {
                buffer = new byte[client2.ReceiveBufferSize];
                data = n2.Read(buffer, 0, client2.ReceiveBufferSize);
                if (data != 0)
                {
                    ch = Encoding.Unicode.GetString(buffer, 0, data);
                    liste.Add(ch);

                }
                else
                    break;
                bos2 = Encoding.Unicode.GetBytes("a");
                n2.Write(bos2, 0, bos2.Length);
                n2.Flush();
                buffer = null;
            }
            client2.Close();
            double a, b;
            for (int i = 0; i < liste.Count / 2; i++)
            {
                a = Convert.ToDouble(liste[i]);
                b = Convert.ToDouble(liste[(liste.Count / 2) + i]);
                points2.Add(new PointLatLng(a, b));
                Console.WriteLine("" + a + "," + "" + b);
            }
            routeCiz(points2);
        }


        public void sorguVerisi()
        {
            List<PointLatLng> points2 = new List<PointLatLng>();
            List<string> liste = new List<string>();
            TcpClient client2 = new TcpClient("127.0.0.1", 1203);
            Console.WriteLine("[Try to connect to server...]");
            NetworkStream n2 = client2.GetStream();
            Console.WriteLine("[Connected]");
            byte[] buffer = null;
            byte[] bos2 = null;
            int data;
            string ch = null;
            while (client2.Connected)
            {
                buffer = new byte[client2.ReceiveBufferSize];
                data = n2.Read(buffer, 0, client2.ReceiveBufferSize);
                if (data != 0)
                {
                    ch = Encoding.Unicode.GetString(buffer, 0, data);
                    liste.Add(ch);

                }
                else
                    break;
                bos2 = Encoding.Unicode.GetBytes("a");
                n2.Write(bos2, 0, bos2.Length);
                n2.Flush();
                buffer = null;
            }
            client2.Close();
            double a, b;
            for (int i = 0; i < liste.Count / 2; i++)
            {
                a = Convert.ToDouble(liste[i]);
                b = Convert.ToDouble(liste[(liste.Count / 2) + i]);
                points2.Add(new PointLatLng(a, b));
                Console.WriteLine("" + a + "," + "" + b);
            }
            markerCiz(points2);
        }
        public void sureAl()
        {
            TcpClient client2 = new TcpClient("127.0.0.1", 1204);
            Console.WriteLine("[Try to connect to server...]");
            NetworkStream n2 = client2.GetStream();
            Console.WriteLine("[Connected]");
            byte[] buffer = null;
            int data;
            string ch = null;
            buffer = new byte[client2.ReceiveBufferSize];
            data = n2.Read(buffer, 0, client2.ReceiveBufferSize);
            ch = Encoding.Unicode.GetString(buffer, 0, data);
            client2.Close();
            Console.WriteLine(ch);
            textBox1.Text = ch;
            textBox2.Text = "%40";

        }
    }
}


