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
     /// Interaction logic for choose_board.xaml
     /// </summary>
     public partial class choose_board : Window
     {
          OracleConnection con;
          public Boolean add_to_board = false;
          public int pin_id;

          public choose_board()
          {
               InitializeComponent();
               con = new OracleConnection(FixedValues.ordb);
               con.Open();
               OracleCommand command = new OracleCommand();
               command.Connection = con;
               command.CommandText = "select board_name from board where user_id= :userid";
               command.CommandType = CommandType.Text;
               command.Parameters.Add("userid", Account.UserID);
               OracleDataReader reader = command.ExecuteReader();
               board_combx.Items.Clear();
               while (reader.Read())
                    board_combx.Items.Add(reader[0].ToString());
               reader.Close();
               con.Close();
          }

          private void choose_btn_Click(object sender, RoutedEventArgs e)
          {
               if (add_to_board)
                    add_to_boardfun();
               else
                    change_board();

               Close();
          }

          private void add_to_boardfun()
          {
               con.Open();

               /////////////////////////////////////////////////////////////////////////////////////////////
               string pinName = "";
               byte[] image = null;
               string description = "";

               OracleCommand command = new OracleCommand();
               command.Connection = con;
               command.CommandText = "select * from pin where pin_id= :pinid";
               command.CommandType = CommandType.Text;
               command.Parameters.Add("pinid", pin_id);
               OracleDataReader reader = command.ExecuteReader();
               if (reader.Read())
               {
                    pinName = reader[1].ToString();
                    image = (byte[])reader[2];
                    description = reader[3].ToString();
               }
               reader.Close();
               //////////////////////////////////////////////////////////////////////////////////////////////
               if (board_combx.SelectedIndex != -1)
               {

                    OracleCommand command1 = new OracleCommand();
                    command1.Connection = con;
                    command1.CommandText = "insert into pin values(pin_id.nextval, :pinName, :pinImage, :descrption, :boardname, :user_id, :pin_Date) ";
                    command1.CommandType = CommandType.Text;
                    command1.Parameters.Add("pinName", pinName);
                    command1.Parameters.Add("pinimage", image);
                    command1.Parameters.Add("descrption", description);
                    command1.Parameters.Add("boardname", board_combx.SelectedItem);
                    command1.Parameters.Add("user_id", Account.UserID);
                    command1.Parameters.Add("Pin_Date", DateTime.Now);

                    command1.ExecuteNonQuery();

               }
               con.Close();
               Close();

          }

          private void change_board()
          {
               if (board_combx.SelectedIndex != -1)
               {
                    con.Open();
                    OracleCommand command = new OracleCommand();
                    command.Connection = con;
                    command.CommandText = "update pin set board_name=:boardname where pin_id= :pinId ";
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("boardname", board_combx.SelectedItem);
                    command.Parameters.Add("pinId", pin_id);
                    command.ExecuteNonQuery();
                    con.Close();
               }
               Close();
          }
     }
}
