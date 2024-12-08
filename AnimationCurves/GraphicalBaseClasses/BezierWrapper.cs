using AnimationCurves.GraphicalClasses;

namespace AnimationCurves.GraphicalBaseClasses
{
    public class BezierWrapper(BezierCurve parCurve)
    {
        public BezierCurve Curve { get; set; } = parCurve;
        public ControlPoint? ControlPoint1 { get; set; }
        public ControlPoint? ControlPoint2 { get; set; } 
        public Point CPOffset1 { get; set; }
        public Point CPOffset2 { get; set; }
    }
}
