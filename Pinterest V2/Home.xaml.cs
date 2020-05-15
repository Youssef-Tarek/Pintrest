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
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using MaterialDesignThemes.Wpf;

namespace Pinterest_V2
{
     /// <summary>
     /// Interaction logic for Home.xaml
     /// </summary>
     public partial class Home : Window
     {
          OracleDataAdapter SearchAdapter;
          public static StackPanel display_info;
          OracleConnection connection = new OracleConnection(FixedValues.ordb);

          public Home()
          {
               InitializeComponent();
               Load_Pins();
               display_info = HomePanel;
          }

          private void Profile_btn_Click(object sender, RoutedEventArgs e)
          {
               HomePanel.Children.Clear();
               UserControls.ProfileUC PUC = new UserControls.ProfileUC();
               HomePanel.Children.Clear();
               HomePanel.Children.Add(PUC);
          }

          private void Exit_btn_Click(object sender, RoutedEventArgs e)
          {
            connection.Open();
            OracleCommand LogoutCommand = new OracleCommand();
            LogoutCommand.Connection = connection;
            LogoutCommand.CommandText = "update PINTEREST_USER set Login= :login where user_id = :userID";
            LogoutCommand.Parameters.Add("login", '0');
            LogoutCommand.Parameters.Add("userID", Account.UserID);
            LogoutCommand.CommandType = CommandType.Text;
            LogoutCommand.ExecuteNonQuery();
            connection.Close();

            Close();
          }

          private void Home_btn_Click(object sender, RoutedEventArgs e)
          {
               HomePanel.Children.Clear();
               Load_Pins();
          }

          private void Chat_btn_Click(object sender, RoutedEventArgs e)
          {
               Chat ch = new Chat();
               ch.Show();
          }

          private void Load_Pins()
          {
               int i = 0;
               string command1 = "select c.category_id  from category c, user_category uc, pinterest_user pu where  c.category_id = uc.category_id and uc.user_id = pu.user_id and pu.user_id =: userId ";
               OracleDataAdapter adapter1 = new OracleDataAdapter(command1, FixedValues.ordb);
               adapter1.SelectCommand.Parameters.Add("userid", Account.UserID);
               DataSet set1 = new DataSet();
               adapter1.Fill(set1);

               int[] categories_user = new int[set1.Tables[0].Rows.Count];
               for (int k = 0; k < set1.Tables[0].Rows.Count; k++)
               {
                    categories_user[k] = Convert.ToInt32(set1.Tables[0].Rows[k]["category_id"].ToString());
                    
                    string command = "select pin_name, pin_image, pin_id from pin , board where board.board_name=pin.board_name and board.user_Id=pin.user_id and  pin.user_id !=:userid and board.privacy='0'  and category_id=:categoryID ";
                    OracleDataAdapter adapter = new OracleDataAdapter(command, FixedValues.ordb);
                    adapter.SelectCommand.Parameters.Add("userid", Account.UserID);
                    adapter.SelectCommand.Parameters.Add("categoryID", categories_user[k]);
                    DataSet set = new DataSet();
                    adapter.Fill(set);

                    //initial coordinates... don't change
                    double initLeft = 50;
                    double initTop = 20;
                    double initRight = 510;
                    double initBottom = 10;

                    //for first login only.. don't change
                    double d;
                
                    for (int j = 0; j < set.Tables[0].Rows.Count; j++)
                    {
                         if ((i + 1) % 5 == 0)
                         {
                              initLeft = 20;
                              initRight = 510;
                              i = 0;
                         }

                         if (i >= 1)
                         {
                              d = 0.5;
                         }

                         else
                              d = i;

                         connection.Open();
                         OracleCommand c = new OracleCommand();
                         c.Connection = connection;
                         c.CommandText = "select react_type from user_react where pin_id=:pin_id and user_id=:user_id";
                         c.CommandType = CommandType.Text;

                         c.Parameters.Add("pin_id", set.Tables[0].Rows[i].ItemArray[2]);
                         c.Parameters.Add("user_id", Account.UserID);
                         OracleDataReader dr = c.ExecuteReader();
                         string react = "";
                         if (dr.Read())
                         {
                              react = dr[0].ToString();
                         }
                         dr.Close();
                         connection.Close();

                         Pinterest_V2.UserControls.othersPinUserControl othersPinsControl = new UserControls.othersPinUserControl();

                         byte[] image = (byte[])set.Tables[0].Rows[j].ItemArray[1];
                         othersPinsControl.otherPinPhoto.Source = FixedValues.LoadImage(image);
                         othersPinsControl.pin_id = Convert.ToInt32(set.Tables[0].Rows[j].ItemArray[2]);

                         othersPinsControl.addIcon.Foreground = Brushes.White;
                        
                         if (react.Equals("likes"))
                              othersPinsControl.likeIcon.Foreground = Brushes.Gray;
                         else
                              othersPinsControl.likeIcon.Foreground = Brushes.White;
                         if (react.Equals("dislikes"))
                              othersPinsControl.dislikeIcon.Foreground = Brushes.Gray;
                         else
                              othersPinsControl.dislikeIcon.Foreground = Brushes.White;

                         othersPinsControl.Margin = new Thickness(initLeft + (i* 230), initTop - (d * 440), initRight - (i * 220), initBottom - 220 * d);

                         HomePanel.Children.Add(othersPinsControl);
                    i++;
                    }
               }
          }

