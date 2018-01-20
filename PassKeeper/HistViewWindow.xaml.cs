using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PassKeeper.Dto;

namespace PassKeeper
{
	/// <summary>
	/// Logika interakcji dla klasy HistViewWindow.xaml
	/// </summary>
	public partial class HistViewWindow : Window
	{
		private int dataId;
		private List<PasswordHistoryDto> dataHist;

		public HistViewWindow(int id)
		{
			InitializeComponent();
			dataId = id;
			Loaded += Window_Loaded;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			GetDecryptAndShow();
		}

		private void GetDecryptAndShow()
		{
			dataHist = DataOperations.PasswordHistoryOperations.SelectPasswordHistory(dataId);

			foreach (var passwordHistoryDto in dataHist)
			{
				passwordHistoryDto.PasswordHist = MyAes.DecryptStringToString(passwordHistoryDto.PasswordHist);
			}

			DataHistGrid.ItemsSource = dataHist;
			DataHistGrid.Columns[0].Visibility = Visibility.Collapsed;
			DataHistGrid.Columns[DataHistGrid.Columns.Count - 1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
			DataHistGrid.IsReadOnly = true;
		}
	}
}
