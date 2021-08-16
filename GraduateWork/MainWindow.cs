using System;
using System.Windows;
using WpfApp3.Models;
using WpfApp3.View;
using System.Data.Entity;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace WpfApp3
{
    public partial class MainWindow : System.Windows.Window
    {
        ElementsStorage db;
        private String _startDate { get; set; }

        private String _endDate { get; set; }

        public string StorageName { get; private set; }

        public string StorageItems { get; private set; }

        private bool _isToggle;

        public MainWindow()
        {
            InitializeComponent();

            #region Загрузка базы данных
            db = new ElementsStorage();

            db.Element.Load();
            db.Entrances.Load();
            db.Remains.Load();
            db.Productions.Load();
            db.Sales.Load();
            elementsGrid.ItemsSource = db.Element.Local.ToBindingList();
            EntrancesGrid.ItemsSource = db.Entrances.Local.ToBindingList();
            RemainsGrid.ItemsSource = db.Remains.Local.ToBindingList();
            ProductionGrid.ItemsSource = db.Productions.Local.ToBindingList();
            SaleGrid.ItemsSource = db.Sales.Local.ToBindingList();
            this.Closing += MainWindow_Closing;
            #endregion


        } 
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        private void AddElement_Click(object sender, RoutedEventArgs e)
        {
            WindowCreateElements createEltments = new WindowCreateElements();
            createEltments.ShowDialog();

            if (createEltments.value == true)
            {
                db.Element.Add(new Storage()
                {
                    Name = createEltments.NewNameElements,
                    Quantity = Convert.ToInt32(createEltments.NewQuantity),
                    Date = Convert.ToDateTime(createEltments.NewDateElements),
                    Supplier = (createEltments.NewSupplier),
                    Price = Convert.ToInt32(createEltments.NewPrice),
                    NameObject = createEltments.NewNameObject,
                    SalePrice = Convert.ToInt32(createEltments.NewSalePrice)
                });

                switch (createEltments.Var)
                {
                    case "Поступление":
                        
                        db.Entrances.Add(new Entrance()
                        {
                            
                            Name = createEltments.NewNameElements,
                            Quantity = Convert.ToInt32(createEltments.NewQuantity),
                            Supplier = (createEltments.NewSupplier),
                            Price = Convert.ToInt32(createEltments.NewPrice)
                        });
                        
                        break;
                    case "Остатки":
                        db.Remains.Add(new Remains()
                        {
                            Name = createEltments.NewNameElements,
                            Quantity = Convert.ToInt32(createEltments.NewQuantity),
                             NameObject = createEltments.NewNameObject
                        });
                        
                        break;
                    case "Производство":
                            db.Productions.Add(new Production()
                            {
                                Name = createEltments.NewNameElements,
                                Quantity = Convert.ToInt32(createEltments.NewQuantity),
                                NameObject = createEltments.NewNameObject
                            });
                        break;
                    case "Продажа":
                        db.Sales.Add(new Sale()
                        {
                            Name = createEltments.NewNameElements,
                            Quantity = Convert.ToInt32(createEltments.NewQuantity),
                            SalePrice = Convert.ToInt32(createEltments.NewSalePrice)
                        });
                       
                        break;
                    default:
                        MessageBox.Show("Вы не выбрали таблицу либо данной таблицы не существует");
                        break;
                }
            }
            elementsGrid.Items.Refresh();
            EntrancesGrid.Items.Refresh();
            RemainsGrid.Items.Refresh();
            ProductionGrid.Items.Refresh();
            SaleGrid.Items.Refresh();
            db.SaveChanges();

        }

        private void elementsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void EntrancesGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void RemainsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void ProductionGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void SaleGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            elementsGrid.Items.Refresh();
            EntrancesGrid.Items.Refresh();
            RemainsGrid.Items.Refresh();
            ProductionGrid.Items.Refresh();
            SaleGrid.Items.Refresh();

            db.SaveChanges();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((ControlTab.SelectedItem as TabItem).Header)
            {
                case "Все":
                    if (elementsGrid.SelectedItems.Count > 0)
                    {
                        for (int i = 0; i < elementsGrid.SelectedItems.Count; i++)
                        {
                            Storage storage = elementsGrid.SelectedItems[i] as Storage;
                            if (storage != null)
                            {
                                db.Element.Remove(storage);
                                foreach (Entrance entrance in db.Entrances)
                                {
                                if (entrance.Name == storage.Name)
                                {

                                    db.Entrances.Remove(entrance);
                                }
                                    
                                }

                                foreach (Remains remains in db.Remains)
                                {
                                    if (remains.Name == storage.Name)
                                    {
                                        db.Remains.Remove(remains);
                                    }
                                }

                                foreach (Production production in db.Productions)
                                {
                                    if (production.Name == storage.Name)
                                    {
                                        db.Productions.Remove(production);
                                    }
                                }

                                foreach (Sale sale in db.Sales)
                                {
                                    if (sale.Name == storage.Name)
                                    {
                                        db.Sales.Remove(sale);
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Не выбранна строка");
                    }
                    break;
                case "Поступление":
                    if (EntrancesGrid.SelectedItems.Count > 0)
                    {
                        for (int i = 0; i < EntrancesGrid.SelectedItems.Count; i++)
                        {
                            Entrance entrance = EntrancesGrid.SelectedItems[i] as Entrance;
                            if (entrance != null)
                            {
                                db.Entrances.Remove(entrance);
                                
                            }


                        }
                    }
                    else
                    {
                        MessageBox.Show("Не выбранна строка");
                    }
                    break;
                case "Остатки":
                    if (RemainsGrid.SelectedItems.Count > 0)
                    {
                        for (int i = 0; i < RemainsGrid.SelectedItems.Count; i++)
                        {
                            Remains remains = RemainsGrid.SelectedItems[i] as Remains;
                            if (remains != null)
                            {
                                db.Remains.Remove(remains);
                               
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не выбранна строка");
                    }
                    break;
                case "Производство":
                    if (ProductionGrid.SelectedItems.Count > 0)
                    {
                        for (int i = 0; i < ProductionGrid.SelectedItems.Count; i++)
                        {
                            Production production = ProductionGrid.SelectedItems[i] as Production;
                            if (production != null)
                            {
                                db.Productions.Remove(production);
                               
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не выбранна строка");
                    }
                    break;
                case "Продажа":
                    if (SaleGrid.SelectedItems.Count > 0)
                    {
                        for (int i = 0; i < SaleGrid.SelectedItems.Count; i++)
                        {
                            Sale sale = SaleGrid.SelectedItems[i] as Sale;
                            if (sale != null)
                            {
                                db.Sales.Remove(sale);
                                
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не выбранна строка");
                    }
                    break;
                default:
                    MessageBox.Show("Не выбранна таблица");
                    break;
            }
        elementsGrid.Items.Refresh();
        EntrancesGrid.Items.Refresh();
        RemainsGrid.Items.Refresh();
        ProductionGrid.Items.Refresh();
        SaleGrid.Items.Refresh();
        db.SaveChanges();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            brd.Width = 0;
        }

        private void DateRange_Click(object sender, RoutedEventArgs e)
        {
            #region Анимация
            DoubleAnimation NewAnimation = new DoubleAnimation();
            if (!_isToggle)
            {
                NewAnimation.To = 140;
                NewAnimation.Duration = TimeSpan.FromSeconds(1);
                brd.BeginAnimation(System.Windows.Controls.Border.WidthProperty, NewAnimation);
                _isToggle = true;
            }
            else
            {
                NewAnimation.To = 0;
                NewAnimation.Duration = TimeSpan.FromSeconds(1);
                brd.BeginAnimation(System.Windows.Controls.Border.WidthProperty, NewAnimation);
                _isToggle = false;
            }
            #endregion

        }
        private void SortRange_Click(object sender, RoutedEventArgs e)
        {
            switch ((ControlTab.SelectedItem as TabItem).Header)
            {
                case "Все":
                    if (StartRange.SelectedDate != null && EndRange.SelectedDate != null)
                    {
                        var query = from element in db.Element
                                    orderby element.Date
                                    select new { element.ID, element.Name, element.Quantity, element.Date };
                        elementsGrid.ItemsSource = query.ToList();
                        elementsGrid.ItemsSource = query.Where(a => a.Date >= StartRange.SelectedDate && a.Date <= EndRange.SelectedDate).ToList();
                    }
                    else
                    {
                        elementsGrid.ItemsSource = db.Element.Local.ToBindingList();
                    }
                    break;
                case "Поступление":
                    MessageBox.Show("Сортировка происходит только в таблице со всеми элементами");
                    break;
                case "Остатки":
                    MessageBox.Show("Сортировка происходит только в таблице со всеми элементами");
                    break;
                case "Производство":
                    MessageBox.Show("Сортировка происходит только в таблице со всеми элементами");
                    break;
                case "Продажа":
                    MessageBox.Show("Сортировка происходит только в таблице со всеми элементами");
                    break;
                default:
                    MessageBox.Show("Не выбранна таблица");
                    break;
            }

            DoubleAnimation NewAnimation = new DoubleAnimation();
            NewAnimation.To = 0;
            NewAnimation.Duration = TimeSpan.FromSeconds(1);
            brd.BeginAnimation(System.Windows.Controls.Border.WidthProperty, NewAnimation);
            _isToggle = false;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            elementsGrid.ItemsSource = db.Element.Local.ToBindingList();


            DoubleAnimation NewAnimation = new DoubleAnimation();
            NewAnimation.To = 0;
            NewAnimation.Duration = TimeSpan.FromSeconds(1);
            brd.BeginAnimation(System.Windows.Controls.Border.WidthProperty, NewAnimation);
            _isToggle = false;
        }

        #region ExportToExel
        private void SaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet = (Worksheet)workbook.Sheets[1];

            switch ((ControlTab.SelectedItem as TabItem).Header)
            {
                case "Все":
                    for (int j = 0; j < elementsGrid.Columns.Count; j++)
                    {
                        Range myRange = (Range)sheet.Cells[1, j + 1];
                        sheet.Columns[j + 1].NumberFormat = "@";
                        sheet.Cells[1, j + 1].Font.Bold = true;
                        sheet.Columns[j + 1].ColumnWidth = 15;
                        myRange.Value2 = elementsGrid.Columns[j].Header;
                    }
                    for (int i = 0; i < elementsGrid.Columns.Count; i++)
                    {
                        for (int j = 0; j < elementsGrid.Items.Count; j++)
                        {
                            TextBlock b = elementsGrid.Columns[i].GetCellContent(elementsGrid.Items[j]) as TextBlock;
                            Microsoft.Office.Interop.Excel.Range myRange = (Range)sheet.Cells[j + 2, i + 1];
                            myRange.Value2 = b.Text;

                        }
                    }
                    break;
                case "Поступление":
                    for (int j = 0; j < EntrancesGrid.Columns.Count; j++)
                    {
                        Range myRange = (Range)sheet.Cells[1, j + 1];
                        sheet.Columns[j + 1].NumberFormat = "@";
                        sheet.Cells[1, j + 1].Font.Bold = true;
                        sheet.Columns[j + 1].ColumnWidth = 15;
                        myRange.Value2 = EntrancesGrid.Columns[j].Header;
                    }
                    for (int i = 0; i < EntrancesGrid.Columns.Count; i++)
                    {
                        for (int j = 0; j < EntrancesGrid.Items.Count; j++)
                        {
                            TextBlock b = EntrancesGrid.Columns[i].GetCellContent(EntrancesGrid.Items[j]) as TextBlock;
                            Microsoft.Office.Interop.Excel.Range myRange = (Range)sheet.Cells[j + 2, i + 1];
                            myRange.Value2 = b.Text;
                        }
                    }
                    break;
                case "Остатки":
                    for (int j = 0; j < RemainsGrid.Columns.Count; j++)
                    {
                        Range myRange = (Range)sheet.Cells[1, j + 1];
                        sheet.Columns[j + 1].NumberFormat = "@";
                        sheet.Cells[1, j + 1].Font.Bold = true;
                        sheet.Columns[j + 1].ColumnWidth = 15;
                        myRange.Value2 = RemainsGrid.Columns[j].Header;
                    }
                    for (int i = 0; i < RemainsGrid.Columns.Count; i++)
                    {
                        for (int j = 0; j < RemainsGrid.Items.Count; j++)
                        {
                            TextBlock b = RemainsGrid.Columns[i].GetCellContent(RemainsGrid.Items[j]) as TextBlock;
                            Microsoft.Office.Interop.Excel.Range myRange = (Range)sheet.Cells[j + 2, i + 1];
                            myRange.Value2 = b.Text;

                        }
                    }
                    break;
                case "Производство":
                    for (int j = 0; j < ProductionGrid.Columns.Count; j++)
                    {
                        Range myRange = (Range)sheet.Cells[1, j + 1];
                        sheet.Columns[j + 1].NumberFormat = "@";
                        sheet.Cells[1, j + 1].Font.Bold = true;
                        sheet.Columns[j + 1].ColumnWidth = 15;
                        myRange.Value2 = ProductionGrid.Columns[j].Header;
                    }
                    for (int i = 0; i < ProductionGrid.Columns.Count; i++)
                    {
                        for (int j = 0; j < ProductionGrid.Items.Count; j++)
                        {
                            TextBlock b = ProductionGrid.Columns[i].GetCellContent(ProductionGrid.Items[j]) as TextBlock;
                            Microsoft.Office.Interop.Excel.Range myRange = (Range)sheet.Cells[j + 2, i + 1];
                            myRange.Value2 = b.Text;

                        }
                    }
                    break;
                case "Продажа":
                    for (int j = 0; j < SaleGrid.Columns.Count; j++)
                    {
                        Range myRange = (Range)sheet.Cells[1, j + 1];
                        sheet.Columns[j + 1].NumberFormat = "@";
                        sheet.Cells[1, j + 1].Font.Bold = true;
                        sheet.Columns[j + 1].ColumnWidth = 15;
                        myRange.Value2 = SaleGrid.Columns[j].Header;
                    }
                    for (int i = 0; i < SaleGrid.Columns.Count; i++)
                    {
                        for (int j = 0; j < SaleGrid.Items.Count; j++)
                        {
                            TextBlock b = SaleGrid.Columns[i].GetCellContent(SaleGrid.Items[j]) as TextBlock;
                            Microsoft.Office.Interop.Excel.Range myRange = (Range)sheet.Cells[j + 2, i + 1];
                            myRange.Value2 = b.Text;

                        }
                    }
                    break;
                default:
                    MessageBox.Show("Не выбранна таблица");
                    break;
            }
        }
        #endregion


        #region Info and edit elemets
        private void Info_Click(object sender, RoutedEventArgs e)
        {
            Storage storage = elementsGrid.SelectedItem as Storage;

            InfoElements infoElements = new InfoElements();

            if (elementsGrid.SelectedItem != null)
            {
                foreach (Entrance entrance1 in db.Entrances)
                {
                    if (entrance1.Name == storage.Name)
                    {
                        infoElements.NameTable.Text = "Поступление";
                    }
                }

                foreach (Remains remains1 in db.Remains)
                {
                    if (remains1.Name == storage.Name)
                    {
                        infoElements.NameTable.Text = "Остатки";
                        infoElements.RemainsValue = true;
                    }
                }

                foreach (Production production1 in db.Productions)
                {
                    if (production1.Name == storage.Name)
                    {
                        infoElements.NameTable.Text = "Производство";
                    }
                }

                foreach (Sale sale1 in db.Sales)
                {
                    if (sale1.Name == storage.Name)
                    {
                        infoElements.NameTable.Text = "Продажа";
                    }
                }

            }

            if (elementsGrid.SelectedItem != null)
            {       
                infoElements.NameElemets.Text = storage.Name;

                infoElements.DateInfo.SelectedDate = storage.Date;

                infoElements.TextBoxQuantity.Text = Convert.ToString(storage.Quantity);
    
                infoElements.TextBoxWork.Text = Convert.ToString(storage.QuantityWork);

                infoElements.TextBoxFaulty.Text = Convert.ToString(storage.Faulty);

                infoElements.NameSupplierBox.Text = Convert.ToString(storage.Supplier);

                infoElements.PriceBox.Text = Convert.ToString(storage.Price);

                infoElements.NameObjectBox.Text = Convert.ToString(storage.NameObject);

                infoElements.SalePriceBox.Text = Convert.ToString(storage.SalePrice);


                infoElements.ShowDialog();


                


                foreach (Entrance entrance in db.Entrances)
                {
                    if (entrance.Name == storage.Name)
                    {
                        entrance.Name = infoElements.NameElemets.Text;
                        entrance.Quantity = Convert.ToInt32(infoElements.TextBoxQuantity.Text);
                        entrance.Supplier = infoElements.NameSupplierBox.Text;
                        entrance.Price = Convert.ToInt32(infoElements.PriceBox.Text);
                    }
                }

                foreach (Remains remains in db.Remains)
                {
                    if (remains.Name == storage.Name)
                    {
                        remains.Name = infoElements.NameElemets.Text;
                        remains.Quantity = Convert.ToInt32(infoElements.TextBoxQuantity.Text);
                        remains.NameObject = infoElements.NameObjectBox.Text;
                    }
                }

                foreach (Production production in db.Productions)
                {
                    if (production.Name == storage.Name)
                    {
                        production.Name = infoElements.NameElemets.Text;
                        production.Quantity = Convert.ToInt32(infoElements.TextBoxQuantity.Text);
                        production.NameObject = infoElements.NameObjectBox.Text;
                    }
                }

                foreach (Sale sale in db.Sales)
                {
                    if (sale.Name == storage.Name)
                    {
                        sale.Name = infoElements.NameElemets.Text;
                        sale.Quantity = Convert.ToInt32(infoElements.TextBoxQuantity.Text);
                        sale.SalePrice = Convert.ToInt32(infoElements.SalePriceBox.Text);
                    }
                }
            }
            else
            {
            MessageBox.Show("Не выбран элемент");
            return;
            }


            storage.Name = infoElements.NameElemets.Text;
            storage.Date = Convert.ToDateTime(infoElements.DateInfo.SelectedDate);
            storage.Quantity = Convert.ToInt32(infoElements.TextBoxQuantity.Text);
            storage.QuantityWork = Convert.ToInt32(infoElements.TextBoxWork.Text);
            storage.Faulty = Convert.ToInt32(infoElements.TextBoxFaulty.Text);
            storage.Supplier = infoElements.NameSupplierBox.Text;
            storage.Price = Convert.ToInt32(infoElements.PriceBox.Text);
            storage.NameObject = infoElements.NameObjectBox.Text;
            storage.SalePrice = Convert.ToInt32(infoElements.SalePriceBox.Text);


            if (infoElements.value == true)
            {
                switch (infoElements.NTable)
                {
                    case "Поступление":
                        db.Entrances.Add(new Entrance()
                        {
                            Name = infoElements.NameElemets.Text,
                            Quantity = Convert.ToInt32(infoElements.TextBoxQuantity.Text),
                            Supplier = infoElements.NameSupplierBox.Text,
                            Price = Convert.ToInt32(infoElements.PriceBox.Text)
                        });
                        break;
                    case "Остатки":
                        db.Remains.Add(new Remains()
                        {
                            Name = infoElements.NameElemets.Text,
                            Quantity = Convert.ToInt32(infoElements.TextBoxQuantity.Text),
                            NameObject = infoElements.NameObjectBox.Text
                        });
                        break;
                    case "Производство":
                        db.Productions.Add(new Production()
                        {
                            Name = infoElements.NameElemets.Text,
                            Quantity = Convert.ToInt32(infoElements.TextBoxQuantity.Text),
                            NameObject = infoElements.NameObjectBox.Text
                        });
                        break;
                    case "Продажа":
                        db.Sales.Add(new Sale()
                        {
                            Name = infoElements.NameElemets.Text,
                            Quantity = Convert.ToInt32(infoElements.TextBoxQuantity.Text),
                            SalePrice = Convert.ToInt32(infoElements.SalePriceBox.Text)
                        });
                        break;
                    default:
                        MessageBox.Show("Вы не выбрали таблицу либо данной таблицы не существует");
                        break;
                }
                
               
               DeleteElement(infoElements.NameTable.Text);
            }

            elementsGrid.Items.Refresh();
            EntrancesGrid.Items.Refresh();
            RemainsGrid.Items.Refresh();
            ProductionGrid.Items.Refresh();
            SaleGrid.Items.Refresh();
            db.SaveChanges();
        }
        #endregion

        private void DeleteElement(string Name)
        {
            switch (Name)
            {
                case "Поступление":
                    for (int i = 0; i < elementsGrid.SelectedItems.Count; i++)
                    {
                        Storage storage = elementsGrid.SelectedItems[i] as Storage;
                        if (storage != null)
                        {

                            foreach (Entrance entrance in db.Entrances)
                            {
                                if (entrance.Name == storage.Name)
                                {
                                    db.Entrances.Remove(entrance);
                                }
                            }

                        }

                    }
                    break;
                case "Остатки":
                    for (int i = 0; i < elementsGrid.SelectedItems.Count; i++)
                    {
                        Storage storage = elementsGrid.SelectedItems[i] as Storage;
                        if (storage != null)
                        {
                            foreach (Remains remains in db.Remains)
                            {
                                if (remains.Name == storage.Name)
                                {
                                    db.Remains.Remove(remains);
                                }
                            }


                        }

                    }
                    break;
                case "Производство":
                    for (int i = 0; i < elementsGrid.SelectedItems.Count; i++)
                    {
                        Storage storage = elementsGrid.SelectedItems[i] as Storage;
                        if (storage != null)
                        {
                            foreach (Production production in db.Productions)
                            {
                                if (production.Name == storage.Name)
                                {
                                    db.Productions.Remove(production);
                                }
                            }
                        }

                    }
                    break;
                case "Продажа":
                    for (int i = 0; i < elementsGrid.SelectedItems.Count; i++)
                    {
                        Storage storage = elementsGrid.SelectedItems[i] as Storage;
                        if (storage != null)
                        {

                            foreach (Sale sale in db.Sales)
                            {
                                if (sale.Name == storage.Name)
                                {
                                    db.Sales.Remove(sale);
                                }
                            }
                        }

                    }
                    break;
                default:
                    MessageBox.Show("Не выбранна таблица");
                    break;
            }
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Storage> storageFilter = new List<Storage>();
            List<Entrance> entranceFilter = new List<Entrance>();
            List<Remains> remainsFilter = new List<Remains>();
            List<Production> productionFilter = new List<Production>();
            List<Sale> saleFilter = new List<Sale>();

            switch ((ControlTab.SelectedItem as TabItem).Header)
            {
                case "Все":
                    storageFilter.Clear();

                    if (SearchText.Text.Equals(""))
                    {
                        storageFilter.AddRange(db.Element);
                    }
                    else
                    {
                        foreach (Storage elemet in db.Element)
                        {

                            if (elemet.Name.Contains(SearchText.Text))
                            {
                                storageFilter.Add(elemet);
                            }
                        }
                    }

                    elementsGrid.ItemsSource = storageFilter.ToList();

                    break;
                case "Поступление":
                    entranceFilter.Clear();

                    if (SearchText.Text.Equals(""))
                    {
                        entranceFilter.AddRange(db.Entrances);
                    }
                    else
                    {
                        foreach (Entrance entrance in db.Entrances)
                        {

                            if (entrance.Name.Contains(SearchText.Text))
                            {
                                entranceFilter.Add(entrance);
                            }
                        }
                    }

                    EntrancesGrid.ItemsSource = entranceFilter.ToList();
                    break;
                case "Остатки":
                    remainsFilter.Clear();

                    if (SearchText.Text.Equals(""))
                    {
                        remainsFilter.AddRange(db.Remains);
                    }
                    else
                    {
                        foreach (Remains remains in db.Remains)
                        {

                            if (remains.Name.Contains(SearchText.Text))
                            {
                                remainsFilter.Add(remains);
                            }
                        }
                    }

                    RemainsGrid.ItemsSource = entranceFilter.ToList();
                    break;
                case "Производство":
                    productionFilter.Clear();

                    if (SearchText.Text.Equals(""))
                    {
                        productionFilter.AddRange(db.Productions);
                    }
                    else
                    {
                        foreach (Production production in db.Productions)
                        {

                            if (production.Name.Contains(SearchText.Text))
                            {
                                productionFilter.Add(production);
                            }
                        }
                    }

                    ProductionGrid.ItemsSource = entranceFilter.ToList();
                    break;
                case "Продажа":
                    saleFilter.Clear();

                    if (SearchText.Text.Equals(""))
                    {
                        saleFilter.AddRange(db.Sales);
                    }
                    else
                    {
                        foreach (Sale sale in db.Sales)
                        {

                            if (sale.Name.Contains(SearchText.Text))
                            {
                                saleFilter.Add(sale);
                            }
                        }
                    }

                    SaleGrid.ItemsSource = entranceFilter.ToList();
                    break;
                default:
                    MessageBox.Show("Не выбранна таблица");
                    break;
            }

        }

        private void PrintTable_Click(object sender, RoutedEventArgs e)
        {
            switch ((ControlTab.SelectedItem as TabItem).Header)
            {
                case "Все":
                    System.Windows.Controls.PrintDialog PrintAll = new System.Windows.Controls.PrintDialog();
                    if ((bool)PrintAll.ShowDialog().GetValueOrDefault())
                    {
                        Size pageSize = new Size(PrintAll.PrintableAreaWidth, PrintAll.PrintableAreaHeight);
                        elementsGrid.Measure(pageSize);
                        elementsGrid.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));
                        PrintAll.PrintVisual(elementsGrid, Title);
                    }
                    break;
                case "Поступление":
                    System.Windows.Controls.PrintDialog PrintEntrances = new System.Windows.Controls.PrintDialog();
                    if ((bool)PrintEntrances.ShowDialog().GetValueOrDefault())
                    {
                        Size pageSize = new Size(PrintEntrances.PrintableAreaWidth, PrintEntrances.PrintableAreaHeight);
                        EntrancesGrid.Measure(pageSize);
                        EntrancesGrid.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));
                        PrintEntrances.PrintVisual(EntrancesGrid, Title);
                    }
                    break;
                case "Остатки":
                    System.Windows.Controls.PrintDialog PrintRemains = new System.Windows.Controls.PrintDialog();
                    if ((bool)PrintRemains.ShowDialog().GetValueOrDefault())
                    {
                        Size pageSize = new Size(PrintRemains.PrintableAreaWidth, PrintRemains.PrintableAreaHeight);
                        RemainsGrid.Measure(pageSize);
                        RemainsGrid.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));
                        PrintRemains.PrintVisual(RemainsGrid, Title);
                    }
                    break;
                case "Производство":
                    System.Windows.Controls.PrintDialog PrintProductions = new System.Windows.Controls.PrintDialog();
                    if ((bool)PrintProductions.ShowDialog().GetValueOrDefault())
                    {
                        Size pageSize = new Size(PrintProductions.PrintableAreaWidth, PrintProductions.PrintableAreaHeight);
                        ProductionGrid.Measure(pageSize);
                        ProductionGrid.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));
                        PrintProductions.PrintVisual(ProductionGrid, Title);
                    }
                    break;
                case "Продажа":
                    System.Windows.Controls.PrintDialog PrintSale = new System.Windows.Controls.PrintDialog();
                    if ((bool)PrintSale.ShowDialog().GetValueOrDefault())
                    {
                        Size pageSize = new Size(PrintSale.PrintableAreaWidth, PrintSale.PrintableAreaHeight);
                        SaleGrid.Measure(pageSize);
                        SaleGrid.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));
                        PrintSale.PrintVisual(SaleGrid, Title);
                    }
                    break;
                default:
                    MessageBox.Show("Не выбранна таблица");
                    break;
            }
        }

        private void FaultyStorage_Click(object sender, RoutedEventArgs e)
        {
            FaultyStorage faultyStorage = new FaultyStorage();
            faultyStorage.ShowDialog();
        }

        private void SaveTable_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }
    }
}
