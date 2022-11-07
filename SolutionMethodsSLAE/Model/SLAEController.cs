using SolutionMethodsSLAE.Model.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SolutionMethodsSLAE.Model
{
	internal class SLAEController : INotifyPropertyChanged
	{
		private static SLAEController _contorller;
		private SystemLinearAlgebraicEquations _SLAE;
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
				Resize(equationsCount: value);
				_equationsCount = value;
				OnPropertyChanged(nameof(SLAE));
			}
		}

		public int CoefficientsCount
		{
			get => _coefficientsCount;
			set
			{
				Resize(coefficientsCount: value);
				_coefficientsCount = value;
				OnPropertyChanged(nameof(SLAE));
			}
		}

		#endregion

		#region Constructor
		private SLAEController()
		{
			//определение первоначальной СЛАУ
			List<Equation> equations = new List<Equation>();
			equations.Add(new Equation(0, 0, 0, 0));
			equations.Add(new Equation(0, 0, 0, 0));
			equations.Add(new Equation(0, 0, 0, 0));
			_SLAE = new SystemLinearAlgebraicEquations(equations);

			_coefficientsCount = _equationsCount = 3;
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
			if (count < 0)
				throw new ApplicationException("");

			foreach (var a in _SLAE.Equations)
			{
				a.AddCoefficients(count);
			}
		}

		public void RemoveCoefficients(int count)
		{
			if (count > _coefficientsCount)
				throw new ApplicationException("");

			if (count < 0)
				throw new ApplicationException("");

			foreach (var a in _SLAE.Equations)
			{
				a.RemoveCoefficients(count);
			}

			//уведомление представления об изменении
			OnPropertyChanged(nameof(SLAE));
		}

		public void AddEquations(int count)
		{
			if (count < 0)
				throw new ApplicationException("");

			for (int i = 0; i < count; i++)
			{
				_SLAE.AddEquation(0, new double[_coefficientsCount]);
			}

			//уведомление представления об изменении
			OnPropertyChanged(nameof(SLAE));
		}

		public void RemoveEquations(int count)
		{
			if (count > _equationsCount)
				throw new ApplicationException("");

			if (count < 0)
				throw new ApplicationException("");

			while (_equationsCount - count != SLAE.Equations.Count)
			{
				SLAE.Equations.Remove(SLAE.Equations.Last());
			}

			//уведомление представления об изменении
			OnPropertyChanged(nameof(SLAE));
		}

		private void Resize(int equationsCount = -1, int coefficientsCount = -1)
		{
			if (equationsCount != -1)
			{
				if (_equationsCount > equationsCount)
					RemoveEquations(_equationsCount - equationsCount);
				else if (_equationsCount < equationsCount)
					AddEquations(-(_equationsCount - equationsCount));
			}
			if (coefficientsCount != -1)
			{
				if (_coefficientsCount > coefficientsCount)
					RemoveCoefficients(_coefficientsCount- coefficientsCount);
				else if (_coefficientsCount < coefficientsCount)
					AddCoefficients(-(_coefficientsCount- coefficientsCount));
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
