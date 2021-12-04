import telebot
from telebot import types
import clr

clr.AddReference("D:\Build\Debug\STLib.dll")

from STLib import Main
from STLib.User import Manage
from STLib.AI import QHandler


Main.Init("D:\Build\Debug\identifier.sqlite") # Иницим либу

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

q = QHandler(0)
users_dict = {}
chat_id = None


@bot.message_handler(commands=commands_list)
def send_start(message):
    user_id = message.from_user.id
    users_dict[user_id] = User()
    users_dict[user_id].chat_id = message.chat.id
    users_dict[user_id].id = message.from_user.id
    
    msg = None
    for _ in range(3):
        q.GetStep()
        msg = bot.send_message(users_dict[user_id].chat_id,q.GetMessageStep())

    bot.register_next_step_handler(msg, process_qhandler)

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
        msg = bot.send_message(users_dict[user_id].chat_id, 'Введите новый пароль:')
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
        else:
            msg = bot.send_message(users_dict[user_id].chat_id, "Регистрация не удалась")
    except Exception as e:
        bot.send_message(users_dict[user_id].chat_id, f'Неполадки в системеx2 {e}')


def process_else_step(message):
    print("here?")

def process_qhandler(message):
    try:
        user_id = message.from_user.id
        chat_id = users_dict[user_id].chat_id
        answer = q.AddAnswerStep(message.text)
        msg = bot.send_message(users_dict[user_id].chat_id, "Отлично" if not answer else answer)
        
        if q.GetStep() == 1:
            msg = bot.send_message(users_dict[user_id].chat_id, q.GetMessageStep())

            bot.register_next_step_handler(msg, process_qhandler)
        else:
            process_register_step(msg);

    except Exception as e:
        bot.send_message(chat_id, f'Неполадки в системе {e}')

bot.enable_save_next_step_handlers(delay=1)

bot.load_next_step_handlers()

bot.infinity_polling()