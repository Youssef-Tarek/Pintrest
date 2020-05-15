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
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Data;


namespace Pinterest_V2
{
     /// <summary>
     /// Interaction logic for Login.xaml
     /// </summary>
     public partial class Login : Window
     {


          public Login()
          {
               InitializeComponent();
          }

          private void Button_Click(object sender, RoutedEventArgs e)
          {

               Account newAccount = new Account();
               int LoginState = 0;

               if (email_txt.Text == "" || password_txt.ToString() == "")
                    MessageBox.Show("Please Fill All Fields");
               else
               {
                    string Email = email_txt.Text;
                    string Password = password_txt.Password.ToString();
                    if (newAccount.IsEmailValid(Email))
                    {
                         LoginState = newAccount.Login(Email, Password);
                    }

                    else
                         MessageBox.Show("Enter VALID Email Please");

                    if (LoginState == 1)
                    {
                         if (Account.FirstLogin == 0)
                         {
                              Categories_user_control_ recommend = new Categories_user_control_();
                              Close();
                              recommend.ShowDialog();
                         }
                         else
                         {
                              Home home1 = new Home();
                              Close();
                              home1.Show();
                         }
                    }

                    else if (LoginState == -1)
                         MessageBox.Show("Email OR Password NOT CORRECT !");
                    else if (LoginState == -2)
                         MessageBox.Show("EXCEPTION !");
               }

          }

          private void Close_icon(object sender, RoutedEventArgs e)
          {
               Close();
          }

          private void SignUpButton_Click(object sender, RoutedEventArgs e)
          {
               SignUp sign_up = new SignUp();
               this.Close();
               sign_up.Show();
          }

          private void ShowPassCheckBox_Checked(object sender, RoutedEventArgs e)
          {
               ShowPasswordFunction();
          }

          private void ShowPassCheckBox_UnChecked(object sender, RoutedEventArgs e)
          {
               HidePasswordFunction();
          }

          private void ShowPasswordFunction()
          {
               PasswordUnmask.Text = password_txt.Password;
               PasswordUnmask.Visibility = Visibility.Visible;
               password_txt.Visibility = Visibility.Hidden;
          }

          private void HidePasswordFunction()
          {
               PasswordUnmask.Visibility = Visibility.Hidden;
               password_txt.Visibility = Visibility.Visible;
          }
     }
}


