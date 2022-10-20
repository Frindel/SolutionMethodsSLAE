using System;

namespace SolutionMethodsSLAE.Model
{
	internal class MatrixMethod
	{
		/// <summary>
		/// Находит определитель квадратной мартицы
		/// </summary>
		/// <param name="matrix"></param>
		/// <returns>Определитель матрицы</returns>
		/// <exception cref="ApplicationException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public double GetDeterminant(double[,] matrix)
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
			if (matrix.GetLength(0) != matrix.GetLength(1))
				throw new ApplicationException("The matrix is not square");

			//проверка матрицы на отсутствие аргументов
			if (matrix.Length == 0)
				throw new ApplicationException("The matrix has no arguments");

			if (matrix.GetLength(0) == 1)
				return matrix[0, 0];

			return GetDeterminant(matrix);
		}
	}
}
