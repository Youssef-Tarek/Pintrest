using Oracle.DataAccess.Client;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pinterest_V2.UserControls
{
     /// <summary>
     /// Interaction logic for myPinUserControl.xaml
     /// </summary>
     public partial class myPinUserControl : UserControl
     {
          public int pin_id;
          OracleConnection con;
          StackPanel board;
          public myPinUserControl(StackPanel x)
          {
               InitializeComponent();
               con = new OracleConnection(FixedValues.ordb);
               board = x;
          }

          private void UserControl_MouseEnter(object sender, MouseEventArgs e)
          {
               moveMyPin_btn.Visibility = System.Windows.Visibility.Visible;
               EditMyPin_btn.Visibility = System.Windows.Visibility.Visible;
               DeleteMyPin_btn.Visibility = System.Windows.Visibility.Visible;
          }

          private void UserControl_MouseLeave(object sender, MouseEventArgs e)
          {
               moveMyPin_btn.Visibility = System.Windows.Visibility.Hidden;
               EditMyPin_btn.Visibility = System.Windows.Visibility.Hidden;
               DeleteMyPin_btn.Visibility = System.Windows.Visibility.Hidden;
          }

          private void myPinButton_Click(object sender, RoutedEventArgs e)
          {
               PinDetails pin = new PinDetails();
               pin.pin_id = pin_id;
               pin.ShowDialog();
          }

          private void moveMyPin_btn_Click(object sender, RoutedEventArgs e)
          {
               choose_board choose_form = new choose_board();
               choose_form.pin_id = pin_id;
               choose_form.ShowDialog();
          }

          private void EditMyPin_Click(object sender, RoutedEventArgs e)
          {
               CreatePin pin = new CreatePin();
               pin.Edit = true;
               pin.pin_id_edit = pin_id;
               pin.ShowDialog();
          }

          private void DeleteMyPin_btn_Click(object sender, RoutedEventArgs e)
          {
               //////////////////// A.6. delete /////////////
               con.Open();
               OracleCommand cmd = new OracleCommand();
               cmd.Connection = con;
               cmd.CommandText = "delete_pin";
               cmd.CommandType = System.Data.CommandType.StoredProcedure;
               cmd.Parameters.Add("pin_id", pin_id);
               cmd.ExecuteNonQuery();
               con.Close();
               board.Children.Remove(this);


          }

     }
}
