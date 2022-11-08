using System.ComponentModel;
using SolutionMethodsSLAE.Model;
using System.Collections.Generic;
using SolutionMethodsSLAE.Model.Data;

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
		
		public int Size { get => _SLAEController.CoefficientsCount; set => _SLAEController.CoefficientsCount = _SLAEController.EquationsCount = value; }

		#endregion

		#region Commands
		
		public RelayCommand Calculate
		{
			get => new RelayCommand(obj =>
			{
				var a = _SLAEController;
			});
		}
		#endregion

		#region Constructors
		public MainWindowVM()
		{
			_SLAEController = SLAEController.Get();
		}
		#endregion

		#region Methods
		private void OnPropertyChanged(string propertyName="")
		{
			if (PropertyChanged!=null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
