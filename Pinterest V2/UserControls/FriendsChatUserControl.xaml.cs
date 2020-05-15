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
    /// Interaction logic for FriendsChatUserControl.xaml
    /// </summary>
    public partial class FriendsChatUserControl : UserControl
    {
          public int FriendID;
        public static int friend; 
          OracleConnection connection = new OracleConnection(FixedValues.ordb);
          StackPanel chatPanel;
          public FriendsChatUserControl(StackPanel chatPanel)
          {
               InitializeComponent();
               this.chatPanel = chatPanel;
               this.chatPanel.Children.Clear();
          }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
               this.chatPanel.Children.Clear();
               connection.Open();
            friend = FriendID;
               OracleCommand totalMessages = new OracleCommand();
               totalMessages.Connection = connection;
               totalMessages.CommandText = "select messege , user_id from CHAT where(USER_ID = :userID and USER_FRIEND_RECIEVER = :recieverId)OR(USER_ID = :recieverId and USER_FRIEND_RECIEVER = :userID) order by time";
               totalMessages.CommandType = CommandType.Text;
               totalMessages.Parameters.Add("userID", Account.UserID);
               totalMessages.Parameters.Add("recieverId", FriendID);

               OracleDataReader MessagesReader = totalMessages.ExecuteReader();
               while (MessagesReader.Read())
               {
                    if(MessagesReader[1].ToString() == Account.UserID.ToString())
                    {
                         Pinterest_V2.UserControls.MessageUserControl msgControl = new UserControls.MessageUserControl();
                         msgControl.Margin = new Thickness(50, 10, 0, 0);
                         msgControl.Message.Text = MessagesReader[0].ToString();
                         msgControl.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFBA4848"));
                         chatPanel.Children.Add(msgControl);
                    }
                    else
                    {
                         Pinterest_V2.UserControls.MessageUserControl msgControl2 = new UserControls.MessageUserControl();
                         msgControl2.Margin = new Thickness(0, 10, 50, 0);
                         msgControl2.Message.Text = MessagesReader[0].ToString();
                         msgControl2.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DD275FF5"));
                         chatPanel.Children.Add(msgControl2);
                    }

               }
               MessagesReader.Close();
            connection.Close();
        }

     }
}
