namespace AnimationCurves.Tools
{
    public static class CoordTrans
    {
        private static readonly float xMin = 0f;
        private static readonly float xMax = 2800f;
        private static readonly float yMin = 0f;
        private static readonly float yMax = 2070f;

        private static readonly int uMin = 0;
        private static readonly int uMax = 1120;
        private static readonly int vMin = 828;
        private static readonly int vMax = 0;

        public static float XRange { get { return Math.Abs(xMax - xMin); } }
        public static float YRange { get { return Math.Abs(yMax - yMin); } }
        public static float URange { get { return Math.Abs(uMax - uMin); } }
        public static float VRange { get { return Math.Abs(vMax - vMin); } }

        public static float XMin { get { return xMin; } }
        public static float YMin { get { return yMin; } }
        public static float XMax { get { return xMax; } }
        public static float YMax { get { return yMax; } }

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
        /// Transformation from window coord (U,V) to world coord (X,Y)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static PointF FromUVtoXYF(Point p)
        {
            return new PointF((p.X - xMin) / (xMax - xMin) * (uMax - uMin) + uMin,
                              (p.Y - yMin) / (yMax - yMin) * (vMax - vMin) + vMin);
        }

        /// <summary>
        /// Transformation from float window coord (U,V) to world coord (X,Y)
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Transformed MatrixF vector</returns>
        public static MatrixF FromUVtoXY(PointF p)
        {
            return new MatrixF(new float[,] {
                { ((float)p.X - uMin) / (uMax - uMin) * (xMax - xMin) + xMin },
                { ((float)p.Y - vMin) / (vMax - vMin) * (yMax - yMin) + yMin },
                { 1 }});
        }

        /// <summary>
        /// Convert Point to MatrixF vector
        /// </summary>
        /// <param name="p">Point</param>
        /// <returns>MatrixF vector</returns>
        public static MatrixF PointToMatrixF(Point p)
        {
            return new MatrixF(new float[,] {
                { p.X },
                { p.Y },
                { 1 }});
        }

        /// <summary>
        /// MatrixFWithPointOffset
        /// </summary>
        /// <param name="p">Point</param>
        /// <returns>MatrixF vector</returns>
        public static MatrixF MatrixFWithPointOffset(MatrixF point, Point offset)
        {
            float ratioX = (uMax - uMin) / (xMax - xMin);
            float ratioY = (vMax - vMin) / (yMax - yMin);

            return new MatrixF(new float[,] {
                { point[0,0] + (offset.X / ratioX)},
                { point[1,0] + (offset.Y / ratioY)},
                { 1 }});
        }
    }
}
