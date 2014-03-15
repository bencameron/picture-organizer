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

namespace PictureOrganizer.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void organizeFiles_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var mgr = new FileManager();
                mgr.OrganizeFiles(this.sourceFolder.Text, this.destBaseFolder.Text);

                MessageBox.Show("Success!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chooseSourceFolder_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void chooseDestFolder_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
