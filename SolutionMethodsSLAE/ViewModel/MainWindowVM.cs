using SolutionMethodsSLAE.Model;
using SolutionMethodsSLAE.Model.Data;
using System.ComponentModel;
using System.Windows;
using System;

namespace SolutionMethodsSLAE.ViewModel
{
	internal class MainWindowVM : INotifyPropertyChanged
	{
		private SLAEController _SLAEController;
		private int[] _sizes;

		#region Events
		public event PropertyChangedEventHandler? PropertyChanged;
		#endregion

		#region Propertys

		public SystemLinearAlgebraicEquations SLAE { get => _SLAEController.SLAE; }

		public int Size { get => _SLAEController.CoefficientsCount; set => _SLAEController.CoefficientsCount = _SLAEController.EquationsCount = value; }
		public int[] Sizes
		{
			get => _sizes;
			set
			{
				_sizes = value;
				OnPropertyChanged(nameof(Sizes));
			}
		}
		#endregion

		#region Commands

		public RelayCommand Calculate
		{
			get => new RelayCommand(obj =>
			{
				if (_SLAEController.SLAE.Equations.Count == 0)
					return;

				Matrix rez= null!;

				switch ((obj as string).ToLower())
				{
					case "матричный метод":
						rez = DirectSolutionMethods.GetRezultOfMatrixMethod(_SLAEController.SLAE);
						break;
					case "метод крамера":

						break;
					case "метод гаусса":
						rez = DirectSolutionMethods.GetRezultOfGaussMethod(_SLAEController.SLAE);
						break;
					case "метод гаусса-жордано":

						break;
				}

				//вывод сообщения в случае невозможности решить СЛАУ
				if (rez == null)
				{
					MessageBox.Show("Невозможно решить СЛАУ");
					return;
				}

				//отображение результата вычисления
				string message = "";
				for (int i = 0; i < rez.RowCount; i++)
					message += $"x{i+1} = {Math.Round(rez[i, 0], 2, MidpointRounding.ToEven)}\n";
				MessageBox.Show(message);

			});
		}
		#endregion

		#region Constructors
		public MainWindowVM()
		{
			_SLAEController = SLAEController.Get();

			//заполнения массива допустимых размеров
			_sizes = new int[20];
			for (int i = 0; i < _sizes.Length; i++)
				_sizes[i] = i + 1;
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
