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
using System.Windows.Shapes;

namespace Pinterest_V2
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {

          OracleConnection connection;
          public Chat()
          {
               InitializeComponent();
               connection = new OracleConnection(FixedValues.ordb);
          }

          private void Grid_Loaded(object sender, RoutedEventArgs e)
          {
               MessagesPanel.Children.Clear();
               FriendsPanel.Children.Clear();
               connection.Open();
               ///////////////////////////  /////////////////////////////////////
               OracleCommand selectFriends = new OracleCommand();
               selectFriends.Connection = connection;
               selectFriends.CommandText = "Select_Friend";
               selectFriends.CommandType = CommandType.StoredProcedure;
               selectFriends.Parameters.Add("userID", Account.UserID);
               selectFriends.Parameters.Add("friendID", OracleDbType.RefCursor, ParameterDirection.Output);
               OracleDataReader FriendsIDData = selectFriends.ExecuteReader();

               while (FriendsIDData.Read())
               {
                    int FriendIndex = 0;
                    if (FriendsIDData[0].ToString().Equals(Account.UserID.ToString()))
                         FriendIndex = 1;
                    /////////////////////// A.2. one ///////////////////////
                    OracleCommand FriendCommand = new OracleCommand();
                    FriendCommand.Connection = connection;
                    FriendCommand.CommandText = "select user_name, profile_pic, login from PINTEREST_USER where user_id=:id";
                    FriendCommand.CommandType = CommandType.Text;

                    FriendCommand.Parameters.Add("id", Convert.ToInt32(FriendsIDData[FriendIndex].ToString()));
                    OracleDataReader UserReader = FriendCommand.ExecuteReader();
                    
                    if (UserReader.Read())
                    {
                         Pinterest_V2.UserControls.FriendsChatUserControl fcControl = new UserControls.FriendsChatUserControl(this.MessagesPanel);
                         fcControl.Friend_Name.Text = UserReader[0].ToString();
                         byte[] image = (byte[])UserReader[1];
                         fcControl.ProfilePic.Fill = new ImageBrush(FixedValues.LoadImage(image));

                         if (UserReader[2].ToString().Equals("1"))
                              fcControl.OnlineCircle.Fill = Brushes.Green;
                         fcControl.FriendID = Convert.ToInt32(FriendsIDData[FriendIndex].ToString());
                         fcControl.Visibility = System.Windows.Visibility.Visible;
                         FriendsPanel.Children.Add(fcControl);
                    }
                    UserReader.Close();
               }
               FriendsIDData.Close();
               connection.Close();
          }

        private void Close_icon(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Send_btn_Click(object sender, RoutedEventArgs e)
        {
               connection.Open();
               if (Message.Text != "")
               {
                //////////////////////// A.3. insert ////////////////////////
                    OracleCommand insertNewMessage = new OracleCommand();
                    insertNewMessage.Connection = connection;
                    insertNewMessage.CommandText = "insert into CHAT (USER_ID,USER_FRIEND_RECIEVER,MESSEGE,TIME) " +
                        "values (:senderid ,:reciverId,:message,:time)";
                    insertNewMessage.Parameters.Add("senderid", Account.UserID);
                    insertNewMessage.Parameters.Add("reciverId", UserControls.FriendsChatUserControl.friend);
                    insertNewMessage.Parameters.Add("message", Message.Text);
                    insertNewMessage.Parameters.Add("time", DateTime.Now);
                    int r = insertNewMessage.ExecuteNonQuery();
                    if (r != -1)
                    {
                         Pinterest_V2.UserControls.MessageUserControl msgControl = new UserControls.MessageUserControl();
                         msgControl.Margin = new Thickness(50, 10, 0, 0);
                         msgControl.Message.Text = Message.Text;
                         msgControl.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFBA4848"));
                         MessagesPanel.Children.Add(msgControl);
                    }
                    Message.Clear();
               }
               connection.Close();


               
        }
    }
}
