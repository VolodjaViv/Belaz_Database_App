using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatabaseApp
{
    public partial class MainWindow : Window
    {
        private ToolTip loginToolTip;
        private ToolTip password1ToolTip;
        private ToolTip password2ToolTip;
        private ToolTip passwordComparisonToolTip;
        private ToolTip emailToolTip;
        private bool loginToolTipChecker;
        private bool password1ToolTipChecker;
        private bool password2ToolTipChecker;
        private bool passwordComparisonToolTipChecker;
        private bool emailToolTipChecker;

        private readonly string accounts_file_path = @"C://Users//Vahmurka//Desktop//Практика//DatabaseApp//DatabaseApp//accounts_File.txt";

        //private string GlobalLogin;
        //private string GlobalPassword;


        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Set_TextBox_Unvisible(textBoxLogin);
            Set_PasswordBox_Unvisible(textBoxPassword1);
            Set_PasswordBox_Unvisible(textBoxPassword2);
            Set_TextBox_Unvisible(textBoxEmail);
            Set_Button_Unvisible(actionButton);
        }

        public void Set_ToolTip_Visible(ToolTip tp) { tp.Visibility = Visibility.Visible; }
        public void Set_ToolTip_Unvisible(ToolTip tp) { tp.Visibility = Visibility.Hidden; }
        public void Set_TextBox_Visible(TextBox tb) { tb.Visibility = Visibility.Visible; }
        public void Set_TextBox_Unvisible(TextBox tb) { tb.Visibility = Visibility.Hidden; }
        public void Set_PasswordBox_Visible(PasswordBox pb) { pb.Visibility = Visibility.Visible; }
        public void Set_PasswordBox_Unvisible(PasswordBox pb) { pb.Visibility = Visibility.Hidden; }
        public void Set_Button_Visible(Button b) { b.Visibility = Visibility.Visible; }
        public void Set_Button_Unvisible(Button b) { b.Visibility = Visibility.Hidden; }
        public void Reset_TextBoxt_Content(TextBox tb) { tb.Text = ""; }
        public void Reset_PasswordBox_Content(PasswordBox pb) { pb.Password = ""; }
        public void Set_Button_Content(Button b, string s) { b.Content = s; }
        public void Set_TextBox_Default_Color(TextBox tb) { tb.Background = Brushes.Lavender; }
        public void Set_TextBox_Alert_Color(TextBox tb) { tb.Background = Brushes.Pink; }
        public void Set_PasswordBox_Default_Color(PasswordBox pb) { pb.Background = Brushes.Lavender; }
        public void Set_PasswordBox_Alert_Color(PasswordBox pb) { pb.Background = Brushes.Pink; }
        public void Activate_Checker(bool b) { b = true; }
        public void Deactivate_Checker(bool b) { b = false; }
        public string Get_Button_Content(Button b) { return (string)b.Content; }
        public string Get_TextBox_Content(TextBox tb) { return (string)tb.Text.Trim().ToLower(); }
        public string Get_PasswordBox_Content(PasswordBox pb) { return (string)pb.Password.Trim(); }
        public bool Check_If_File_Exists(string path) { return File.Exists(path); }
        public FileStream Open_File(string path) { FileStream file = new FileStream(path, FileMode.Append); return file; }
        public void Close_File(FileStream file) { file.Close(); }
        public void Save_New_Account(string login, string password, string email)
        {
            using (StreamWriter streamWriter = new StreamWriter(Open_File(accounts_file_path))) {
                streamWriter.WriteLine(Encrypt_Text(login));
                streamWriter.WriteLine(Encrypt_Text(password));
                streamWriter.WriteLine(Encrypt_Text(email));
                streamWriter.WriteLine("-------------------------------");
            }
        }
        public string[] Read_Account(string login, string password)
        {
            string[] ac_array = new string[4];
            if (Check_If_File_Exists(accounts_file_path)) {
                using (StreamReader streamReader = new StreamReader(accounts_file_path)) {
                    while (true) {
                        ac_array[0] = Decrypt_Text(streamReader.ReadLine());
                        ac_array[1] = Decrypt_Text(streamReader.ReadLine());
                        ac_array[2] = Decrypt_Text(streamReader.ReadLine());
                        ac_array[3] = streamReader.ReadLine();
                        if (ac_array[0] == login && ac_array[1] == password) return ac_array;
                        if (streamReader.Peek() == -1) {
                            for (int i = 0; i < 4; i++) ac_array[i] = "account_not_found";
                            return ac_array;
                        }
                    }
                }
            }
            else {
                Close_File(Open_File(accounts_file_path));
                using (StreamReader streamReader = new StreamReader(accounts_file_path)) {
                    while (true) {
                        ac_array[0] = streamReader.ReadLine();
                        ac_array[1] = streamReader.ReadLine();
                        ac_array[2] = streamReader.ReadLine();
                        ac_array[3] = streamReader.ReadLine();
                        if (ac_array[0] == login && ac_array[1] == password) return ac_array;
                        if (streamReader.Peek() == -1) {
                            for (int i = 0; i < 4; i++) ac_array[i] = "account_not_found";
                            return ac_array;
                        }
                    }
                }
            }
        }
        public string Encrypt_Text(string s)
        {
            var simpleTextBytes = Encoding.UTF8.GetBytes(s);
            string enText = Convert.ToBase64String(simpleTextBytes);
            return enText;
        }
        public string Decrypt_Text(string s)
        {
            var enTextBytes = Convert.FromBase64String(s);
            string deText = Encoding.UTF8.GetString(enTextBytes);
            return deText;
        }


        private void registrationButton_Click(object sender, RoutedEventArgs e)
        {
            Set_Button_Content(actionButton, "Зарегестрироваться");
            Set_Button_Visible(actionButton);

            Reset_TextBoxt_Content(textBoxLogin);
            Set_TextBox_Visible(textBoxLogin);

            Reset_PasswordBox_Content(textBoxPassword1);
            Set_PasswordBox_Visible(textBoxPassword1);

            Reset_PasswordBox_Content(textBoxPassword2);
            Set_PasswordBox_Visible(textBoxPassword2);

            Reset_TextBoxt_Content(textBoxEmail);
            Set_TextBox_Visible(textBoxEmail);
            
            if (loginToolTipChecker) {
                Deactivate_Checker(loginToolTipChecker);
                Set_ToolTip_Unvisible(loginToolTip); 
            }
            Set_TextBox_Default_Color(textBoxLogin);

            if (password1ToolTipChecker) {
                Deactivate_Checker(password1ToolTipChecker);
                Set_ToolTip_Unvisible(password1ToolTip); 
            }
            Set_PasswordBox_Default_Color(textBoxPassword1);

            if (password2ToolTipChecker) {
                Deactivate_Checker(password2ToolTipChecker);
                Set_ToolTip_Unvisible(password2ToolTip); 
            }
            Set_PasswordBox_Default_Color(textBoxPassword2);

            if (passwordComparisonToolTipChecker) {
                Deactivate_Checker(passwordComparisonToolTipChecker);
                Set_ToolTip_Unvisible(passwordComparisonToolTip); 
            }
            Set_PasswordBox_Default_Color(textBoxPassword2);

            if (emailToolTipChecker) {
                Deactivate_Checker(emailToolTipChecker);
                Set_ToolTip_Unvisible(emailToolTip); 
            }
            Set_TextBox_Default_Color(textBoxEmail);
        }

        private void authorizationButton_Click(object sender, RoutedEventArgs e)
        {
            Set_Button_Content(actionButton, "Авторизоваться");
            Set_Button_Visible(actionButton);

            Reset_TextBoxt_Content(textBoxLogin);
            Set_TextBox_Visible(textBoxLogin);

            Reset_PasswordBox_Content(textBoxPassword1);
            Set_PasswordBox_Visible(textBoxPassword1);

            Reset_PasswordBox_Content(textBoxPassword2);
            Set_PasswordBox_Unvisible(textBoxPassword2);

            Reset_TextBoxt_Content(textBoxEmail);
            Set_TextBox_Unvisible(textBoxEmail);

            if (loginToolTipChecker) {
                Deactivate_Checker(loginToolTipChecker);
                Set_ToolTip_Unvisible(loginToolTip); 
            }
            Set_TextBox_Default_Color(textBoxLogin);

            if (password1ToolTipChecker) {
                Deactivate_Checker(password1ToolTipChecker);
                Set_ToolTip_Unvisible(password1ToolTip); 
            }
            Set_PasswordBox_Default_Color(textBoxPassword1);
        }

        private void Action_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Get_Button_Content(actionButton) == "Авторизоваться") {  Authorization_Action_Button_Click(sender, e); }
            if (Get_Button_Content(actionButton) == "Зарегестрироваться") { Registration_Action_Button_Click(sender, e); }
        }

        private void Authorization_Action_Button_Click(object sender, RoutedEventArgs e)
        {
            string[] acc = Read_Account(Get_TextBox_Content(textBoxLogin), Get_PasswordBox_Content(textBoxPassword1));
            if(acc[0] == "account_not_found" && acc[1] == "account_not_found") { MessageBox.Show("Такого пользователя не существует!"); }
            if (Get_TextBox_Content(textBoxLogin) == acc[0] && Get_PasswordBox_Content(textBoxPassword1) == acc[1]) {
                MessageBox.Show("Добро пожаловать, " + Get_TextBox_Content(textBoxLogin) + "!", "Авторизация прошла успешно");

                this.Hide();
                Window1 window1 = new Window1();
                window1.Show();
            }
        }

        private void Registration_Action_Button_Click(object sender, RoutedEventArgs e)
        {
            Set_TextBox_Default_Color(textBoxLogin);
            Set_PasswordBox_Default_Color(textBoxPassword1);
            Set_PasswordBox_Default_Color(textBoxPassword2);
            Set_TextBox_Default_Color(textBoxEmail);

            string login = Get_TextBox_Content(textBoxLogin);
            string password_1 = Get_PasswordBox_Content(textBoxPassword1);
            string password_2 = Get_PasswordBox_Content(textBoxPassword2);
            string email = Get_TextBox_Content(textBoxEmail);

            if (login.Length < 2) {
                Activate_Checker(loginToolTipChecker);
                loginToolTip = new ToolTip();
                loginToolTip.Content = "Это поле введено некорректно!";
                Set_ToolTip_Visible(loginToolTip);
                textBoxLogin.ToolTip = loginToolTip;
                Set_TextBox_Alert_Color(textBoxLogin); 
            }
            else if (login.Length >= 2 && loginToolTipChecker) { 
                Set_ToolTip_Unvisible(loginToolTip);
                Deactivate_Checker(loginToolTipChecker);
            }

            if (password_2.Length < 8) {
                Activate_Checker(password1ToolTipChecker);
                password2ToolTip = new ToolTip();
                password2ToolTip.Content = "Пароль слишком короткий!";
                Set_ToolTip_Visible(password2ToolTip);
                textBoxPassword2.ToolTip = password1ToolTip;
                Set_PasswordBox_Alert_Color(textBoxPassword2);
            }
            else if (password_2.Length >= 8 && password2ToolTipChecker) { 
                Set_ToolTip_Unvisible(password2ToolTip);
                Deactivate_Checker(password2ToolTipChecker);
            }

            if (password_1 != password_2) {
                Activate_Checker(passwordComparisonToolTipChecker);
                passwordComparisonToolTip = new ToolTip();
                passwordComparisonToolTip.Content = "Пароли не совпадают!";
                Set_ToolTip_Visible(passwordComparisonToolTip);
                textBoxPassword2.ToolTip = passwordComparisonToolTip;
                Set_PasswordBox_Alert_Color(textBoxPassword2);
            }
            else if (password_1 == password_2 && passwordComparisonToolTipChecker) {
                Set_ToolTip_Unvisible(passwordComparisonToolTip);
                Deactivate_Checker(passwordComparisonToolTipChecker);
            }

            if (password_1.Length < 8) {
                Activate_Checker(password1ToolTipChecker);
                password1ToolTip = new ToolTip();
                password1ToolTip.Content = "Пароль слишком короткий!";
                Set_ToolTip_Visible(password1ToolTip);
                textBoxPassword1.ToolTip = password1ToolTip;
                Set_PasswordBox_Alert_Color(textBoxPassword1);
            }
            else if (password_1.Length >= 8 && password1ToolTipChecker) { 
                Set_ToolTip_Unvisible(password1ToolTip);
                Deactivate_Checker(password1ToolTipChecker);
            }

            if (!email.Contains("@") || !email.Contains(".")) {
                Activate_Checker(emailToolTipChecker);
                emailToolTip = new ToolTip();
                emailToolTip.Content = "Почта введена некорректно!";
                Set_ToolTip_Visible(emailToolTip);
                textBoxEmail.ToolTip = emailToolTip;
                Set_TextBox_Alert_Color(textBoxEmail);
            }
            else if (email.Contains("@") && email.Contains(".") && emailToolTipChecker) { 
                Set_ToolTip_Unvisible(emailToolTip);
                Deactivate_Checker(emailToolTipChecker);
            }

            if (password_1 == password_2 && password_1.Length >= 8 && login.Length >= 2 && email.Contains("@") && email.Contains(".")) {
                //GlobalLogin = login;
                //GlobalPassword = password_1;
                Save_New_Account(login, password_1, email);
                MessageBox.Show(login, "Регистрация прошла успешно!");
            }
        }
    }
}
