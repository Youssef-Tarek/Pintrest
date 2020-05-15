using MaterialDesignThemes.Wpf;
using Oracle.DataAccess.Client;
using Pinterest;
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
    /// Interaction logic for NotificationsUserControl.xaml
    /// </summary>
    public partial class NotificationsUserControl : UserControl
    {
        public string id_user;
        public string name;
        OracleConnection connection = new OracleConnection(FixedValues.ordb);
        public NotificationsUserControl()
        {
            InitializeComponent();
        }

        private void Notification_Click(object sender, RoutedEventArgs e)
        {
            if (icon.Kind == PackIconKind.PersonPlus) // if person make request freind
            {
                Message message = new Message(true, id_user,"Request Of Friend "+name);
                message.ShowDialog();
            }
            else if (icon.Kind == PackIconKind.PersonStar) // if person make follow
            {
                Boolean follow = false;
                int friend = -1;
                connection.Open();
                OracleCommand c = new OracleCommand();
                c.Connection = connection;
                c.CommandText = "select user_following from user_following  where user_id=:id";
                c.CommandType = CommandType.Text;
                c.Parameters.Add("id", Account.UserID);
                OracleDataReader dr = c.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[0].ToString() == id_user)
                    {
                        follow = true; break;
                    }
                }
                dr.Close();
                c = new OracleCommand();
                c.Connection = connection;
                c.CommandText = "select Friend_State from user_friend where user_id=:id and user_friend=:id_user";
                c.CommandType = CommandType.Text;
                c.Parameters.Add("id", Account.UserID);
                c.Parameters.Add("id_user",id_user);
                dr = c.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["Friend_State"].ToString() == "0")
                    {
                        friend = 0;
                    }
                    else
                    {
                        friend = 1;
                    }
                }
                dr.Close();
                Home.display_info.Children.Clear();
                OtherProfile userx = new OtherProfile(follow, friend);
                userx.OtherUserID =Convert.ToInt32(id_user);
                Home.display_info.Children.Clear();
                Home.display_info.Children.Add(userx);
            }
            else if (icon.Kind == PackIconKind.Chat) //if person chat with you
            {
                    Chat ch = new Chat();
                    ch.Show();
            }
        }
    }
}
