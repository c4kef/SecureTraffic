using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace STLib.Utils
{
	public class Users
	{
		[PrimaryKey]
		public int Id { get; set; }
		[NotNull]
		public string Name { get; set; }
		[NotNull]
		public string Password { get; set; }
	}
}
