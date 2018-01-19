using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassKeeper.Dto;

namespace PassKeeper.DataOperations
{
	public static class PasswordHistoryOperations
	{
		public static List<PasswordHistoryDto> SelectPasswordHistory(int dataId)
		{
			using (var db = new DbModel())
			{
				var hist = from x in db.PASSWORD_HISTORY
					join y in db.USER_DATA on x.DATA_ID equals y.ID
					where y.ID == dataId
					select new PasswordHistoryDto()
					{
						DateHist = x.DATE_HIST,
						Id = x.ID,
						PasswordHist = x.PASSWORD_HIST
					};

				return hist.ToList();
			}
		}

		public static int SelectPasswordHistoryCount(int dataId)
		{
			using (var db = new DbModel())
			{
				var histCount = (from x in db.PASSWORD_HISTORY
					join y in db.USER_DATA on x.DATA_ID equals y.ID
					where y.ID == dataId
					select x).Count();

				return histCount;
			}
		}

		public static void AddPasswordHistory(DateTime dateHist, string passwordHist, int dataId)
		{
			PASSWORD_HISTORY hist = new PASSWORD_HISTORY()
			{
				DATA_ID = dataId,
				DATE_HIST = dateHist,
				PASSWORD_HIST = MyAes.EncryptStringToString(passwordHist)
			};

			using (var db = new DbModel())
			{
				db.PASSWORD_HISTORY.Add(hist);
				db.SaveChanges();
			}
		}

		public static void DeletePasswordHistory(int id)
		{
			using (var db = new DbModel())
			{
				var deleteHist = (from x
					in db.PASSWORD_HISTORY
					where x.ID == id
					select x).SingleOrDefault();

				if (deleteHist != null)
				{
					db.PASSWORD_HISTORY.Remove(deleteHist);
					db.SaveChanges();
				}
			}
		}

		public static int SelectOldest(int dataId)
		{
			using (var db = new DbModel())
			{
				var hist = (from x
					in db.PASSWORD_HISTORY
					where x.DATA_ID == dataId
					orderby x.DATE_HIST ascending
					select x).First();

				return hist.ID;
			}
		}
	}
}