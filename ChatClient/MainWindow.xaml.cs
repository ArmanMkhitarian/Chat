using System;
using System.Windows;
using ChatClient.ChatService;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IChatServiceCallback
    {
        ChatServiceClient ChatClient;
        bool isConnected = false;
        int ID;

        public MainWindow()
        {
            InitializeComponent();
        }

        void Connect()
        {
            if(!isConnected)
            {
                try
                {
                    ChatClient = new ChatServiceClient(new System.ServiceModel.InstanceContext(this));
                    ID = ChatClient.Connect(NameText.Text);
                    NameText.IsEnabled = false;
                    ConDisconButton.Content = "Disconnect";
                    isConnected = true;
                }
                catch
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
            }
        }

        void Disconnect()
        {
            if (isConnected)
            {
                ChatClient.Disonnect(ID);
                ChatClient = null;
                NameText.IsEnabled = true;
                ConDisconButton.Content = "Connect";
                isConnected = false;
            }
        }
           

        private void ConDisconButton_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                Disconnect();
            }
            else
            {
                Connect();
            }
        }

        void IChatServiceCallback.MessageCallback(string message)
        {
            ChatList.Items.Add(message);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Disconnect();
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageText.Text != "")
            {
                if (ChatClient != null)
                {
                    ChatClient.SendMessage(MessageText.Text, ID);
                    MessageText.Text = string.Empty; //Очищаем строку ввода сообщения после отправки
                }
            }         
        }

        private void ShowLogButton_Click(object sender, RoutedEventArgs e)
        {
            var location = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\ChatServer\\bin\\Debug\\Log.txt";
            if(File.Exists(location))
            {
                Process.Start(location);
            }
            else
            {
                MessageBox.Show("Файл не найден");
            }      
        }
    }
}
