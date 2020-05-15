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
    /// Interaction logic for helpUserControl.xaml
    /// </summary>
    public partial class helpUserControl : UserControl
    {
        OracleConnection connection = new OracleConnection(FixedValues.ordb);
        public helpUserControl()
        {
            InitializeComponent();
        }

        private void like_btn_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection;
            cmd.CommandText = "help_rate";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("id_user", Account.UserID);
            cmd.Parameters.Add("question", Question_txt.Text);
            cmd.ExecuteNonQuery();
            connection.Close();

            if (likeIcon.Foreground == Brushes.Gray)
            {
                likeIcon.Foreground = Brushes.Blue;
                count_text.Content = (Convert.ToInt32(count_text.Content) + 1).ToString();
            }
            else
            {
                likeIcon.Foreground = Brushes.Gray;
                count_text.Content = (Convert.ToInt32(count_text.Content) - 1).ToString();
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            connection.Open();
            OracleCommand command = new OracleCommand();
            command.Connection = connection;
            command.CommandText = "select count(*) from user_help  where  question= :question_text";
            command.CommandType = CommandType.Text;
            command.Parameters.Add("question_text", Question_txt.Text);
            OracleDataReader reader = command.ExecuteReader();
            if (reader.Read())
                count_text.Content = reader[0];


            connection.Close();


            connection.Open();
            OracleCommand c = new OracleCommand();
            c.Connection = connection;
            c.CommandText = "select count(*) from user_help where user_id=:user_id and question=:question_text";
            c.CommandType = CommandType.Text;
            c.Parameters.Add("user_id", Account.UserID);
            c.Parameters.Add("question_text", Question_txt.Text);
           
            OracleDataReader dr = c.ExecuteReader();
            if (dr.Read())
            {
                if(dr[0].ToString()!="0")
                    likeIcon.Foreground = Brushes.Blue;
            }
            dr.Close();
            connection.Close();
        }

    }
}
