using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Pinterest_V2.UserControls
{
     /// <summary>
     /// Interaction logic for Profile.xaml
     /// </summary>
     public partial class ProfileUC : UserControl
     {
          OracleDataAdapter ProfileDataAdapter;
          DataSet ProfileDataSet;
          Account newAccount = new Account();
          OracleConnection con;

          public ProfileUC()
          {
               InitializeComponent();
               con = new OracleConnection(FixedValues.ordb);

          }

          private void Categories_btn_Click(object sender, RoutedEventArgs e)
          {
               boardPanel.Children.Clear();
               Categories_user_control_ categoryUC = new Categories_user_control_();
               categoryUC.Show();
          }

          private void Pins_btn_Click(object sender, RoutedEventArgs e)
          {
               con.Open();
               boardPanel.Children.Clear();

               OracleCommand command = new OracleCommand();
               command.Connection = con;
               command.CommandText = "select pin_name, pin_image, pin_id from pin  where user_id= :userid ";
               command.CommandType = CommandType.Text;
               command.Parameters.Add("userid", Account.UserID);
               OracleDataReader reader = command.ExecuteReader();

               //initial coordinates... don't change
               double initLeft = 40;
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

                    myPinsControl.myPinButton.Foreground = Brushes.White;
                    myPinsControl.moveIcon.Foreground = Brushes.White;
                    myPinsControl.DeleteMyPin_btn_Icon.Foreground = Brushes.White;
                    myPinsControl.EditMyPin_Icon.Foreground = Brushes.White;
                    myPinsControl.pin_id = Convert.ToInt32(reader[2]);

                    myPinsControl.Margin = new Thickness(initLeft + (i * 230), initTop - (d * 440), initRight - (i * 220), initBottom - 220 * d);
                    boardPanel.Children.Add(myPinsControl);

                    i++;
               }

               reader.Close();
               con.Close();
          }

          private void Board_btn_Click(object sender, RoutedEventArgs e)
          {
               con.Open();
               boardPanel.Children.Clear();

               OracleCommand BoardCommand = new OracleCommand();
               BoardCommand.Connection = con;
               BoardCommand.CommandText = "select board_name from board where user_id= :userid ";
               BoardCommand.CommandType = CommandType.Text;
               BoardCommand.Parameters.Add("userid", Account.UserID);
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
                    PinCommand.Connection = con;

                    PinCommand.CommandText = "select pin_image from pin  where board_name= :boardname and user_id= :userid ";
                    PinCommand.CommandType = CommandType.Text;
                    PinCommand.Parameters.Add("boardname", BoardReader[0].ToString());
                    PinCommand.Parameters.Add("userid", Account.UserID);
                    OracleDataReader PinReader = PinCommand.ExecuteReader();

                    BitmapImage BoardImage;
                    if (PinReader.Read())
                    {
                         byte[] image = (byte[])PinReader[0];
                         BoardImage = FixedValues.LoadImage(image);
                    }
                    else
                         BoardImage = new BitmapImage(new Uri(picName, UriKind.Relative));
                    PinReader.Close();


                    Pinterest_V2.UserControls.boardUserControl boardsControl = new UserControls.boardUserControl(boardPanel);

                    boardsControl.boardButton.Content = BoardReader[0];

                    boardsControl.boardButton.Background = new ImageBrush(BoardImage);
                    boardsControl.boardButton.Foreground = Brushes.White;
                    boardsControl.DeleteMyBoard_btn_Icon.Foreground = Brushes.White;
                    boardsControl.EditMyBoard_Icon.Foreground = Brushes.White;

                    boardsControl.Margin = new Thickness(initLeft + (i * 230), initTop - (d * 440), initRight - (i * 220), initBottom - 220 * d);
                    boardPanel.Children.Add(boardsControl);
                    i++;
               }

               BoardReader.Close();
               con.Close();
          }

          private void CreatePin_btn_Click(object sender, RoutedEventArgs e)
          {
               con.Open();
               OracleCommand command = new OracleCommand();
               command.Connection = con;
               command.CommandText = "select count(*)  from board where user_id= :userid";
               command.CommandType = CommandType.Text;
               command.Parameters.Add("userid", Account.UserID);
               OracleDataReader reader = command.ExecuteReader();
               int i = -1;
               if (reader.Read())
                    i = Convert.ToInt32(reader[0]);
               reader.Close();
               con.Close();

               boardPanel.Children.Clear();
               if (i == 0)
               {
                    MessageBox.Show("Please, create board First");
                    CreateBoard board1 = new CreateBoard();
                    board1.ShowDialog();
               }
               else
               {
                    CreatePin pin1 = new CreatePin();
                    pin1.ShowDialog();
               }

          }

          private void CreateBoard_btn_Click(object sender, RoutedEventArgs e)
          {
               boardPanel.Children.Clear();
               CreateBoard board1 = new CreateBoard();
               board1.ShowDialog();
          }

          private void Grid_Loaded(object sender, RoutedEventArgs e)
          {
               string cmdstr = "select user_name, profile_pic from PINTEREST_USER where user_id=:id";

               ProfileDataAdapter = new OracleDataAdapter(cmdstr, FixedValues.ordb);
               ProfileDataAdapter.SelectCommand.Parameters.Add("id", Account.UserID);
               ProfileDataSet = new DataSet();
               ProfileDataAdapter.Fill(ProfileDataSet);
               ProfileName.Content = ProfileDataSet.Tables[0].Rows[0][0];
               byte[] image = (byte[])ProfileDataSet.Tables[0].Rows[0][1];

               BitmapImage ProfileImage = FixedValues.LoadImage(image);
               ProfilePic.Fill  = new ImageBrush(ProfileImage);

               con.Open();
               OracleCommand command = new OracleCommand();
               command.Connection = con;
               command.CommandText = "select count(*) from user_following where user_following = :userid ";
               command.CommandType = CommandType.Text;
               command.Parameters.Add("userid", Account.UserID);
               OracleDataReader reader = command.ExecuteReader();
               if (reader.Read())
               {
                    num_Followers.Content = reader[0].ToString();
               }
               command = new OracleCommand();
               command.Connection = con;
               command.CommandText = "select count(*) from user_following where user_id = :userid ";
               command.CommandType = CommandType.Text;
               command.Parameters.Add("userid", Account.UserID);
               reader = command.ExecuteReader();
               if (reader.Read())
               {
                    num_Following.Content = reader[0].ToString();
               }
               reader.Close();
          }

          private void Followers_btn_Click(object sender, RoutedEventArgs e)
          {
               ListOf_Follow follower = new ListOf_Follow("Follower");
               follower.ShowDialog();
          }

          private void Followering_btn_Click(object sender, RoutedEventArgs e)
          {
               ListOf_Follow follower = new ListOf_Follow("Following");
               follower.ShowDialog();
          }
     }
}