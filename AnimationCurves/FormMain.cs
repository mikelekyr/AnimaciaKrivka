using AnimationCurves.GraphicalBaseClasses;
using AnimationCurves.GraphicalClasses;
using AnimationCurves.Tools;

namespace AnimationCurves
{
    public partial class FormMain : Form
    {
        private BezierCurve bezierCurve;

        public FormMain()
        {
            InitializeComponent();

            Random rand = new ();

            // bezierova krivka
            bezierCurve = new BezierCurve();

            for (int i = 0; i < 4; i++)
                bezierCurve.AddControlPoint(
                    new ControlPoint(MatrixF.BuildPointVector(((float)rand.NextDouble() * CoordTrans.xRange) + CoordTrans.xMin, ((float)rand.NextDouble() * CoordTrans.yRange) + CoordTrans.yMin)));

        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void doubleBufferPanel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            bezierCurve.Draw(g);
        }
    }
}
