using System;

namespace SolutionMethodsSLAE.Model
{
	static class MatrixOperations
	{
		/// <summary>
		/// Находит определитель квадратной мартицы
		/// </summary>
		/// <param name="matrix"></param>
		/// <returns>Определитель матрицы</returns>
		/// <exception cref="ApplicationException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public static double GetDeterminant(Matrix matrix)
		{
			double GetDeterminant(double[,] _matrix)
			{
				//базовый случай; возврат определителя матрицы 2x2
				if (_matrix.GetLength(0) == 2)
				{
					return _matrix[0, 0] * _matrix[1, 1] - _matrix[0, 1] * _matrix[1, 0];
				}
				//рекурсивный случай

				int mSize = _matrix.GetLength(0);

				double determinant = 0;

				for (int i = 0; i < mSize; i++)
				{
					double[,] newMatrix = new double[mSize - 1, mSize - 1];

					int rowIndex = 0;
					for (int j = 0; j < mSize; j++)
					{
						int colIndex = 0;

						if (j == i)
							continue;

						for (int k = 1; k < mSize; k++)
						{
							newMatrix[rowIndex, colIndex] = _matrix[j, k];
							colIndex++;
						}
						rowIndex++;
					}
					determinant += _matrix[i, 0] * GetDeterminant(newMatrix) * ((i + 1) % 2 == 0 ? -1 : 1);
				}

				return determinant;
			}

			//проверка матрицы на nullable тип
			if (matrix == null)
				throw new ArgumentNullException();

			//проверка равенства столбцов и строк матрицы
			if (matrix.RowCount != matrix.ColumnCount)
				throw new ApplicationException("The matrix is not square");

			//проверка матрицы на отсутствие аргументов
			if (matrix.RowCount == 0)
				throw new ApplicationException("The matrix has no arguments");

			if (matrix.RowCount == 1)
				return matrix[0, 0];

			return GetDeterminant(matrix.ToArray());
		}


		public static Matrix Exclude(Matrix origin, int row, int col)
        {
			if (row > origin.RowCount || col > origin.ColumnCount)
				throw new IndexOutOfRangeException("Строка или столбец не принадлежат матрице");

			Matrix result = new Matrix(origin.RowCount - 1, origin.ColumnCount - 1);
			int offsetX = 0;
            for (int i = 0; i < origin.RowCount; i++)
            {
				int offsetY = 0;
				if (i==row) 
				{ 
					offsetX++; 
					continue; 
				}
                for (int j = 0; j < origin.ColumnCount; j++)
                {
					if (j == col) 
					{ 
						offsetY++;
						continue;
					}
					result.SetValue(i - offsetX, j - offsetY, origin.GetValue(i, j));
				}

            }
			return result;
		}
		public static Matrix Inverse(Matrix origin, int round)
		{
			if (origin.RowCount != origin.ColumnCount) throw new ApplicationException();
			double determinant = GetDeterminant(origin);

			if (determinant != 0)
			{
				Matrix matrix = new Matrix(origin.RowCount);
				for (int i = 0; i < origin.RowCount; i++)
				{
					for (int j = 0; j < origin.ColumnCount; j++)
					{
						Matrix temp = Exclude(origin, i, j);
						matrix[j, i] = round == 0 ?
							(1 / determinant) * Math.Pow(-1, i + j) * GetDeterminant(temp) :
							Math.Round(((1 / determinant) * Math.Pow(-1, i + j) * GetDeterminant(temp)), (int)round, MidpointRounding.ToEven);
					}
				}
				return matrix;
			}
			else return origin;


		}

		public static Matrix ReplaceColumn(int replaceAt, Matrix matrix, params double[] array)
        {
			if(matrix.RowCount != array.Length)
				throw new ApplicationException();
			Matrix res = new Matrix(matrix.RowCount, matrix.ColumnCount);
            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    if (replaceAt != j)
						res[i, j] = matrix[i, j];
					else res[i, j] = array[i];
                }
            }
			return res;
        }
		public static Matrix Transpose(Matrix matrix)
        {
			Matrix transposed = new Matrix(matrix.ColumnCount, matrix.RowCount);
            for (int i = 0; i < transposed.RowCount; i++)
            {
                for (int j = 0; j < transposed.ColumnCount; j++)
                {
					transposed[i, j] = matrix[j, i];
                }
            }
			return transposed;
        }
		
	}
}
