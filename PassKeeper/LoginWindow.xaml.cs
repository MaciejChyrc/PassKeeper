using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using PassKeeper.DataOperations;
using PassKeeper.Dto;
using Timer = System.Timers.Timer;

namespace PassKeeper
{
	/// <summary>
	/// Logika interakcji dla klasy LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		private AppUserDto userDto;
		private Stopwatch stopwatch;
		private DispatcherTimer timer;
		private int timeElapsed;
		private int TimeElapsed
		{
			get
			{ return timeElapsed; }
			set
			{
				timeElapsed = value;
				if (timeElapsed >= 300) //5 minut
				{
					AuthBox.Visibility = Visibility.Collapsed;
					AuthBtn.Visibility = Visibility.Collapsed;
					AuthBox.IsEnabled = false;
					AuthBtn.IsEnabled = false;
					LoginBtn.IsEnabled = true;
					RegisterBtn.IsEnabled = true;

					timer.Stop();
					stopwatch.Stop();
					stopwatch.Reset();
					timeElapsed = 0;
				}
			}
		}
		private int authCode;

		//private CancellationTokenSource cts;
		//private CancellationToken ct;
		public LoginWindow()
		{
			InitializeComponent();
			Loaded += LoginWindow_Loaded;
		}

		private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
		{
			//cts = new CancellationTokenSource();
			//ct = cts.Token;
			timer = new DispatcherTimer();
			stopwatch = new Stopwatch();

			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			TimeElapsed += 1;
			string onTimeoutBlock = $"{stopwatch.Elapsed.Minutes:00}:{stopwatch.Elapsed.Seconds:00}";
			TimeoutLbl.Text = onTimeoutBlock;
		}

		private void SendAuthCode(string emailAddress)
		{
			Random rand = new Random();
			authCode = rand.Next(1000, 10000);
			string subject = "PassKeeper - kod dostępu";
			string body = "Twój kod: <br/>" + "<b>" + authCode + "</b>";
			SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
			MailMessage msg = new MailMessage("bd2passkeeper@gmail.com", emailAddress, subject, body);
			
			smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtpClient.EnableSsl = true;
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = new NetworkCredential("bd2passkeeper@gmail.com", "AuTHpSSwd2");
			msg.IsBodyHtml = true;

			smtpClient.Send(msg);
		}

		private void LoginBtn_Click(object sender, RoutedEventArgs e)
		{
			string user = LoginBox.Text;
			string pswd = PasswordBox.Password;

			if (MyAes.EncryptStringToString(pswd).Length <= 255)
				userDto = AppUserOperations.SelectAppUser(user, pswd);

			if (userDto != null)
			{
				AuthBox.Visibility = Visibility.Visible;
				AuthBtn.Visibility = Visibility.Visible;
				AuthBox.IsEnabled = true;
				AuthBtn.IsEnabled = true;
				LoginBtn.IsEnabled = false;
				RegisterBtn.IsEnabled = false;

				timer.Start();
				stopwatch.Start();
				try
				{
					Task.Run(() => { SendAuthCode(user); });
				}
				catch (SmtpException exc)
				{
					Debug.WriteLine(exc);
				}				
			}
			else
			{
				MessageBox.Show("Nieprawidłowe dane logowania.");
			}
		}

		private void AuthBtn_Click(object sender, RoutedEventArgs e)
		{
			int res = 0;
			if (int.TryParse(AuthBox.Text, out res))
			{
				if (res == authCode)
				{
					DataStatic.LoggedUser = userDto;
					System.Diagnostics.Debug.WriteLine(DataStatic.LoggedUser.Email + DataStatic.LoggedUser.Password);
					App.Current.MainWindow = new DataViewWindow();
					App.Current.MainWindow.Show();
					this.Close();
				}
				else
				{
					MessageBox.Show("Nieprawidłowy kod.");
				}
			}
		}

		private void RegisterBtn_Click(object sender, RoutedEventArgs e)
		{
			string user = LoginBox.Text;
			string pswd = PasswordBox.Password;
			if (MyAes.EncryptStringToString(pswd).Length <= 255)
			{
				bool succeeded = AppUserOperations.AddAppUser(user, pswd);

				if (succeeded)
					MessageBox.Show("Założono nowe konto.");
				else MessageBox.Show("Zakładanie konta nie powiodło się.");
			}
			else MessageBox.Show("Zakładanie konta nie powiodło się.");
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			if (!App.Current.MainWindow.IsActive)
			{
				App.Current.Shutdown(0);
			}
		}
	}
}