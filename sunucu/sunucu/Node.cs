using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sunucu
{
    public class Node
    {
        public double x, y;
        public QuadTree NW, NE, SW, SE;


        public Node(double x, double y)
        {
            this.x = x;
            this.y = y;

        }
    }
}