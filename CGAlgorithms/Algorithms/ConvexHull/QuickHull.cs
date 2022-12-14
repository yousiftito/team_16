using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public static double distance(Point a, Point b, Point c)
        {
            double distance = 0;
            distance = Math.Sqrt(Math.Pow(c.X - a.X, 2) + Math.Pow(c.Y - a.Y, 2));
            distance = distance + Math.Sqrt(Math.Pow(c.X - b.X, 2) + Math.Pow(c.Y - b.Y, 2));

            return distance;
        }
        public static List<Point> quickhall(List<Point> points, Point mini, Point max, String orientation)
        {
            var O = Enums.TurnType.Right;

            if (orientation == "Left")
            {
                O = Enums.TurnType.Left;
            }

            int indexe = -1;
            double max_distance = 0;

            for (int i = 0; i < points.Count; i++)
            {
                if (HelperMethods.CheckTurn(new Line(mini, max), points[i]) == O && distance(mini, max, points[i]) > max_distance)
                {
                    indexe = i;
                    max_distance = distance(mini, max, points[i]);
                }
            }

            List<Point> answer = new List<Point>();
            if (indexe == -1)
            {
                answer.Add(mini);
                answer.Add(max);
                return answer;
            }
            List<Point> points1 = new List<Point>();
            List<Point> points2 = new List<Point>();
            if (HelperMethods.CheckTurn(new Line(points[indexe], mini), max) == Enums.TurnType.Right)
            {
                points1 = quickhall(points, points[indexe], mini, "Left");

            }
            else
                points1 = quickhall(points, points[indexe], mini, "Right");

            if (HelperMethods.CheckTurn(new Line(points[indexe], max), mini) == Enums.TurnType.Right)
            {
                points2 = quickhall(points, points[indexe], max, "Left");

            }
            else
                points2 = quickhall(points, points[indexe], max, "Right");

            points1.AddRange(points2);

            return points1;
        }


        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            Point Max_x = new Point(double.MinValue, 0);
            Point Mini_x = new Point(double.MaxValue, 0);



            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X < Mini_x.X)
                {
                    Mini_x = points[i];
                }
                if (points[i].X > Max_x.X)
                {
                    Max_x = points[i];
                }
            }
            List<Point> right = new List<Point>();
            List<Point> left = new List<Point>();

            right = quickhall(points, Mini_x, Max_x, "Right");
            left = quickhall(points, Mini_x, Max_x, "Left");

            right.AddRange(left);

            for (int i = 0; i < right.Count; i++)
            {
                if (!outPoints.Contains(right[i]))
                {
                    outPoints.Add(right[i]);
                }
            }


        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
