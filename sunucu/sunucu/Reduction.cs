using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Diagnostics;
namespace sunucu
{
    public class Reduction
    {
        Stopwatch watch = new Stopwatch();
        public TimeSpan sure = new TimeSpan();

        public List<PointLatLng> ReductionFunction(List<PointLatLng> Points, Double Tolerance)
        {

            watch.Start();
            if (Points == null || Points.Count < 3)
                return Points;

            Int32 firstPoint = 0;
            Int32 lastPoint = Points.Count - 1;
            List<Int32> pointIndexsToKeep = new List<Int32>();


            pointIndexsToKeep.Add(firstPoint);
            pointIndexsToKeep.Add(lastPoint);


            while (Points[firstPoint].Equals(Points[lastPoint]))
            {
                lastPoint--;
            }

            ReductionFunction(Points, firstPoint, lastPoint,
            Tolerance, ref pointIndexsToKeep);

            List<PointLatLng> returnPoints = new List<PointLatLng>();
            pointIndexsToKeep.Sort();
            foreach (Int32 index in pointIndexsToKeep)
            {
                returnPoints.Add(Points[index]);
            }
            watch.Stop();
            sure = watch.Elapsed;
            return returnPoints;
        }
        private void ReductionFunction(List<PointLatLng>
        points, Int32 firstPoint, Int32 lastPoint, Double tolerance,
        ref List<Int32> pointIndexsToKeep)
        {
            Double maxDistance = 0;
            Int32 farIndex = 0;

            for (Int32 index = firstPoint; index < lastPoint; index++)
            {
                Double distance = vertDistance
                    (points[firstPoint], points[lastPoint], points[index]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farIndex = index;
                }
            }

            if (maxDistance > tolerance && farIndex != 0)
            {

                pointIndexsToKeep.Add(farIndex);

                ReductionFunction(points, firstPoint,
                farIndex, tolerance, ref pointIndexsToKeep);
                ReductionFunction(points, farIndex,
                lastPoint, tolerance, ref pointIndexsToKeep);
            }
        }
        public Double vertDistance(PointLatLng Point1, PointLatLng Point2, PointLatLng Point)
        {

            Double area = Math.Abs(.5 * (Point1.Lat * Point2.Lng + Point2.Lat *
            Point.Lng + Point.Lat * Point1.Lng - Point2.Lat * Point1.Lng - Point.Lat *
            Point2.Lng - Point1.Lat * Point.Lng));
            Double bottom = Math.Sqrt(Math.Pow(Point1.Lat - Point2.Lat, 2) +
            Math.Pow(Point1.Lng - Point2.Lng, 2));
            Double height = area / bottom * 2;

            return height;
        }
    }
}