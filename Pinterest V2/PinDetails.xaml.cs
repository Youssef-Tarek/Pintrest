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
     /// Interaction logic for PinDetails.xaml
     /// </summary>

     public partial class PinDetails : Window
     {
          public int pin_id;
          public PinDetails()
          {
               InitializeComponent();

          }
          private void loadPage(object sender, RoutedEventArgs e)
          {
               ////////////////////////// B. 1. //////////////////////////////////////////////
               string command = "select pin_name, pin_image, board_name, description from pin where pin_id= :pinid  ";
               OracleDataAdapter adapter = new OracleDataAdapter(command, FixedValues.ordb);
               adapter.SelectCommand.Parameters.Add("pinid", pin_id);
               DataSet set = new DataSet();
               adapter.Fill(set);
               byte[] image = (byte[])set.Tables[0].Rows[0].ItemArray[1];
               Pin_pic.Source = FixedValues.LoadImage(image);
               PinName_txt.Text = set.Tables[0].Rows[0].ItemArray[0].ToString();
               Description_txt.Text = set.Tables[0].Rows[0].ItemArray[3].ToString();
               board_combx.Text = set.Tables[0].Rows[0].ItemArray[2].ToString();
          }

          private void Back_btn_Click(object sender, RoutedEventArgs e)
          {
               Close();

          }
     }
}
