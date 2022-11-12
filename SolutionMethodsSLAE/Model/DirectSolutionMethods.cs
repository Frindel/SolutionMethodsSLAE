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

		public static Matrix GetResultOfCramerMethod(SystemLinearAlgebraicEquations slae)
		{
			Matrix result = new Matrix(slae.Equations.Count, 0);
			Matrix coefficients = slae.GetCoefficientsMatrix();
			double[] freevalues = new double[coefficients.RowCount];
            for (int i = 0; i < freevalues.Length; i++)
            {
				freevalues[i] = slae.GetFreeValuesMatrix().ToArray()[i, 0];
			}

			double mainDeterminant = MatrixOperations.GetDeterminant(coefficients);
			System.Collections.Generic.List<double> resultVector = new System.Collections.Generic.List<double>();
			for (int i = 0; i < slae.Equations.Count; i++)
				resultVector.Add((MatrixOperations
					.GetDeterminant(MatrixOperations
						.ReplaceColumn(0, coefficients, freevalues))) 
							/ mainDeterminant);
			result = new Matrix(resultVector.Count - 1, 1);
            for (int i = 0; i < result.RowCount; i++)
				result[i, 0] = resultVector[i];
			return result;
		}
	}
}
