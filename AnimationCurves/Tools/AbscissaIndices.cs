namespace AnimationCurves.Tools
{
    public sealed class AbscissaIndices(int pointStartID, int pointEndID)
    {
        public int PointStartID { get; set; } = pointStartID;
        public int PointEndID { get; set; } = pointEndID;
    }
}