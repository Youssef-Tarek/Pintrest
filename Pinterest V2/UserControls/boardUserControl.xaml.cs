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
     /// Interaction logic for boardUserControl.xaml
     /// </summary>
     public partial class boardUserControl : UserControl
     {
          public string boardname = "";

          StackPanel board;
          public boardUserControl(StackPanel x)
          {
               InitializeComponent();

               board = x;
          }
          private void boardButton_Click(object sender, RoutedEventArgs e)
          {
               myPins myPins1 = new myPins(boardButton.Content.ToString());
               myPins1.ShowDialog();

          }

          private void EditMyBoard_Click(object sender, RoutedEventArgs e)
          {
               CreateBoard board = new CreateBoard();
               board.update = true;
               board.boardName = boardButton.Content.ToString();
               board.ShowDialog();
          }

          private void DeleteMyBoard_btn_Click(object sender, RoutedEventArgs e)
          {
               //////////////////////////////// b. 3. Delete ///////////////////////////
               boardname = boardButton.Content.ToString();
               string command = "select * from pin";
               OracleDataAdapter adapter = new OracleDataAdapter(command, FixedValues.ordb);
               OracleCommandBuilder builder;
               DataSet set = new DataSet();
               adapter.Fill(set);
               for (int i = 0; i < set.Tables[0].Rows.Count; i++)
                    if (set.Tables[0].Rows[i].ItemArray[4].ToString() == boardname && set.Tables[0].Rows[i].ItemArray[5].ToString() == Account.UserID.ToString())
                    {
                         set.Tables[0].Rows[i].Delete();
                    }
               builder = new OracleCommandBuilder(adapter);
               adapter.Update(set.Tables[0]);

               ///////////////////////////////////// ////////////////////////////////////////////////////////
               string command1 = "select * from board";
               OracleDataAdapter adapter1 = new OracleDataAdapter(command1, FixedValues.ordb);
               OracleCommandBuilder builder1;
               DataSet set1 = new DataSet();
               adapter1.Fill(set1);
               for (int i = 0; i < set1.Tables[0].Rows.Count; i++)
                    if (set1.Tables[0].Rows[i].ItemArray[0].ToString() == boardname && set1.Tables[0].Rows[i].ItemArray[4].ToString() == Account.UserID.ToString())
                    {
                         set1.Tables[0].Rows[i].Delete();
                    }
               builder1 = new OracleCommandBuilder(adapter1);
               adapter1.Update(set1.Tables[0]);

               board.Children.Remove(this);


          }

          private void UserControl_MouseEnter(object sender, MouseEventArgs e)
          {
               EditMyBoard_btn.Visibility = System.Windows.Visibility.Visible;
               DeleteMyBoard_btn.Visibility = System.Windows.Visibility.Visible;
          }

          private void UserControl_MouseLeave(object sender, MouseEventArgs e)
          {
               EditMyBoard_btn.Visibility = System.Windows.Visibility.Hidden;
               DeleteMyBoard_btn.Visibility = System.Windows.Visibility.Hidden;
          }
     }
}