          private void LogOut_btn_Click(object sender, RoutedEventArgs e)
          {
               connection.Open();
               OracleCommand LogoutCommand = new OracleCommand();
               LogoutCommand.Connection = connection;
               LogoutCommand.CommandText = "update PINTEREST_USER set Login= :login where user_id = :userID";
               LogoutCommand.Parameters.Add("login", '0');
               LogoutCommand.Parameters.Add("userID", Account.UserID);
               LogoutCommand.CommandType = CommandType.Text;
               LogoutCommand.ExecuteNonQuery();
               connection.Close();

               Login login1 = new Login();
               login1.Show();
               Close();
          }

          private void Help_btn_Click(object sender, RoutedEventArgs e)
          {
               HelpForm help = new HelpForm();
               help.Show();
          }

          private void PopupBox_Opened(object sender, RoutedEventArgs e)
          {
               connection.Open();
               List.Items.Clear(); //important
               OracleCommand cmd = new OracleCommand();
               cmd.Connection = connection;
               cmd.CommandText = "Notifications";
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Add("id", Account.UserID);
               cmd.Parameters.Add("info", OracleDbType.RefCursor, ParameterDirection.Output);
               OracleDataReader dr = cmd.ExecuteReader();

               while (dr.Read())
               {

                    if (dr[0].ToString() != "")
                    {
                         Pinterest_V2.UserControls.NotificationsUserControl notifiControl = new UserControls.NotificationsUserControl();
                         string name = get_name(dr[0].ToString());
                         notifiControl.Notification.Content = "new follower: " + name;
                         notifiControl.icon.Kind = PackIconKind.PersonStar;
                         notifiControl.id_user = dr[0].ToString();
                         notifiControl.name = name;
                         notifiControl.Visibility = System.Windows.Visibility.Visible;
                         List.Items.Add(notifiControl);

                    }
                    if (dr[1].ToString() != "")
                    {
                         Pinterest_V2.UserControls.NotificationsUserControl notifiControl = new UserControls.NotificationsUserControl();
                         string name = get_name(dr[1].ToString());
                         notifiControl.Notification.Content = "new chat: " + name;
                         notifiControl.icon.Kind = PackIconKind.Chat;
                         notifiControl.id_user = dr[1].ToString();
                         notifiControl.name = name;
                         notifiControl.Visibility = System.Windows.Visibility.Visible;
                         List.Items.Add(notifiControl);

                    }
                    if (dr[2].ToString() != "")
                    {
                         Pinterest_V2.UserControls.NotificationsUserControl notifiControl = new UserControls.NotificationsUserControl();
                         string name = get_name(dr[2].ToString());
                         notifiControl.Notification.Content = "new friend: " + name;
                         notifiControl.icon.Kind = PackIconKind.PersonPlus;
                         notifiControl.id_user = dr[2].ToString();
                         notifiControl.name = name;
                         notifiControl.Visibility = System.Windows.Visibility.Visible;
                         List.Items.Add(notifiControl);
                    }
               }               cmd = new OracleCommand();               cmd.Connection = connection;               cmd.CommandText = "select p.pin_name, re.react_type , re.user_id from user_react re, pin p  where re.pin_id= p.pin_id  and p.user_id =:id";               cmd.CommandType = CommandType.Text;               cmd.Parameters.Add("id", Account.UserID);
               dr = cmd.ExecuteReader();
               while (dr.Read())
               {                    if (dr[0].ToString() != "")                    {
                         Pinterest_V2.UserControls.NotificationsUserControl notifiControl = new UserControls.NotificationsUserControl();
                         notifiControl.Notification.Content = get_name(dr[2].ToString()) + " " + dr[1].ToString() + " " + dr[0].ToString();
                         if (dr[1].ToString() == "likes")                              notifiControl.icon.Kind = PackIconKind.Like;
                         else
                              notifiControl.icon.Kind = PackIconKind.Dislike;
                         notifiControl.id_user = dr[2].ToString();
                         notifiControl.Visibility = System.Windows.Visibility.Visible;
                         List.Items.Add(notifiControl);
                    }
               }

               dr.Close();
               connection.Close();
          }

