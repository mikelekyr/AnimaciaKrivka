using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalBaseClasses
{
    public sealed class ControlPoint(MatrixF paPosition, bool paSelected = false)
    {
        private MatrixF position = paPosition;
        private bool selected = paSelected;

        public MatrixF Position 
        { 
            get 
            { 
                return CoordTrans.MatrixFWithPointOffset(position, PositionOffset); 
            } 
            set 
            { 
                position = value; 
            }
        }

        public Point PositionOffset { get; set; }
        public bool Selected { get { return selected; } set { selected = value; } }

        /// <summary>
        /// Is point hit by U, V coordinates
        /// </summary>
        public bool IsHitByUV(Point p)
        {
            MatrixF xyMatrix = CoordTrans.FromUVtoXY(p);
            PointF xyPoint = new((float)xyMatrix[0, 0] - 20, (float)xyMatrix[1, 0] - 20);
            RectangleF r = new(xyPoint, new Size(40, 40));
            PointF point = new((float)Position[0, 0], (float)Position[1, 0]);
            return r.Contains(point);
        }
    }
}
