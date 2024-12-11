using System.Drawing.Drawing2D;
using System.Text;

namespace AnimationCurves.Tools
{
    public enum Direction
    {
        Vertical,
        Horizontal
    }

    public sealed class MatrixF
    {
        #region Properties

        private readonly float[,] MatrixArray;

        public int Width => Columns;
        public int Height => Rows;

        public int Rows => MatrixArray.GetLength(0);
        public int Columns => MatrixArray.GetLength(1);
        public Direction Direction; 

        public float this[int row, int column]
        {
            get => MatrixArray[row, column];
            set => MatrixArray[row, column] = value;
        }

        #endregion 

        #region Constructors

        /// <summary>
        /// Creates a new empty matrix.
        /// </summary>
        /// <param name="rows">Number of rows of the new matrix</param>
        /// <param name="columns">Number of columns of the new matrix</param>
        public MatrixF(int rows, int columns)
        {
            MatrixArray = new float[rows, columns];
        }

        /// <summary>
        /// Creates a new matrix from the provided array. If required, the array can be transposed.
        /// </summary>
        /// <param name="matrix">The array that will be used as this matrix's values.</param>
        /// <param name="transpose">A flag whether the provided array should be transposed before it is used.</param>
        public MatrixF(float[,] matrix, bool transpose = false)
        {
            // Usually, the provided array is created inline and its format is [row, column]. That is what we want.
            if (!transpose)
            {
                MatrixArray = matrix;
                return;
            }

            // Sometimes the array was created in other way and has to be transposed.
            // In that case, the provided matrix has format [column, row] and we have to transpose it to [row, column].
            var rows = matrix.GetLength(1);
            var columns = matrix.GetLength(0);

            MatrixArray = new float[rows, columns];

            // Transpose the array
            for (int row = 0; row < rows; row++)
                for (int column = 0; column < columns; column++)
                    MatrixArray[row, column] = matrix[column, row];
        }

        /// <summary>
        /// Creates a vector: a one-dimensional matrix. The matrix will not be truly one dimensional,
        /// but one of its dimensions will have size 1 -> only one row or only one column.
        /// </summary>
        /// <param name="vector">The array that will be used as this vector's values.</param>
        /// <param name="direction">Direction of the created vector.</param>
        public MatrixF(float[] vector, Direction direction)
        {
            if (direction == Direction.Horizontal)
            {
                MatrixArray = new float[1, vector.Length];
                for (int i = 0; i < vector.Length; i++)
                    MatrixArray[0, i] = vector[i];
            }
            else
            {
                MatrixArray = new float[vector.Length, 1];
                for (int i = 0; i < vector.Length; i++)
                    MatrixArray[i, 0] = vector[i];
            }
        }

        #endregion 

        #region Public methods

        /// <summary>
        /// ToString
        /// </summary>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                    stringBuilder.Append($"{MatrixArray[row, column]:0.0000} ");

                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Builds and returns a completely new matrix that has the same values as this matrix.
        /// </summary>
        /// <returns>A perfect clone of this matrix.</returns>
        public MatrixF Clone()
        {
            int rows = Rows;
            int columns = Columns;

            var newArray = new float[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                    newArray[row, column] = MatrixArray[row, column];
            }

            return new MatrixF(newArray);
        }

        #endregion Public Methods

        #region Static Methods

        /// <summary>
        /// GetIdentityMatrix
        /// </summary>
        public static MatrixF GetIdentityMatrix(int size)
        {
            MatrixF matrix = new MatrixF(size, size);
            for (int i = 0; i < size; i++)
                matrix[i, i] = 1;

            return matrix;
        }

        /// <summary>
        /// GetVector
        /// </summary>
        public static MatrixF GetVector(int length, Direction direction)
        {
            MatrixF matrix;

            if (direction == Direction.Horizontal)
                matrix = new MatrixF(1, length);
            else
                matrix = new MatrixF(length, 1);

            return matrix;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Opeartor *
        /// </summary>
        public static MatrixF operator *(MatrixF left, MatrixF right)
        {
            int leftRows = left.Rows;
            int leftColumns = left.Columns;

            int rightRows = right.Rows;
            int rightColumns = right.Columns;

            if (leftColumns != rightRows)
            {
                throw new ApplicationException($"These matrices cannot be multiplied! The number of columns on the left ({leftColumns}) must be equal to the number of rows on the right ({rightRows})!");
            }

            MatrixF result = new MatrixF(leftRows, rightColumns);

            for (int leftRow = 0; leftRow < leftRows; leftRow++)
            {
                for (int rightColumn = 0; rightColumn < rightColumns; rightColumn++)
                {
                    float multiplicationResult = 0;

                    for (int i = 0; i < leftColumns; i++)
                    {
                        multiplicationResult += left[leftRow, i] * right[i, rightColumn];
                    }

                    result[leftRow, rightColumn] = multiplicationResult;
                }
            }

            return result;
        }

        /// <summary>
        /// Operator *
        /// </summary>
        public static MatrixF operator *(MatrixF matrix, float scalar)
        {
            int rows = matrix.Rows;
            int columns = matrix.Columns;

            var newArray = new float[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    newArray[row, column] = matrix[row, column] * scalar;
                }
            }

            return new MatrixF(newArray);
        }

