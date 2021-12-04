from telebot.types import Message
from const import token, n, s1, s3, m
from imports import telebot, clr, telebot, apihelper, Main, Manage


def a1(message):
    l = bot.send_message(message.chat.id,'Введите логин')
    @bot.message_handler(content_types='text')
    def message_reply(message):
        h = message.text
        print(h)

        

def a2(message):
    l = bot.send_message(message.chat.id,'Введите пароль')
    @bot.message_handler(content_types='text')
    def message_reply(message):
        h = message.text
        print(h)
        bot.delete_message(l.chat.id, l.message_id)
        bot.delete_message(message.chat.id, message.message_id)

@bot.message_handler(commands=[s1])
def start_message(message):
    keyboard = telebot.types.ReplyKeyboardMarkup(True)
    keyboard.row(n)
    bot.send_message(message.chat.id, 'Привет', reply_markup = keyboard)  
    user_id = message.from_user.id
    print(user_id)
    if  Manage.CheckExists(user_id) == 'true':
        print("yj")
    else:
        a1(message)
        bot.register_next_step_handler(message, a2)
        

            
            

















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