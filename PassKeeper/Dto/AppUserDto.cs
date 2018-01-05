using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassKeeper.Dto
{
	public class AppUserDto
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}
}