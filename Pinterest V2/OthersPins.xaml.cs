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
     /// Interaction logic for OthersPins.xaml
     /// </summary>
     public partial class OthersPins : Window
     {
          OracleConnection connection = new OracleConnection(FixedValues.ordb);
          public int user_id;
          public int board_USERID;
          public string board_Name;

          public OthersPins()
          {
               InitializeComponent();   
          }

          private void Close_icon(object sender, RoutedEventArgs e)
          {
               Close();
          }

          private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
          {
               connection.Open();
               OracleCommand command = new OracleCommand();
               command.Connection = connection;
               command.CommandText = "select p.pin_image, p.pin_id  from pin p, board b where p.board_name  = b.board_name  and p.user_id = :board_userID and  b.board_name = :boardName";
               command.CommandType = CommandType.Text;
               command.Parameters.Add("board_userID", board_USERID);
               command.Parameters.Add("boardName", board_Name);

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
                    if ((i + 1) % 3 == 0)
                    {
                         initLeft = 20;
                         initRight = 510;
                         i = 0;
                    }

                    if (i >= 1)
                         d = 0.5;

                    else
                         d = i;

                    Pinterest_V2.UserControls.othersPinUserControl OtherPinsControl = new UserControls.othersPinUserControl();

                    byte[] image = (byte[])reader[0];
                    OtherPinsControl.otherPinPhoto.Source = FixedValues.LoadImage(image);
                    OtherPinsControl.pin_id = Convert.ToInt32(reader[1]);

                    OtherPinsControl.addPinToBoard_btn.Foreground = Brushes.White;
                    OtherPinsControl.like_btn.Foreground = Brushes.White;
                    OtherPinsControl.dislike_btn.Foreground = Brushes.White;
                    OtherPinsControl.pin_id = Convert.ToInt32(reader[1]);

                    OtherPinsControl.Margin = new Thickness(initLeft + (i * 230), initTop - (d * 440), initRight - (i * 220), initBottom - 220 * d);

                    BoardsPanel.Children.Add(OtherPinsControl);

                    i++;
               }
               
               reader.Close();
               connection.Close();
          }
     }
}
