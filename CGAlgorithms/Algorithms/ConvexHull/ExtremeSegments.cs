using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            string ss = @"C:\Users\yousif mohamed\Desktop\New folder (6)";
            if (points.Count  <= 3)
            {
                outPoints = points;
                return ;
            }

            for(int x=0;x<points.Count;x++)
            {
                for(int y= 0; y < points.Count; y++)
                {
                    Point i=points[x];
                    Point j= points[y];
                    if (x==y) continue;

                    Line line = new Line(i, j);
                    bool right = false, left = false,co=false;
                    bool insegment = false;
                    for (int z = 0; z < points.Count; z++)
                    {
                        Point k = points[z];


                        if (z==y || z==x) continue;
                        if(i.Equals(new Point(100,-100))==true && j.Equals(new Point(100, 0))==true 
                            && k.Equals(new Point(100, 100))==true){
                            int v = 0;

                        }
                        
                        Enums.TurnType tt = HelperMethods.CheckTurn(line, k);
                        if(tt == Enums.TurnType.Left)left = true;
                        if(tt == Enums.TurnType.Right)right = true;
                        if(tt == Enums.TurnType.Colinear)
                        {
                            co = true;
                            insegment = HelperMethods.PointOnSegment(k, i, j);
                            if (!insegment)
                            {
                                double d1 = GetDistance(i, k);
                                double d2 = GetDistance(j, k);
                                if (d1 > d2)
                                {
                                    j = new Point(k.X, k.Y);
                                }
                                else
                                {
                                    i = new Point(k.X, k.Y);
                                }
                                line = new Line(i, j);
                            }
                        }
                        if (left && right) break;

                    }
                    
                    if (!(left && right))
                    {
                        //if (co)
                        //{
                        //    if (insegment)
                        //    {
                        //        outPoints.Add(line.Start);
                        //        outPoints.Add(line.End);
                        //    }           
                        //    continue;
                        //}
                        outPoints.Add(line.Start);
                        outPoints.Add(line.End);
                    }
                    
                }
            }

            outPoints = distnctpont(outPoints);


            int ggggggggggggggg = 0;


        }
        private List<Point> distnctpont(List<Point> points)
        {
            List<Point> temp=new List<Point>();
            Dictionary<Point,bool>dict=new Dictionary<Point,bool>();
            foreach(Point p in points)
            {
                if (!temp.Contains(p))
                {
                    temp.Add(p);
                }
             
            }
            return temp;
        }
        private static double GetDistance(Point p1,Point p2)
        {
            return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }
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


        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
        
    }
}
