using AnimationCurves.Interfaces;
using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalBaseClasses
{
    public sealed class Line2D : IDrawable2DObject
    {
        MatrixF pointStart;
        MatrixF pointEnd;
        Color color;

        public MatrixF PointStart { get { return pointStart; } set { pointStart = value; } }
        public MatrixF PointEnd { get { return pointEnd; } set { pointEnd = value; } }
        public Color Color { get { return color; } set { color = value; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pointStart"></param>
        /// <param name="pointEnd"></param>
        public Line2D(MatrixF pointStart, MatrixF pointEnd)
        {
            this.pointStart = pointStart;
            this.pointEnd = pointEnd;
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            Point pStart = CoordTrans.FromXYtoUV(pointStart);
            Point pEnd = CoordTrans.FromXYtoUV(pointEnd);

            using Pen p = new (color);
            g.DrawLine(p, pStart, pEnd);
        }
    }
}
