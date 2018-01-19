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

namespace PassKeeper
{
    /// <summary>
    /// Logika interakcji dla klasy AddUserDataWindow.xaml
    /// </summary>
    public partial class AddUserDataWindow : Window
    {
	    private bool editMode;
	    private int dataId;

		public AddUserDataWindow()
        {
            InitializeComponent();
	        editMode = false;
	        dataId = -1;
        }

	    public AddUserDataWindow(int id, string comment, string servName, string servPassword, bool edit)
	    {
		    InitializeComponent();
		    editMode = edit;
		    dataId = id;

		    if (editMode)
		    {
			    PassBox.Text = servPassword;
			    NameBox.Text = servName;
			    CommentBox.Text = comment;

				PassBox.Background = Brushes.White;
			    AddBtn.Content = "Edytuj";
				AddBtn.IsEnabled = true;
			}
	    }

		private void PassBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (String.IsNullOrEmpty(PassBox.Text) || String.IsNullOrWhiteSpace(PassBox.Text) || PassBox.Text.Length > 255)
			{
				PassBox.Background = Brushes.LightCoral;
				AddBtn.IsEnabled = false;
			}
			else
			{
				PassBox.Background = Brushes.White;
				AddBtn.IsEnabled = true;
			}
		}

		private void AddBtn_Click(object sender, RoutedEventArgs e)
		{
			if (editMode) EditUserData(NameBox.Text, PassBox.Text, CommentBox.Text);
			else AddUserData(NameBox.Text, PassBox.Text, CommentBox.Text);
			this.Close();
		}

	    private void AddUserData(string servName, string servPassword, string comment)
	    {
		    if (MyAes.EncryptStringToString(servPassword).Length > 255)
		    {
			    MessageBox.Show("Za długie hasło.\nMoże tak się zdarzyć przy zbyt dużej ilości polskich liter.");
				return;
		    }
			DataOperations.UserDataOperations.AddUserData(comment, servName, servPassword, DataStatic.LoggedUser.Id);
		    int newest = DataOperations.UserDataOperations.SelectNewest(DataStatic.LoggedUser.Id);
			DataOperations.PasswordHistoryOperations.AddPasswordHistory(DateTime.Today, servPassword, newest);
	    }

	    private void EditUserData(string servName, string servPassword, string comment)
	    {
		    if (MyAes.EncryptStringToString(servPassword).Length > 255)
		    {
			    MessageBox.Show("Za długie hasło.\nMoże tak się zdarzyć przy zbyt dużej ilości polskich liter.");
			    return;
		    }

			int histCount = DataOperations.PasswordHistoryOperations.SelectPasswordHistoryCount(dataId);
			if (histCount >= 10)
			{
				DataOperations.PasswordHistoryOperations.DeletePasswordHistory(DataOperations.PasswordHistoryOperations.SelectOldest(dataId));
			}

			DataOperations.UserDataOperations.UpdateUserData(comment, servName, servPassword, dataId);
			DataOperations.PasswordHistoryOperations.AddPasswordHistory(DateTime.Today, servPassword, dataId);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
		}
	}
}