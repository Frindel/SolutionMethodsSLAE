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
			var a = MatrixOperations.GetTriangularMatrix(SLAE);

			return null;
		}
	}
}
