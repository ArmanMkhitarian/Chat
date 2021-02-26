using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatWCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ChatService : IChatService
    {
        List<User> users = new List<User>();
        int NewUserId = 1;

        public int Connect(string name)
        {
            User user = new User()
            {
                ID = NewUserId,
                Name = name,
                OperationContext = OperationContext.Current
            };

            NewUserId++; //Генерируем новый Id для следующего юзера
            SendMessage(" " + user.Name +" присоединился к чату", 0);
            users.Add(user);
            return user.ID;
        }

        public void Disonnect(int id)
        {
            var user = users.FirstOrDefault(x => x.ID == id);
            if(user != null)
            {
                users.Remove(user);
                SendMessage(" " +user.Name + " вышел из чата", 0);
            }
        }

        public void SendMessage(string message, int id)
        {
            string answer = "";
            foreach(var item in users)
            {
                answer = DateTime.Now.ToShortTimeString();
                var user = users.FirstOrDefault(x => x.ID == id);
                if (user != null)
                {
                    answer += " " + user.Name + ": "; 
                }
                answer += message;

                item.OperationContext.GetCallbackChannel<IServerChatCallback>().MessageCallback(answer);         
            }
            WriteLog(answer); //Запись логов
        } 

        public void WriteLog(string message)
        {
            var location = Directory.GetCurrentDirectory() + "\\Log.txt";
            if (File.Exists(location))
            {
                string[] arStr = File.ReadAllLines(location); //Считываем все строки из файла
                List<string> ArrayRows = new List<string>();  //Лист для строк
                foreach (var item in arStr)
                {
                    ArrayRows.Add(item); //Заполняем лист
                }
                ArrayRows.Add(message);  //Добавляем в лист новое сообщение
                ArrayRows.Sort();    //Сортируем
                File.WriteAllLines(location, ArrayRows); //Перезаписываем файл
            }
            else
            {
                File.AppendAllText(location, message + Environment.NewLine);
            }
        }
    }
}
