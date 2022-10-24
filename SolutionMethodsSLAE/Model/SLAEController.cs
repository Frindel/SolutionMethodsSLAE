using System.Collections.Generic;
using System.ComponentModel;

namespace SolutionMethodsSLAE.Model
{
	internal class SLAEController : INotifyPropertyChanged
	{
		private static SLAEController _contorller;
		private SystemLinearAlgebraicEquations _SLAE;
		private int _SLAECoefficientsCount;
		private int _equationsCount;
		private int _coefficientsCount;

		#region Events
		public event PropertyChangedEventHandler? PropertyChanged;
		#endregion

		#region Propertys
		public SystemLinearAlgebraicEquations SLAE
		{
			get => _SLAE;
			private set
			{
				_SLAE = value;
				OnPropertyChanged(nameof(SLAE));
			}
		}

		public int EquationsCount
		{
			get => _equationsCount;
			set
			{
				Resize(value, null);
				_equationsCount = value;
				OnPropertyChanged(nameof(EquationsCount));
			}
		}

		public int CoefficientsCount
		{
			get => _coefficientsCount;
			set
			{
				Resize(null, value);
				_coefficientsCount = value;
				OnPropertyChanged(nameof(CoefficientsCount));
			}
		}
		#endregion

		#region Constructor
		private SLAEController()
		{
			//определение первоначальной СЛАУ
			List<Equation> equations = new List<Equation>();
			equations.Add(new Equation(0, 0, 0));
			equations.Add(new Equation(0, 0, 0));
			_SLAE = new SystemLinearAlgebraicEquations(equations);

			_SLAECoefficientsCount = 2;
		}
		public static SLAEController Get()
		{
			if (_contorller == null)
				_contorller = new SLAEController();
			return _contorller;
		}
		#endregion

		#region Methods
		public void AddCoefficients(int count)
		{
			foreach (var a in _SLAE.Equations)
			{
				a.AddCoefficients(count);
			}
		}

		public void RemoveCoefficients(int count)
		{
			foreach (var a in _SLAE.Equations)
			{
				a.RemoveCoefficients(count);
			}
		}
		public void AddEquations(int count)
		{
			for (int i = 0; i < count; i++)
			{
				_SLAE.AddEquation(0, new double[count]);
			}

			//уведомление представления об изменении
			OnPropertyChanged(nameof(SLAE));
		}

		public void RemoveEquations(int count)
		{
			if (SLAE.Equations.Count == 0)
				return;

			while (count != 0 && SLAE.Equations.Count != 0)
			{
				Equation equation = _SLAE.Equations[_SLAE.Equations.Count - 1];
				SLAE.Equations.Remove(equation);

				count--;
			}

			//уведомление представления об изменении
			OnPropertyChanged(nameof(SLAE));
		}

		private void Resize(int? equationsCount, int? coefficientsCount)
		{
			if (equationsCount != null)
			{
				if (_equationsCount > equationsCount)
					RemoveEquations((int)equationsCount);
				else if (_equationsCount < equationsCount)
					AddEquations((int)equationsCount);
			}
			if (coefficientsCount != null)
			{
				if (_coefficientsCount > coefficientsCount)
					RemoveEquations((int)coefficientsCount);
				else if (_coefficientsCount < CoefficientsCount)
					AddEquations((int)coefficientsCount);
			}
		}

		//!контроллер должен сам уведомлять о своем состоянии
		private void OnPropertyChanged(string propertyName = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
