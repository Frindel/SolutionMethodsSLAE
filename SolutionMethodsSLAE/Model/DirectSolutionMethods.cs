using SolutionMethodsSLAE.Model.Data;
using SolutionMethodsSLAE.Model;

namespace SolutionMethodsSLAE.Model
{
	static class DirectSolutionMethods
	{
		/// <summary>
		/// Выполняет решение СЛАУ матричным методом
		/// </summary>
		/// <param name="SLAE">СЛАУ</param>
		/// <returns>Результат решения СЛАУ</returns>
		public static Matrix GetRezultOfMatrixMethod (SystemLinearAlgebraicEquations SLAE)
		{
			if (MatrixOperations.GetDeterminant(SLAE.GetCoefficientsMatrix())==0)
				return null;

			return MatrixOperations.Inverse(SLAE.GetCoefficientsMatrix(),0) * SLAE.GetFreeValuesMatrix();
			//return MatrixOperations.GetInverseMatrix(SLAE.GetCoefficientsMatrix()) * SLAE.GetFreeValuesMatrix();

		}

		/// <summary>
		/// Выполняет решение СЛАУ методом Гаусса
		/// </summary>
		/// <param name="SLAE">СЛАУ</param>
		/// <returns>Результат решения СЛАУ</returns>
		public static Matrix GetRezultOfGaussMethod(SystemLinearAlgebraicEquations SLAE)
		{
			return MatrixOperations.ReverseSubstitution(MatrixOperations.GetTriangularMatrix(SLAE));
		}

		/// <summary>
		/// Выполняет решение СЛАУ методом Крамера
		/// </summary>
		/// <param name="slae"></param>
		/// <returns></returns>
		public static Matrix GetResultOfCramerMethod(SystemLinearAlgebraicEquations slae)
		{
			Matrix result = new Matrix(slae.CoefficientsCount, 0);
			Matrix coefficients = slae.GetCoefficientsMatrix();

			double[] freevalues = new double[coefficients.RowCount];
			for (int i = 0; i < freevalues.Length; i++)
			{
				freevalues[i] = slae.GetFreeValuesMatrix().ToArray()[i, 0];
			}

			double mainDeterminant = MatrixOperations.GetDeterminant(coefficients);

			System.Collections.Generic.List<double> resultVector
				= new System.Collections.Generic.List<double>();

			for (int i = 0; i < slae.EquationsCount; i++)
				resultVector.Add((MatrixOperations
					.GetDeterminant(MatrixOperations
						.ReplaceColumn(i, coefficients, freevalues)))
							/ mainDeterminant);

			result = new Matrix(resultVector.Count, 1);

			for (int i = 0; i < resultVector.Count; i++)
			{
				result[i, 0] = resultVector[i];

			}

			return result;
		}

		/// <summary>
		/// Выполняет решение СЛАУ методом прогонки
		/// </summary>
		/// <param name="slae"></param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException"></exception>
		public static Matrix GetResultOfTridiagonalAlgorithm(SystemLinearAlgebraicEquations slae)
		{
			if (slae.GetCoefficientsMatrix().RowCount == 1)
			{
				Matrix r = new Matrix(1, 1);
				r[0, 0] = slae.GetFreeValuesMatrix()[0, 0] / slae.GetCoefficientsMatrix()[0, 0];
				return r;
			}

			if (!slae.GetCoefficientsMatrix().IsDiagonal(offset: 1))
				throw new System.ArgumentException("This matrix isn't tridianonal matrix");


			Matrix result = new Matrix(slae.CoefficientsCount, 1);
			Matrix coefs = slae.GetCoefficientsMatrix();
			Matrix freeVal = slae.GetFreeValuesMatrix();

			int last = coefs.RowCount;
			int penult = last - 1;
			double[] a = new double[last];
			double[] b = new double[last];
			double[] res = new double[last];
			double y = coefs[0, 0];
			a[0] = -coefs[0, 1] / y;
			b[0] = freeVal[0, 0] / y;

			for (int i = 1; i < penult; i++)
			{
				y = coefs[i, i] + coefs[i, i - 1] * a[i - 1];
				a[i] = -coefs[i, i + 1] / y;
				b[i] = (freeVal[i, 0] - coefs[i, i - 1] * b[i - 1]) / y;
			}

			res[penult] = (freeVal[penult, 0] - coefs[penult, penult - 1] * b[penult - 1])
				/ (coefs[penult, penult] + coefs[penult, penult - 1] * a[penult - 1]);

			for (int i = penult - 1; i >= 0; i--)
			{
				res[i] = a[i] * res[i + 1] + b[i];
			}

			for (int i = 0; i < result.RowCount; i++)
			{
				result[i, 0] = res[i];
			}

			return result;
		}

		/// <summary>
		/// Выполняет решение СЛАУ методом Жордана-Гаусса
		/// </summary>
		/// <param name="slae"></param>
		/// <returns></returns>
		public static Matrix GetResultOfJordaneGaussMethod(SystemLinearAlgebraicEquations slae)
        {
			Matrix temp = MatrixOperations.GetDiagonalDominatingMatrix(slae);

            for (int i = 0; i < temp.ColumnCount-1; i++)
            {
				temp[i, temp.ColumnCount - 1] /= temp[i, i];
				temp[i, i] = 1;
			}

			Matrix result = new Matrix(temp.RowCount, 1);

            for (int i = 0; i < result.RowCount; i++)
				result[i, 0] = temp[i, temp.ColumnCount - 1];

			return result;
		}

		public static Matrix GrtResultsOfLUMethod(SystemLinearAlgebraicEquations slae)
		{
			Matrix b = slae.GetFreeValuesMatrix();
			var a = MatrixOperations.GetLUMatrix(slae.GetCoefficientsMatrix());

			var l = MatrixOperations.GetLMatrix(a);
			var u = MatrixOperations.GetUMatrix(a);

			var InverseL = MatrixOperations.Inverse(MatrixOperations.GetLMatrix(a), 0);
			var InverseU = MatrixOperations.Inverse(MatrixOperations.GetUMatrix(a), 0);
			var y = InverseL * b;
			var x = InverseU * y;
			return x;
		}
	}
}
