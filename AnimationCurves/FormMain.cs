using AnimationCurves.Enums;
using AnimationCurves.GraphicalBaseClasses;
using AnimationCurves.GraphicalClasses;
using AnimationCurves.Tools;

namespace AnimationCurves
{
    public partial class FormMain : Form
    {
        private BezierCurve? bezierCurve;
        private BezierCubicSpline? bezierCubicSpline;
        private EnumEditorState state;
        private EnumCurveType curveType;
        private Keys? key;
        private Point? lastLocation = null;
        private readonly Random rand = new();

        public FormMain()
        {
            InitializeComponent();

            state = EnumEditorState.Edit;
            curveType = EnumCurveType.BezierCurve;

            ResetInitialObject();
        }

        private void ResetInitialObject()
        {
            bezierCubicSpline = null;
            bezierCurve = null;

            if (curveType == EnumCurveType.BezierCurve)
            {
                bezierCurve = new();

                for (int i = 0; i < 4; i++)
                    bezierCurve.AddControlPoint(
                    new ControlPoint(MatrixF.BuildPointVector(((float)rand.NextDouble() * CoordTrans.xRange) + CoordTrans.xMin, ((float)rand.NextDouble() * CoordTrans.yRange) + CoordTrans.yMin)));
            }
            else if (curveType == EnumCurveType.BezierCubicSpline)
            {
                bezierCubicSpline = new();
            }

            doubleBufferPanel.Invalidate();
        }

        private void DoubleBufferPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // draw curve if it exists
            bezierCurve?.Draw(g);

            // draw spline if it exists
            bezierCubicSpline?.Draw(g);
        }

        private void DoubleBufferPanel_MouseDown(object sender, MouseEventArgs e)
        {
            #region Bezier curve

            if (curveType == EnumCurveType.BezierCurve)
            {
                if (bezierCurve == null)
                    return;

                if (state == EnumEditorState.Edit)
                {
                    lastLocation = e.Location;

                    int vertexID = bezierCurve.GetVertexIDByUV(e.Location);

                    if (vertexID == CurveBase.ID_INVALID)
                    {
                        bezierCurve.UnselectAllVertices();
                    }
                    else
                    {
                        if (key != Keys.ControlKey)
                            bezierCurve.UnselectAllVertices();

                        bezierCurve.SelectVertexByID(vertexID);
                    }


                    //if (!network.SelectNode(e.Location, ctrlPressed))
                    //{
                    //    network.SelectNode(new Rectangle(), ctrlPressed);
                    //    state = EnumEditorState.SelectBegin;

                    //    if (framedSelectionBox)
                    //        SelectionBoxFramed.InitSelectionBox(e.Location);
                    //    else
                    //        SelectionBox.InitSelectionBox(e.Location);
                    //}
                }
                else if (state == EnumEditorState.InsertNode)
                {
                    MatrixF controlPoint = CoordTrans.FromUVtoXY(e.Location);

                    bezierCurve.AddControlPoint(new ControlPoint(controlPoint));
                }
                else if (state == EnumEditorState.DeleteNode)
                {
                    bezierCurve.Remove(bezierCurve.GetVertexIDByUV(e.Location));
                }
            }
            #endregion

            #region Bezier cubic spline

            if (curveType == EnumCurveType.BezierCubicSpline)
            {
                if (bezierCubicSpline == null)
                    return;

                bezierCubicSpline.ControlPoints.Add(new ControlPoint(CoordTrans.FromUVtoXY(e.Location)));
            }

            #endregion

            doubleBufferPanel.Invalidate();
        }

        private void DoubleBufferPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (state == EnumEditorState.Edit)
            {
                if (lastLocation == null) return;

                var diffX = e.Location.X - lastLocation.Value.X;
                var diffY = e.Location.Y - lastLocation.Value.Y;

                foreach (var index in bezierCurve.SelectedControlPointIndices ?? [])
                {
                    var position = bezierCurve[index];
                    bezierCurve.Move(MatrixF.BuildPointVector(position[0, 0] + diffX, position[1, 0] - diffY), index);
                }

                lastLocation = e.Location;
                doubleBufferPanel.Invalidate();
            }
        }

        private void DoubleBufferPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (state == EnumEditorState.Edit)
            {
                lastLocation = null;
            }
        }

        private void RadioButtonEdit_CheckedChanged(object sender, EventArgs e)
        {
            state = EnumEditorState.Edit;
        }

        private void RadioButtonDeleteNode_CheckedChanged(object sender, EventArgs e)
        {
            state = EnumEditorState.DeleteNode;
        }

        private void RadioButtonInsertNode_CheckedChanged(object sender, EventArgs e)
        {
            state = EnumEditorState.InsertNode;
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            key = e.KeyCode;
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            key = null;
        }

        private void RadioButtonBezierCurve_CheckedChanged(object sender, EventArgs e)
        {
            if (curveType == EnumCurveType.BezierCubicSpline)
            {
                curveType = EnumCurveType.BezierCurve;
                ResetInitialObject();
            }
        }

        private void RadioButtonBezierSpline_CheckedChanged(object sender, EventArgs e)
        {
            if (curveType == EnumCurveType.BezierCurve)
            {
                curveType = EnumCurveType.BezierCubicSpline;
                ResetInitialObject();
            }
        }
    }
}
