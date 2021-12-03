from const import token, n, s1, s3
from imports import telebot, clr, Class1, telebot, apihelper



bot = telebot.TeleBot(token)



@bot.message_handler(commands=[s1])
def start_message(message):
    keyboard = telebot.types.ReplyKeyboardMarkup(True)
    keyboard.row(n)
    bot.send_message(message.chat.id, 'Привет', reply_markup = keyboard)
    user_id = message.from_user.id
    print(user_id)



@bot.message_handler(commands = [s3])
def start_message(message):
    bot.delete_message(message.chat.id, message.message_id)
    markup = telebot.types.InlineKeyboardMarkup()
    a = 1
    for x in Class1.GetQuestions():
        markup.add(telebot.types.InlineKeyboardButton(text = str(a) + '. ' + x, callback_data = x))
        a+=1
    bot.send_message(message.chat.id, text = "Добрый день, выберите задачу:", reply_markup = markup)
    

 
@bot.callback_query_handler(func = lambda call: True)
def query_handler(call):
    bot.answer_callback_query(callback_query_id = call.id, text = 'Спасибо за честный ответ!')
    print(call.data)
    answer = Class1.GetAnswer(call.data)
    bot.send_message(call.message.chat.id, answer)
    bot.edit_message_reply_markup(call.message.chat.id, call.message.message_id)

    

bot.polling()