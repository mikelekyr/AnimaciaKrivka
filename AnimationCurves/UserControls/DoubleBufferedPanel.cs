using System.Windows.Forms;

namespace AnimationCurves.UserControls
{
    public class DoubleBufferPanel : Panel
    {
        public DoubleBufferPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | //Do not erase the background, reduce flicker
                 ControlStyles.OptimizedDoubleBuffer | //Double buffering
                 ControlStyles.UserPaint, //Use a custom redraw event to reduce flicker
                 true);
        }
    }
}
