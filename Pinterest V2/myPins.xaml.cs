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
using Oracle.DataAccess.Client;

namespace Pinterest_V2
{
     /// <summary>
     /// Interaction logic for myPins.xaml
     /// </summary>
     public partial class myPins : Window
     {
          string board_name = "";
          OracleConnection con;
          public myPins(string boardname)
          {
               board_name = boardname;
               InitializeComponent();
               con = new OracleConnection(FixedValues.ordb);
          }

          private void Grid_Loaded(object sender, RoutedEventArgs e)
          {
               boardNameLabel.Content = board_name;
               con.Open();
               OracleCommand command = new OracleCommand();
               command.Connection = con;
               command.CommandText = "select pin_name, pin_image, pin_id from pin  where user_id= :userid  and board_name= :boardname";
               command.CommandType = CommandType.Text;
               command.Parameters.Add("userid", Account.UserID);
               command.Parameters.Add("boardname", board_name);
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
                    if ((i + 1) % 4 == 0)
                    {
                         initLeft = 20;
                         initRight = 510;
                         i = 0;
                    }

                    if (i >= 1)
                         d = 0.5;

                    else
                         d = i;

                    Pinterest_V2.UserControls.myPinUserControl myPinsControl = new UserControls.myPinUserControl(pinsPanel);

                    myPinsControl.myPinButton.Content = reader[0];

                    byte[] image = (byte[])reader[1]; 
                    myPinsControl.myPinButton.Background = new ImageBrush(FixedValues.LoadImage(image));

                    myPinsControl.pin_id = Convert.ToInt32(reader[2]);

                    myPinsControl.myPinButton.Foreground = Brushes.White;
                    myPinsControl.moveIcon.Foreground = Brushes.White;
                    myPinsControl.DeleteMyPin_btn_Icon.Foreground = Brushes.White;
                    myPinsControl.EditMyPin_Icon.Foreground = Brushes.White;
                    myPinsControl.Margin = new Thickness(initLeft + (i * 230), initTop - (d * 440), initRight - (i * 220), initBottom - 220 * d);

                    pinsPanel.Children.Add(myPinsControl);

                    i++;
               }
               reader.Close();
               con.Close();
          }

          private void Close_icon(object sender, RoutedEventArgs e)
          {
               Close();
          }
     }
}
