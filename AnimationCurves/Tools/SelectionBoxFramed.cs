namespace cv1.Tools
{
    public static class SelectionBoxFramed
    {
        private static Rectangle trackerRectangleFromPreviousStep;
        private static readonly SolidBrush fillBrush;
        private static readonly Pen borderPen;

        public static Point StartingMousePosition;
        public static bool IsActive { get; set; }
        public static Rectangle TrackedRectangle { get { return trackerRectangleFromPreviousStep; } }

        /// <summary>
        /// Constructor
        /// </summary>
        static SelectionBoxFramed()
        {
            fillBrush = new SolidBrush(Color.FromArgb(100, 0, 116, 231));
            borderPen = new Pen(Color.FromArgb(230, 0, 116, 231));
        }

        /// <summary>
        /// GetRect
        /// </summary>
        private static Rectangle GetRect(Point currentMouseLocation)
        {
            Rectangle selectorRectangle = new Rectangle(StartingMousePosition.X,
                                              StartingMousePosition.Y,
                                              currentMouseLocation.X - StartingMousePosition.X,
                                              currentMouseLocation.Y - StartingMousePosition.Y);

            if (selectorRectangle.Width < 0)
                selectorRectangle = new Rectangle(currentMouseLocation.X,
                                        selectorRectangle.Y,
                                        -selectorRectangle.Width,
                                        selectorRectangle.Height);

            if (selectorRectangle.Height < 0)
                selectorRectangle = new Rectangle(selectorRectangle.X,
                                        currentMouseLocation.Y,
                                        selectorRectangle.Width,
                                        -selectorRectangle.Height);

            return selectorRectangle;
        }

        /// <summary>
        /// InitSelectionBox
        /// </summary>
        public static void InitSelectionBox(Point currentMouseLocation)
        {
            StartingMousePosition = currentMouseLocation;
            IsActive = true;
            trackerRectangleFromPreviousStep = new Rectangle(0, 0, -1, -1);
        }

        /// <summary>
        /// Draw
        /// </summary>
        public static void Draw(Graphics g)
        {
            // If mouse button isn't held down...
            if (!IsActive)
            {
                // ... don't render anything
                return;
            }

            // Draws the selection area
            var fillRectangle = trackerRectangleFromPreviousStep;
            g.FillRectangle(fillBrush, fillRectangle);

            // Draws the border around the selection area. Requires a different rectangle than the selection area itself.
            var borderRectangle = new Rectangle(fillRectangle.X - 1, fillRectangle.Y - 1, fillRectangle.Width + 1, fillRectangle.Height + 1);
            g.DrawRectangle(borderPen, borderRectangle);
        }

        /// <summary>
        /// Track
        /// </summary>
        public static Region Track(Point currentMouseLocation)
        {
            Rectangle currentTrackerRectangle = GetRect(currentMouseLocation);

            // Prepare bounding boxes used to calculate the new invalidation region. Requires rectangles that contain both the fill and the border of the selection area.
            var newSelectionArea = RectangleUtils.ResizeRectangle(currentTrackerRectangle, 1);

            // Consider the old selection area only if it is a valid rectangle
            Rectangle? oldSelectionArea = null;
            if (trackerRectangleFromPreviousStep.Width != -1 && trackerRectangleFromPreviousStep.Height != -1)
            {
                oldSelectionArea = RectangleUtils.ResizeRectangle(trackerRectangleFromPreviousStep, 1);
            }

            var staticArea = new Rectangle(trackerRectangleFromPreviousStep.X, trackerRectangleFromPreviousStep.Y, trackerRectangleFromPreviousStep.Width, trackerRectangleFromPreviousStep.Height);
            staticArea.Intersect(currentTrackerRectangle);

            // If the left border didn't move...
            if (oldSelectionArea != null && oldSelectionArea.Value.Left == newSelectionArea.Left)
            {
                // ... enlarge the valid area by 1px to the left. Left border doesn't need to be re-rendered.
                staticArea = RectangleUtils.ResizeRectangle(staticArea, left: 1);
            }

            // If the right border didn't move...
            if (oldSelectionArea != null && oldSelectionArea.Value.Right == newSelectionArea.Right)
            {
                // ... enlarge the valid area by 1px to the right. Right border doesn't need to be re-rendered.
                staticArea = RectangleUtils.ResizeRectangle(staticArea, right: 1);
            }

            // If the top border didn't move...
            if (oldSelectionArea != null && oldSelectionArea.Value.Top == newSelectionArea.Top)
            {
                // ... enlarge the valid area by 1px up. Top border doesn't need to be re-rendered.
                staticArea = RectangleUtils.ResizeRectangle(staticArea, top: 1);
            }

            // If the bottom border didn't move...
            if (oldSelectionArea != null && oldSelectionArea.Value.Bottom == newSelectionArea.Bottom)
            {
                // ... enlarge the valid area by 1px down. Bottom border doesn't need to be re-rendered.
                staticArea = RectangleUtils.ResizeRectangle(staticArea, bottom: 1);
            }

            // Save the current tracker rectangle for future invalidation region calculations
            trackerRectangleFromPreviousStep = currentTrackerRectangle;

            // Build the invalidation region
            var invalidationRegion = new Region(newSelectionArea);

            if (oldSelectionArea != null)
            {
                invalidationRegion.Union(oldSelectionArea.Value);
            }

            invalidationRegion.Xor(staticArea);
            return invalidationRegion;
        }
    }
}
