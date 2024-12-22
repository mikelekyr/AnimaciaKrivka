namespace AnimationCurves.GraphicalBaseClasses
{
    public class CurveSegment(CurveBase segment)
    {
        public CurveBase Curve { get; set; } = segment;
        public Point CPOffset1 { get; set; }
        public Point CPOffset2 { get; set; }
    }
}
