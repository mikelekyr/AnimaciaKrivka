using AnimationCurves.GraphicalBaseClasses;
using AnimationCurves.Interfaces;
using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalClasses
{
    public sealed class BezierCurve : CurveBase, IDrawable2DObject
    {
        public void Draw(Graphics g)
        {
            // draw bezier curve points
            for (int i = 0; i < curvePoints.Count - 1; i++)
            {
                PointF point1 = CoordTrans.FromXYtoUVF(curvePoints[i]);
                PointF point2 = CoordTrans.FromXYtoUVF(curvePoints[i + 1]);

                g.DrawLine(Pens.Pink, point1, point2);

                Point2D pt = new(curvePoints[i])
                {
                    Antialiased = true,
                    Color = Color.Red
                };

                pt.Draw(g);
            }

            // Draw lines between control points for better visibility
            float[] dashValues = { 10, 6 };

            using (Pen pen = new(Color.DarkGray))
                for (int i = 0; i < controlPoints.Count - 1; i++)
                {
                    PointF point1 = CoordTrans.FromXYtoUV(controlPoints[i].Position);
                    PointF point2 = CoordTrans.FromXYtoUV(controlPoints[i + 1].Position);

                    pen.DashPattern = dashValues;
                    g.DrawLine(pen, point1, point2);
                }

            using Font font = new("Arial", 10);

            foreach (var (cp, index) in controlPoints.Select((value, i) => (value, i)))
            {
                bool isFirst = index == 0;
                bool isLast = index == controlPoints.Count - 1;

                string name = "C" + index.ToString();

                if (isFirst)
                    name = "Start";
                else if (isLast)
                    name = "End";

                Point position = CoordTrans.FromXYtoUV(cp.Position);
                Point point = new(position.X - 4, position.Y - 4);
                Rectangle rect = new(point, new Size(8, 8));

                if (cp.Selected)
                {
                    Rectangle rectSelected = new(rect.X - 3, rect.Y - 3, rect.Width + 6, rect.Height + 6);
                    g.FillRectangle(Brushes.LimeGreen, rectSelected);
                    g.DrawRectangle(Pens.LightGray, rect);
                }

                g.FillRectangle(isFirst || isLast ? Brushes.BlueViolet : Brushes.DarkOrange, rect);
                g.DrawRectangle(Pens.Black, rect);
                g.DrawString(name, font, Brushes.Black, new Point(rect.X + 6, rect.Y + 6));
            }
        }


        public override MatrixF GetPointAndAngleOnCurve(float time, out float angle)
        {
            throw new NotImplementedException();
        }

        protected override void RecalculateCurve()
        {
            curvePoints = DeCasteljau.GetCurvePoints(CurvePointsPositions, curvePrecision);
        }
    }
}
