using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
	/// Logika interakcji dla klasy DataViewWindow.xaml
	/// </summary>
	public partial class DataViewWindow : Window
	{
		private int maxDataCount = 30;
		//private int maxHistCount = 10;

		public DataViewWindow()
		{
			InitializeComponent();
			Loaded += Window_Loaded;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			UserDataGrid.Width = this.Width - 40;
			UserDataGrid.Height = this.Height - 80;
			GetDecryptAndShow();
			ImageBrush imgBrush = new ImageBrush();
			Image img = new Image();
			img.Source = new BitmapImage(new Uri("C:\\Users\\Szatan\\Pictures\\Jebaited.png"));
			imgBrush.ImageSource = img.Source;
			UserDataGrid.Background = imgBrush;
		}

		private void GetDecryptAndShow()
		{
			DataStatic.UserData = DataOperations.UserDataOperations.SelectUserData(DataStatic.LoggedUser.Id);

			foreach (var userDataDto in DataStatic.UserData)
			{
				userDataDto.ServPassword = MyAes.DecryptStringToString(userDataDto.ServPassword);
			}

			UserDataGrid.ItemsSource = DataStatic.UserData;
			UserDataGrid.Columns[0].Visibility = Visibility.Collapsed;
			UserDataGrid.Columns[UserDataGrid.Columns.Count - 1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
			UserDataGrid.Columns[1].Header = "Nazwa serwisu";
			UserDataGrid.Columns[2].Header = "Hasło";
			UserDataGrid.Columns[3].Header = "Komentarz";
		}

		private void AddDataItem_Click(object sender, RoutedEventArgs e)
		{
			if (DataOperations.UserDataOperations.SelectUserDataCount(DataStatic.LoggedUser.Id) < maxDataCount)
			{
				var addWindow = new AddUserDataWindow();
				addWindow.Closing += AddWindowOnClosing;
				addWindow.Show();
			}
		}

		private void AddWindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
		{
			GetDecryptAndShow();
		}

		private void EditDataItem_OnClick(object sender, RoutedEventArgs e)
		{
			try
			{
				var menuItem = (MenuItem)sender;
				var contextMenu = (ContextMenu)menuItem.Parent;
				var item = (DataGrid)contextMenu.PlacementTarget;
				var toEdit = (UserDataDto)item.SelectedCells[0].Item;

				var addWindow = new AddUserDataWindow(toEdit.Id, toEdit.Comment, toEdit.ServName, toEdit.ServPassword, true);
				addWindow.Closing += AddWindowOnClosing;
				addWindow.Show();
			}
			catch (Exception exception)
			{
				Debug.WriteLine(exception);
				//throw;
			}			
		}

		private void DeleteDataItem_OnClick(object sender, RoutedEventArgs e)
		{
			var menuItem = (MenuItem) sender;
			var contextMenu = (ContextMenu) menuItem.Parent;
			var item = (DataGrid) contextMenu.PlacementTarget;
			var toDelete = (UserDataDto)item.SelectedCells[0].Item;
			//MessageBox.Show("Id: " + toDelete.Id + "\nPass: " + toDelete.ServPassword);
			
			DataOperations.UserDataOperations.DeleteUserData(toDelete.Id);
			GetDecryptAndShow();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			App.Current.Shutdown(0);
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			UserDataGrid.Width = this.Width - 40;
			UserDataGrid.Height = this.Height - 80;
		}

		private void ShowHistOfItem_Click(object sender, RoutedEventArgs e)
		{
			var menuItem = (MenuItem)sender;
			var contextMenu = (ContextMenu)menuItem.Parent;
			var item = (DataGrid)contextMenu.PlacementTarget;
			var toShowHist = (UserDataDto)item.SelectedCells[0].Item;

			var histWindow = new HistViewWindow(toShowHist.Id);
			histWindow.Show();
		}
	}
}