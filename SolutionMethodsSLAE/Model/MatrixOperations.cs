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

			//получение определителя суммированием коефициентов основной диагонали LU-матрицы
			Matrix LUMatrix = GetLUMatrix(matrix);
			double sum = LUMatrix[0, 0];
			for (int i = 1; i < LUMatrix.ColumnCount; i++)
			{
				sum *= LUMatrix[i, i];
			}
			return sum;
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

						if (double.IsNaN(matrix[j, i]))
							matrix[j, i] = 0;
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

				for (int j = extendMatrix.ColumnCount - 2; j > i; j--)
				{
					sum += extendMatrix[i, j] * rez[j, 0];
				}
				rez[i, 0] = extendMatrix[i, extendMatrix.ColumnCount - 1] - sum;
				rez[i, 0] /= extendMatrix[i, i];
			}

			return rez;
		}

		/// <summary>
		/// Заменяет строку на новую
		/// </summary>
		/// <param name="replaceAt">Индекс строки</param>
		/// <param name="matrix">Матрица</param>
		/// <param name="array">Новые значения строки</param>
		/// <returns>Транспонированная матрица</returns>
		/// <exception cref="ApplicationException">Количество значений в новой строке не равно количеству строк в старой</exception>
		public static Matrix ReplaceColumn(int replaceAt, Matrix matrix, params double[] array)
		{
			if (matrix.RowCount != array.Length)
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

		/// <summary>
		/// Выполняет транспонирование матрицы
		/// </summary>
		/// <param name="matrix">Матрица</param>
		/// <returns>Транспонированная матрица</returns>
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

		/// <summary>
		/// Вычисляет LU-матрицу
		/// </summary>
		/// <param name="matrix">Матрица</param>
		/// <returns>LU-матрица</returns>
		public static Matrix GetLUMatrix(Matrix matrix)
		{
			Matrix LUMatrix = new Matrix(matrix.RowCount, matrix.ColumnCount);

			for (int i = 0; i < matrix.RowCount; i++)
			{
				//Вычисление значений элементов L матрицы
				for (int j = i; j < matrix.RowCount; j++)
				{
					double sum = 0;

					for (int k = 0; k<i;k++)
					{
						sum += LUMatrix[j, k] * LUMatrix[k, i];
					}

					LUMatrix[j, i] = matrix[j, i] - sum;
				}

				//Вычисление значений U матрцы
				for (int j = i + 1; j < matrix.ColumnCount; j++)
				{
					double sum = 0;

					for(int k =0;k<i;k++)
					{
						sum += LUMatrix[i, k] * LUMatrix[k, j];
					}

					LUMatrix[i, j] = (matrix[i, j] - sum) / LUMatrix[i, i];
				}
			}

			return LUMatrix;
		}

		/// <summary>
		/// Выполняет пребразование матрицы в матрицу с диагональным преобладанием
		/// </summary>
		/// <param name="slae">СЛАУ</param>
		/// <returns>Матрица с диагональным преобладанием</returns>
		public static Matrix GetDiagonalDominanceMatrix(SystemLinearAlgebraicEquations slae)
		{
			void Print(Matrix result)
            {
				string s = string.Empty;
				for (int i = 0; i < result.RowCount; i++)
				{
					s += "\n";
					for (int j = 0; j < result.ColumnCount; j++)
					{
						s += result[i, j] + "\t";
					}
				}
				System.Windows.MessageBox.Show(s);
			}

			Matrix result = new Matrix(slae.EquationsCount, slae.CoefficientsCount + 1);
			Matrix referenceMatrix = new Matrix(result.RowCount, result.ColumnCount - 1);
			for (int i = 0; i < referenceMatrix.RowCount; i++)
				for (int j = 0; j < referenceMatrix.ColumnCount; j++)
					if (i == j) referenceMatrix[i, j] = 1000;
					else referenceMatrix[i, j] = 1;
			Matrix newCoefs = MatrixOperations.Inverse(slae.GetCoefficientsMatrix(), 0) * referenceMatrix;
			Matrix newFreeValues = newCoefs * slae.GetFreeValuesMatrix();


			for (int i = 0; i < referenceMatrix.RowCount; i++)
				for (int j = 0; j < referenceMatrix.ColumnCount; j++)
					result[i, j] = referenceMatrix[i, j];


			for (int i = 0; i < newFreeValues.RowCount; i++)
				result[i, result.RowCount] = newFreeValues[i, 0];

			Print(result);


			return result;
		}

		public static Matrix GetDiagonalDominatingMatrix(SystemLinearAlgebraicEquations slae)
        {

			void Print(Matrix result)
			{
				string s = string.Empty;
				for (int i = 0; i < result.RowCount; i++)
				{
					s += "\n";
					for (int j = 0; j < result.ColumnCount; j++)
					{
						s += result[i, j] + "\t";
					}
				}
				System.Windows.MessageBox.Show(s);
			}

			Matrix result = new Matrix(slae.EquationsCount, slae.CoefficientsCount + 1);

            #region Переносим значения из слау в новую матрицу

            for (int i = 0; i < slae.GetCoefficientsMatrix().RowCount; i++)
				for (int j = 0; j < slae.GetCoefficientsMatrix().ColumnCount; j++)
					result[i, j] = slae.GetCoefficientsMatrix()[i, j];


			for (int i = 0; i < slae.GetFreeValuesMatrix().RowCount; i++)
				result[i, result.RowCount] = slae.GetFreeValuesMatrix()[i, 0];

			#endregion

			#region Для начала получаем нулевой нижний треугольник при помощи вычитания строк 
			for (int column = 0; column < result.ColumnCount; column++)
			{
				for (int row = 0; row < result.RowCount; row++)
				{
					if (result[row, column] == 0 || row <= column) continue;

                    double difference = result[row, column] / result[column, column];
					for (int coefficientIndex = 0; coefficientIndex < result.ColumnCount; coefficientIndex++)
						result[row, coefficientIndex] -= difference * result[column, coefficientIndex]; //Потом наверное убрать округление
				}
            }
			#endregion

			#region Теперь получаем нули в верхнем треугольнике тем же образом

			for (int column = 0; column < result.ColumnCount-1; column++)
			{
				for (int row = 0; row < result.RowCount; row++)
				{
					if (result[row, column] == 0 || row >= column) continue;

					double difference = result[row, column] / result[column, column];
					for (int coefficientIndex = 0; coefficientIndex < result.ColumnCount; coefficientIndex++)
						result[row, coefficientIndex] -= difference * result[column, coefficientIndex];//Потом наверное убрать округление
				}

			}
			#endregion

			//Print(result); 

			return result;

		}

		public static Matrix GetLMatrix(Matrix LUMatrix)
		{
			Matrix LMatrix = new Matrix(LUMatrix.RowCount, LUMatrix.ColumnCount);

			if (LUMatrix == null)
				return null;

			for (int i = 0; i < LUMatrix.RowCount; i++)
			{
				for (int j = 0; j < LUMatrix.ColumnCount; j++)
				{
					if (i >= j)
						LMatrix[i, j] = LUMatrix[i,j];

				}
			}

			return LMatrix;
		}

		public static Matrix GetUMatrix(Matrix LUMatrix)
		{
			Matrix UMatrix = new Matrix(LUMatrix.RowCount, LUMatrix.ColumnCount);



			if (LUMatrix == null)
				return null;

			for (int i = 0; i < LUMatrix.RowCount; i++)
			{
				for (int j = i; j < LUMatrix.ColumnCount; j++)
				{
					if (i < j)
						UMatrix[i, j] = LUMatrix[i, j];

					if (i == j)
						UMatrix[i, j] = 1;
				}
			}

			return UMatrix;
		}
	}
}
