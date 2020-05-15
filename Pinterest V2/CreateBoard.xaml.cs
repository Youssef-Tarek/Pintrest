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
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Pinterest_V2
{
     /// <summary>
     /// Interaction logic for CreateBoard.xaml
     /// </summary>
     public partial class CreateBoard : Window
     {
          OracleConnection con;
          public Boolean update = false;
          public string boardName;
          public CreateBoard()
          {
               InitializeComponent();
               con = new OracleConnection(FixedValues.ordb);
          }

          private void Done_btn_Click(object sender, RoutedEventArgs e)
          {
               if (update)
                    Update_Board();
               else
                    Create_Board();
          }

          private void Close_icon(object sender, RoutedEventArgs e)
          {
               Close();
          }

          private void Private_Checkbox_Checked(object sender, RoutedEventArgs e)
          {
               private_Checkbox.IsChecked = true;
          }

          private void Private_Checkbox_Unchecked(object sender, RoutedEventArgs e)
          {
               private_Checkbox.IsChecked = false;
          }

          private void board_Loaded(object sender, RoutedEventArgs e)
          {
               //////////////////////// A.5. ///////////////////////////
               con.Open();
               OracleCommand command = new OracleCommand();
               command.Connection = con;
               command.CommandText = "getCategories";
               command.CommandType = CommandType.StoredProcedure;
               command.Parameters.Add("categoriesName", OracleDbType.RefCursor, ParameterDirection.Output);
               OracleDataReader reader = command.ExecuteReader();
               category_combx.Items.Clear();
               while (reader.Read())
                    category_combx.Items.Add(reader[0].ToString());
               reader.Close();
               con.Close();
               ///////////////////////////////////////////////////////////
               if (update)
               {
                    Title.Source = new BitmapImage(new Uri("/images/edit board.png", UriKind.Relative));
                    Done_btn.Content = "Edit";
                    BoardName_txt.IsReadOnly = true;

                    con.Open();
                    OracleCommand command1 = new OracleCommand();
                    command1.Connection = con;
                    command1.CommandText = "select  board_name, description, category_name ,privacy  from  board b, category c where b.category_id=c.category_id and  board_name=:boardname";
                    command1.CommandType = CommandType.Text;
                    command1.Parameters.Add("boardname", boardName);
                    OracleDataReader reader1 = command1.ExecuteReader();
                    if (reader1.Read())
                    {
                         category_combx.SelectedItem = reader1[2].ToString();
                         BoardName_txt.Text = reader1[0].ToString();
                         Description_txt.Text = reader1[1].ToString();
                         if (reader1[3].ToString() == "1")
                              private_Checkbox.IsChecked = true;

                    }
                    reader1.Close();
                    con.Close();
               }
          }

          private void Create_Board()
          {
               con.Open();
               int userId = Account.UserID;
               string BoardName = BoardName_txt.Text.ToString().Trim();
               string description = Description_txt.Text.ToString();
               string board_Category = category_combx.SelectedItem.ToString();
               int private_board = (private_Checkbox.IsChecked.Value ? 1 : 0);

               if (BoardName == "" || description == "" || board_Category == "")
                    MessageBox.Show("Please Fill All Fields");
               else
               {
                    OracleCommand command = new OracleCommand();
                    command.Connection = con;
                    command.CommandText = "select count(*) from board  where board_name= :boardname and user_id= :userid";
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("boardname", BoardName);
                    command.Parameters.Add("userid", Account.UserID);
                    OracleDataReader reader = command.ExecuteReader();
                    if (reader.Read() && reader[0].ToString() != "0")
                         MessageBox.Show("Name Already Exist.");
                    else
                    {
                         OracleCommand command1 = new OracleCommand();
                         command1.Connection = con;
                         command1.CommandText = "select category_id from category  where category_name= :categoryname";
                         command1.CommandType = CommandType.Text;
                         command1.Parameters.Add("categoryname", category_combx.SelectedItem.ToString());
                         OracleDataReader reader1 = command1.ExecuteReader();
                         int category_id = -1;
                         if (reader1.Read())
                              category_id = Convert.ToInt32(reader1[0]);

                        
                         /////////////////////////// b. 3. insert //////////////////////////////
                         string command2 = "select * from board ";
                         OracleDataAdapter adapter = new OracleDataAdapter(command2, FixedValues.ordb);
                         OracleCommandBuilder builder;
                         DataSet set = new DataSet();
                         adapter.Fill(set);

                         DataRow row = set.Tables[0].NewRow();
                         row["user_id"] = Account.UserID;
                         row["board_name"] = BoardName;
                         row["category_id"] = category_id;
                         row["description"] = description;
                         row["privacy"] = private_board.ToString();
                         set.Tables[0].Rows.Add(row);
                         builder = new OracleCommandBuilder(adapter);
                         adapter.Update(set.Tables[0]);

                         Close();
                    }

               }
               con.Close();
          }

          private void Update_Board()
          {
               con.Open();
               int userId = Account.UserID;
               string BoardName = BoardName_txt.Text.ToString().Trim();
               string description = Description_txt.Text.ToString();
               string board_Category = category_combx.SelectedItem.ToString();
               int private_board = (private_Checkbox.IsChecked.Value ? 1 : 0);

               OracleCommand command1 = new OracleCommand();
               command1.Connection = con;
               command1.CommandText = "select category_id from category  where category_name= :categoryname";
               command1.CommandType = CommandType.Text;
               command1.Parameters.Add("categoryname", category_combx.SelectedItem.ToString());
               OracleDataReader reader1 = command1.ExecuteReader();
               int category_id = -1;
               if (reader1.Read())
                    category_id = Convert.ToInt32(reader1[0]);
               /////////////////////////// A. 6. update //////////////////////////////
               OracleCommand command2 = new OracleCommand();
               command2.Connection = con;
               command2.CommandText = "update_board";
               command2.CommandType = CommandType.StoredProcedure;
               command2.Parameters.Add("boardName", boardName);
               command2.Parameters.Add("description_board", description);
               command2.Parameters.Add("categoryId", category_id);
               command2.Parameters.Add("N_privacy", private_board);
               command2.ExecuteNonQuery();
               con.Close();

               Close();
          }

          

     }
}
