using CrystalDecisions.Shared;
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

namespace Pinterest_V2
{
    /// <summary>
    /// Interaction logic for DataDesplay.xaml
    /// </summary>
    public partial class DataDisplay : Window
    {
        public DataDisplay()
        {
            InitializeComponent();
        }
        Pinterest_V2.Pin1  pinData;
        Category cat_data;
        public bool pin = true;
        private void Enter_btn_Click(object sender, RoutedEventArgs e)
        {
            if (pin) {
                pinData.SetParameterValue(0, board_cmb.Text);
                pinData.SetParameterValue(1, date_txt.Text);
                crystal.ViewerCore.ReportSource = pinData;
            }
            else
            {
                cat_data.SetParameterValue(0, category_cmb.Text);
                cat_data.SetParameterValue(1, privacy_txt.Text);
                crystal.ViewerCore.ReportSource = cat_data;
            }

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (pin) {
                pinData = new Pinterest_V2.Pin1();
                foreach (ParameterDiscreteValue v in pinData.ParameterFields[0].DefaultValues)
                    board_cmb.Items.Add(v.Value);
                board_cmb.Visibility = Visibility.Visible;
                category_cmb.Visibility = Visibility.Hidden;
                date_txt.Visibility = Visibility.Visible;
                privacy_txt.Visibility = Visibility.Hidden;

            }
            else
            {

                cat_data = new Category();
                foreach (ParameterDiscreteValue v in cat_data.ParameterFields[0].DefaultValues)
                    category_cmb.Items.Add(v.Value);
                board_cmb.Visibility = Visibility.Hidden;
                category_cmb.Visibility = Visibility.Visible;
                date_txt.Visibility = Visibility.Hidden;
                privacy_txt.Visibility = Visibility.Visible;
            }
        }
    }
}
