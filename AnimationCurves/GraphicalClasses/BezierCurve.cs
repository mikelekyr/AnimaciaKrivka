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

                Point2D pt = new Point2D(curvePoints[i])
                {
                    Antialiased = true,
                    Color = Color.Red
                };

                pt.Draw(g);
            }

            // Draw lines between control points for better visibility
            float[] dashValues = { 10, 6 };

            using (Pen pen = new (Color.DarkGray))
            for (int i = 0; i < controlPoints.Count - 1; i++)
            {
                PointF point1 = CoordTrans.FromXYtoUV(controlPoints[i].Position);
                PointF point2 = CoordTrans.FromXYtoUV(controlPoints[i + 1].Position);

                pen.DashPattern = dashValues;
                g.DrawLine(pen, point1, point2);
            }

            for (int i = 0; i < controlPoints.Count; i++)
            {
                Point controlPoint = CoordTrans.FromXYtoUV(controlPoints[i].Position);
                Point point = new(controlPoint.X - 5, controlPoint.Y - 5);
                Rectangle rect = new(point, new Size(10, 10));

                g.FillRectangle(Brushes.DarkOrange, rect);
                g.DrawRectangle(Pens.Black, rect);
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
