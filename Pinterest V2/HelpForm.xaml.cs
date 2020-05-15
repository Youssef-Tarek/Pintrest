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
    /// Interaction logic for HelpForm.xaml
    /// </summary>
    public partial class HelpForm : Window
    {
        OracleDataAdapter helpDataAdapter;
        DataSet helperDataSet;

        public HelpForm()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            /////////////////// B.2. ///////////////////////
             string command = "select * from help";

             helpDataAdapter = new OracleDataAdapter(command, FixedValues.ordb);
             helperDataSet = new DataSet();
             helpDataAdapter.Fill(helperDataSet);

             int questionsNo = 9;

             for (int i = 0; i < questionsNo; i++)
             {
                 Pinterest_V2.UserControls.helpUserControl helpUC = new UserControls.helpUserControl();

                 helpUC.Question_txt.Text = helperDataSet.Tables[0].Rows[i][0].ToString();
                 helpUC.Answer_txt.Text = helperDataSet.Tables[0].Rows[i][1].ToString();

                 helpUC.Margin = new Thickness(0, 30, 0, 0);

                 helpPanel.Children.Add(helpUC);
             }
        }
    }
}
