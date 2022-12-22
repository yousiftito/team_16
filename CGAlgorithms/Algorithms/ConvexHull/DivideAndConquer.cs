using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {

        List<Point> checkIfExistInList(List<Point> P1, List<Point> outp1)
        {
            for (int i = 0; i < P1.Count; ++i)
            {
                if (!outp1.Contains(P1[i]))
                    outp1.Add(P1[i]);
            }
            return outp1;
        }
        public int getMLP(List<Point> a)
        {
            int MLP = 0;

            for (int i = 1; i < a.Count; i++)
            {
                if (a[i].X > a[MLP].X || (a[i].X == a[MLP].X && a[i].Y > a[MLP].Y))
                    MLP = i;
            }
            return MLP;
        }
        public int getMRP(List<Point> b)
        {
            int MRP = 0;
            for (int i = 1; i < b.Count; i++)
            {
                if (b[i].X < b[MRP].X || (b[i].X == b[MRP].X && b[i].Y < b[MRP].Y))
                    MRP = i;
            }
            return MRP;
        }
        public List<Point> merger(List<Point> P1, List<Point> P2)
        {
            List<Point> a = new List<Point>();
            List<Point> b = new List<Point>();

            a = checkIfExistInList(P1, a);
            b = checkIfExistInList(P2, b);
            int MLP = getMLP(a);
            int MRP = getMRP(b);
            int ULP = MLP;
            int URP = MRP;
            bool found = false;
            while (!found)
            {
                found = true;
                while (HelperMethods.CheckTurn(new Line(b[URP].X,
                           b[URP].Y, a[ULP].X, a[ULP].Y),
                           a[(ULP + 1) % a.Count]) == Enums.TurnType.Right)
                {
                    ULP = (ULP + 1) % a.Count;
                    found = false;
                }
                if (found == true &&
                    (HelperMethods.CheckTurn(new Line(b[URP].X, b[URP].Y, a[ULP].X, a[ULP].Y),
                         a[(ULP + 1) % a.Count]) == Enums.TurnType.Colinear))
                    ULP = (ULP + 1) % a.Count;

                while (HelperMethods.CheckTurn(new Line(a[ULP].X, a[ULP].Y, b[URP].X, b[URP].Y), b[(b.Count + URP - 1) % b.Count]) == Enums.TurnType.Left)
                {
                    URP = (b.Count + URP - 1) % b.Count;
                    found = false;
                }
                if (found == true && (HelperMethods.CheckTurn(new Line(a[ULP].X, a[ULP].Y, b[URP].X, b[URP].Y), b[(URP + b.Count - 1) % b.Count]) == Enums.TurnType.Colinear))
                    URP = (URP + b.Count - 1) % b.Count;
            }
            int DLP = MLP;
            int DRP = MRP;
            found = false;

            while (!found)
            {
                found = true;
                while (HelperMethods.CheckTurn(new Line(b[DRP].X, b[DRP].Y, a[DLP].X, a[DLP].Y), a[(DLP + (a.Count) - 1) % (a.Count)]) == Enums.TurnType.Left)
                {
                    DLP = (DLP + (a.Count) - 1) % a.Count;
                    found = false;
                }
                if (found == true &&
                    (HelperMethods.CheckTurn(new Line(b[DRP].X, b[DRP].Y, a[DLP].X, a[DLP].Y),
                         a[(DLP + (a.Count) - 1) % a.Count]) == Enums.TurnType.Colinear))
                    DLP = (DLP + (a.Count) - 1) % a.Count;
                while (HelperMethods.CheckTurn(new Line(a[DLP].X, a[DLP].Y, b[DRP].X, b[DRP].Y), b[(DRP + 1) % b.Count]) == Enums.TurnType.Right)
                {
                    DRP = (DRP + 1) % b.Count;
                    found = false;
                }
                if (found == true && (HelperMethods.CheckTurn(new Line(a[DLP].X, a[DLP].Y, b[DRP].X, b[DRP].Y), b[(DRP + 1) % b.Count]) == Enums.TurnType.Colinear))
                    DRP = (DRP + 1) % b.Count;
            }
            List<Point> out_points = new List<Point>();
            int x = ULP;
            if (!out_points.Contains(a[ULP]))
                out_points.Add(a[ULP]);
            while (x != DLP)
            {
                x = (x + 1) % a.Count;
                if (!out_points.Contains(a[x]))
                {
                    out_points.Add(a[x]);
                }
            }
            x = DRP;
            if (!out_points.Contains(b[DRP]))
                out_points.Add(b[DRP]);

            while (x != URP)
            {
                x = (x + 1) % b.Count;
                if (!out_points.Contains(b[x]))
                    out_points.Add(b[x]);
            }
            return out_points;
        }

        public List<Point> divide(List<Point> a)
        {
            if (a.Count == 1)
            {
                return a;
            }
            List<Point> left = new List<Point>();
            List<Point> right = new List<Point>();
            for (int i = 0; i < a.Count / 2; i++)
                left.Add(a[i]);
            for (int i = a.Count / 2; i < a.Count; i++)
                right.Add(a[i]);

            List<Point> leftt = divide(left);
            List<Point> rightt = divide(right);
            return merger(leftt, rightt);
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            points = points.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
            List<Point> nP = divide(points);
            foreach (Point p in nP)
                if (!outPoints.Contains(p))
                {
                    outPoints.Add(p);
                }
        }
        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}