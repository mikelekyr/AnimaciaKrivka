﻿using AnimationCurves.GraphicalBaseClasses;
using AnimationCurves.Interfaces;
using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalClasses
{
    public class BezierCubicSpline : IDrawable2DObject
    {
        private readonly List<ControlPoint> controlPoints;
        private readonly List<BezierCurve> curves;

        public List<ControlPoint> ControlPoints
        {
            get 
            { 
                return controlPoints; 
            }
        }

        public BezierCubicSpline()
        {
            controlPoints = [];
            curves = [];
        }

        public void Draw(Graphics g)
        {
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

            //foreach (var curve in curves)
            //{
            //    curve.Draw(g);
            //}
        }
    }
}