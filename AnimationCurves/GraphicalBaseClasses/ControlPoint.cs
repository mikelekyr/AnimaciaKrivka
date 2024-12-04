using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalBaseClasses
{
    public sealed class ControlPoint(MatrixF paPosition, bool paSelected = false)
    {
        private MatrixF position = paPosition;
        private bool selected = paSelected;

        public MatrixF Position { get { return position; } set { position = value; } }
        public bool Selected { get { return selected; } set { selected = value; } }
    }
}
