using System;

namespace SolutionMethodsSLAE.Model
{
	internal class Equation
	{
		private double[] _coefficientsArr;
		public int CoefficientsCount { get => _coefficientsArr.Length; }
		public double FreeValue { get; set; }
		public double this[int index]
		{
			get => _coefficientsArr[index];
			set => _coefficientsArr[index] = value;
		}
		public Equation(double[] coefficients, double freeValue)
		{
			_coefficientsArr = (double[])coefficients.Clone();
			FreeValue = freeValue;
		}
        public Equation(int coefficientsCount) 
        {
            _coefficientsArr=new double[coefficientsCount];
			FreeValue = 0;
        }

		public Equation(double freeValue, params double[] coefficients) : this(coefficients, freeValue)
		{ }

		public double[] GetCoefficients()
        {
			return _coefficientsArr;
        }
		public double GetValue(int index)
		{
			return _coefficientsArr[index];
		}
		public void SetValue(int index, double value)
		{
			_coefficientsArr[index] = value;
		}

		public void AddCoefficients(int count)
		{
			double[] newArr = new double[_coefficientsArr.Length+count];
			for(int i = 0; i<_coefficientsArr.Length;i++)
				newArr[i]=_coefficientsArr[i];
			_coefficientsArr = newArr;
		}
		public void RemoveCoefficients(int count)
		{
			if (_coefficientsArr.Length < count)
				throw new ApplicationException("");

			double[] newArr = new double[count];
			for (int i = 0; i < count; i++)
				newArr[i] = _coefficientsArr[i];
			_coefficientsArr = newArr;
		}
	}
}
