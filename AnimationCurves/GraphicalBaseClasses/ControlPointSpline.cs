using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalBaseClasses
{
    public sealed class ControlPointSpline(MatrixF paPosition, bool paSelected = false) : ControlPoint(paPosition, paSelected)
    {
        public CurveSegment? Next { get; set; }
        public CurveSegment? Previous { get; set; }

        public ControlPoint? NextControlPoint { get; set; }
        public ControlPoint? PreviousControlPoint { get; set; }
    }
}
