using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.ObjectModel;

namespace SolutionMethodsSLAE.Model.Data
{
	public class SystemLinearAlgebraicEquations
	{
		#region Propertys

		public ObservableCollection<Equation> Equations { get; set; } = new ObservableCollection<Equation>();

		#endregion

		#region Constructors
		public SystemLinearAlgebraicEquations(IEnumerable<Equation> equations)
		{
			foreach (var a in equations)
				Equations.Add(a);
		}
		#endregion

		#region Methods

		public void AddEquation(double freeValue, params double[] coefficients)
		{
			AddEquation(new Equation(coefficients, freeValue));
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
			Matrix matrix = new Matrix(Equations.Count, Equations.Max(k => k.CouefficientsCount));

			for (int i = 0; i < Equations.Count; i++)
			{
				Equation equation = Equations[i];
				for (int j = 0; j < equation.CouefficientsCount; j++)
					matrix[i, j] = equation[j].Value;
			}
			return matrix;
		}

		public Matrix GetFreeValuesMatrix()
		{
			Matrix matrix = new Matrix(Equations.Count, 1);
			for (int i = 0; i < Equations.Count; i++)
				matrix.SetValue(i, 0, Equations[i].FreeValue);
			return matrix;
		}

		#endregion
	}
}
