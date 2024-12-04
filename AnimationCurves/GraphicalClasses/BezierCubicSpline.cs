using AnimationCurves.Interfaces;

namespace AnimationCurves.GraphicalClasses
{
    public class BezierCubicSpline : IDrawable2DObject
    {
        private readonly List<BezierCurve> curves;

        public BezierCubicSpline()
        {
            curves = [];
        }

        public void Draw(Graphics g)
        {
            foreach (var curve in curves)
            {
                curve.Draw(g);
            }
        }
    }
}