          string get_name(string id)
          {
            //////////////////////// A. 2. Select one Row Bind, Command Variables///////////////////////////////////////
            string name = "";
               OracleCommand cmd = new OracleCommand();
               cmd.Connection = connection;
               cmd.CommandText = " select user_name  from pinterest_user where user_id=:id";
               cmd.CommandType = CommandType.Text;
               cmd.Parameters.Add("id", id);
               OracleDataReader dr = cmd.ExecuteReader();
               if (dr.Read())
               {
                    name = dr["user_name"].ToString();
               }
               dr.Close();
               return name;
          }

          private void Search_btn_Click(object sender, RoutedEventArgs e)
          {
               HomePanel.Children.Clear();
               if (search_txt.Text == "")
                    MessageBox.Show("Please Enter search name");
               else
               {
                    if (SearchType_combx.SelectedIndex == 0)
                         Search_User(search_txt.Text);
                    else if (SearchType_combx.SelectedIndex == 1)
                         Search_Board(search_txt.Text);
                    else if (SearchType_combx.SelectedIndex == 2)
                         Search_Pin(search_txt.Text);
                    else
                         MessageBox.Show("Please Choose Type");
               }
          }


                    //################## SEARCH FUNCTION ##################\\

          private void Search_User(string Search_txt)
          {
               /////////////////////////// b. 1. ////////////////////////////////////
               string cmdstr = @"Select user_id, user_name, profile_pic  From pinterest_user Where lower(user_name) = lower(:SearchName) and user_id != :userID";
               OracleDataAdapter SearchAdapter = new OracleDataAdapter(cmdstr, FixedValues.ordb);
               SearchAdapter.SelectCommand.Parameters.Add("SearchName", Search_txt);
               SearchAdapter.SelectCommand.Parameters.Add("userID", Account.UserID);
               DataSet SearchDS = new DataSet();
               SearchAdapter.Fill(SearchDS);
               DataRow row = SearchDS.Tables[0].NewRow();

               //initial coordinates... don't change
               double initLeft = 50;
               double initTop = 50;
               double initRight = 510;
               double initBottom = 10;

               double d;
               int i = 0;
               for (int j = 0; j < SearchDS.Tables[0].Rows.Count; j++)
               {
                    if ((i + 1) % 5 == 0)
                    {
                         initLeft = 20;
                         initRight = 510;

                         i = 0;
                    }

                    if (i >= 1)
                         d = 0.5;

                    else
                         d = i;

                    row = SearchDS.Tables[0].Rows[j];

                    Pinterest_V2.UserControls.UserSearchUserControl OtherUserControl = new UserControls.UserSearchUserControl();
                    OtherUserControl.OtherUserID = Convert.ToInt32(SearchDS.Tables[0].Rows[j][0]);
                    OtherUserControl.Profilbtn.Content = SearchDS.Tables[0].Rows[j][1];
                    byte[] image = (byte[])SearchDS.Tables[0].Rows[i][2];
                    OtherUserControl.Profilbtn.Background = new ImageBrush(FixedValues.LoadImage(image));

                    OtherUserControl.Profilbtn.Foreground = Brushes.White;
                    OtherUserControl.Margin = new Thickness(initLeft + (i * 230), initTop - (d * 440), initRight - (i * 220), initBottom - 220 * d);

                    HomePanel.Children.Add(OtherUserControl);
                i++;
               }

               if (SearchDS.Tables[0].Rows.Count == 0)
                    MessageBox.Show("No Result Found");
          }

