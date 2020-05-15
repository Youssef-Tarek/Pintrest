using MaterialDesignThemes.Wpf;
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
     /// Interaction logic for OtherProfile.xaml
     /// </summary>
     public partial class OtherProfile : UserControl
     {
          OracleConnection connection = new OracleConnection(FixedValues.ordb);
          OracleDataAdapter ProfileDataAdapter;
          DataSet ProfileDataSet;
          Boolean IsFollowed; //assign this value according to database
          int IsFriend;
          public int OtherUserID;

          public OtherProfile(Boolean IsFollowed, int IsFriend)
          {
               InitializeComponent();

               this.IsFollowed = IsFollowed;
               this.IsFriend = IsFriend;

               if (IsFollowed)
                    Follow_btn.Content = "UnFollow";
               else
                    Follow_btn.Content = "Follow";

               if (IsFriend == 1)
               {
                    friend_state.Content = "Friend";
                    state_icon.Kind = PackIconKind.Person;
               }
               else if (IsFriend == 0)
               {
                    friend_state.Content = "request sending";
                    state_icon.Kind = PackIconKind.Person;
               }
               else
               {
                    friend_state.Content = "Add Friend";
                    state_icon.Kind = PackIconKind.PersonAdd;
               }
          }

          private void Follow_btn_Click(object sender, RoutedEventArgs e)
          {
               connection.Open();
               OracleCommand c;
               if (IsFollowed == true)
               {

                    c = new OracleCommand();
                    c.Connection = connection;
                    c.CommandText = "delete  from  user_following where user_following=:user_follow and user_id=:user_id";
                    c.Parameters.Add("user_id", OtherUserID);
                    c.Parameters.Add("user_follow", Account.UserID);
                    int r = c.ExecuteNonQuery();
                    if (r != -1)
                    {
                         Follow_btn.Content = "UnFollow";
                         IsFollowed = false;
                    }
               }

               else
               {
                    Follow_btn.Content = "UnFollow";
                    c = new OracleCommand();
                    c.Connection = connection;
                    c.CommandText = "insert into user_following (user_id, user_following,date_follow) values (:id_user , :id_follow,:date_follow)";
                    c.Parameters.Add("id_user", Account.UserID);
                    c.Parameters.Add("id_follow", OtherUserID);
                    c.Parameters.Add("data_follow", DateTime.Now.Date);
                    int r = c.ExecuteNonQuery();
                    if (r != -1)
                    {
                         Follow_btn.Content = "Follow";
                         IsFollowed = true;
                    }

               }
               connection.Close();
          }

          private void Board_btn_Click(object sender, RoutedEventArgs e)
          {
               boardPanel.Children.Clear();
               connection.Open();
               OracleCommand BoardCommand = new OracleCommand();
               BoardCommand.Connection = connection;
               BoardCommand.CommandText = "select board_name from board where user_id= :userid and privacy=:privacy";
               BoardCommand.CommandType = CommandType.Text;
               BoardCommand.Parameters.Add("userid", OtherUserID);
               BoardCommand.Parameters.Add("privacy", "0");
               OracleDataReader BoardReader = BoardCommand.ExecuteReader();

               string picName = FixedValues.getCurrentPath() + "images\\empty.png";

               //initial coordinates... don't change
               double initLeft = 0;
               double initTop = 10;
               double initRight = 670;
               double initBottom = 10;

               double d;
               int i = 0;
               while (BoardReader.Read())
               {
                    if ((i + 1) % 5 == 0)
                    {
                         initLeft = 0;
                         initRight = 670;

                         i = 0;
                    }

                    if (i >= 2)
                         d = 1;

                    else
                         d = i;


                    OracleCommand PinCommand = new OracleCommand();
                    PinCommand.Connection = connection;
                    PinCommand.CommandText = "select pin_image from pin  where board_name= :boardname and user_id= :userid ";
                    PinCommand.CommandType = CommandType.Text;
                    PinCommand.Parameters.Add("boardname", BoardReader[0].ToString());
                    PinCommand.Parameters.Add("userid", OtherUserID);
                    OracleDataReader PinReader = PinCommand.ExecuteReader();

                    BitmapImage image;
                    if (PinReader.Read())
                    {
                         byte[] image1 = (byte[])PinReader[0];
                         image = FixedValues.LoadImage(image1);
                    }
                    else
                         image = new BitmapImage(new Uri(picName, UriKind.Relative));
                    PinReader.Close();

                    UserControls.otherUserBoardUserControl OtherBoardControl = new UserControls.otherUserBoardUserControl();
                    OtherBoardControl.boardButton.Content = BoardReader[0];
                    OtherBoardControl.board_USERID = OtherUserID;
                    OtherBoardControl.boardButton.Background = new ImageBrush(image);
                    OtherBoardControl.boardButton.Foreground = Brushes.WhiteSmoke;

                    OtherBoardControl.Margin = new Thickness(initLeft + (i * 230), initTop - (d * 440), initRight - (i * 220), initBottom - 220 * d);
                    boardPanel.Children.Add(OtherBoardControl);

                    i++;
               }
               BoardReader.Close();
               connection.Close();
          }

          private void Pin_btn_Click(object sender, RoutedEventArgs e)
          {
               boardPanel.Children.Clear();
               connection.Open();
               OracleCommand command = new OracleCommand();
               command.Connection = connection;
               command.CommandText = "select pin.pin_name,pin_image,pin. pin_id from pin  , board where pin.board_name = board.board_name and pin.user_id = board.user_id and pin.user_id = :user_id  and board.privacy =:privacy ";
               command.CommandType = CommandType.Text;
               command.Parameters.Add("user_id", OtherUserID);
               command.Parameters.Add("privacy", "0");
               OracleDataReader reader = command.ExecuteReader();

               //initial coordinates... don't change
               double initLeft = 20;
               double initTop = 10;
               double initRight = 510;
               double initBottom = 10;

               //for first login only.. don't change
               double d;
               int i = 0;
               while (reader.Read())
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

                    Pinterest_V2.UserControls.myPinUserControl myPinsControl = new UserControls.myPinUserControl(boardPanel);

                    myPinsControl.myPinButton.Content = reader[0];
                    byte[] image = (byte[])reader[1];
                    myPinsControl.myPinButton.Background = new ImageBrush(FixedValues.LoadImage(image));
                    myPinsControl.pin_id = Convert.ToInt32(reader[2]);

                    myPinsControl.myPinButton.Foreground = Brushes.WhiteSmoke;
                    myPinsControl.moveIcon.Foreground = Brushes.WhiteSmoke;
                    myPinsControl.DeleteMyPin_btn_Icon.Foreground = Brushes.WhiteSmoke;
                    myPinsControl.EditMyPin_Icon.Foreground = Brushes.WhiteSmoke;

                    myPinsControl.Margin = new Thickness(initLeft + (i * 230), initTop - (d * 440), initRight - (i * 220), initBottom - 220 * d);
                    boardPanel.Children.Add(myPinsControl);

                    i++;
               }
               reader.Close();
               connection.Close();
          }

          private void Grid_Loaded(object sender, RoutedEventArgs e)
          {
               
               string cmdstr = "select user_name, profile_pic from PINTEREST_USER where user_id=:id";
               ProfileDataAdapter = new OracleDataAdapter(cmdstr, FixedValues.ordb);
               ProfileDataAdapter.SelectCommand.Parameters.Add("id", OtherUserID);

               ProfileDataSet = new DataSet();
               ProfileDataAdapter.Fill(ProfileDataSet);
               ProfileName_label.Content = ProfileDataSet.Tables[0].Rows[0][0];
               byte[] image = (byte[])ProfileDataSet.Tables[0].Rows[0][1];
               ProfilePic.Fill = new ImageBrush(FixedValues.LoadImage(image));

               cmdstr = "select count(*) from user_following where user_id = :userid";
               ProfileDataAdapter = new OracleDataAdapter(cmdstr, FixedValues.ordb);
               ProfileDataAdapter.SelectCommand.Parameters.Add("userid", OtherUserID);
               ProfileDataSet = new DataSet();
               ProfileDataAdapter.Fill(ProfileDataSet);

               ProfileFollowers_label.Content = ProfileDataSet.Tables[0].Rows[0][0];
          }

          private void Friend_state_Click(object sender, RoutedEventArgs e)
          {
               connection.Open();
               OracleCommand c;
               if (IsFriend == 1)
               {

                    c = new OracleCommand();
                    c.Connection = connection;
                    c.CommandText = "delete  from  user_friend where user_friend=:user_friend and user_id=:user_id";
                    c.Parameters.Add("user_id", Account.UserID);
                    c.Parameters.Add("user_friend", OtherUserID);
                    int r = c.ExecuteNonQuery();
                    if (r != -1)
                    {
                         Follow_btn.Content = "Add Friend";
                         state_icon.Kind = PackIconKind.PersonAdd;
                         IsFriend = 0;
                    }

               }

               else if (IsFriend == -1)
               {
                    c = new OracleCommand();
                    c.Connection = connection;
                    c.CommandText = "insert into user_friend(user_id, user_friend, friend_state,date_request_friend) values(:id_user , :id_friend,:state,:current_date)";
                    c.Parameters.Add("id_user", Account.UserID);
                    c.Parameters.Add("id_friend", OtherUserID);
                    c.Parameters.Add("state", "0");
                    c.Parameters.Add("current_date", DateTime.Now.Date);
                    int r = c.ExecuteNonQuery();
                    if (r != -1)
                    {
                         friend_state.Content = "RequestSending";
                         state_icon.Kind = PackIconKind.Person;
                         IsFriend = 0;
                    }

               }
               connection.Close();
          }
     }
}
