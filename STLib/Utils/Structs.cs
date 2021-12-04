﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using STLib.Utils;

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
	public struct BaseStartStruct
	{
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
		public List<BaseStartStruct> instance { get; set; }
		/// <summary>
		/// Создание списка
		/// </summary>
		public BaseStart() => instance = new List<BaseStartStruct>();
		/// <summary>
		/// Загрузка образа
		/// </summary>
		/// <returns>null если не найден, а так вернет образ BaseStart'а</returns>
		public static BaseStart Load(string path) => File.ReadAllText(path).FromJson<BaseStart>();
	}

	/// <summary>
	///Образ таблицы пользователей (для добавления и создания) 
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