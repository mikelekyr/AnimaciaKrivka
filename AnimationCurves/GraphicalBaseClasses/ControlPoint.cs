using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalBaseClasses
{
    public sealed class ControlPoint(MatrixF paPosition, bool paSelected = false)
    {
        private MatrixF position = paPosition;
        private bool selected = paSelected;

        public MatrixF Position { get { return position; } set { position = value; } }
        public bool Selected { get { return selected; } set { selected = value; } }

        /// <summary>
        /// Is point hit by U, V coordinates
        /// </summary>
        public static bool IsHitByUV(ControlPoint controlPoint, Point p)
        {
            MatrixF xyMatrix = CoordTrans.FromUVtoXY(p);
            PointF xyPoint = new((float)xyMatrix[0, 0] - 5, (float)xyMatrix[1, 0] - 5);
            RectangleF r = new(xyPoint, new Size(10, 10));
            PointF point = new((float)controlPoint.Position[0, 0], (float)controlPoint.Position[1, 0]);
            return r.Contains(point);
        }
    }
}
