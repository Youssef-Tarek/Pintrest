using MaterialDesignThemes.Wpf;
using Oracle.DataAccess.Client;
using Pinterest_V2;
using Pinterest_V2.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ListOf_Follow.xaml
    /// </summary>
    public partial class ListOf_Follow : Window
    {
        List<info_user> users = new List<info_user>();
        OracleConnection connection= new OracleConnection(FixedValues.ordb);
       OracleDataAdapter ProfileDataAdapter;
        DataSet ProfileDataSet;
        string state = "";
        public ListOf_Follow(string state)
        {
            InitializeComponent();
            connection.Open();
            this.state = state;
        }
        
        private void List_of_Follower_Selected(object sender, RoutedEventArgs e)
        {
        }

        private void Close_icon(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            /////////////////////////////// A.2. ///////////////////////////////////////
            this.DataContext = this;
            users.Clear();
            this.List_of_Follower.Items.Clear();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection;
            if(state.Equals("Follower"))
                cmd.CommandText = "select user_id from user_following where user_following=:id";

            else if(state.Equals("Following"))
                 cmd.CommandText = "select user_following from user_following where user_id=:id";

            cmd.Parameters.Add("id" , Account.UserID );
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                string cmdstr = "select user_name, profile_pic from PINTEREST_USER where user_id=:id";
                ProfileDataAdapter = new OracleDataAdapter(cmdstr, FixedValues.ordb);
                ProfileDataAdapter.SelectCommand.Parameters.Add("id", dr[0]);
                ProfileDataSet = new DataSet();
                ProfileDataAdapter.Fill(ProfileDataSet);
                byte[] image = (byte[])ProfileDataSet.Tables[0].Rows[0][1];
                BitmapImage ProfileImage = FixedValues.LoadImage(image);
                users.Add(new info_user { Name_user = ProfileDataSet.Tables[0].Rows[0][0].ToString(), Image_user = new ImageBrush(ProfileImage) ,Id_user= dr[0].ToString()});

            }
           this.List_of_Follower.ItemsSource = users;
        }

        private void List_of_Follower_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /////////////////////////////// A.2. ///////////////////////////////////////
            Close();
            Boolean follow = false;
            int friend = -1;
            int curItem = List_of_Follower.SelectedIndex;
            OracleCommand c = new OracleCommand();
            c.Connection = connection;
            c.CommandText = "select user_following from user_following  where user_id=:id";
            c.CommandType = CommandType.Text;
            c.Parameters.Add("id", Account.UserID);
            OracleDataReader dr = c.ExecuteReader();
            while(dr.Read())
            {
                if (dr[0].ToString() == users[curItem].Id_user)
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
            c.Parameters.Add("id_user", users[curItem].Id_user);
            dr = c.ExecuteReader();
            if (dr.Read())
            {
                if (dr["Friend_State"].ToString()=="0")
                   friend = 0;

                else
                   friend = 1;
                
            }
            dr.Close();

            Home.display_info.Children.Clear();
            UserControls.OtherProfile userx =new UserControls.OtherProfile(follow,friend);
            userx.OtherUserID = Convert.ToInt32(users[curItem].Id_user);
            Home.display_info.Children.Clear();
            Home.display_info.Children.Add(userx);
        }
    }
}
