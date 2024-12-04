namespace AnimationCurves.Tools
{
    public static class CoordTrans
    {
        public static float xMin = -296.0f;
        public static float xMax = 296.0f;
        public static float yMin = -225.0f;
        public static float yMax = 225.0f;

        public static int uMin = 0;
        public static int uMax = 606;
        public static int vMin = 450;
        public static int vMax = 0;

        public static float xRange { get { return Math.Abs(xMax - xMin); } }
        public static float yRange { get { return Math.Abs(yMax - yMin); } }
        public static float uRange { get { return Math.Abs(uMax - uMin); } }
        public static float vRange { get { return Math.Abs(vMax - vMin); } }

        /// <summary>
        /// Transforms world coordinates to screen coordinates
        /// </summary>
        /// <param name="worldPoint">Point in world coordinates</param>
        /// <returns>Transformed point in screen coordinates</returns>
        public static Point FromXYtoUV(PointF worldPoint)
        {
            return new Point(
                (int)((worldPoint.X - xMin) / (xMax - xMin) * (uMax - uMin)) + uMin,
                (int)((worldPoint.Y - yMin) / (yMax - yMin) * (vMax - vMin)) + vMin
            );
        }

        /// <summary>
        /// Woorld coordinates to window coordinates
        /// </summary>
        /// <param name="worldPoint"></param>
        /// <returns></returns>
        public static PointF FromXYtoUVF(PointF worldPoint)
        {
            return new PointF(
                (worldPoint.X - xMin) / (xMax - xMin) * (uMax - uMin) + uMin,
                (worldPoint.Y - yMin) / (yMax - yMin) * (vMax - vMin) + vMin
            );
        }

        /// <summary>
        /// Transformation from world coord (X,Y) to window coord (U,V)
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public static Point FromXYtoUV(MatrixF v)
        {
            if (v == null || v.Rows != 3 || v.Columns != 1)
                throw new ApplicationException($"Wrong vertex data input!");

            return new Point((int)Math.Round((v[0, 0] - xMin) / (xMax - xMin) * (uMax - uMin)) + uMin,
                             (int)Math.Round((v[1, 0] - yMin) / (yMax - yMin) * (vMax - vMin)) + vMin);
        }

        /// <summary>
        /// Transformation from world coord (X,Y) to window coord (U,V) as float
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public static PointF FromXYtoUVF(MatrixF v)
        {
            if (v == null || v.Rows != 3 || v.Columns != 1)
                throw new ApplicationException($"Wrong vertex data input!");

            return new PointF((v[0, 0] - xMin) / (xMax - xMin) * (uMax - uMin) + uMin,
                              (v[1, 0] - yMin) / (yMax - yMin) * (vMax - vMin) + vMin);
        }

        /// <summary>
        /// Transformation from window coord (U,V) to world coord (X,Y)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static MatrixF FromUVtoXY(Point p)
        {
            return new MatrixF(new float[,] {
                { ((float)p.X - uMin) / (uMax - uMin) * (xMax - xMin) + xMin },
                { ((float)p.Y - vMin) / (vMax - vMin) * (yMax - yMin) + yMin },
                { 1 }});
        }

        /// <summary>
        /// Transformation from float window coord (U,V) to world coord (X,Y)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static MatrixF FromUVtoXY(PointF p)
        {
            return new MatrixF(new float[,] {
                { ((float)p.X - uMin) / (uMax - uMin) * (xMax - xMin) + xMin },
                { ((float)p.Y - vMin) / (vMax - vMin) * (yMax - yMin) + yMin },
                { 1 }});
        }
    }
}
