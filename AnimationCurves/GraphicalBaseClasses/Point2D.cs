using AnimationCurves.Interfaces;
using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalBaseClasses
{
    public sealed class Point2D : IDrawable2DObject
    {
        private MatrixF position;
        private Color color = Color.Black;
        private bool antialiased;

        public bool Antialiased { get { return antialiased; } set { antialiased = value; } }
        public Color Color { get { return color; } set { color = value; } }

        public MatrixF PositionMatrixF { get { return position; } set { position = value; } }
        public Point PositionPoint { get { return MatrixF.GetPoint(position); } set { position = MatrixF.BuildPointVector(value.X, value.Y); } }
        public PointF PositionPointF { get { return MatrixF.GetPointF(position); } set { position = MatrixF.BuildPointVector(value.X, value.Y); } }

        /// <summary>
        /// Cosntructor 1
        /// </summary>
        /// <param name="position"></param>
        /// <param name="antialiased"></param>
        public Point2D(MatrixF position, bool antialiased = false)
        {
            this.position = position;
            this.antialiased = antialiased;
        }

        /// <summary>
        /// Cosntructor 2
        /// </summary>
        /// <param name="position"></param>
        /// <param name="antialiased"></param>
        public Point2D(Point position, bool antialiased = false)
        {
            this.position = MatrixF.BuildPointVector(position.X, position.Y);
            this.antialiased = antialiased;
        }

        /// <summary>
        /// Constructor 3
        /// </summary>
        /// <param name="position"></param>
        /// <param name="antialiased"></param>
        public Point2D(PointF position, bool antialiased = false)
        {
            this.position = MatrixF.BuildPointVector(position.X, position.Y);
            this.antialiased = antialiased;
        }

        /// <summary>
        /// Draw method
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            if (Antialiased)
            {
                using SolidBrush b = new(color);
                PointF locationF = CoordTrans.FromXYtoUVF(position);

                float xLeft = locationF.X - 1.0f;
                float xRight = locationF.X + 1.0f;
                float yTop = locationF.Y - 1.0f;
                float yBottom = locationF.Y + 1.0f;

                float xLeftOverhang = (float)Math.Ceiling(xLeft) - xLeft;
                float xRightOverhang = xRight - (float)Math.Floor(xRight);
                float yTopOverhang = (float)Math.Ceiling(yTop) - yTop;
                float yBottomOverhang = yBottom - (float)Math.Floor(yBottom);

                int x = (int)locationF.X;
                int y = (int)locationF.Y;

                // Antialiased pixel takes up 3 x 3 grid, with the middle pixel fully opaque. Other pixels are made partially transparent.
                // Upper row
                GetBrushByAlpha(b, xLeftOverhang * yTopOverhang);
                g.FillRectangle(b, x - 1, y - 1, 1, 1);

                GetBrushByAlpha(b, yTopOverhang);
                g.FillRectangle(b, x, y - 1, 1, 1);

                GetBrushByAlpha(b, xRightOverhang * yTopOverhang);
                g.FillRectangle(b, x + 1, y - 1, 1, 1);

                // Middle row
                GetBrushByAlpha(b, xLeftOverhang);
                g.FillRectangle(b, x - 1, y, 1, 1);

                GetBrushByAlpha(b, 1.0f);
                g.FillRectangle(b, x, y, 1, 1);

                GetBrushByAlpha(b, xRightOverhang);
                g.FillRectangle(b, x + 1, y, 1, 1);

                // Lower row
                GetBrushByAlpha(b, xLeftOverhang * yBottomOverhang);
                g.FillRectangle(b, x, y + 1, 1, 1);

                GetBrushByAlpha(b, yBottomOverhang);
                g.FillRectangle(b, x + 1, y + 1, 1, 1);

                GetBrushByAlpha(b, xRightOverhang * yBottomOverhang);
                g.FillRectangle(b, x + 1, y, 1, 1);
            }
            else
            {
                Point p = CoordTrans.FromXYtoUV(PositionPoint);

                using var b = new SolidBrush(Color);
                g.FillRectangle(b, new Rectangle(new Point(p.X - 1, p.Y - 1), new Size(2, 2)));
            }
        }

        /// <summary>
        /// GetBrushByAlpha
        /// </summary>
        /// <param name="b"></param>
        /// <param name="alpha"></param>
        /// <param name="baseColor"></param>
        private void GetBrushByAlpha(SolidBrush b, float alpha)
        {
            int colorAlpha = (int)(alpha * 255);
            b.Color = Color.FromArgb(colorAlpha, color);
        }
    }
}
