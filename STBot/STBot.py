import telebot
from telebot import types
import clr
import json

clr.AddReference("D:\Build\Debug\STLib.dll")

from STLib import Main
from STLib.User import Manage
from STLib.AI import QHandler
from STLib.AI import LHandler

Main.Init("C:\\Users\\artem\\source\\repos\\SecureTraffic\\identifier.sqlite", "D:\\Build\\Debug\\basestart.json") # Иницим либу

API_TOKEN = '2045425760:AAHossESgiQy5DAgtnUse3vDFTyTgkYufGk'

bot = telebot.TeleBot(API_TOKEN)

commands_list = {'test', 'start', 'test', 'reg'}

class User:
    def __init__(self):
        self.id = None
        self.chat_id = None
        self.login = None
        self.password = None
        self.auth_message_id: list = []
        self.qhandler = None
        self.lhandler = None
        self.selectedQuestion = None

users_dict = {}
chat_id = None

@bot.message_handler(commands=commands_list)
def send_start(message):
    user_id = message.from_user.id
    users_dict[user_id] = User()
    users_dict[user_id].qhandler = QHandler(user_id)
    users_dict[user_id].lhandler = LHandler(user_id)
    users_dict[user_id].chat_id = message.chat.id
    users_dict[user_id].id = message.from_user.id
    
    if Manage.CheckExists(user_id):
        process_select_test(message)
    else:
        msg = None
        for _ in range(3):
            users_dict[user_id].qhandler.GetStep()
            msg = bot.send_message(users_dict[user_id].chat_id,users_dict[user_id].qhandler.GetMessageStep())

        bot.register_next_step_handler(msg, process_qhandler)

#Генерация вопросов для регистрации
def process_qhandler(message):
    try:
        user_id = message.from_user.id
        chat_id = users_dict[user_id].chat_id
        answer = users_dict[user_id].qhandler.AddAnswerStep(message.text)
        msg = bot.send_message(users_dict[user_id].chat_id, "Отлично" if not answer else answer)
        
        if users_dict[user_id].qhandler.GetStep() == 1:
            msg = bot.send_message(users_dict[user_id].chat_id, users_dict[user_id].qhandler.GetMessageStep())

            bot.register_next_step_handler(msg, process_qhandler)
        else:
            process_register_step(message);

    except Exception as e:
        bot.send_message(chat_id, f'Неполадки в системе {e}')

#REGISTER STEP
def process_register_step(message):
    user_id = message.from_user.id
    msg = bot.send_message(users_dict[user_id].chat_id, "Мы видим вас впервые, введите логин:")
    users_dict[user_id].auth_message_id.append(msg.message_id)
    bot.register_next_step_handler(msg, process_login_step)

def process_login_step(message):
    try:
        user_id = message.from_user.id
        users_dict[user_id].chat_id = message.chat.id
        users_dict[user_id].login = message.text

        users_dict[user_id].auth_message_id.append(message.message_id)
        msg = bot.send_message(users_dict[user_id].chat_id, 'Введите пароль:')
        users_dict[user_id].auth_message_id.append(msg.message_id)
        bot.register_next_step_handler(msg, process_pass_step)
    except Exception as e:
        bot.send_message(chat_id, f'Неполадки в системеx1 {e}')

def process_pass_step(message):
    try:
        user_id = message.from_user.id
        users_dict[user_id].chat_id = message.chat.id
        users_dict[user_id].auth_message_id.append(message.message_id)
        users_dict[user_id].password = message.text

        request = Manage.Register(user_id, users_dict[user_id].login, users_dict[user_id].password)

        for message_id in users_dict[user_id].auth_message_id:
            bot.delete_message(users_dict[user_id].chat_id, message_id, 0.2)
        users_dict[user_id].auth_message_id.clear()
        if request:
            msg = bot.send_message(users_dict[user_id].chat_id, "Регистрация успешна")
            process_select_test(message)
        else:
            msg = bot.send_message(users_dict[user_id].chat_id, "Регистрация не удалась")
    except Exception as e:
        bot.send_message(users_dict[user_id].chat_id, f'Неполадки в системеx2 {e}')
######

#SELECT THEME STEP
@bot.callback_query_handler(func = lambda call: True)
def answer_for_button(call):
    user_id = call.from_user.id
    chat_id = users_dict[user_id].chat_id
    if users_dict[user_id].lhandler.SelectTest(call.data) == 0:
        answer = users_dict[user_id].lhandler.CheckAnswer(call.data)
        bot.send_message(users_dict[user_id].chat_id, "Отлично" if answer else "Плохо")
   
    bot.edit_message_reply_markup(call.message.chat.id, call.message.message_id, reply_markup=None)
    
    if users_dict[user_id].lhandler.CheckSteps() == 1:
        markup = telebot.types.InlineKeyboardMarkup()
        js = json.loads(users_dict[user_id].lhandler.GetQuestion())

        for x in js["answers"]:
            markup.add(telebot.types.InlineKeyboardButton(text = x, callback_data = x))

        bot.send_message(chat_id, text = js["question"], reply_markup = markup)
    else:
        process_select_test(call)

def process_select_test(message):
    markup = telebot.types.InlineKeyboardMarkup()

    for x in json.loads(users_dict[message.from_user.id].lhandler.GetRecomendation()):
        markup.add(telebot.types.InlineKeyboardButton(text = x["title"], callback_data = x["title"]))

    bot.send_message(users_dict[message.from_user.id].chat_id, text = "Привет, выбери материал для изучения", reply_markup = markup)
######

bot.enable_save_next_step_handlers(delay=1)

bot.load_next_step_handlers()

bot.infinity_polling()