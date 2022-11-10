namespace SolutionMethodsSLAE.Model
{
	static class DirectSolutionMethods
	{
		public static Matrix GetRezultOfMatrixMethod (Matrix matrix)
		{
			return null;
		}
		public static Matrix GetResultOfCramerMethod(SystemLinearAlgebraicEquations slae)
		{
			Matrix result = new Matrix(slae.EquationsCount, 0);
			Matrix coefficients = slae.GetCoefficientsMatrix();
			double[] freevalues = new double[coefficients.RowCount];
            for (int i = 0; i < freevalues.Length; i++)
            {
				freevalues[i] = slae.GetFreeValuesMatrix().ToArray()[i, 0];
			}

			double mainDeterminant = MatrixOperations.GetDeterminant(coefficients);
			System.Collections.Generic.List<double> resultVector = new System.Collections.Generic.List<double>();
			for (int i = 0; i < slae.EquationsCount; i++)
				resultVector.Add((MatrixOperations
					.GetDeterminant(MatrixOperations
						.ReplaceColumn(0, coefficients, freevalues))) 
							/ mainDeterminant);
			result = new Matrix(resultVector.Count - 1, 0);
            for (int i = 0; i < result.RowCount; i++)
				result[i, 0] = resultVector[i];
			return result;
		}
	}
}
