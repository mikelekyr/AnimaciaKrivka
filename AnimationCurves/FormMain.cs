using AnimationCurves.Enums;
using AnimationCurves.GraphicalBaseClasses;
using AnimationCurves.GraphicalClasses;
using AnimationCurves.GraphicalObjects;
using AnimationCurves.Tools;
using System.Diagnostics;

namespace AnimationCurves
{
    public partial class FormMain : Form
    {
        private readonly Stopwatch stopwatch = new();
        private Airplane? airplane;
        private BezierCurve? bezierCurve;
        private ControlPoint? controlPointSingleEdit;
        private BezierCubicSpline? bezierCubicSpline;
        private EnumEditorMode mode;
        private EnumEditorState state;
        private EnumCurveType curveType;
        private Keys? key;
        private readonly Random rand = new();
        private Point startMousePos;

        public FormMain()
        {
            InitializeComponent();

            mode = EnumEditorMode.InsertNode;
            state = EnumEditorState.None;

            curveType = EnumCurveType.BezierCubicSpline;

            //timerAnimation.Start();
            //stopwatch.Start();

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
                    new ControlPoint(MatrixF.BuildPointVector(((float)rand.NextDouble() * CoordTrans.XRange) + CoordTrans.XMin, ((float)rand.NextDouble() * CoordTrans.YRange) + CoordTrans.YMin)));

                bezierCurve.DrawAllControls = true;
            }
            else if (curveType == EnumCurveType.BezierCubicSpline)
            {
                bezierCubicSpline = new();
            }

