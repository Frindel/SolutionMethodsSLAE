using SolutionMethodsSLAE.Model.Data;
using SolutionMethodsSLAE.Model;

namespace SolutionMethodsSLAE.Model
{
	static class DirectSolutionMethods
	{
		public static Matrix GetRezultOfMatrixMethod (SystemLinearAlgebraicEquations SLAE)
		{
			if (MatrixOperations.GetDeterminant(SLAE.GetCoefficientsMatrix())==0)
				return null;

			return MatrixOperations.Inverse(SLAE.GetCoefficientsMatrix(),0) * SLAE.GetFreeValuesMatrix();
			//return MatrixOperations.GetInverseMatrix(SLAE.GetCoefficientsMatrix()) * SLAE.GetFreeValuesMatrix();

		}
	}
}
