using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sunucu
{
    public class QuadTree
    {
        public Node point;

        public void insert(double x, double y)
        {
            //Ekleme   
            if (point == null)
            {

                point = new Node(x, y);
                Console.WriteLine(" " + x + " " + y);
            }
            else
            {

                insert(point, x, y);

            }
        }

        public void insert(Node point, double x, double y)
        {
            //Ekleme    
            if (x <= point.x && y <= point.y)
            {

                if (point.SW == null)
                {
                    point.SW = new QuadTree();

                }

                point.SW.insert(x, y);

            }
            else if (x <= point.x && y > point.y)
            {

                if (point.NW == null)
                {
                    point.NW = new QuadTree();


                }

                point.NW.insert(x, y);
            }
            else if (x > point.x && y <= point.y)
            {

                if (point.SE == null)
                {
                    point.SE = new QuadTree();

                }
                point.SE.insert(x, y);
            }
            else if (x > point.x && y > point.y)
            {
                if (point.NE == null)
                {

                    point.NE = new QuadTree();

                }
                point.NE.insert(x, y);
            }
        }
        public void ara(double x1, double x2, double y1, double y2)
        {



            if (point == null)
            {
                // Console.WriteLine("Nokta Yok");
            }
            else
            {
                if (x1 < point.x && point.x < x2 && y1 < point.y && point.y < y2)
                {


                    if (point.x != 0 && point.y != 0)
                    {
                        Program.sorgux.Add(point.x);//Kesişen xleri diziye atma
                        Program.sorguy.Add(point.y);//Kesişen yleri diziye atma  
                    }

                    ara(point, x1, x2, y1, y2);
                }
                else
                {
                    ara(point, x1, x2, y1, y2);
                }

            }


        }
        public void ara(Node point, double x1, double x2, double y1, double y2)
        {


            if (point.SE == null)
            {
                //Nokta Bitti 
            }
            else
                point.SE.ara(x1, x2, y1, y2);

            if (point.NE == null)
            {
                //Nokta Bitti  
            }
            else
                point.NE.ara(x1, x2, y1, y2);

            if (point.SW == null)
            {
                //Nokta Bitti;
            }
            else
                point.SW.ara(x1, x2, y1, y2);

            if (point.NW == null)
            {
                //Nokta Bitti;
            }
            else
                point.NW.ara(x1, x2, y1, y2);
        }



    }
}

