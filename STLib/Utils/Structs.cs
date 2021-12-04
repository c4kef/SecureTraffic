using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace STLib.Utils
{
	/// <summary>
	/// Типы данных
	/// </summary>
	public enum BaseStartTypes
    {
		/// <summary>
		/// приветствие
		/// </summary>
		greetings,
		/// <summary>
		/// вопросы на генерацию маски
		/// </summary>
		gen_mask_question,
		/// <summary>
		/// пользователь не ответил на вопрос маски
		/// </summary>
		incorrect_answer_gen_mask,
		/// <summary>
		/// контрвопрос
		/// </summary>
		counterquestion
	}

	/// <summary>
	/// Объявление структуры для хранения вопросов
	/// </summary>
	public class BaseStartStr
	{
		public int id { get; set; }
		/// <summary>
		/// список возможных ответов
		/// </summary>
		public string[] results { get; set; }
		/// <summary>
		/// взять случайный ответ из списка?
		/// </summary>
		public bool randomResult { get; set; }
		/// <summary>
		/// нужен ли встречный ответ?
		/// </summary>
		public bool answerNeeded { get; set; }
		/// <summary>
		/// тип вопроса
		/// </summary>
		public BaseStartTypes type { get; set; }
		/// <summary>
		/// уровень вопроса
		/// </summary>
		public int level { get; set; }
		/// <summary>
		/// ключевые слова для обнаружения
		/// </summary>
		public string[] keywords { get; set; }
	}

	/// <summary>
	/// Класс сериализации (хранение и сохранение данных для работы ИИ)
	/// </summary>
	[Serializable]
	public class BaseStart
	{
		/// <summary>
		/// Максимальный уровень
		/// </summary>
		public int maxLevel { get; set; } = 1;
		/// <summary>
		/// Список запросов
		/// </summary>
		public List<BaseStartStr> instance { get; set; }
		/// <summary>
		/// Создание списка
		/// </summary>
		public BaseStart() => instance = new List<BaseStartStr>();
		/// <summary>
		/// Загрузка образа
		/// </summary>
		/// <returns>null если не найден, а так вернет образ BaseStart'а</returns>
		public void Load()
		{
			BaseStartStr baseStart = new BaseStartStr();
			baseStart.results = new string[] { "Здравствуйте, меня зовут Александра", "Привет! Меня зовут Александра" };
			baseStart.randomResult = "1" == "1" ? true : false;
			baseStart.answerNeeded = "0" == "1" ? true : false;
			baseStart.type = (BaseStartTypes)0;
			baseStart.level = 0;
			baseStart.keywords = new string[] { };

			instance.Add(baseStart);

			baseStart = new BaseStartStr();
			baseStart.results = new string[] { "Я работаю системным администратором в Вашей компании. Мне требуется задать несколько вопросов чтобы подготовить Вам премию." };
			baseStart.randomResult = "0" == "1" ? true : false;
			baseStart.answerNeeded = "0" == "1" ? true : false;
			baseStart.type = (BaseStartTypes)1;
			baseStart.level = 0;
			baseStart.keywords = new string[] { };

			instance.Add(baseStart);


			baseStart = new BaseStartStr();
			baseStart.results = new string[] { "Как печально :( Ну да ладно, давайте дальше!", "Понимаю. Давайте попробуем дальше", "Как жаль, давайте в таком случае перейдем к следующим вопросам" };
			baseStart.randomResult = "1" == "1" ? true : false;
			baseStart.answerNeeded = "0" == "1" ? true : false;
			baseStart.type = (BaseStartTypes)2;
			baseStart.level = 1;
			baseStart.keywords = new string[] { };

			instance.Add(baseStart);


			baseStart = new BaseStartStr();
			baseStart.results = new string[] { "Я не могу на это ответить, прости. Но я попрошу тебя ответить на прошлый вопрос еще раз" };
			baseStart.randomResult = "0" == "1" ? true : false;
			baseStart.answerNeeded = "0" == "1" ? true : false;
			baseStart.type = (BaseStartTypes)3;
			baseStart.level = 1;
			baseStart.keywords = new string[] { "зачем", "почему" };

			instance.Add(baseStart);

			baseStart = new BaseStartStr();
			baseStart.results = new string[] { "Как долго Вы работаете в компании?" };
			baseStart.randomResult = "0" == "1" ? true : false;
			baseStart.answerNeeded = "1" == "1" ? true : false;
			baseStart.type = (BaseStartTypes)1;
			baseStart.level = 1;
			baseStart.keywords = new string[] { "%num%","около","где-то","где то","примерно","долго","вечно" };

			instance.Add(baseStart);

			baseStart = new BaseStartStr();
			baseStart.results = new string[] { "В какие дни у Вас выходные?" };
			baseStart.randomResult = "0" == "1" ? true : false;
			baseStart.answerNeeded = "1" == "1" ? true : false;
			baseStart.type = (BaseStartTypes)1;
			baseStart.level = 1;
			baseStart.keywords = new string[] { "пон", "вт", "ср", "чет", "пят", "суб", "вос", "всег" };

			instance.Add(baseStart);
		}
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
