using CGUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {

        private void addRecord(List<Point> points, string path)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(@path, false))
                {
                    file.WriteLine("X,Y");
                    foreach (var p in points)
                    {
                        file.WriteLine(p.X + "," + p.Y);

                    }
                    file.Close();
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("this program :", ex);
            }
        }

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            //addRecord(points, @"E:\Book1.csv");
            List<Point> opoints = new List<Point>();
            HelperMethods helper = new HelperMethods();
            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }
            for (int p = 0; p < points.Count; p++)
            {
                bool inside = false;
                for (int j = 0; j < points.Count; j++)
                {
                    for (int k = 0; k < points.Count; k++)
                    {
                        for (int i = 0; i < points.Count; i++)
                        {

                            if (!inside)
                            {
                                if (points[i].Equals(points[j]) ||
                                points[i].Equals(points[k]) ||
                                points[i].Equals(points[p]) ||
                                points[j].Equals(points[k]) ||
                                points[j].Equals(points[p]) ||
                                points[k].Equals(points[p])) continue;

                                if (HelperMethods.PointInTriangle(points[p], points[j], points[k], points[i]) != Enums.PointInPolygon.Outside)
                                {
                                    inside = true;
                                    break;
                                }
                            }
                        }
                        if (inside)
                            break;
                    }
                    if (inside)
                        break;
                }
                if (!inside)
                    opoints.Add(points[p]);

            }
            // outPoints = opoints;
            outPoints = dis(opoints);
            //addRecord(outPoints, @"E:\Book2.csv");



        }
        private List<Point> dis(List<Point> points)
        {
            List<Point> OUT = new List<Point>();
            Dictionary<Point, bool> D = new Dictionary<Point, bool>();
            foreach (var pi in points)
            {
                if (!OUT.Contains(pi))
                {
                    OUT.Add(pi);
                }
            }
            return OUT;
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}