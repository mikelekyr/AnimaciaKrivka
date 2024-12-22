using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalBaseClasses
{
    public sealed class ControlPointSpline(MatrixF paPosition, bool paSelected = false) : ControlPoint(paPosition, paSelected)
    {
        private CurveSegment? next;
        private CurveSegment? previous;

        public CurveSegment? Next { get { return next; } set { next = value; } }
        public CurveSegment? Previous { get { return previous; } set { previous = value; } }
    }
}
