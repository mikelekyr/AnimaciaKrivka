using AnimationCurves.GraphicalBaseClasses;
using AnimationCurves.Interfaces;
using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalClasses
{
    public class BezierCubicSpline : IDrawable2DObject
    {
        private readonly List<ControlPoint> controlPoints;
        private readonly List<BezierCurve> curves;

        public BezierCubicSpline()
        {
            controlPoints = [];
            curves = [];
        }

        public void AddControlPoint(ControlPoint controlPoint)
        {
            controlPoints?.Add(controlPoint);

            UpdateBeziers();
        }

        private void UpdateBeziers()
        {
            curves.Clear();

            if (controlPoints.Count < 3)
                return;

            float divConst = 3.0f;

            foreach (var (cp, index) in controlPoints.Select((value, i) => (value, i)))
            {
                BezierCurve bezierCurve = new();

                bool isFirst = index == 0;
                bool isLast = index == controlPoints.Count - 1;

                if (isFirst)
                {
                    bezierCurve.AddControlPoint(controlPoints[index]);

                    float startX = controlPoints[index].Position[0, 0];
                    float startY = controlPoints[index].Position[1, 0];

                    float endX = controlPoints[index + 2].Position[0, 0];
                    float endY = controlPoints[index + 2].Position[1, 0];

                    float refX = controlPoints[index + 1].Position[0, 0];
                    float refY = controlPoints[index + 1].Position[1, 0];

                    float diffX = (endX - startX) / divConst;
                    float diffY = (endY - startY) / divConst;

                    refX -= diffX;
                    refY -= diffY;

                    bezierCurve.AddControlPoint(new ControlPoint(MatrixF.BuildPointVector(refX, refY)));
                    bezierCurve.AddControlPoint(controlPoints[index + 1]);
                }
                else if (isLast)
                {
                    bezierCurve.AddControlPoint(controlPoints[index - 1]);

                    float startX = controlPoints[index - 2].Position[0, 0];
                    float startY = controlPoints[index - 2].Position[1, 0];

                    float endX = controlPoints[index].Position[0, 0];
                    float endY = controlPoints[index].Position[1, 0];

                    float refX = controlPoints[index - 1].Position[0, 0];
                    float refY = controlPoints[index - 1].Position[1, 0];

                    float diffX = (endX - startX) / divConst;
                    float diffY = (endY - startY) / divConst;

                    refX += diffX;
                    refY += diffY;

                    bezierCurve.AddControlPoint(new ControlPoint(MatrixF.BuildPointVector(refX, refY)));
                    bezierCurve.AddControlPoint(controlPoints[index]);
                }
                else if (!isLast && !isFirst && controlPoints.Count > 3)
                {
                    if (index == 1)
                        continue;

                    bezierCurve.AddControlPoint(controlPoints[index - 1]);

                    float startX = controlPoints[index - 2].Position[0, 0];
                    float startY = controlPoints[index - 2].Position[1, 0];

                    float endX = controlPoints[index].Position[0, 0];
                    float endY = controlPoints[index].Position[1, 0];

                    float refX = controlPoints[index - 1].Position[0, 0];
                    float refY = controlPoints[index - 1].Position[1, 0];

                    float diffX = (endX - startX) / divConst;
                    float diffY = (endY - startY) / divConst;

                    refX += diffX;
                    refY += diffY;

                    bezierCurve.AddControlPoint(new ControlPoint(MatrixF.BuildPointVector(refX, refY)));

                    startX = controlPoints[index - 1].Position[0, 0];
                    startY = controlPoints[index - 1].Position[1, 0];

                    endX = controlPoints[index + 1].Position[0, 0];
                    endY = controlPoints[index + 1].Position[1, 0];

                    refX = controlPoints[index].Position[0, 0];
                    refY = controlPoints[index].Position[1, 0];

                    diffX = (endX - startX) / 2.0f;
                    diffY = (endY - startY) / 2.0f;

                    refX -= diffX;
                    refY -= diffY;

                    bezierCurve.AddControlPoint(new ControlPoint(MatrixF.BuildPointVector(refX, refY)));
                    bezierCurve.AddControlPoint(controlPoints[index]);
                }

                if (bezierCurve.ControlPoints.Count >= 3)
                    curves.Add(bezierCurve);
            }
        }

        public void Draw(Graphics g)
        {
            foreach (var curve in curves)
            {
                curve.Draw(g);
            }

            using Font font = new("Arial", 8);

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
                    Rectangle rectSelected = new(rect.X - 2, rect.Y - 2, rect.Width + 4, rect.Height + 4);
                    g.FillRectangle(Brushes.LimeGreen, rectSelected);
                    g.DrawRectangle(Pens.LightGray, rect);
                }

                g.FillRectangle(isFirst || isLast ? Brushes.BlueViolet : Brushes.DarkOrange, rect);
                g.DrawRectangle(Pens.Black, rect);
                g.DrawString(name, font, Brushes.Black, new Point(rect.X + 6, rect.Y + 6));
            }
        }
    }
}
