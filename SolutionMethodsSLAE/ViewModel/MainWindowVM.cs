using System.ComponentModel;
using SolutionMethodsSLAE.Model;
using System.Collections.Generic;
using System.Data;

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
		public double[,] Coefficients
		{
			get => SLAE.GetCoefficientsMatrix().ToArray(); 
			set { /* Идея 1: чистить коллекцию коэффициентов и снова инициазировать ее массивом */
			   	  /* Идея 2: убрать в классе SLAE поле Equations, заменив на поля Coefficients + freeValues и сделать привязку именно к ним */
				  /* Идея 3: пойти потрогать траву и подышать свежим воздухом */
			}
		}
		public double[,] FreeValues
		{
			get => SLAE.GetFreeValuesMatrix().ToArray(); 
			set { }
		}

        public DataView DataView { get; set; }


		public int Size { get => _SLAEController.CoefficientsCount; set => _SLAEController.CoefficientsCount = value; }



      /*  public object Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }

        }*/
        #endregion

        #region Commands
        private RelayCommand calculate;
		private RelayCommand show;
		public RelayCommand Calculate
		{
			get
			{
				return calculate ??
				(calculate = new RelayCommand(obj =>
				{
					UpdateMatrix(Size);
				}));
			}
		}
		public RelayCommand Show
		{
			get
			{
				return show ??
				(show = new RelayCommand(obj =>
				{
					string tmp = string.Empty;
					int i = 0;
					foreach (var item in SLAE.GetCoefficientsMatrix().ToArray())
                    {
						i += 1;
						if(i<Size)
							tmp += item.ToString() + " ";
						else
                        {
							tmp += item.ToString() + '\n';
							i = 0;
                        }
                    }
					System.Windows.MessageBox.Show(tmp);
				}));
			}
		}

		//! добавление/удаление уравнений и столбцов выполняем черех вызов команд
		#endregion

		#region Constructors
		public MainWindowVM()
		{
			_SLAEController = SLAEController.Get();

			//пример работы с классом SystemLinearAlgebraicEquations
			List<Equation> equations = new List<Equation>();
			equations.Add(new Equation(1, -1, 1));
			equations.Add(new Equation(2, 4));
			var a = new SystemLinearAlgebraicEquations(equations);
			var b = a.GetCoefficientsMatrix();
			var c = a.GetFreeValuesMatrix();
		}
		#endregion

		#region Methods
		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		private void UpdateMatrix(int size)
		{
			SLAE.Clear();
			System.Windows.MessageBox.Show(size.ToString());
			for (int i = 0; i < Size; i++)
			{
				SLAE.AddEquation(new Equation(size));
			}
			OnPropertyChanged("SLAE");
			OnPropertyChanged("Coefficients");
			OnPropertyChanged("FreeValues");
		}

		#endregion
	}
}
