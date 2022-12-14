using CGUtilities;
using System;
using System.Collections.Generic;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        private double getDistance(Point point1, Point point2)
        {
            double distance = (point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y);
            return distance;
        }
        private int assignPoint(Point currentPoint, int nextPosIndex, List<Point> points, int currentPosIndex)
        {
            bool comparePoints = Math.Abs(currentPoint.X - points[nextPosIndex].X) <= Constants.Epsilon
                    && Math.Abs(currentPoint.Y - points[nextPosIndex].Y) <= Constants.Epsilon;
            while (comparePoints && nextPosIndex != currentPosIndex)
            {
                nextPosIndex = (nextPosIndex + 1) % points.Count;
            }
            if (nextPosIndex == currentPosIndex)
            {
                return -1;
            }
            return 0;
        }
        private int checkDirection(Line extremeSegment, Point nextPoint, Point currentPoint, Point tmpPoint)
        {
            Enums.TurnType right = Enums.TurnType.Right;
            Enums.TurnType colinear = Enums.TurnType.Colinear;
            Enums.TurnType turn = HelperMethods.CheckTurn(extremeSegment, tmpPoint);
            if (turn == right)
            {
                return 0;
            }
            else if (turn == colinear)
            {
                double currentDistance = getDistance(currentPoint, nextPoint);
                double tmpDistance = getDistance(currentPoint, tmpPoint);
                if (tmpDistance > currentDistance)
                {
                    return 1;
                }
            }
            return -1;
        }

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            List<Point> convexHullList = new List<Point>();

            // getting the first minimum point:
            int index = 0;
            for (int i = 1; i < points.Count; i++)
                if (points[i].X < points[index].X)
                    index = i;

            // assigning the first point and it's index:
            int minimumPointIndex = index;
            Point currentPoint = points[minimumPointIndex];
            int currentPosIndex = minimumPointIndex;
            convexHullList.Add(currentPoint);

            while (true)
            {
                // get next point: 
                int nextPosIndex = (currentPosIndex + 1) % points.Count;
                int assign = assignPoint(currentPoint, nextPosIndex, points, currentPosIndex);
                if (assign == -1) { break; }


                // assigning next point:
                Point nextPoint = points[nextPosIndex];
                Line extremeSegment = new Line(currentPoint, nextPoint);

                // checking direction:
                for (int i = 0; i < points.Count; i++)
                {
                    Point tmpPoint = points[i];
                    int dir = checkDirection(extremeSegment, nextPoint, currentPoint, tmpPoint);
                    if (dir == 0 || dir == 1)
                    {
                        nextPosIndex = i;
                        nextPoint = tmpPoint;
                        extremeSegment.End = tmpPoint;
                    }
                }

                if (nextPosIndex == minimumPointIndex)
                {
                    break;
                }

                convexHullList.Add(nextPoint);
                currentPoint = nextPoint;
                currentPosIndex = nextPosIndex;
            }
            outPoints = convexHullList;
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
