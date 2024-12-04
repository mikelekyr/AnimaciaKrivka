using AnimationCurves.Interfaces;
using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalBaseClasses
{
    public sealed class Rectangle2D : IDrawable2DObject
    {
        private MatrixF position;
        private Size size;
        private Color color = Color.Black;
        private bool border;

        public MatrixF PositionMatrixF { get { return position; } set { position = value; } }
        public Point PositionPoint { get { return MatrixF.GetPoint(position); } set { position = MatrixF.BuildPointVector(value.X, value.Y); } }
        public PointF PositionPointF { get { return MatrixF.GetPoint(position); } set { position = MatrixF.BuildPointVector(value.X, value.Y); } }

        public bool Border { get { return border; } set { border = value; } }
        public Color Color { get { return color; } set { color = value; } }
        public Size Size { get { return size; } set { size = value; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public Rectangle2D(MatrixF position, Size size, bool border = false)
        {
            this.position = position;
            this.size = size;
            this.border = border;
        }

        public void Draw(Graphics g)
        {
            if (size.Width <= 1 || size.Height <= 1)
                throw new ApplicationException($"Too small for rectangle!");

            Point p = CoordTrans.FromXYtoUV(PositionPoint);

            Point point = new(p.X - (int)Math.Round(size.Width / 2.0), p.Y - (int)Math.Round(size.Height / 2.0));
            Rectangle rect = new(point, new Size(size.Width, size.Height));

            using SolidBrush b = new(color);
            g.FillRectangle(b, rect);

            if (border)
                g.DrawRectangle(Pens.Gray, rect);
        }
    }
}
