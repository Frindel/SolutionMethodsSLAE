using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SolutionMethodsSLAE.Model.Data
{
	public class Equation
	{
		private ObservableCollection<Couefficient<double>> _coefficientsArr;

		#region Propertys

		public int CouefficientsCount { get => _coefficientsArr.Count; }

		public ObservableCollection<Couefficient<double>> Couefficients { get => _coefficientsArr; set => _coefficientsArr = value; }

		public double FreeValue { get; set; }

		public Couefficient<double> this[int index]
		{
			get => _coefficientsArr[index];
			set => _coefficientsArr[index] = value;
		}

		#endregion

		#region Constructors

		public Equation(double[] coefficients, double freeValue)
		{
			_coefficientsArr = new ObservableCollection<Couefficient<double>>(coefficients.Select(k => new Couefficient<double>() { Value = k }).ToArray());
			FreeValue = freeValue;
		}

		public Equation(double freeValue, params double[] coefficients) : this(coefficients, freeValue)
		{ }

		#endregion

		#region Methods

		public double GetValue(int index)
		{
			return _coefficientsArr[index].Value;
		}

		public void SetValue(int index, double value)
		{
			_coefficientsArr[index].Value = value;
		}

		public void AddCoefficients(int count)
		{
			for (int i = 0; i < count; i++)
				_coefficientsArr.Add(new Couefficient<double>() { Value = 0});
		}

		public void RemoveCoefficients(int count)
		{
			if (_coefficientsArr.Count < count)
				throw new ApplicationException("");

			for (int i = 0; i < count; i++)
				_coefficientsArr.Remove(_coefficientsArr.Last());
		}

		#endregion
	}
}