        /// <summary>
        /// Operator *
        /// </summary>
        public static MatrixF operator *(float scalar, MatrixF matrix)
        {
            return matrix * scalar;
        }

        /// <summary>
        /// Operator +
        /// </summary>
        public static MatrixF operator +(MatrixF left, MatrixF right)
        {
            int leftRows = left.Rows;
            int leftColumns = left.Columns;

            int rightRows = right.Rows;
            int rightColumns = right.Columns;

            if (leftRows != rightRows)
                throw new ApplicationException($"These matrices cannot be multiplied! The number of rows on the left ({leftRows}) must be equal to the number of rows on the right ({rightRows})!");

            if (leftColumns != rightColumns)
                throw new ApplicationException($"These matrices cannot be multiplied! The number of columns on the left ({leftColumns}) must be equal to the number of columns on the right ({rightColumns})!");

            var newArray = new float[leftRows, leftColumns];

            for (int row = 0; row < leftRows; row++)
            {
                for (int column = 0; column < leftColumns; column++)
                {
                    newArray[row, column] = left[row, column] + right[row, column];
                }
            }

            return new MatrixF(newArray);
        }

        /// <summary>
        /// Operator -
        /// </summary>
        public static MatrixF operator -(MatrixF left, MatrixF right)
        {
            int leftRows = left.Rows;
            int leftColumns = left.Columns;

            int rightRows = right.Rows;
            int rightColumns = right.Columns;

            if (leftRows != rightRows)
                throw new ApplicationException($"These matrices cannot be multiplied! The number of rows on the left ({leftRows}) must be equal to the number of rows on the right ({rightRows})!");

            if (leftColumns != rightColumns)
                throw new ApplicationException($"These matrices cannot be multiplied! The number of columns on the left ({leftColumns}) must be equal to the number of columns on the right ({rightColumns})!");

            var newArray = new float[leftRows, leftColumns];

            for (int row = 0; row < leftRows; row++)
            {
                for (int column = 0; column < leftColumns; column++)
                {
                    newArray[row, column] = left[row, column] - right[row, column];
                }
            }

            return new MatrixF(newArray);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Build point vector
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static MatrixF BuildPointVector(float x, float y, Direction direction = Direction.Vertical)
        {
            var vectorArray = new float[] { x, y, 1 };

            return new MatrixF(vectorArray, direction);
        }

        /// <summary>
        /// Returns vector as a PointF structure
        /// </summary>
        /// <param name="pointVector"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public static PointF GetPointF(MatrixF pointVector)
        {
            if ((pointVector == null) || (pointVector.Rows != 3 || pointVector.Columns != 1))
                throw new ApplicationException($"Wrong data input in GetPointF!");

            return new PointF(pointVector[0, 0], pointVector[1, 0]);
        }

        /// <summary>
        /// Returns vector as a Point structure
        /// </summary>
        /// <param name="pointVector"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public static Point GetPoint(MatrixF pointVector)
        {
            if ((pointVector == null) || (pointVector.Rows != 3 || pointVector.Columns != 1))
                throw new ApplicationException($"Wrong data input in GetPoint!");

            return new Point((int)pointVector[0, 0], (int)pointVector[1, 0]);
        }

        /// <summary>
		/// BuildTranslationMatrix
		/// </summary>
		public static MatrixF BuildTranslationMatrix(float x, float y)
        {
            var matrixValues = new float[,]
            {
                { 1, 0, x },
                { 0, 1, y },
                { 0, 0, 1 }
            };

            return new MatrixF(matrixValues);
        }

        /// <summary>
        /// Create a rotation matrix that rotates values around point (0,0)
        /// </summary>
        public static MatrixF BuildRotationMatrix(float angle)
        {
            var matrixValues = new float[,]
            {
                { (float)Math.Cos(angle), (float)-Math.Sin(angle), 0 },
                { (float)Math.Sin(angle), (float)Math.Cos(angle), 0 },
                { 0, 0, 1 }
            };

            return new MatrixF(matrixValues);
        }

        /// <summary>
        /// BuildScalingMatrix
        /// </summary>
        public static MatrixF BuildScalingMatrix(float xScale, float yScale)
        {
            var matrixValues = new float[,]
            {
                { xScale, 0, 0 },
                { 0, yScale, 0 },
                { 0, 0, 1 }
            };

            return new MatrixF(matrixValues);
        }

        #endregion
    }
}