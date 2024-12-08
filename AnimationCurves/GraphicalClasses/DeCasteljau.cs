using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalClasses
{
    public static class DeCasteljau
    {
        /// <summary>
        /// GetCurvePoint
        /// </summary>
        public static MatrixF GetCurvePoint(List<MatrixF> controlPoints, float t)
        {
            if (controlPoints.Count == 1)
                return controlPoints[0];

            List<MatrixF> nextLevel = [];

            for (int i = 0; i < controlPoints.Count - 1; i++)
            {
                nextLevel.Add(Lerp(controlPoints[i], controlPoints[i + 1], t));
            }

            return GetCurvePoint(nextLevel, t);
        }

        /// <summary>
        /// GetCurvePoints
        /// </summary>
        public static List<MatrixF>? GetCurvePoints(List<MatrixF> bezierControlPoints, int pointCount)
        {
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

                result[i] = GetCurvePoint(bezierControlPoints, time);
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
