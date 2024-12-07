using AnimationCurves.Tools;
using System.Xml.Linq;

namespace AnimationCurves.GraphicalBaseClasses
{
    public abstract class CurveBase
    {
        #region Constants
        public const int ID_INVALID = -1;
        #endregion

        #region Properties

        protected readonly List<ControlPoint> controlPoints;
        protected List<MatrixF> curvePoints;
        protected List<float> segmentLengths;
        protected int curvePrecision = 70;
        protected float length = 0;

        public Point ControlPointOffset { get; set; }

        /// <summary>
        /// Curve length
        /// </summary>
        public float Length { get { return length; } private set { } }

        /// <summary>
        /// Curve precision
        /// </summary>
        public int RenderPrecision
        {
            get
            {
                return curvePrecision;
            }
            set
            {
                if (value < 5 || value > 100)
                    curvePrecision = 70;
                else
                    curvePrecision = value;

                RecalculateCurve();
            }
        }

        /// <summary>
        /// Curve control points
        /// </summary>
        public List<ControlPoint> ControlPoints
        {
            get
            {
                var newList = new List<ControlPoint>(controlPoints.Count);

                foreach (ControlPoint point in controlPoints)
                    newList.Add(new ControlPoint(point.Position, point.Selected));

                return newList;
            }
        }

        /// <summary>
        /// Curve points positions
        /// </summary>
        public List<MatrixF> CurvePointsPositions
        {
            get
            {
                var newList = new List<MatrixF>(controlPoints.Count);

                foreach (ControlPoint point in controlPoints)
                    newList.Add(point.Position);

                return newList;
            }
        }

        /// <summary>
        /// ControlPointIsSelected
        /// </summary>
        public bool ControlPointIsSelected(int controlPointIndex)
        {
            if (SelectedControlPointIndices == null || SelectedControlPointIndices.Length == 0)
                return false;

            foreach (var index in SelectedControlPointIndices)
            {
                if (index == controlPointIndex)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// SelectedControlPointIndex
        /// </summary>
        public int? SelectedControlPointIndex
        {
            get
            {
                if (SelectedControlPointIndices != null && SelectedControlPointIndices.Length > 0)
                    return SelectedControlPointIndices[0];

                return null;
            }
            internal set
            {
                if (value == null)
                    SelectedControlPointIndices = null;
                else
                    SelectedControlPointIndices = [value.Value];
            }
        }

        /// <summary>
        /// SelectedControlPointIndices
        /// </summary>
        public int[]? SelectedControlPointIndices { get; set; } = null;

        /// <summary>
        /// SelectedControlPoint
        /// </summary>
        public ControlPoint? SelectedControlPoint
        {
            get
            {
                if (SelectedControlPointIndex == null)
                    return null;

                return controlPoints[SelectedControlPointIndex.Value];
            }
        }

        /// <summary>
        /// Matrix
        /// </summary>
        public MatrixF this[int index]
        {
            get => controlPoints[index].Position;
            set => controlPoints[index] = new ControlPoint(value, false);
        }

        #endregion

        #region Konstruktor

        /// <summary>
        /// Konstruktor
        /// </summary>
        public CurveBase()
        {
            controlPoints = [];
            curvePoints = [];
            segmentLengths = [];
        }

        #endregion


        /// <summary>
        /// Prepocitanie krivky podla potreby
        /// </summary>
        protected abstract void RecalculateCurve();
        public abstract MatrixF GetPointAndAngleOnCurve(float time, out float angle);

        #region Public methods

        /// <summary>
        /// Add
        /// </summary>
        public void AddControlPoint(ControlPoint point)
        {
            controlPoints.Add(point);

            RecalculateCurve();
        }

        /// <summary>
        /// Insert
        /// </summary>
        public void Insert(int index, ControlPoint point)
        {
            controlPoints.Insert(index, point);

            RecalculateCurve();
        }

        /// <summary>
        /// Remove
        /// </summary>
        public ControlPoint? Remove(int? index)
        {
            if (index == null)
                return null;

            if (controlPoints.Count <= 2)
                return null;    

            ControlPoint removedPoint = controlPoints[index.Value];
            controlPoints.RemoveAt(index.Value);

            RecalculateCurve();

            return removedPoint;
        }

        /// <summary>
        /// Get vertex ID by U, V coordinates or ID_INVALID if not hit
        /// </summary>
        public int GetVertexIDByUV(Point p)
        {
            for (int i = controlPoints.Count - 1; i >= 0; i--)
            {
                if (ControlPoint.IsHitByUV(controlPoints[i], p))
                    return i;
            }
            return ID_INVALID;
        }

        /// <summary>
        /// Move
        /// </summary>
        public void Move(MatrixF p, int? vID)
        {
            if (vID == null)
                return;

            bool s = controlPoints[vID.Value].Selected;
            controlPoints[vID.Value] = new ControlPoint(p, s);

            RecalculateCurve();
        }

        /// <summary>
        /// Select vertex by ID
        /// </summary>
        public void SelectVertexByID(int vID)
        {
            if (controlPoints[vID].Selected == true) return;
            controlPoints[vID].Selected = true;
            SelectedControlPointIndices = [..SelectedControlPointIndices ?? [], vID];
        }

        /// <summary>
        /// UnselectAllVertices
        /// </summary>
        public void UnselectAllVertices()
        {
            foreach (var v in controlPoints) v.Selected = false;
            SelectedControlPointIndices = null;
        }

        /// <summary>
        /// SelectNode
        /// </summary>
        public bool SelectNode(Point mousePosition, bool addSelect = false)
        {
            if (!addSelect)
                controlPoints.ForEach(x => x.Selected = false);

            foreach (var node in controlPoints)
            {
                if (ControlPoint.IsHitByUV(node, mousePosition))
                {
                    node.Selected = true;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// SelectNode
        /// </summary>
        public bool SelectNode(Rectangle selectRectangle, bool addSelect = false)
        {
            if (!addSelect)
                controlPoints.ForEach(x => x.Selected = false);

            if (selectRectangle.IsEmpty)
                return false;

            foreach (var node in controlPoints)
            {
                Point p = new((int)node.Position[0, 0], (int)node.Position[1, 0]);

                if (selectRectangle.Contains(p))
                {
                    node.Selected = true;
                }
            }

            return false;
        }

        /// <summary>
        /// UpdateControlPointsPositionAfterDrag
        /// </summary>
        public void UpdateControlPointsPositionAfterDrag()
        {
            foreach (var node in controlPoints)
            {
                if (node.Selected)
                    node.Position = CoordTrans.PointToMatrixF(ControlPointOffset);
            }
        }

        #endregion
    }
}
