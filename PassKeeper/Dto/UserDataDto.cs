using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassKeeper.Dto
{
    public class UserDataDto
    {
		public int Id { get; set; }
		public string ServName { get; set; }
		public string ServPassword { get; set; }
		public string Comment { get; set; }
	}
}