using AnimationCurves.Enums;
using AnimationCurves.GraphicalBaseClasses;
using AnimationCurves.GraphicalClasses;
using AnimationCurves.Tools;
using Microsoft.VisualBasic.Devices;
using System.Net;

namespace AnimationCurves
{
    public partial class FormMain : Form
    {
        private BezierCurve? bezierCurve;
        private BezierCubicSpline? bezierCubicSpline;
        private EnumEditorMode mode;
        private EnumEditorState state;
        private EnumCurveType curveType;
        private Keys? key;
        private Point? lastLocation = null;
        private readonly Random rand = new();
        private Point startMousePos;
        private Point currentMousePos;


        public FormMain()
        {
            InitializeComponent();

            mode = EnumEditorMode.Edit;
            state = EnumEditorState.None;

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

                bezierCurve.DrawAllControls = true; 
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

            if (state == EnumEditorState.Selecting)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                SelectionBoxFramed.Draw(g);
            }
        }

        private void DoubleBufferPanel_MouseDown(object sender, MouseEventArgs e)
        {
            #region Bezier curve

            if (curveType == EnumCurveType.BezierCurve)
            {
                if (bezierCurve == null)
                    return;

                if (state == EnumEditorState.PossibleDrag)
                {
                    state = EnumEditorState.NodeDragging;
                    startMousePos = e.Location;
                }
                if (mode == EnumEditorMode.Edit)
                {
                    bool ctrlPressed = key == Keys.ControlKey;

                    if (!bezierCurve.SelectNode(e.Location, ctrlPressed))
                    {
                        bezierCurve.SelectNode(new Rectangle(), ctrlPressed);

                        state = EnumEditorState.SelectBegin;
                        SelectionBoxFramed.InitSelectionBox(e.Location);
                    }
                }
                else if (mode == EnumEditorMode.InsertNode)
                {
                    MatrixF controlPoint = CoordTrans.FromUVtoXY(e.Location);

                    bezierCurve.AddControlPoint(new ControlPoint(controlPoint));
                }
                else if (mode == EnumEditorMode.DeleteNode)
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

                bezierCubicSpline.AddControlPoint(new ControlPoint(CoordTrans.FromUVtoXY(e.Location)));
            }

            #endregion

            doubleBufferPanel.Invalidate();
        }

        private void DoubleBufferPanel_MouseMove(object sender, MouseEventArgs e)
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

                    lastLocation = e.Location;
                    doubleBufferPanel.Invalidate();
                }
            }
        }

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

                    bezierCurve.ControlPointOffset = new(0, 0);
                }
            }

            state = EnumEditorState.None;
            SelectionBoxFramed.IsActive = false;
            currentMousePos = new(0, 0);
            startMousePos = new(0, 0);

            doubleBufferPanel.Invalidate();
        }

        private void RadioButtonEdit_CheckedChanged(object sender, EventArgs e)
        {
            mode = EnumEditorMode.Edit;
        }

        private void RadioButtonDeleteNode_CheckedChanged(object sender, EventArgs e)
        {
            mode = EnumEditorMode.DeleteNode;
        }

        private void RadioButtonInsertNode_CheckedChanged(object sender, EventArgs e)
        {
            mode = EnumEditorMode.InsertNode;
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