            doubleBufferPanel.Invalidate();
        }

        /// <summary>
        /// Panel draw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleBufferPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // draw curve if it exists
            bezierCurve?.Draw(g);

            // draw spline if it exists
            bezierCubicSpline?.Draw(g);

            if (state == EnumEditorState.Selecting)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                SelectionBoxFramed.Draw(g);
            }
            else
            {
                //airplane.Draw(g);
            }
        }

        /// <summary>
        /// Mouse down
        /// </summary>
        private void DoubleBufferPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (curveType == EnumCurveType.BezierCurve)
            {
                if (bezierCurve == null)
                    return;

                if (mode == EnumEditorMode.Edit)
                {
                    bool ctrlPressed = key == Keys.ControlKey;

                    if (state == EnumEditorState.PossibleDrag)
                    {
                        int selectedCP = bezierCurve.GetControlPointIDByUV(e.Location);

                        if (selectedCP != CurveBase.ID_INVALID)
                        {
                            if (!bezierCurve.ControlPointIsSelected(selectedCP))
                            {
                                if (!ctrlPressed)
                                    bezierCurve.UnselectAllVertices();

                                bezierCurve.SelectVertexByID(selectedCP);
                            }
                        }

                        state = EnumEditorState.NodeDragging;
                        startMousePos = e.Location;
                    }
                    else
                    {
                        if (!bezierCurve.SelectNode(e.Location, ctrlPressed))
                        {
                            state = EnumEditorState.SelectBegin;
                            SelectionBoxFramed.InitSelectionBox(e.Location);
                        }
                    }
                }
                else if (mode == EnumEditorMode.InsertNode)
                {
                    MatrixF controlPoint = CoordTrans.FromUVtoXY(e.Location);

                    bezierCurve.AddControlPoint(new ControlPoint(controlPoint));
                }
                else if (mode == EnumEditorMode.DeleteNode)
                {
                    bezierCurve.Remove(bezierCurve.GetControlPointIDByUV(e.Location));
                }
            }

            if (curveType == EnumCurveType.BezierCubicSpline)
            {
                if (bezierCubicSpline == null)
                    return;

                if (mode == EnumEditorMode.Edit)
                {
                    bool ctrlPressed = key == Keys.ControlKey;

                    if (state == EnumEditorState.PossibleDrag)
                    {
                        int selectedCP = bezierCubicSpline.GetControlPointIDByUV(e.Location);

                        if (selectedCP != CurveBase.ID_INVALID)
                        {
                            if (!bezierCubicSpline.ControlPointIsSelected(selectedCP))
                            {
                                if (!ctrlPressed)
                                    bezierCubicSpline.UnselectAllVertices();

                                bezierCubicSpline.SelectVertexByID(selectedCP);
                            }
                        }

                        state = EnumEditorState.NodeDragging;
                        startMousePos = e.Location;
                    }
                    else if (state == EnumEditorState.PossibleDragHair)
                    {
                        bezierCubicSpline.UnselectAllVertices();

                        if (controlPointSingleEdit != null)
                        {
                            controlPointSingleEdit.Selected = true;

                            state = EnumEditorState.NodeDraggingHair;
                            startMousePos = e.Location;
                        }
                    }
                    else
                    {
                        if (!bezierCubicSpline.SelectNode(e.Location, ctrlPressed))
                        {
                            state = EnumEditorState.SelectBegin;
                            SelectionBoxFramed.InitSelectionBox(e.Location);
                        }
                    }
                }
                else if (mode == EnumEditorMode.InsertNode)
                {
                    MatrixF controlPoint = CoordTrans.FromUVtoXY(e.Location);

                    bezierCubicSpline.AddControlPoint(new ControlPointSpline(controlPoint));
                }
                else if (mode == EnumEditorMode.DeleteNode)
                {
                    bezierCubicSpline.Remove(bezierCubicSpline.GetControlPointIDByUV(e.Location));
                }
            }

            doubleBufferPanel.Invalidate();
        }

        /// <summary>
        /// Mouse move
        /// </summary>
        private void DoubleBufferPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mode == EnumEditorMode.Edit)
                {
                    if (curveType == EnumCurveType.BezierCurve)
                    {
                        if (bezierCurve == null)
                            return;

                        if (state == EnumEditorState.NodeDragging)
                        {
                            bezierCurve.ControlPointOffset = new(e.Location.X - startMousePos.X, e.Location.Y - startMousePos.Y);
                        }
                        else if (state == EnumEditorState.Selecting || state == EnumEditorState.SelectBegin)
                        {
                            state = EnumEditorState.Selecting;

                            using Region r = SelectionBoxFramed.Track(e.Location);
                            doubleBufferPanel.Invalidate(r);

                            return;
                        }

                        doubleBufferPanel.Invalidate();
                    }
                    else if (curveType == EnumCurveType.BezierCubicSpline)
                    {
                        if (bezierCubicSpline == null)
                            return;

                        if (state == EnumEditorState.NodeDragging)
                        {
                            bezierCubicSpline.ControlPointOffset = new(e.Location.X - startMousePos.X, e.Location.Y - startMousePos.Y);
                        }
                        else if (state == EnumEditorState.NodeDraggingHair)
                        {
                            if (controlPointSingleEdit != null)
                            {
                                controlPointSingleEdit.PositionOffset = new(e.Location.X - startMousePos.X, e.Location.Y - startMousePos.Y);
                                bezierCubicSpline.RecalculateCurve();
                            }
                        }
                        else if (state == EnumEditorState.Selecting || state == EnumEditorState.SelectBegin)
                        {
                            state = EnumEditorState.Selecting;

                            using Region r = SelectionBoxFramed.Track(e.Location);
                            doubleBufferPanel.Invalidate(r);

                            return;
                        }

                        doubleBufferPanel.Invalidate();
                    }
                }
            }
            else if (e.Button == MouseButtons.None)
            {
                if (curveType == EnumCurveType.BezierCurve)
                {
                    if (bezierCurve == null)
                        return;

                    if (bezierCurve.HoverOverControlPoint(e.Location))
                    {
                        state = EnumEditorState.PossibleDrag;
                        Cursor = Cursors.Hand;
                    }
                    else
                    {
                        state = EnumEditorState.None;
                        Cursor = Cursors.Default;
                    }
                }
                else if (curveType == EnumCurveType.BezierCubicSpline)
                {
                    if (bezierCubicSpline == null)
                        return;

                    controlPointSingleEdit = bezierCubicSpline.HoverOverControlPointSpline(e.Location);
                    
                    if (controlPointSingleEdit == null)
                    {
                        state = EnumEditorState.None;
                        Cursor = Cursors.Default;
                    }
                    else if (controlPointSingleEdit is ControlPointSpline)
                    {
                        state = EnumEditorState.PossibleDrag;
                        Cursor = Cursors.Hand;
                    }
                    else
                    {
                        state = EnumEditorState.PossibleDragHair;
                        Cursor = Cursors.Hand;
                    }
                }
            }
        }

        /// <summary>
        /// Mouse up
        /// </summary>
        private void DoubleBufferPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (mode == EnumEditorMode.Edit)
            {
                if (curveType == EnumCurveType.BezierCurve)
                {
                    if (bezierCurve == null)
                        return;

                    if (state == EnumEditorState.NodeDragging)
                    {
                        bezierCurve.UpdateControlPointsPositionAfterDrag();
                    }
                    else if (state == EnumEditorState.Selecting)
                    {
                        bool ctrlPressed = key == Keys.ControlKey;

                        bezierCurve.SelectNode(SelectionBoxFramed.TrackedRectangle, ctrlPressed);
                    }

                    bezierCurve.ControlPointOffset = new();
                }
                else if (curveType == EnumCurveType.BezierCubicSpline)
                {
                    if (bezierCubicSpline == null)
                        return;

                    if (state == EnumEditorState.NodeDragging)
                    {
                        bezierCubicSpline.UpdateControlPointsPositionAfterDrag();
                    }
                    else if (state == EnumEditorState.NodeDraggingHair)
                    {
                        if (controlPointSingleEdit != null)
                        {
                            controlPointSingleEdit.Position = controlPointSingleEdit.Position;
                            controlPointSingleEdit.PositionOffset = new();  
                            bezierCubicSpline.RecalculateCurve();

                            controlPointSingleEdit.Selected = false;
                            controlPointSingleEdit = null;
                        }
                    }
                    else if (state == EnumEditorState.Selecting)
                    {
                        bool ctrlPressed = key == Keys.ControlKey;

                        bezierCubicSpline.SelectNode(SelectionBoxFramed.TrackedRectangle, ctrlPressed);
                    }

                    bezierCubicSpline.ControlPointOffset = new();
                }
            }

            state = EnumEditorState.None;
            SelectionBoxFramed.IsActive = false;
            startMousePos = new(0, 0);

            doubleBufferPanel.Invalidate();
        }

        /// <summary>
        /// RadioButtonEdit_CheckedChanged
        /// </summary>
        private void RadioButtonEdit_CheckedChanged(object sender, EventArgs e)
        {
            mode = EnumEditorMode.Edit;
        }

        /// <summary>
        /// RadioButtonDeleteNode_CheckedChanged
        /// </summary>
        private void RadioButtonDeleteNode_CheckedChanged(object sender, EventArgs e)
        {
            mode = EnumEditorMode.DeleteNode;
        }

        /// <summary>
        /// RadioButtonInsertNode_CheckedChanged
        /// </summary>
        private void RadioButtonInsertNode_CheckedChanged(object sender, EventArgs e)
        {
            mode = EnumEditorMode.InsertNode;
        }

        /// <summary>
        /// FormMain_KeyDown
        /// </summary>
        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            key = e.KeyCode;
        }

        /// <summary>
        /// FormMain_KeyUp
        /// </summary>
        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            key = null;
        }

        /// <summary>
        /// RadioButtonBezierCurve_CheckedChanged
        /// </summary>
        private void RadioButtonBezierCurve_CheckedChanged(object sender, EventArgs e)
        {
            if (curveType == EnumCurveType.BezierCubicSpline)
            {
                curveType = EnumCurveType.BezierCurve;
                ResetInitialObject();
            }
        }

        /// <summary>
        /// RadioButtonBezierSpline_CheckedChanged
        /// </summary>
        private void RadioButtonBezierSpline_CheckedChanged(object sender, EventArgs e)
        {
            if (curveType == EnumCurveType.BezierCurve)
            {
                curveType = EnumCurveType.BezierCubicSpline;
                ResetInitialObject();
            }
        }

        /// <summary>
        /// TimerAnimation_Tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerAnimation_Tick(object sender, EventArgs e)
        {
            if (bezierCurve == null)
                return;

            double elapsedMS = stopwatch.Elapsed.TotalSeconds % 7;

            float angle = 0.0f;
            var pointOnCurve = DeCasteljau.GetCurvePoint(bezierCurve.CurvePointsPositions, (float)(elapsedMS / 7.0f), ref angle);

            var matrixTranslate = MatrixF.BuildTranslationMatrix(pointOnCurve[0,0], pointOnCurve[1, 0]);
            var matrixScale = MatrixF.BuildScalingMatrix(5.0f, 5.0f);
            var matrixRotate = MatrixF.BuildRotationMatrix(angle);

            airplane?.Transform(matrixTranslate * matrixScale * matrixRotate);

            doubleBufferPanel.Invalidate();
        }
    }
}
