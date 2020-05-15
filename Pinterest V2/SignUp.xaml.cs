using Microsoft.Win32;
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

namespace Pinterest_V2
{
     /// <summary>
     /// Interaction logic for SignUp3.xaml
     /// </summary>
     public partial class SignUp : Window
     {
          public SignUp()
          {
               InitializeComponent();
          }

          private void Close_icon(object sender, RoutedEventArgs e)
          {
               Close();
          }

          private void SignUp_btn_Click(object sender, RoutedEventArgs e)
          {
               Account newAccount = new Account();
               int SignUpState = 0;

               if (FirstName_txt.Text == "" || LastName_txt.Text == "" || email_txt.Text == "" || password_txt.ToString() == "" || birthday_dataP.ToString() == "" || path_txt.Text == "")
                    MessageBox.Show("Please Fill All Fields");
               else
               {
                    string Name = FirstName_txt.Text + " " + LastName_txt.Text;
                    string Email = email_txt.Text;
                    string Password = password_txt.Text;
                    DateTime Birthday = Convert.ToDateTime(birthday_dataP.SelectedDate.Value.Date.ToString());
                    byte[] imageData = newAccount.Getimage(path_txt.Text);

                    if (newAccount.IsEmailValid(Email))
                    {
                         SignUpState = newAccount.SignUp(Name, Email, Password, Birthday, imageData);
                    }

                    else
                         MessageBox.Show("Enter VALID Email Please");

                    if (SignUpState == 1)
                    {
                         Login login1 = new Login();
                         Close();
                         login1.Show();
                    }
                    else if (SignUpState == -1)
                         MessageBox.Show("Email ALREADY EXIST !");
                    else if (SignUpState == -2)
                         MessageBox.Show("EXCEPTION !");
               }
          }

          private void browse_btn_Click(object sender, RoutedEventArgs e)
          {
               OpenFileDialog ofd = new OpenFileDialog();
               ofd.ShowDialog();
               path_txt.Text = ofd.FileName;
               profile_pic.Fill = new ImageBrush( new BitmapImage(new Uri(path_txt.Text)));
          }

          private void LoginButton_Click(object sender, RoutedEventArgs e)
          {
               Login login1 = new Login();
               Close();
               login1.Show();
          }
     }
}

