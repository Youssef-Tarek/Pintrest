using Microsoft.Win32;
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
using System.Windows.Shapes;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Data;

namespace Pinterest_V2
{
     /// <summary>
     /// Interaction logic for myPins.xaml
     /// </summary>
     public partial class CreatePin : Window
     {
          OracleConnection con;
          byte[] imageData;
          public Boolean Edit = false;
          public int pin_id_edit;
          public CreatePin()
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

          private void browse_btn_Click(object sender, RoutedEventArgs e)
          {
               Account newAccount = new Account();


               OpenFileDialog ofd = new OpenFileDialog();
               ofd.ShowDialog();
               path_txt.Text = ofd.FileName;
               Pin_pic.Source = new BitmapImage(new Uri(path_txt.Text));
               imageData = newAccount.Getimage(path_txt.Text);
          }

          private void Done_Click(object sender, RoutedEventArgs e)
          {

               if (Edit)
                    update_pin();
               else
                    Create_Pin();
          }


          private void Grid_Loaded(object sender, RoutedEventArgs e)
          {
               if (Edit)
               {
                    Title.Source = new BitmapImage(new Uri("/images/editpin.png", UriKind.Relative));
                    Done_btn.Content = "Edit";


                    con.Open();
                    OracleCommand command = new OracleCommand();
                    command.Connection = con;
                    command.CommandText = "select  pin_name, pin_image, description, board_name  from  pin where pin_id=:pinId ";
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("pinId", pin_id_edit);
                    OracleDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                         board_combx.SelectedItem = reader[3].ToString();
                         PinName_txt.Text = reader[0].ToString();
                         Description_txt.Text = reader[2].ToString();
                         byte[] im = (byte[])reader[1];
                         Pin_pic.Source = FixedValues.LoadImage(im);
                         imageData = im;
                    }
                    reader.Close();
                    con.Close();


               }
          }
          private void Create_Pin()
          {
               int userId = Account.UserID;
               string BoardName = board_combx.SelectedItem.ToString().Trim();
               string PinName = PinName_txt.Text;
               string description = Description_txt.Text.ToString();
               if (PinName_txt.Text == "" || Description_txt.ToString() == "" || board_combx.SelectedItem.ToString() == "")
                    MessageBox.Show("Please Fill All Fields");
               else
               {
                    con.Open();
                    OracleCommand command = new OracleCommand();
                    command.Connection = con;
                    command.CommandText = "insert into pin values(pin_id.nextval, :pinName, :pinImage, :descrption, :boardname, :user_id, :pin_Date) ";
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("pinName", PinName);
                    command.Parameters.Add("pinimage", imageData);
                    command.Parameters.Add("descrption", description);
                    command.Parameters.Add("boardname", BoardName);
                    command.Parameters.Add("user_id", userId);
                    command.Parameters.Add("pin_date", DateTime.Now);
                    command.ExecuteNonQuery();
                    con.Close();

                    Close();
               }
          }
          private void update_pin()
          {
               int userId = Account.UserID;
               string BoardName = board_combx.SelectedItem.ToString().Trim();
               string PinName = PinName_txt.Text;
               string description = Description_txt.Text.ToString();
               if (PinName_txt.Text == "" || Description_txt.ToString() == "" || board_combx.SelectedItem.ToString() == "")
                    MessageBox.Show("Please Fill All Fields");
               else
               {
                    // con.Open();
                    ///////////////////////// b. 3. update ////////////////////////////////////
                    ////////////////////////////////////////////////////////////////
                    string command = "select * from pin";
                    OracleDataAdapter adapter = new OracleDataAdapter(command, FixedValues.ordb);
                    OracleCommandBuilder builder;
                    DataSet set = new DataSet();
                    adapter.Fill(set);
                    DataRow row = set.Tables[0].NewRow();
                    for (int i = 0; i < set.Tables[0].Rows.Count; i++)
                         if (set.Tables[0].Rows[i].ItemArray[0].ToString() == pin_id_edit.ToString())
                         {
                              row = set.Tables[0].Rows[i];
                              break;
                         }
                    row["pin_name"] = PinName;
                    row["pin_image"] = imageData;
                    row["description"] = description;
                    row["board_name"] = BoardName;
                    builder = new OracleCommandBuilder(adapter);
                    adapter.Update(set.Tables[0]);

                    ////////////////////////////////////////////////////////////////////////////////



                    Close();
               }
          }

          private void Close_icon(object sender, RoutedEventArgs e)
          {
               Close();
          }
     }
}
