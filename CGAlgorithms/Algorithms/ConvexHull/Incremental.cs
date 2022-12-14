using CGUtilities;
using CGUtilities.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        private Point center, baseVector;
        private OrderedSet<Point> convexHull;
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count > 3)
            {
                convexHull = new OrderedSet<Point>(new Comparison<Point>(comparePoints));
                center = (points[0] + points[1] + points[2]) / 3;
                baseVector = center.Vector(new Point(center.X + 100, center.Y));
                for (int i = 0; i < 3; i++)
                    convexHull.Add(points[i]);
                for (int i = 3; i < points.Count; i++)
                {
                    Point p = points[i];
                    int index = convexHull.IndexOf(p);
                    //There is a point that has the same angle of another point
                    if (index != -1)
                    {
                        //Keep the Longest Vector
                        if (p.Magnitude() > convexHull[index].Magnitude())
                            convexHull.Add(p);
                    }
                    else
                    {
                        Point pre = getPointFromPair(convexHull.DirectUpperAndLower(p), Enums.TurnType.Right),
                            next = getPointFromPair(convexHull.DirectUpperAndLower(p), Enums.TurnType.Left);
                        //The point is outside the polygon
                        if (HelperMethods.CheckTurn(pre.Vector(next), next.Vector(p)) == Enums.TurnType.Right)
                        {
                            List<Point> removingList = new List<Point>();

                            //Left Supporting Line
                            determineSupportingLine(p, pre, removingList, Enums.TurnType.Right);

                            //Right Supporting Line
                            determineSupportingLine(p, next, removingList, Enums.TurnType.Left);

                            convexHull.RemoveMany(removingList);
                            convexHull.Add(p);
                        }
                    }

                }
                outPoints.AddRange(convexHull.ToList());
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                    outPoints.Add(points[i]);
            }
        }

        private void determineSupportingLine(Point target, Point suppPoint, List<Point> removingList, Enums.TurnType opositeTurn)
        {
            Point newSuppPoint = getPointFromPair(convexHull.DirectUpperAndLower(suppPoint), opositeTurn);
            Enums.TurnType turn = HelperMethods.CheckTurn(target.Vector(suppPoint), suppPoint.Vector(newSuppPoint));
            while (turn != opositeTurn)
            {
                removingList.Add(suppPoint);
                suppPoint = newSuppPoint;
                newSuppPoint = getPointFromPair(convexHull.DirectUpperAndLower(suppPoint), opositeTurn);
                turn = HelperMethods.CheckTurn(target.Vector(suppPoint), suppPoint.Vector(newSuppPoint));
            }
        }

        private Point getPointFromPair(KeyValuePair<Point, Point> valuePair, Enums.TurnType opositeTurn)
        {
            switch (opositeTurn)
            {
                case Enums.TurnType.Left:
                    return valuePair.Key != null ? valuePair.Key : convexHull.First();

                case Enums.TurnType.Right:
                    return valuePair.Value != null ? valuePair.Value : convexHull.Last();

                default:
                    return null;
            }
        }

        private int comparePoints(Point p1, Point p2)
        {
            Point centerToP1 = center.Vector(p1), centerToP2 = center.Vector(p2);
            double p1Angle = calculateAngle(centerToP1), p2Angle = calculateAngle(centerToP2);
            if (p1Angle < p2Angle)
                return -1;
            else if (p1Angle > p2Angle)
                return 1;
            else
                return 0;
        }

        private double calculateAngle(Point target)
        {
            double dotProduct = target.X * baseVector.X + target.Y * baseVector.Y;
            double denominator = target.Magnitude() * baseVector.Magnitude();
            double cosResult = denominator != 0 ? dotProduct / denominator : 0;
            Enums.Quarter quarter = determineQuarter(target);
            switch (quarter)
            {
                case Enums.Quarter.Second:
                    return 180 - (Math.Acos(Math.Abs(cosResult)) * 180 / Math.PI);

                case Enums.Quarter.Third:
                    return 180 + (Math.Acos(Math.Abs(cosResult)) * 180 / Math.PI);

                case Enums.Quarter.Forth:
                    return 360 - (Math.Acos(Math.Abs(cosResult)) * 180 / Math.PI);

                default:
                    return Math.Acos(cosResult) * 180 / Math.PI;

            }
        }

        private Enums.Quarter determineQuarter(Point vector)
        {
            if (vector.X > 0)
            {
                if (vector.Y > 0)
                    return Enums.Quarter.First;
                else
                    return Enums.Quarter.Forth;
            }
            else
            {
                if (vector.Y > 0)
                    return Enums.Quarter.Second;
                else
                    return Enums.Quarter.Third;
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }

        private void exportPoints(List<Point> points)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.cgds");
            string content = "BeginSection:Points\n";
            for (int i = 0; i < points.Count; i++)
            {
                content += $"{points[i].X},{points[i].Y}\n";
            }
            content += @"EndSection:Points
BeginSection:Lines
EndSection:Lines
BeginSection:Polygons
EndSection:Polygons";
            File.WriteAllText(path, content);
        }
    }
}
