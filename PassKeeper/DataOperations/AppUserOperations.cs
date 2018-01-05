using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassKeeper.Dto;

namespace PassKeeper.DataOperations
{
	public static class AppUserOperations
	{
		public static AppUserDto SelectAppUser(string email, string password)
		{
			using (var db = new DbModel())
			{
				var user = (from x
					in db.APP_USER
					where x.EMAIL.Equals(email)
					select new AppUserDto()
					{
						Email = x.EMAIL,
						Id = x.ID,
						Password = x.PASSWORD
					}).SingleOrDefault();

				if (user != null && user.Password.Equals(MyAes.EncryptStringToString(password)))
					return user;
				return null;
			}
		}

		public static bool AddAppUser(string email, string password)
		{
			bool returnValue = false;

			using (var db = new DbModel())
			{
				var users = from x
					in db.APP_USER
					where x.EMAIL.Equals(email)
					select x;

				if (!users.Any())
				{
					APP_USER newUser = new APP_USER()
					{
						EMAIL = email,
						PASSWORD = MyAes.EncryptStringToString(password)
					};
					db.APP_USER.Add(newUser);
					db.SaveChanges();

					returnValue = true;
				}
			}

			return returnValue;
		}
	}
}
