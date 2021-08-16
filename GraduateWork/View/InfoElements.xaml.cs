using System.Windows;
using System.Windows.Controls;

namespace WpfApp3.View
{
    public partial class InfoElements : Window
    {

        public string NTable;
        public bool RemainsValue = new bool();
        //public bool ProductionValue = new bool();
        //public bool SaleValue = new bool();
        public bool value = new bool();

        public MainWindow mainWindow = new MainWindow();

        public InfoElements()
        {
            InitializeComponent();

            
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.elementsGrid.Items.Refresh();
            mainWindow.EntrancesGrid.Items.Refresh();
            mainWindow.RemainsGrid.Items.Refresh();
            mainWindow.ProductionGrid.Items.Refresh();
            mainWindow.SaleGrid.Items.Refresh();

            Window.GetWindow(this).Close();
        }

        private void SelectedTable_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            NTable = selectedItem.Content.ToString();

            switch(selectedItem.Content.ToString())
            {
                case "Производство":
                    if (RemainsValue == true)
                    {
                        RemainQuantity.Visibility = Visibility.Visible;
                        NameObjectRemain.Visibility = Visibility.Visible;
                        PriceRemain.Visibility = Visibility.Collapsed;
                    }
                    break;
                case "Продажа":
                    if (RemainsValue == true)
                    {
                        RemainQuantity.Visibility = Visibility.Visible;
                        PriceRemain.Visibility = Visibility.Visible;
                        NameObjectRemain.Visibility = Visibility.Collapsed;
                    }
                    break;
                default:
                    RemainQuantity.Visibility = Visibility.Collapsed;
                    NameObjectRemain.Visibility = Visibility.Collapsed;
                    PriceRemain.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void SaveTable_Click(object sender, RoutedEventArgs e)
        {
            value = true;
            mainWindow.elementsGrid.Items.Refresh();
            mainWindow.EntrancesGrid.Items.Refresh();
            mainWindow.RemainsGrid.Items.Refresh();
            mainWindow.ProductionGrid.Items.Refresh();
            mainWindow.SaleGrid.Items.Refresh();

            Window.GetWindow(this).Close();
        }
    }
}
