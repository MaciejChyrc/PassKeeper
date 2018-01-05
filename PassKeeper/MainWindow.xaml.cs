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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PassKeeper.Dto;

namespace PassKeeper
{
	/// <summary>
	/// Logika interakcji dla klasy MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			using (var db = new DbModel())
			{
				//var user = DataOperations.AppUserOperations.SelectAppUser(@"adres1@gmail.com", "xd");
				//System.Diagnostics.Debug.WriteLine(user.Id + user.Email + user.Password);
				var userData = DataOperations.UserDataOperations.SelectUserData(1);
				foreach (UserDataDto dto in userData)
				{
					System.Diagnostics.Debug.WriteLine(dto.Id + dto.ServName + dto.ServPassword);
				}
				/*var user = from x in db.APP_USER where x.EMAIL.Equals("xdxc@gmail.com") select x;
				List<APP_USER> au = user.ToList();
				List<USER_DATA> ud = new List<USER_DATA>();
				List<PASSWORD_HISTORY> ph = new List<PASSWORD_HISTORY>();

				AppUserDto appUserDto = new AppUserDto()
				{
					Email = au[0].EMAIL,
					Password = au[0].PASSWORD,
					Id = au[0].ID
				};
				List<UserDataDto> userDataDtos = new List<UserDataDto>();
				List<List<PasswordHistoryDto>> passHistDtos = new List<List<PasswordHistoryDto>>();
				if (user.Any()) //sprawdzenie czy jest chociaz 1 rekord
				{
					foreach (APP_USER appUser in user)
					{
						System.Diagnostics.Debug.WriteLine(appUser.ID + " " + appUser.EMAIL + " " + appUser.PASSWORD);
					}

					foreach (APP_USER appUser in user)
					{
						ud = appUser.USER_DATA.ToList();

						foreach (USER_DATA userData in appUser.USER_DATA)
						{
							ph = userData.PASSWORD_HISTORY.ToList();
						}
					}
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("user empty");
				}*/

				/*string toEncrypt = "Jakiś string.";
				System.Diagnostics.Debug.WriteLine(toEncrypt);
				string encrypted = MyAes.EncryptStringToString(toEncrypt);
				System.Diagnostics.Debug.WriteLine(encrypted);
				string decrypted = MyAes.DecryptStringToString(encrypted);
				System.Diagnostics.Debug.WriteLine(decrypted);*/

				/*APP_USER newUser = new APP_USER()
				{
					EMAIL = "xd@gmail.com",
					PASSWORD = "zxcvasdf"
				};
				db.APP_USER.Add(newUser);
				db.SaveChanges();*/

				/*var dto = from x in db.APP_USER where x.EMAIL.Equals("xd@gmail.com") select x;
				foreach (APP_USER appUser in dto)
				{
					System.Diagnostics.Debug.WriteLine(appUser.ID + " " + appUser.EMAIL + " " + appUser.PASSWORD);
				}*/
				/*var hist = from x in db.PASSWORD_HISTORY orderby x.DATE_HIST ascending select x;
				foreach (var element in hist)
				{
					System.Diagnostics.Debug.WriteLine(element.ID + " " + element.PASSWORD_HIST + " " + element.DATE_HIST.ToShortDateString());
				}*/
			}
		}
	}
}