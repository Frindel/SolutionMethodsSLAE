using System.Collections.Generic;
using System.Linq;

namespace SolutionMethodsSLAE.Model
{
	internal class SystemLinearAlgebraicEquations
	{

		public List<Equation> Equations { get; }
		public int EquationsCount 
			=> Equations.Count;
		public int CoefficientsCount
			=> Equations[0].CoefficientsCount;
		public SystemLinearAlgebraicEquations(IEnumerable<Equation> equations)
		{
			Equations = equations.ToList();
		}

		public void AddEquation(double freeValue, params double[] coefficients)
		{
			AddEquation(new Equation(coefficients,freeValue));
		}
		public void AddEquation(Equation equation)
		{
			Equations.Add(equation);
		}
		public void RemoveEquation(Equation equation)
		{
			Equations.Remove(equation);
		}
		public Matrix GetCoefficientsMatrix()
		{
			Matrix matrix = new Matrix(Equations.Count, Equations.Max(k => k.CoefficientsCount));

			for (int i = 0; i<Equations.Count;i++)
			{
				Equation equation = Equations[i];
				for (int j = 0; j < equation.CoefficientsCount; j++)
					matrix[i, j] = equation[j];
			}
			return matrix;
		}
		
		public Matrix GetFreeValuesMatrix()
		{
			Matrix matrix =  new Matrix(Equations.Count, 1);
			for (int i = 0; i < Equations.Count; i++)
				matrix.SetValue(i, 0, Equations[i].FreeValue);
			return matrix;
		}
		public void Clear()
        {
			Equations.Clear();
        }
	}
}
