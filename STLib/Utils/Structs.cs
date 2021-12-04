using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace STLib.Utils
{
	/// <summary>
	/// Типы данных
	/// </summary>
	public enum BaseStartTypes
    {
		greetings,
		gen_mask_question,
		incorrect_answer_gen_mask,
		counterquestion
	}

	/// <summary>
	/// Объявление структуры для хранения вопросов
	/// </summary>
	public struct BaseStartStruct
    {
		public string[] results { get; set; }
		public bool randomResult { get; set; }
		public bool answerNeeded { get; set; }
		public BaseStartTypes type { get; set; }
		public int level { get; set; }
		public string[] keywords { get; set; }
    }

	/// <summary>
	/// Класс сериализации (хранение и сохранение данных для работы ИИ)
	/// </summary>
	[Serializable]
	public class BaseStart
    {
		public int maxLevel { get; set; }
		public List<BaseStartStruct> instance { get; set; }

		public BaseStart() => instance = new List<BaseStartStruct>();

		public static BaseStart? Load() => JsonConvert.DeserializeObject<BaseStart>(File.ReadAllText(@"basestart.json"));

		public static bool Exists() => File.Exists(@"basestart.json");

		public async void Save() => await File.WriteAllTextAsync(@"basestart.json", JsonConvert.SerializeObject(this, Formatting.Indented));
	}

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
