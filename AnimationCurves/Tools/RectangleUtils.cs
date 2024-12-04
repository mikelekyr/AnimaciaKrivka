namespace cv1.Tools
{
    public static class RectangleUtils
    {
        [Flags]
        public enum RectangleType
        {
            Basic = 0,
            Fill = 1,
            Border = 2,
            BoundingBox = 4,
            FillBoundingBox = Fill | BoundingBox,
            BorderBoundingBox = Border | BoundingBox
        }

        public static Rectangle BuildRectangle(Point point1, Point point2, RectangleType type)
        {
            // Location of the upper left corner
            var x = Math.Min(point1.X, point2.X);
            var y = Math.Min(point1.Y, point2.Y);

            // Selection area size
            var width = Math.Abs(point1.X - point2.X);
            var height = Math.Abs(point1.Y - point2.Y);

            var rectangle = new Rectangle(x, y, width, height);

            // Adjust the rectangle based on the requested flags

            // Currently, only border rectangles need adjustments
            if (type.HasFlag(RectangleType.Border))
            {
                rectangle.X--;
                rectangle.Y--;

                if (type.HasFlag(RectangleType.BoundingBox))
                {
                    rectangle.Width += 2;
                    rectangle.Height += 2;
                }
                else
                {
                    rectangle.Width++;
                    rectangle.Height++;
                }
            }

            return rectangle;
        }

        public static Rectangle ResizeRectangle(Rectangle rectangle, int allSides)
        {
            return ResizeRectangle(rectangle, top: allSides, right: allSides, bottom: allSides, left: allSides);
        }

        public static Rectangle ResizeRectangle(Rectangle rectangle, int top = 0, int right = 0, int bottom = 0, int left = 0)
        {
            // Top
            rectangle.Y -= top;
            rectangle.Height += top;

            // Bottom
            rectangle.Height += bottom;

            // Left
            rectangle.X -= left;
            rectangle.Width += left;

            // Right
            rectangle.Width += right;

            return rectangle;
        }
    }
}
