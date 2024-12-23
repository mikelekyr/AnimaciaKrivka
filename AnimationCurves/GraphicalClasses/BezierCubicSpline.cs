﻿using AnimationCurves.GraphicalBaseClasses;
using AnimationCurves.Interfaces;
using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalClasses
{
    public class BezierCubicSpline : CurveBase, IDrawable2DObject
    {
        /// <summary>
        /// RecalculateCurve
        /// </summary>
        public override void RecalculateCurve()
        {
            if (controlPoints.Count < 3)
            {
                return;
            }

            float divConst = 5.0f;

            var cpsArray = controlPoints.Cast<ControlPointSpline>().ToArray();

            foreach (var (cp, index) in cpsArray.Select((value, i) => (value, i)))
            {
                bool isFirst = index == 0;
                bool isLast = index == controlPoints.Count - 1;
                bool isBeforeLast = index == controlPoints.Count - 2;

                if (isFirst)
                {
                    if (cpsArray[index].Next != null)
                    {
                        cpsArray[index].Next?.Curve.RecalculateCurve();
                        continue;
                    }

                    var segment = new CurveSegment(new BezierCurve());
                    cpsArray[index].Next = segment;
                    cpsArray[index + 1].Previous = segment;

                    segment.Curve.AddControlPoint(controlPoints[index]);

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

                    var cp1 = new ControlPoint(MatrixF.BuildPointVector(refX, refY));
                    segment.Curve.AddControlPoint(cp1);
                    cpsArray[index].NextControlPoint = cp1;

                    var cp2 = new ControlPoint(MatrixF.BuildPointVector(refX, refY));
                    segment.Curve.AddControlPoint(cp2);
                    cpsArray[index + 1].PreviousControlPoint = cp2;

                    segment.Curve.AddControlPoint(controlPoints[index + 1]);
                }
                else if (isLast)
                {
                    if (cpsArray[index].Previous != null)
                    {
                        cpsArray[index].Previous?.Curve.RecalculateCurve();
                        continue;
                    }

                    var segment = new CurveSegment(new BezierCurve());
                    cpsArray[index].Next = null;
                    cpsArray[index].Previous = segment;

                    segment.Curve.AddControlPoint(controlPoints[index - 1]);

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

                    var cp1 = new ControlPoint(MatrixF.BuildPointVector(refX, refY));
                    segment.Curve.AddControlPoint(cp1);
                    cpsArray[index - 1].NextControlPoint = cp1;

                    var cp2 = new ControlPoint(MatrixF.BuildPointVector(refX, refY));
                    segment.Curve.AddControlPoint(cp2);
                    cpsArray[index].PreviousControlPoint = cp2;

                    segment.Curve.AddControlPoint(controlPoints[index]);
                }
                else if (!isLast && !isFirst && !isBeforeLast && controlPoints.Count > 3)
                {
                    if (cpsArray[index].Next != null)
                    {
                        cpsArray[index].Next?.Curve.RecalculateCurve();
                        continue;
                    }

                    var segment = new CurveSegment(new BezierCurve());
                    cpsArray[index].Next = segment;
                    cpsArray[index + 1].Previous = segment;

                    segment.Curve.AddControlPoint(controlPoints[index]);

                    float startX = controlPoints[index - 1].Position[0, 0];
                    float startY = controlPoints[index - 1].Position[1, 0];

                    float endX = controlPoints[index + 1].Position[0, 0];
                    float endY = controlPoints[index + 1].Position[1, 0];

                    float refX = controlPoints[index].Position[0, 0];
                    float refY = controlPoints[index].Position[1, 0];

                    float diffX = (endX - startX) / divConst;
                    float diffY = (endY - startY) / divConst;

                    refX += diffX;
                    refY += diffY;

                    var cp1 = new ControlPoint(MatrixF.BuildPointVector(refX, refY));
                    segment.Curve.AddControlPoint(cp1);
                    cpsArray[index].NextControlPoint = cp1;

                    startX = controlPoints[index].Position[0, 0];
                    startY = controlPoints[index].Position[1, 0];

                    endX = controlPoints[index + 2].Position[0, 0];
                    endY = controlPoints[index + 2].Position[1, 0];

                    refX = controlPoints[index + 1].Position[0, 0];
                    refY = controlPoints[index + 1].Position[1, 0];

                    diffX = (endX - startX) / divConst;
                    diffY = (endY - startY) / divConst;

                    refX -= diffX;
                    refY -= diffY;

                    var cp2 = new ControlPoint(MatrixF.BuildPointVector(refX, refY));
                    segment.Curve.AddControlPoint(cp2);
                    cpsArray[index + 1].PreviousControlPoint = cp2;

                    segment.Curve.AddControlPoint(controlPoints[index + 1]);
                }
            }
        }

        /// <summary>
        /// Draw
        /// </summary>
        public override void Draw(Graphics g)
        {
            foreach (var (cp, index) in controlPoints.Select((value, i) => (value, i)))
            {
                bool isLast = index == controlPoints.Count - 1;

                var cps = (cp as ControlPointSpline);

                if (isLast)
                    cps?.Previous?.Curve.Draw(g);
                else
                    cps?.Next?.Curve.Draw(g);
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


        /// <summary>
        /// HoverOverControlPoint
        /// </summary>
        public ControlPoint? HoverOverControlPointSpline(Point mousePosition)
        {
            var cpsArray = controlPoints.Cast<ControlPointSpline>().ToArray();

            foreach (var node in cpsArray)
            {
                if (node.IsHitByUV(mousePosition))
                    return node; 

                if (node.NextControlPoint != null && node.NextControlPoint.IsHitByUV(mousePosition))
                    return node.NextControlPoint;

                if (node.PreviousControlPoint != null && node.PreviousControlPoint.IsHitByUV(mousePosition))
                    return node.PreviousControlPoint; 
            }

            return null;
        }

        /// <summary>
        /// GetPointAndAngleOnCurve
        /// </summary>
        public override MatrixF GetPointAndAngleOnCurve(float time, out float angle)
        {
            throw new NotImplementedException();
        }
    }
}
