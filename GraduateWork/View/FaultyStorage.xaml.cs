using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using WpfApp3.Models;

namespace WpfApp3.View
{
   
    public partial class FaultyStorage : Window
    {
        ElementsStorage db;
        Storage storage = new Storage();
        public FaultyStorage()
        {
            InitializeComponent();

            db = new ElementsStorage();

            db.Element.Load();
            foreach(Storage x in db.Element)
            {
                if (x.Faulty <= 0)
                {
                    
                }
                else
                {
                    faultyGrid.ItemsSource = db.Element.Local.ToBindingList();
                }
            }
            
            this.Closing += MainWindow_Closing;

            faultyGrid.Items.Refresh();
            db.SaveChanges();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }
    }
}
