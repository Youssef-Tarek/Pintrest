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
     /// Interaction logic for categoryUserControl.xaml
     /// </summary>
     public partial class CategoryUserControl : UserControl
     {
          public Boolean btn_clicked;
          public CategoryUserControl()
          {
               InitializeComponent();
               btn_clicked = false;
          }

          private void categoryButton_Click(object sender, RoutedEventArgs e)
          {
               //changing the look of button on clicking .... Don't change
               if (btn_clicked)
               {
                    categoryButton.Opacity = 1;
                    btn_clicked = false;
                    Categories_user_control_.selectedItems.Remove(categoryButton.Content.ToString());
               }
               else
               {
                    categoryButton.Opacity = 0.6;
                    btn_clicked = true;
                    Categories_user_control_.selectedItems.Add(categoryButton.Content.ToString());

               }
          }

     }
}
