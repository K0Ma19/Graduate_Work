using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp3
{   
    public partial class WindowCreateElements : Window
    {
        public bool value = new bool();
        public string Var;

        public String NewNameElements { get; private set; }

        public String NewQuantity { get; private set; }

        public String NewDateElements { get; private set; }

        public String NewSupplier { get; private set; }

        public String NewPrice { get; private set; }
        public String NewSalePrice { get; private set; }

        public String NewNameObject { get; private set; }

        public WindowCreateElements()
        {
            InitializeComponent();

        }  

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            

            if (nameText.Text == String.Empty || 
                quantityText.Text == String.Empty || 
                dateNew.Text == String.Empty || 
                SupplierText.Text == String.Empty || 
                PriceText.Text == String.Empty || 
                ObjectName.Text == String.Empty || 
                SaleText.Text == String.Empty)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            else
            {
                value = true;
                NewNameElements = nameText.Text;
                NewQuantity = quantityText.Text;
                NewDateElements = Convert.ToString(dateNew);
                NewSupplier = SupplierText.Text;
                NewPrice = PriceText.Text;
                NewNameObject = ObjectName.Text;
                NewSalePrice = SaleText.Text;
            }

            

            Window.GetWindow(this).Close();
            
        }

        private void TableName_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            Var = selectedItem.Content.ToString();
        }
    }
}