          private void Search_Board(string Search_txt)
          {
               /////////////////////////// b. 1. ////////////////////////////////////
               string cmdstr = @"Select board_name, user_id From Board Where lower(board_name) = lower(:SearchName) and privacy = 0 and user_id <> :userID";
               OracleDataAdapter SearchAdapter = new OracleDataAdapter(cmdstr, FixedValues.ordb);
               SearchAdapter.SelectCommand.Parameters.Add("SearchName", Search_txt);
               SearchAdapter.SelectCommand.Parameters.Add("userID", Account.UserID);
               DataSet SearchDS = new DataSet();
               SearchAdapter.Fill(SearchDS);
               DataRow row = SearchDS.Tables[0].NewRow();

               //initial coordinates... don't change
               double initLeft = 50;
               double initTop = 50;
               double initRight = 510;
               double initBottom = 10;

               double d;
               int i = 0;
               for (int j = 0; j < SearchDS.Tables[0].Rows.Count; j++)
               {
                    if ((i + 1) % 5 == 0)
                    {
                         initLeft = 20;
                         initRight = 510;

                         i = 0;
                    }

                    if (i >= 1)
                    {
                         d = 0.5;
                    }

                    else
                         d = i;

                    row = SearchDS.Tables[0].Rows[j];

                    Pinterest_V2.UserControls.otherUserBoardUserControl OtherBoardControl = new UserControls.otherUserBoardUserControl();
                    OtherBoardControl.boardButton.Foreground = Brushes.Gray;

                    OtherBoardControl.boardButton.Content = SearchDS.Tables[0].Rows[j][0];
                    OtherBoardControl.board_USERID = Convert.ToInt32(SearchDS.Tables[0].Rows[j][1]);

                    OtherBoardControl.Margin = new Thickness(initLeft + (i * 230), initTop - (d * 440), initRight - (i * 220), initBottom - 220 * d);
                    HomePanel.Children.Add(OtherBoardControl);
                    i++;
               }

               if (SearchDS.Tables[0].Rows.Count == 0)
                    MessageBox.Show("No Result Found");
          }

          private void Search_Pin(string Search_txt)
          {
            /////////////////////////// b. 1. ////////////////////////////////////
               string cmdstr = @"Select p.pin_id, p.pin_name, p.pin_image From Pin p , board b Where p.board_name = b.board_name and b.privacy = 0 and lower(p.pin_name) = lower(:SearchName) and p.user_id != :userID";
               SearchAdapter = new OracleDataAdapter(cmdstr, FixedValues.ordb);
               SearchAdapter.SelectCommand.Parameters.Add("SearchName", Search_txt);
               SearchAdapter.SelectCommand.Parameters.Add("userID", Account.UserID);
               DataSet SearchDS = new DataSet();
               SearchAdapter.Fill(SearchDS);
               DataRow row = SearchDS.Tables[0].NewRow();

               //initial coordinates... don't change
               double initLeft = 50;
               double initTop = 50;
               double initRight = 510;
               double initBottom = 10;

               double d;
               int i=0;
               for (int j = 0; j < SearchDS.Tables[0].Rows.Count; j++)
               {
                    if ((i + 1) % 5 == 0)
                    {
                         initLeft = 20;
                         initRight = 510;

                         i = 0;
                    }

                    if (i >= 1)
                         d = 0.5;

                    else
                         d = i;


                    row = SearchDS.Tables[0].Rows[j];

                    Pinterest_V2.UserControls.othersPinUserControl OtherPinControl = new UserControls.othersPinUserControl();

                    OtherPinControl.pin_id = Convert.ToInt32(SearchDS.Tables[0].Rows[j][0]);
                    byte[] image = (byte[])SearchDS.Tables[0].Rows[j][2];
                    OtherPinControl.otherPinPhoto.Source = FixedValues.LoadImage(image);

                    OtherPinControl.Margin = new Thickness(initLeft + (i * 230), initTop - (d * 440), initRight - (i * 220), initBottom - 220 * d);

                    HomePanel.Children.Add(OtherPinControl);
                    i++;
               }

               if (SearchDS.Tables[0].Rows.Count == 0)
                    MessageBox.Show("No Result Found");
          }

        private void cat_rep_btn_Click(object sender, RoutedEventArgs e)
        {
            DataDisplay rep = new DataDisplay();
            rep.pin = false;
            rep.Show();

        }

        private void pin_rep_btn_Click(object sender, RoutedEventArgs e)
        {
            DataDisplay rep = new DataDisplay();
            rep.pin = true;
            rep.Show();
        }
    }
}
