﻿using DataBaseClasses;
using static DataBaseClasses.DBInteract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace DesktopApp
{
    /// <summary>
    /// Логика взаимодействия для EnterWindow.xaml
    /// </summary>
    public partial class EnterWindow : Window
    {
        public EnterWindow()
        {
            InitializeComponent();
            Window1 window4 = new Window1();
            window4.Show();
            //MessageBox.Show(Directory.GetFiles(Directory.GetCurrentDirectory())[2]);                      так можно брать фото для генерации клиентов
            //Close();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            LogIn();
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                LogIn();
        }

        void LogIn()
        {
            Account? account = db.accounts.Where(x => x.login == loginTextBox.Text).FirstOrDefault();
            if (account != null && account.password == passwordTextBox.Text)
            {
                PatientsWindow patientsWindow = new PatientsWindow();
                patientsWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неверные логин или пароль", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
