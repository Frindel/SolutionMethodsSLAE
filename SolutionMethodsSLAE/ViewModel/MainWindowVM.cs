using SolutionMethodsSLAE.Model;
using SolutionMethodsSLAE.Model.Data;
using System.ComponentModel;
using System;

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
				if (_SLAEController.SLAE.Equations.Count == 0)
					return;

				switch ((obj as string).ToLower())
				{
					case "матричный метод":
						var rez = DirectSolutionMethods.GetRezultOfMatrixMethod(_SLAEController.SLAE);
						break;
				}
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
		private void OnPropertyChanged(string propertyName = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
