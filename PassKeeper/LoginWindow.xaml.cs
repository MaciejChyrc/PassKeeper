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
					authBox.Visibility = Visibility.Collapsed;
					authBtn.Visibility = Visibility.Collapsed;
					authBox.IsEnabled = false;
					authBtn.IsEnabled = false;

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
			timeoutLbl.Text = onTimeoutBlock;
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

		private void loginBtn_Click(object sender, RoutedEventArgs e)
		{
			string user = loginBox.Text;
			string pswd = passwordBox.Password;

			userDto = AppUserOperations.SelectAppUser(user, pswd);

			if (userDto != null)
			{
				authBox.Visibility = Visibility.Visible;
				authBtn.Visibility = Visibility.Visible;
				authBox.IsEnabled = true;
				authBtn.IsEnabled = true;

				timer.Start();
				stopwatch.Start();
				try
				{
					Task.Run((() => { SendAuthCode(user); }));
				}
				catch (SmtpException exc)
				{
					Debug.WriteLine(exc);
				}				
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Nieprawidłowe dane logowania, lub błąd w logowaniu.");
			}
		}

		private void authBtn_Click(object sender, RoutedEventArgs e)
		{
			DataStatic.LoggedUser = userDto;
			System.Diagnostics.Debug.WriteLine(DataStatic.LoggedUser.Email + DataStatic.LoggedUser.Password);
		}

		private void registerBtn_Click(object sender, RoutedEventArgs e)
		{
			string user = loginBox.Text;
			string pswd = passwordBox.Password;

			bool succeeded = AppUserOperations.AddAppUser(user, pswd);

			if (succeeded)
				MessageBox.Show("Założono nowe konto.");
			else MessageBox.Show("Zakładanie konta nie powiodło się.");
		}
	}
}