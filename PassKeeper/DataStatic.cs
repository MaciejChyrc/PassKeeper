using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassKeeper.Dto;

namespace PassKeeper
{
	public static class DataStatic
	{
		public static AppUserDto LoggedUser { get; set; }
		public static List<UserDataDto> UserData { get; set; }
	}
}
