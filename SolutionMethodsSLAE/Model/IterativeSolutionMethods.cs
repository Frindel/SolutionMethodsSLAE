using SolutionMethodsSLAE.Model.Data;
using SolutionMethodsSLAE.Model;
using System;

namespace SolutionMethodsSLAE.Model
{
	public static class IterativeSolutionMethods
	{
		/// <summary>
		/// Возвращает результат решения СЛАУ методом простых итераций
		/// </summary>
		/// <param name="SLAE">СЛАУ</param>
		public static Matrix GetRezultOfMethodSimpleIterations (SystemLinearAlgebraicEquations SLAE)
		{
			//условие завершения приближения
			const double condition = 0.001;

			//полуение сходимой матицы
			Matrix matrix = null!;
			if (SLAE.GetCoefficientsMatrix().IsConvergent())
				matrix = MatrixOperations.CreateExtendMatrix(SLAE.GetCoefficientsMatrix(), SLAE.GetFreeValuesMatrix());
			else
				matrix = MatrixOperations.GetDiagonalDominanceMatrix(SLAE);

			//нулевое приближение
			double[] values = new double[matrix.RowCount];

			for (int i = 0; i < matrix.RowCount; i++)
			{
				values[i] = matrix[i, matrix.ColumnCount - 1] / matrix[i, i];
			}

			double[] Zooming(double[] values)
			{
				//объявление массива для хранения значений текущей итерации рекурсии
				double[] newValues = new double[values.Length];

				//перебор всех строк массива
				for (int i = 0; i < matrix.RowCount; i++)
				{
					//определение значений x-ов
					double sum = 0; 
					for (int j = 0; j < matrix.ColumnCount-1; j++)
					{
						if (i == j)
							continue;
						sum += values[j] * matrix[i, j];
					}
					newValues[i] = (matrix[i, matrix.ColumnCount-1] - sum) / matrix[i, i];
				}

				for (int i = 0; i < newValues.Length; i++)
				{
					var a = Math.Abs(values[i] - newValues[i]) / Math.Abs(newValues[i]);

					if (Math.Abs(newValues[i]- values[i]) / Math.Abs(newValues[i]) > condition)
					{
						//рекурсивный случай
						return Zooming(newValues);
					}
				}

				//базовый случай
				return newValues;

			}

			double[] zoomingRezult = Zooming(values);

			Matrix rez = new Matrix(3,1);
			for (int i = 0; i<zoomingRezult.Length;i++)
			{
				rez[i,0] = zoomingRezult[i];
			}

			return rez;
		}

		/// <summary>
		/// Возвращает резульата решения СЛАУ методом Зейделя
		/// </summary>
		/// <param name="SLAE"></param>
		/// <returns></returns>
		public static Matrix GetRezultOfSeidelMethod(SystemLinearAlgebraicEquations SLAE)
		{
			//условие завершения приближения
			const double condition = 0.001;

			//полуение сходимой матицы
			Matrix matrix = null!;
			if (SLAE.GetCoefficientsMatrix().IsConvergent())
				matrix = MatrixOperations.CreateExtendMatrix(SLAE.GetCoefficientsMatrix(), SLAE.GetFreeValuesMatrix());
			else
				matrix = MatrixOperations.GetDiagonalDominanceMatrix(SLAE);

			//нулевое приближение
			double[] values = new double[matrix.RowCount];

			for (int i = 0; i < matrix.RowCount; i++)
			{
				values[i] = matrix[i, matrix.ColumnCount - 1] / matrix[i, i];
			}

			double[] Zooming(double[] values)
			{
				//объявление массива для хранения значений текущей итерации рекурсии
				double[] newValues = new double[values.Length];

				//перебор всех строк массива
				for (int i = 0; i < matrix.RowCount; i++)
				{
					//определение значений x-ов
					double sum = 0;
					for (int j = 0; j < matrix.ColumnCount - 1; j++)
					{
						if (i == j)
							continue;
						sum += ((j<i) ? newValues[j] : values[j]) * matrix[i, j];

					}
					newValues[i] = (matrix[i, matrix.ColumnCount - 1] - sum) / matrix[i, i];
				}

				for (int i = 0; i < newValues.Length; i++)
				{
					var a = Math.Abs(values[i] - newValues[i]) / Math.Abs(newValues[i]);

					if (Math.Abs(newValues[i] - values[i]) / Math.Abs(newValues[i]) > condition)
					{
						//рекурсивный случай
						return Zooming(newValues);
					}
				}

				//базовый случай
				return newValues;

			}

			double[] zoomingRezult = Zooming(values);

			Matrix rez = new Matrix(3, 1);
			for (int i = 0; i < zoomingRezult.Length; i++)
			{
				rez[i, 0] = zoomingRezult[i];
			}

			return rez;
		}
	}
}
