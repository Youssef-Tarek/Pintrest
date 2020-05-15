using Oracle.DataAccess.Client;
using Pinterest_V2;
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

namespace Pinterest
{
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class Message : Window
    {
        bool isfriend = false;
        string id_user;
        OracleConnection connection = new OracleConnection(FixedValues.ordb);
        public Message(bool isfriend, string id_user,string text_t)
        {
            InitializeComponent();
            this.isfriend = isfriend;
            this.id_user = id_user;
            text.Text = text_t;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (isfriend)
            {
                ////////////////////////////// A.3. Update //////////////////////////////////////////
                connection.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connection;
                cmd.CommandText = "update  user_friend set friend_state = :state where user_friend = :id_friend and user_id = :id";
                cmd.Parameters.Add("id", id_user);
                cmd.Parameters.Add("id_friend", Account.UserID);
                cmd.Parameters.Add("state", 1);
                cmd.ExecuteNonQuery();
                Close();
            }

        }
    }
}
