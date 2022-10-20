using System.ComponentModel;
using SolutionMethodsSLAE.Model;

namespace SolutionMethodsSLAE.ViewModel
{
	internal class MainWindowVM : INotifyPropertyChanged
	{
		private Matrix _matrix;

		#region Events
		public event PropertyChangedEventHandler? PropertyChanged;
		#endregion

		#region Propertys
		public Matrix Matrix
		{
			get=> _matrix;
			set
			{
				_matrix = value;
				OnPropertyChanged(nameof(Matrix));
			}
		}
		#endregion

		#region Commands
		public RelayCommand Calculate
		{
			get => new RelayCommand(obj =>
			{

			});
		}
		#endregion

		#region Constructors
		public MainWindowVM()
		{
			//матрица - объект хранения данных
			_matrix = new Matrix(2,2);
		}
		#endregion

		#region Methods
		public void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged!=null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
