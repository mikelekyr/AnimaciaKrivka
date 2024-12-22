using AnimationCurves.GraphicalBaseClasses;
using AnimationCurves.Interfaces;
using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalClasses
{
    public sealed class BezierCurve : CurveBase
    {
        public bool DrawAllControls = false;

        /// <summary>
        /// Draw
        /// </summary>
        public override void Draw(Graphics g)
        {
            if (curvePoints != null)
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
            }

            // Draw lines between control points for better visibility
            float[] dashValues = [8, 4];

            using Pen pen = new(Color.DarkGray);

            if (DrawAllControls)
            {
                for (int i = 0; i < controlPoints.Count - 1; i++)
                {
                    PointF point1 = CoordTrans.FromXYtoUV(controlPoints[i].Position);
                    PointF point2 = CoordTrans.FromXYtoUV(controlPoints[i + 1].Position);

                    pen.DashPattern = dashValues;
                    g.DrawLine(pen, point1, point2);
                }
            }
            else
            {
                PointF point1 = CoordTrans.FromXYtoUV(controlPoints[0].Position);
                PointF point2 = CoordTrans.FromXYtoUV(controlPoints[1].Position);

                pen.DashPattern = dashValues;
                g.DrawLine(pen, point1, point2);

                point1 = CoordTrans.FromXYtoUV(controlPoints[^1].Position);
                point2 = CoordTrans.FromXYtoUV(controlPoints[^2].Position);

                pen.DashPattern = dashValues;
                g.DrawLine(pen, point1, point2);
            }

            using Font font = new("Arial", 8);

            foreach (var (cp, index) in controlPoints.Select((value, i) => (value, i)))
            {
                bool isFirst = index == 0;
                bool isLast = index == controlPoints.Count - 1;

                if (!DrawAllControls && (isFirst || isLast))
                    continue;

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

                g.FillRectangle(isFirst || isLast ? Brushes.BlueViolet : Brushes.LightSlateGray, rect);
                g.DrawRectangle(Pens.Black, rect);

                if (DrawAllControls)
                    g.DrawString(name, font, Brushes.Black, new Point(rect.X + 6, rect.Y + 6));
            }
        }

        /// <summary>
        /// GetPointAndAngleOnCurve
        /// </summary>
        public override MatrixF GetPointAndAngleOnCurve(float time, out float angle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// RecalculateCurve
        /// </summary>
        protected override void RecalculateCurve()
        {
            curvePoints = DeCasteljau.GetCurvePoints(CurvePointsPositions, curvePrecision);
        }
    }
}
