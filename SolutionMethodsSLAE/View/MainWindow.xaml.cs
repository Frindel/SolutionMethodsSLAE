using System.Windows;
using System.Windows.Controls;

namespace SolutionMethodsSLAE.View
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new ViewModel.MainWindowVM();
		}
	}
}
