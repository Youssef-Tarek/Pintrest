using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
     /// Interaction logic for Categories_user_control_.xaml
     /// </summary>
     public partial class Categories_user_control_ : Window
     {
          public static List<string> selectedItems = new List<string>();
          public Categories_user_control_()
          {
               InitializeComponent();
          }

          private void Grid_Loaded(object sender, RoutedEventArgs e)
          {

               OracleConnection con = new OracleConnection(FixedValues.ordb);
               con.Open();

               //initial coordinates... don't change
               double initLeft = 50;
               double initTop = 10;
               double initRight = 510;
               double initBottom = 10;

               if (Account.FirstLogin != 0)
               {
                    selectedItems.Clear();
                    OracleCommand command2 = new OracleCommand();
                    command2.Connection = con;
                    ////////////////////// A. 2. /////////////////////////
                    command2.CommandText = "select category_name from category c, user_category uc where c.category_id = uc.category_id and uc.user_id = :userId";
                    command2.CommandType = CommandType.Text;
                    command2.Parameters.Add("userId", Account.UserID);
                    OracleDataReader reader2 = command2.ExecuteReader();
                    while (reader2.Read())
                         selectedItems.Add(reader2[0].ToString());

                    reader2.Close();
                    //change this into number of remaining categories that user didn't choose
               }

               OracleCommand command = new OracleCommand();
               command.Connection = con;
               ////////////////////// A. 1. /////////////////////////
               command.CommandText = "select category_name, Category_Picture from category";
               command.CommandType = CommandType.Text;
               OracleDataReader reader = command.ExecuteReader();

               double d;
               int categoriesCounter = 0;
               int i = 0;
               categoryPanel.Children.Clear();
               while (reader.Read())
               {
                    if ((i + 1) % 4 == 0)
                    {
                         initLeft = 50;
                         initRight = 510;
                         i = 0;
                    }

                    if (i >= 2)
                    {
                         d = 1;
                    }

                    else
                         d = i;

                    byte[] image = (byte[])reader[1];

                    Pinterest_V2.UserControls.CategoryUserControl categoriesControl = new UserControls.CategoryUserControl();
                    BitmapImage bitmapimage = FixedValues.LoadImage(image);
                    ImageBrush buttonBrush = new ImageBrush(bitmapimage);
                    if (selectedItems.Contains(reader[0].ToString()))
                    {
                         buttonBrush.Opacity = 0.6;
                         categoriesControl.btn_clicked = true;
                    }

                    categoriesControl.categoryButton.Background = buttonBrush;
                    categoriesControl.categoryButton.Foreground = Brushes.White;
                    categoriesControl.categoryButton.Content = reader[0].ToString();



                    categoriesControl.Margin = new Thickness(initLeft + (i * 230), initTop - (d * 440), initRight - (i * 220), initBottom - 220 * d);
                    categoryPanel.Children.Add(categoriesControl);

                    categoriesCounter++;
                    i++;
               }
               reader.Close();
               con.Close();
          }

          private void DoneButton_Click(object sender, RoutedEventArgs e)
          {

               /////////////////////////// insert categories into database /////////////////////////////
               ///
               OracleConnection con = new OracleConnection(FixedValues.ordb);
               con.Open();
               OracleCommand command = new OracleCommand();
               command.Connection = con;
               /////////////////////////// A.3. Delete  /////////////////////////////
               command.CommandText = "delete from user_category where user_id= :userID";
               command.CommandType = CommandType.Text;
               command.Parameters.Add("userID", Account.UserID);
               command.ExecuteNonQuery();
               ///////////////////////////////////////////////////////////////
               for (int i = 0; i < selectedItems.Count; i++)
               {

                    OracleCommand command2 = new OracleCommand();
                    command2.Connection = con;
                    ///////////////////////////  A. 4. /////////////////////////////
                    command2.CommandText = "get_CatgoryID";
                    command2.CommandType = CommandType.StoredProcedure;
                    command2.Parameters.Add("categoryName", selectedItems[i]);
                    command2.Parameters.Add("category_ID", OracleDbType.Int32, ParameterDirection.Output);
                    command2.ExecuteNonQuery();
                    int category_id = Convert.ToInt32(command2.Parameters[1].Value.ToString());
                    ////////////////////// A. 3. insert /////////////////////////
                    OracleCommand command3 = new OracleCommand();
                    command3.Connection = con;
                    command3.CommandText = "insert into USER_CATEGORY values( :userid, :category_id)";
                    command3.CommandType = CommandType.Text;
                    command3.Parameters.Add("userid", Account.UserID);
                    command3.Parameters.Add("category_id", category_id);
                    command3.ExecuteNonQuery();
               }
               con.Close();
               if (Account.FirstLogin == 0)
               {
                    Home home1 = new Home();
                    Close();
                    home1.Show();
                    Account.FirstLogin = 1;
               }
               else
                    Close();



          }


     }
}
