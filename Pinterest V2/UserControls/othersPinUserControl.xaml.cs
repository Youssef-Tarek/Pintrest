using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pinterest_V2.UserControls
{
     /// <summary>
     /// Interaction logic for othersPinUserControl.xaml
     /// </summary>
     public partial class othersPinUserControl : UserControl
     {
          public int pin_id = -1;
          OracleConnection connection;

          public othersPinUserControl()
          {
               InitializeComponent();
               connection = new OracleConnection(FixedValues.ordb);
          }

          private void UserControl_MouseEnter(object sender, MouseEventArgs e)
          {
               addPinToBoard_btn.Visibility = System.Windows.Visibility.Visible;
               like_btn.Visibility = System.Windows.Visibility.Visible;
               dislike_btn.Visibility = System.Windows.Visibility.Visible;
          }

          private void UserControl_MouseLeave(object sender, MouseEventArgs e)
          {
               addPinToBoard_btn.Visibility = System.Windows.Visibility.Hidden;
               like_btn.Visibility = System.Windows.Visibility.Hidden;
               dislike_btn.Visibility = System.Windows.Visibility.Hidden;
          }

          private void addPinToBoard_btn_Click(object sender, RoutedEventArgs e)
          {
               connection.Open();
               OracleCommand command = new OracleCommand();
               command.Connection = connection;
               command.CommandText = "select count(*)  from board where user_id= :userid";
               command.CommandType = CommandType.Text;
               command.Parameters.Add("userid", Account.UserID);
               OracleDataReader reader = command.ExecuteReader();
               int i = -1;
               if (reader.Read())
                    i = Convert.ToInt32(reader[0]);
               reader.Close();
               connection.Close();

               if (i == 0)
               {
                    MessageBox.Show("Please, create board First");
               }
               else
               {
                    choose_board choose_form = new choose_board();
                    choose_form.add_to_board = true;
                    choose_form.pin_id = pin_id;
                    choose_form.ShowDialog();
               }
          }

          private void like_btn_Click(object sender, RoutedEventArgs e)
          {
               connection.Open();
               OracleCommand cmd = new OracleCommand();
               cmd.Connection = connection;
               cmd.CommandText = "react_pin";
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Add("id_user", Account.UserID);
               cmd.Parameters.Add("id_pin", pin_id);
               cmd.Parameters.Add("react", "likes");
               cmd.ExecuteNonQuery();
               connection.Close();

               if (dislikeIcon.Foreground == Brushes.Gray)
               {
                    dislikeIcon.Foreground = Brushes.White;
               }
               likeIcon.Foreground = Brushes.Gray;

          }

          private void dislike_btn_Click(object sender, RoutedEventArgs e)
          {
               connection.Open();
               OracleCommand cmd = new OracleCommand();
               cmd.Connection = connection;
               cmd.CommandText = "react_pin";
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Add("id_user", Account.UserID);
               cmd.Parameters.Add("id_pin", pin_id);
               cmd.Parameters.Add("react", "dislikes");
               cmd.ExecuteNonQuery();
               connection.Close();
               if (likeIcon.Foreground == Brushes.Gray)
               {
                    likeIcon.Foreground = Brushes.White;
               }
               dislikeIcon.Foreground = Brushes.Gray;

          }
     }
}
