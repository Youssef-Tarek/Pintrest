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
     /// Interaction logic for UserSearchUserControl.xaml
     /// </summary>
     public partial class UserSearchUserControl : UserControl
     {
          public int OtherUserID;
          Boolean follow = false;
          int friend = -1;
          OracleConnection connection = new OracleConnection(FixedValues.ordb);

          public UserSearchUserControl()
          {
               InitializeComponent();
          }

          private void Profilbtn_Click(object sender, RoutedEventArgs e)
          {
               connection.Open();
               OracleCommand SearchCommand = new OracleCommand();
               SearchCommand.Connection = connection;
               SearchCommand.CommandText = "select user_following from user_following  where user_id=:id";
               SearchCommand.CommandType = CommandType.Text;
               SearchCommand.Parameters.Add("id", Account.UserID);
               OracleDataReader StateReader = SearchCommand.ExecuteReader();
               while (StateReader.Read())
               {
                    if (Convert.ToInt32(StateReader[0]) == OtherUserID)
                    {
                         follow = true; break;
                    }
               }
               StateReader.Close();

               SearchCommand = new OracleCommand();
               SearchCommand.Connection = connection;
               SearchCommand.CommandText = "select Friend_State from user_friend where user_id=:id and user_friend=:id_user";
               SearchCommand.CommandType = CommandType.Text;
               SearchCommand.Parameters.Add("id", Account.UserID);
               SearchCommand.Parameters.Add("id_user", OtherUserID);
               StateReader = SearchCommand.ExecuteReader();
               if (StateReader.Read())
               {
                    if (StateReader["Friend_State"].ToString() == "0")
                         friend = 0;

                    else
                         friend = 1;
               }
               StateReader.Close();

               Home.display_info.Children.Clear();
               UserControls.OtherProfile OtherUser = new UserControls.OtherProfile(follow, friend);
               OtherUser.OtherUserID = Convert.ToInt32(OtherUserID);
               Home.display_info.Children.Add(OtherUser);
               //CurrentPanel.Children.Add(userx);
          }
     }
}