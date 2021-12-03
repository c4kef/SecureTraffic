using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace STLib.Utils
{
	/// <summary>
	/// Образ таблицы пользователей (для добавления и создания)
	/// </summary>
	public class Users
	{
		/// <summary>
		/// id пользователя Telegram
		/// </summary>
		[PrimaryKey]
		public int Id { get; set; }
		/// <summary>
		/// Имя пользователя
		/// </summary>
		[NotNull]
		public string Name { get; set; }
		/// <summary>
		/// Пароль пользователя
		/// </summary>
		[NotNull]
		public string Password { get; set; }
	}
}
