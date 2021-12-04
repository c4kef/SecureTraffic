from logging import NullHandler
from telebot.types import Message
from const import token, n, s1, s3, m
from imports import telebot, clr, telebot, apihelper, Main, Manage

currentStep = 0
botMessage =  None

token = '2045425760:AAHossESgiQy5DAgtnUse3vDFTyTgkYufGk'
bot = telebot.TeleBot(token)

@bot.message_handler(commands=[s1])
def start_message(message):
    global botMessage
    keyboard = telebot.types.ReplyKeyboardMarkup(True)
    keyboard.row(n)
    bot.send_message(message.chat.id, 'Привет', reply_markup = keyboard)  
    user_id = message.from_user.id
    botMessage = bot.send_message(message.chat.id,'Введите логин')

@bot.message_handler(content_types='text')
def a1(message):
    global currentStep
    global botMessage
    if currentStep == 0:
        print(message.text)
        bot.delete_message(botMessage.chat.id, botMessage.message_id)
        bot.delete_message(message.chat.id, message.message_id)
        botMessage = bot.send_message(message.chat.id,'Введите пароль')
        currentStep += 1
    elif currentStep == 1:
        print(message.text)
        bot.delete_message(botMessage.chat.id, botMessage.message_id)
        bot.delete_message(message.chat.id, message.message_id)
        botMessage = bot.send_message(message.chat.id,'Успех!')
        currentStep += 1

@bot.message_handler(commands = [s3])
def start_message(message):
    markup = telebot.types.InlineKeyboardMarkup()
    a = 1
    for x in Main.GetQuestions():
        markup.add(telebot.types.InlineKeyboardButton(text = str(a) + '. ' + x, callback_data = x))
        a+=1
    bot.send_message(message.chat.id, text = "Добрый день, выберите задачу:", reply_markup = markup)
    

@bot.callback_query_handler(func = lambda call: True)
def query_handler(call):
    bot.answer_callback_query(callback_query_id = call.id, text = 'Спасибо за честный ответ!')
    print(call.data)
    answer = Main.GetAnswer(call.data)
    bot.send_message(call.message.chat.id, answer)
    bot.edit_message_reply_markup(call.message.chat.id, call.message.message_id)
    

bot.polling()