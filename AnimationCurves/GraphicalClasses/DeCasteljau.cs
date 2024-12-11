using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalClasses
{
    public static class DeCasteljau
    {
        /// <summary>
        /// GetCurvePoint
        /// </summary>
        public static MatrixF GetCurvePoint(List<MatrixF> controlPoints, float t, ref float angle)
        {
            if (controlPoints.Count == 2)
            {
                var pt2 = controlPoints[1];
                var pt1 = controlPoints[0];

                float dY = pt2[1, 0] - pt1[1, 0];
                float dX = pt2[0, 0] - pt1[0, 0];

                angle = (float)Math.Atan2(dY, dX);
            }

            if (controlPoints.Count == 1)
                return controlPoints[0];

            List<MatrixF> nextLevel = [];

            for (int i = 0; i < controlPoints.Count - 1; i++)
            {
                nextLevel.Add(Lerp(controlPoints[i], controlPoints[i + 1], t));
            }

            return GetCurvePoint(nextLevel, t, ref angle);
        }

        /// <summary>
        /// GetCurvePoints
        /// </summary>
        public static List<MatrixF>? GetCurvePoints(List<MatrixF> bezierControlPoints, int pointCount)
        {
            float angle = 0.0f;

            if (pointCount < 2)
                throw new ApplicationException($"Invalid parameter: you must request at least 2 points to be returned from the curve!");

            if (bezierControlPoints == null || bezierControlPoints.Count < 2)
                return null;

            var result = new MatrixF[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                float time;

                if (i == 0)
                    time = 0;
                else
                    time = i / (float)(pointCount - 1);

                result[i] = GetCurvePoint(bezierControlPoints, time, ref angle);
            }

            return new List<MatrixF>(result);
        }

        /// <summary>
        /// Lerp
        /// </summary>
        private static MatrixF Lerp(MatrixF p1, MatrixF p2, float t)
        {
            return new MatrixF(new float[,] {
                { (1 - t) * p1[0,0] + t * p2[0,0] },
                { (1 - t) * p1[1,0] + t * p2[1,0] },
                { 1 }});
        }
    }
}
