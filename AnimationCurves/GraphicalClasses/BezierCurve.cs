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

            using (Pen pen = new (Color.DarkGray))
            for (int i = 0; i < controlPoints.Count - 1; i++)
            {
                PointF point1 = CoordTrans.FromXYtoUV(controlPoints[i].Position);
                PointF point2 = CoordTrans.FromXYtoUV(controlPoints[i + 1].Position);

                pen.DashPattern = dashValues;
                g.DrawLine(pen, point1, point2);
            }

            using Font font = new("Arial", 10);

            // Draw first control point
            Point controlPoint = CoordTrans.FromXYtoUV(controlPoints[0].Position);
            Point point = new Point(controlPoint.X - 5, controlPoint.Y - 5);
            Rectangle rect = new Rectangle(point, new Size(10, 10));
            g.FillRectangle(controlPoints[0].Selected ? Brushes.LimeGreen : Brushes.OrangeRed, rect);
            g.DrawRectangle(Pens.Black, rect);
            g.DrawString("V start", font, Brushes.Black, new Point(rect.X + 10, rect.Y + 10));

            // Draw intermediate control points
            for (int i = 1; i < controlPoints.Count - 1; i++)
            {
                controlPoint = CoordTrans.FromXYtoUV(controlPoints[i].Position);
                point = new Point(controlPoint.X - 5, controlPoint.Y - 5);
                rect = new Rectangle(point, new Size(10, 10));
                g.FillRectangle(controlPoints[i].Selected ? Brushes.LimeGreen : Brushes.DarkOrange, rect);
                g.DrawRectangle(Pens.Black, rect);
                g.DrawString("C" + i.ToString(), font, Brushes.Black, new Point(rect.X + 10, rect.Y + 10));
            }

            // Draw last control point
            controlPoint = CoordTrans.FromXYtoUV(controlPoints[controlPoints.Count - 1].Position);
            point = new Point(controlPoint.X - 5, controlPoint.Y - 5);
            rect = new Rectangle(point, new Size(10, 10));
            g.FillRectangle(controlPoints[controlPoints.Count - 1].Selected ? Brushes.LimeGreen : Brushes.OrangeRed, rect);
            g.DrawRectangle(Pens.Black, rect);
            g.DrawString("V end", font, Brushes.Black, new Point(rect.X + 10, rect.Y + 10));
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
