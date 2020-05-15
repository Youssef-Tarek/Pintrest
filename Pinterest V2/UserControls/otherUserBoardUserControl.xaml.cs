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
     /// Interaction logic for otherUserBoardUserControl.xaml
     /// </summary>
     public partial class otherUserBoardUserControl : UserControl
     {
          public int board_USERID;
          public otherUserBoardUserControl()
          {
               InitializeComponent();
          }

          private void boardButton_Click(object sender, RoutedEventArgs e)
          {
               OthersPins otherUserPins = new OthersPins();
               otherUserPins.boardNameLabel.Content = this.boardButton.Content;
               otherUserPins.board_USERID = this.board_USERID;
               otherUserPins.board_Name = this.boardButton.Content.ToString();
               otherUserPins.ShowDialog();
          }
     }
}
