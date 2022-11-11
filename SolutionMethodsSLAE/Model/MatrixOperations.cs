using SolutionMethodsSLAE.Model.Data;
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

		///// <summary>
		///// Вовзвращает матрицу алгебраических дополнений
		///// </summary>
		///// <param name="matrix"></param>
		///// <returns></returns>
		//public static Matrix GetMatrixAlgebraicAdditions(Matrix matrix)
		//{
		//	Matrix rez = new Matrix(matrix.RowCount,matrix.ColumnCount);

		//	for (int i = 0; i < matrix.RowCount; i++)
		//		for (int j = 0; j < matrix.ColumnCount; j++)
		//			rez[i, j] = GetDeterminant(Exclude(matrix,i,j));

		//	return rez;
		//}

		///// <summary>
		///// Возвращает транспонированную матрицу
		///// </summary>
		///// <param name="matrix"></param>
		///// <returns></returns>
		//public static Matrix GetTransposedMatrix(Matrix matrix)
		//{
		//	Matrix rez = new Matrix(matrix.ColumnCount, matrix.RowCount);

		//	for (int i = 0; i <matrix.RowCount;i++)
		//		for (int j = 0; j<matrix.ColumnCount;j++)
		//			rez[i,j] = matrix[j,i];

		//	return rez;
		//}

		///// <summary>
		///// Возвращает обратную матрицу
		///// </summary>
		///// <param name="matrix"></param>
		///// <returns></returns>
		//public static Matrix GetInverseMatrix(Matrix matrix)
		//{
		//	double determinant = GetDeterminant(matrix);
		//	if (determinant == 0)
		//		return null;

		//	return GetTransposedMatrix(GetMatrixAlgebraicAdditions(matrix)) / determinant;
		//}

		private static Matrix Exclude(Matrix origin, int row, int col)
		{
			if (row > origin.RowCount || col > origin.ColumnCount)
				throw new IndexOutOfRangeException("Строка или столбец не принадлежат матрице");

			Matrix result = new Matrix(origin.RowCount - 1, origin.ColumnCount - 1);
			int offsetX = 0;
			for (int i = 0; i < origin.RowCount; i++)
			{
				int offsetY = 0;
				if (i == row)
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

		/// <summary>
		/// Возвращает обратную матрицу
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="round"></param>
		/// <returns></returns>
		/// <exception cref="ApplicationException"></exception>
		public static Matrix Inverse(Matrix origin, int round)
		{
			if (origin.RowCount != origin.ColumnCount) throw new ApplicationException("Ошибка: ошибка (ошибка)");
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

		/// <summary>
		/// Возвращает расширенную матрицу
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static Matrix CreateExtendMatrix(Matrix first, Matrix second)
		{
			if (first.RowCount != second.RowCount || second.ColumnCount != 1)
				return null;

			Matrix rez = (Matrix)first.Clone();
			rez.AddColumn();

			int colIndex = first.ColumnCount;
			for (int i = 0; i < second.RowCount; i++)
				rez[i, colIndex] = second[i, 0];

			return rez;
		}

		/// <summary>
		/// Выполняет прямое исключение, применяемое в методе Гаусса
		/// </summary>
		/// <param name="matrix">Матрица</param>
		/// <returns>Матрица треугольного вида</returns>
		public static Matrix GetTriangularMatrix(SystemLinearAlgebraicEquations SLAE)
		{
			//создание расширенной матрицы
			Matrix extendMatrix = CreateExtendMatrix(SLAE.GetCoefficientsMatrix(), SLAE.GetFreeValuesMatrix());

			void RowReverse(int firstIndex, int secondIndex)
			{
				for (int i = 0; i < extendMatrix.ColumnCount; i++)
				{
					extendMatrix[firstIndex, i] += extendMatrix[secondIndex, i];
					extendMatrix[secondIndex, i] = extendMatrix[firstIndex, i] - extendMatrix[secondIndex, i];
					extendMatrix[firstIndex, i] -= extendMatrix[secondIndex, i];
				}
			}

			//проверка оси на оствутствие нулей и замена оси в случае их наличия
			for (int i = 0; i < extendMatrix.RowCount; i++)
			{
				if (extendMatrix[i, i] != 0)
					continue;

				for (int j = 0; j < extendMatrix.RowCount; j++)
				{
					if (i == j)
						continue;

					//реверс строк в случае отсутствия нулей в рассматриваемом участке оси
					if (extendMatrix[j, i] != 0 && extendMatrix[i, j] != 0)
					{
						RowReverse(i, j);

						goto forend;
					}
				}

				//возврат null значения в случае невозможности получения оси без нулей
				return null;
			forend:;
			}

			//перебор всех строк расширенной матрицы
			for (int i = 0; i < extendMatrix.RowCount - 1; i++)
			{
				//получение преобразованных значений текущей строки инерации
				double firstValue = extendMatrix[i, i];
				for (int j = i; j < extendMatrix.ColumnCount; j++)
					extendMatrix[i, j] = extendMatrix[i, j] / firstValue;

				//выполнение действий со строками ниже
				for (int j = i + 1; j < extendMatrix.RowCount; j++)
				{
					double firstValueNew = extendMatrix[j, i];
					if (firstValueNew == 0)
						continue;
					for (int k = i; k < extendMatrix.ColumnCount; k++)
					{
						extendMatrix[j, k] = extendMatrix[j, k] / firstValueNew - extendMatrix[i, k];
					}
				}
			}

			return extendMatrix;
		}

		/// <summary>
		/// Выполняет обратную подстановку в треугольной расширенной матрице
		/// </summary>
		/// <param name="extendMatrix">Раширенная матрица</param>
		/// <returns>Результат решения треугольной расширенной матрицы</returns>
		public static Matrix ReverseSubstitution(Matrix extendMatrix)
		{
			Matrix rez = new Matrix(extendMatrix.RowCount, 1);

			for (int i = extendMatrix.RowCount - 1; i >= 0; i--)
			{
				double sum = 0;

				for (int j = extendMatrix.ColumnCount-2; j>i;j--)
				{
					sum += extendMatrix[i, j] * rez[j, 0];
				}
				rez[i,0] = extendMatrix[i, extendMatrix.ColumnCount - 1] - sum;
				rez[i, 0] /= extendMatrix[i, i];
			}

			return rez;
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
