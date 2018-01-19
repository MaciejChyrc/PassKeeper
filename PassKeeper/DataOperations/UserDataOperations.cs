using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PassKeeper.Dto;

namespace PassKeeper.DataOperations
{
	public static class UserDataOperations
	{
		public static List<UserDataDto> SelectUserData(int userId)
		{
			using (var db = new DbModel())
			{
				var data = from x in db.USER_DATA
					join y in db.APP_USER on x.APP_USER_ID equals y.ID
					where y.ID == userId
					select new UserDataDto()
					{
						Comment = x.COMMENT,
						Id = x.ID,
						ServName = x.SERV_NAME,
						ServPassword = x.SERV_PASSWORD
					};

				return data.ToList();
			}
		}

		public static int SelectNewest(int userId)
		{
			using (var db = new DbModel())
			{
				var newest = (from x in db.USER_DATA
					join y in db.APP_USER on x.APP_USER_ID equals y.ID
					where y.ID == userId
					orderby x.ID descending
					select x).First();

				return newest.ID;
			}
		}

		public static int SelectUserDataCount(int userId)
		{
			using (var db = new DbModel())
			{
				var dataCount = (from x in db.USER_DATA
					join y in db.APP_USER on x.APP_USER_ID equals y.ID
					where y.ID == userId
					select x).Count();

				return dataCount;
			}
		}

		public static void AddUserData(string comment, string servName, string servPassword, int userId)
		{
			USER_DATA data = new USER_DATA()
			{
				APP_USER_ID = userId,
				COMMENT = comment,
				SERV_NAME = servName,
				SERV_PASSWORD = MyAes.EncryptStringToString(servPassword)
			};

			using (var db = new DbModel())
			{
				db.USER_DATA.Add(data);
				db.SaveChanges();
			}
		}

		public static void DeleteUserData(int id)
		{
			using (var db = new DbModel())
			{
				var deleteData = (from x
					in db.USER_DATA
					where x.ID == id
					select x).SingleOrDefault();

				if (deleteData != null)
				{
					db.USER_DATA.Remove(deleteData);
					db.SaveChanges();
				}
			}
		}

		public static void UpdateUserData(string comment, string servName, string servPassword, int id)
		{
			using (var db = new DbModel())
			{
				var updateData = (from x
					in db.USER_DATA
					where x.ID == id
					select x).SingleOrDefault();

				if (updateData != null)
				{
					updateData.COMMENT = comment;
					updateData.SERV_NAME = servName;
					updateData.SERV_PASSWORD = MyAes.EncryptStringToString(servPassword);
					db.SaveChanges();
				}
			}
		}
	}
}