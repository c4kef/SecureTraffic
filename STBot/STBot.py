import telebot
import clr

clr.AddReference("D:\Build\Debug\STLib.dll")
from STLib import Class1
from telebot import apihelper

token = '2045425760:AAHossESgiQy5DAgtnUse3vDFTyTgkYufGk'
bot = telebot.TeleBot(token)

@bot.message_handler(commands=['start'])
def start_message(message):
    keyboard = telebot.types.ReplyKeyboardMarkup(True)
    keyboard.row('/test')
    bot.send_message(message.chat.id, 'Привет!', reply_markup=keyboard)

@bot.message_handler(commands=['test'])
def start_message(message):
    markup = telebot.types.InlineKeyboardMarkup()
    for x in Class1.GetStrings():
        markup.add(telebot.types.InlineKeyboardButton(text = x, callback_data = x))
    bot.send_message(message.chat.id, text="Добрый день, выберите задачу:", reply_markup=markup)

@bot.callback_query_handler(func=lambda call: True)
def query_handler(call):
    b = ['1. Задачи', '2. Задачи', '3. Задачи']
    bot.answer_callback_query(callback_query_id=call.id, text='Спасибо за честный ответ!')
    print(call.data)

    answer = 'Вы троечник!'
    bot.send_message(call.message.chat.id, answer)
    bot.edit_message_reply_markup(call.message.chat.id, call.message.message_id)

bot.polling()
    


