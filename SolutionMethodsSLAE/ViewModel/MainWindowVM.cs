using System.ComponentModel;
using SolutionMethodsSLAE.Model;
using System.Collections.Generic;

namespace SolutionMethodsSLAE.ViewModel
{
	internal class MainWindowVM : INotifyPropertyChanged
	{
		private SLAEController _SLAEController;

		#region Events
		public event PropertyChangedEventHandler? PropertyChanged;
		#endregion

		#region Propertys
		public SystemLinearAlgebraicEquations SLAE { get => _SLAEController.SLAE; }
		public int CoefficientsCount { get => _SLAEController.CoefficientsCount; set => _SLAEController.CoefficientsCount = value; }
		public int EquationsCount { get => _SLAEController.EquationsCount; set => _SLAEController.EquationsCount = value; }

		//при получении результата нужно будет уведомлять об изменении, т. к. результат не яляется уникальной сущностью.

		//public object Rezult
		//{ 
		//	get=> _rezult;
		//	set
		//	{
		//		_rezult = value;
		//		OnPropertyChanged(nameof(Rezult));
		//	}
		//}
		#endregion

		#region Commands
		public RelayCommand Calculate
		{
			get => new RelayCommand(obj =>
			{

			});
		}
		//! добавление/удаление уравнений и столбцов выполняем черех вызов команд
		#endregion

		#region Constructors
		public MainWindowVM()
		{
			_SLAEController = SLAEController.Get();

			//пример работы с классом SystemLinearAlgebraicEquations
			List<Equation> equations = new List<Equation>();
			equations.Add(new Equation(1,-1,1));
			equations.Add(new Equation(2,4));
			var a = new SystemLinearAlgebraicEquations(equations);
			var b = a.GetCoefficientsMatrix();
			var c = a.GetFreeValuesMatrix();
		}
		#endregion

		#region Methods
		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged!=null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
