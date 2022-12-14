using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        int no=0;
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            

            Point minPoint =this.getMinPointY(points);
            Point x = new Point(minPoint.X + 10, minPoint.Y);
            List<Tuple<Point,double,double>>angels=new List<Tuple<Point,double, double>>();
            foreach(var point in points)
            {       
                double angle = this.angelBetweenTwoVector(minPoint.Vector(x), minPoint.Vector(point));
                double d = GetDistance(minPoint, point);
                angels.Add(new Tuple<Point, double,double>(point, Math.Round(angle,2),d));
            }
            angels=angels.OrderBy(a => a.Item2).ThenBy(a => a.Item3).ToList();
            angels.Insert(0, new Tuple<Point, double, double>(minPoint, 0, 0));
            for(int i=2; i<angels.Count; i++)
            {
                if (angels[i].Item2 == angels[i - 1].Item2 )
                {
                    angels.RemoveAt(i-1);
                    i--;
                }
            }
            if (points.Count < 4)
            {
                outPoints = new List<Point>(points);
                return;
            }
            List<Point>r=new List<Point>();
            for (int i = 0; i < angels.Count; i++) r.Add(angels[i].Item1);         
            Line line;
            outPoints.Add(minPoint);
            int n;
            for (int i = 1; i < angels.Count; i++)
            {
                if (outPoints.Count == 1)
                {
                    outPoints.Add(angels[i].Item1);
                    continue;
                }
                if (angels[i].Equals(angels[i - 1])) continue;
                n = outPoints.Count;
                line = new Line(outPoints[n - 2], outPoints[n - 1]);
                Enums.TurnType tt = HelperMethods.CheckTurn(line, angels[i].Item1);
                while (tt == Enums.TurnType.Right)
                {
                    outPoints.RemoveAt(n - 1);
                    n = outPoints.Count;
                    line = new Line(outPoints[n - 2], outPoints[n - 1]);
                    tt = HelperMethods.CheckTurn(line, angels[i].Item1);
                }
                if (tt == Enums.TurnType.Colinear)
                {
                    bool flag = HelperMethods.PointOnSegment(angels[i].Item1, line.Start, line.End);
                    if (flag == false)
                    {                                          
                        outPoints.RemoveAt(n - 1);
                        outPoints.Add(angels[i].Item1);
                    }
                    continue;
                }
                outPoints.Add(angels[i].Item1);
            }
            n = outPoints.Count;
            line = new Line(minPoint, outPoints[outPoints.Count - 2]);
            bool flag1 = HelperMethods.PointOnSegment(outPoints[outPoints.Count - 1], line.Start, line.End);
            if (flag1 == true)
            {
                outPoints.RemoveAt(n - 1);
            }
        }
        private Double angelBetweenTwoVector(Point v1,Point v2)
        {
            double magnOfv1 = v1.Magnitude();
            double magnOfv2 = v2.Magnitude();
            if (magnOfv1 == 0 || magnOfv2 == 0) return 0;
            double crossProduct=HelperMethods.dotProduct(v1,v2);
            double angle = Math.Acos((crossProduct) / (magnOfv1 * magnOfv2)); 
            return angle*(180/Math.PI);

        }

        private static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }
        private Point getMinPointY(List<Point> points)
        {
            Point minPoint = new Point(0,int.MaxValue);
            foreach(var point in points)
            {
                if(point.Y < minPoint.Y)
                {
                    minPoint = point;
                }
                else if (point.Y == minPoint.Y)
                {
                    if (point.X < minPoint.X)
                    {
                        minPoint = point;
                    }
                }
            }
            return minPoint;
        }
        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
