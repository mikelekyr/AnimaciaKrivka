using AnimationCurves.GraphicalBaseClasses;
using AnimationCurves.Interfaces;
using AnimationCurves.Tools;

namespace AnimationCurves.GraphicalObjects
{
    public sealed class Airplane : IDrawable2DObject
    {
        private readonly MatrixF[] vertexBufferOrig;
        private MatrixF[] vertexBuffer;
        private readonly AbscissaIndices[] indexBuffer;

        public Airplane()
        {
            vertexBufferOrig =
                [
                new (new float[,] { { -1, 8 , 1} }, transpose: true),// V1
				new (new float[,] { {  1, 8 , 1} }, transpose: true),// V2
				new (new float[,] { {  2, 1 , 1} }, transpose: true),// V3
				new (new float[,] { {  4, 1 , 1} }, transpose: true),// V4
				new (new float[,] { {  5, 1 , 1} }, transpose: true),// V5
				new (new float[,] { {  5, -1, 1} }, transpose: true),// V6
				new (new float[,] { {  4, -1, 1} }, transpose: true),// V7
				new (new float[,] { {  2, -1, 1} }, transpose: true),// V8
				new (new float[,] { {  1, -8, 1} }, transpose: true),// V9
				new (new float[,] { { -1, -8, 1} }, transpose: true),// V10
				new (new float[,] { { -2, -1, 1} }, transpose: true),// V11
				new (new float[,] { { -5, -1, 1} }, transpose: true),// V12
				new (new float[,] { { -5, -3, 1} }, transpose: true),// V13
				new (new float[,] { { -6, -3, 1} }, transpose: true),// V14
				new (new float[,] { { -6, 3 , 1} }, transpose: true),// V15
				new (new float[,] { { -5, 3 , 1} }, transpose: true),// V16
				new (new float[,] { { -5, 1 , 1} }, transpose: true),// V17
				new (new float[,] { { -2, 1 , 1} }, transpose: true) // V18
				];

            indexBuffer =
            [
                new AbscissaIndices(0,1),	//U1
				new AbscissaIndices(1,2),	//U2
				new AbscissaIndices(2,3),	//U3
				new AbscissaIndices(3,4),	//U4
				new AbscissaIndices(4,5),	//U5
				new AbscissaIndices(5,6),	//U6
				new AbscissaIndices(3,6),	//U7
				new AbscissaIndices(6,7),	//U8
				new AbscissaIndices(7,8),	//U9
				new AbscissaIndices(8,9),	//U10
				new AbscissaIndices(9,10),	//U11
				new AbscissaIndices(10,11),	//U12
				new AbscissaIndices(11,12),	//U13
				new AbscissaIndices(12,13),	//U14
				new AbscissaIndices(13,14),	//U15
				new AbscissaIndices(14,15),	//U16
				new AbscissaIndices(15,16),	//U17
				new AbscissaIndices(16,17),	//U18
				new AbscissaIndices(17,0),	//U19
			];

            vertexBuffer = vertexBufferOrig;
        }

        /// <summary>
        /// Transform
        /// </summary>
        public void Transform(MatrixF transformationMatrix)
        {
            if (transformationMatrix == null)
                throw new ApplicationException("Matrix is null");

            vertexBuffer = new MatrixF[vertexBufferOrig.Length];

            for (int i = 0; i < vertexBufferOrig.Length; i++)
                vertexBuffer[i] = transformationMatrix * vertexBufferOrig[i];
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="g">Graphics object</param>
        public void Draw(Graphics g)
        {
            var first = true;

            // draw topology
            foreach (var abscissa in indexBuffer)
            {
                var lineStart = CoordTrans.FromXYtoUV(vertexBuffer[abscissa.PointStartID]);
                var lineEnd = CoordTrans.FromXYtoUV(vertexBuffer[abscissa.PointEndID]);

                if (first)
                {
                    first = false;
                    g.DrawLine(Pens.Red, lineStart, lineEnd);
                }
                else
                    g.DrawLine(Pens.Black, lineStart, lineEnd);
            }

            // draw vertices
            foreach(var vertex in vertexBuffer)
			{
                Point2D pt = new(vertex)
                {
                    Antialiased = true,
                    Color = Color.Red
                };

                pt.Draw(g);
            }
        }
	}
}
