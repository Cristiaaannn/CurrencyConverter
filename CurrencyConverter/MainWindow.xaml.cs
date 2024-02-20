using System;
using System.Collections.Generic;
using System.Data;
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

namespace CurrencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            BindCurrency();
        }

        private void BindCurrency()
        {
            DataTable dtCurrency = new DataTable();
            dtCurrency.Columns.Add("Text");
            dtCurrency.Columns.Add("Value");
            dtCurrency.Rows.Add("INR", 1);
            dtCurrency.Rows.Add("USD", 75);
            dtCurrency.Rows.Add("EUR", 85);
            dtCurrency.Rows.Add("SAR", 20);
            dtCurrency.Rows.Add("POUND", 5);
            dtCurrency.Rows.Add("DEM", 43);

            ComboBoxCurr1.ItemsSource = dtCurrency.DefaultView;
            ComboBoxCurr1.DisplayMemberPath = "Text";
            ComboBoxCurr1.SelectedValuePath = "Value";
            ComboBoxCurr1.SelectedIndex = 2;

            ComboBoxCurr2.ItemsSource = dtCurrency.DefaultView;
            ComboBoxCurr2.DisplayMemberPath = "Text";
            ComboBoxCurr2.SelectedValuePath = "Value";
            ComboBoxCurr2.SelectedIndex = 2;
        }
        public List<string> Currencies { get; set; } = new List<string>() { "EUR", "USD", "RON" };


        private void Convert()
        {
            double convertedValue;
            double a;
            if (Double.TryParse(Curr1Input.Text, out a))
            {

                if (ComboBoxCurr1.SelectedIndex == ComboBoxCurr2.SelectedIndex)
                {
                    ConvertedValue.Content = a.ToString();
                }
                else
                {
                    convertedValue = (double.Parse(ComboBoxCurr1.SelectedValue.ToString()) * double.Parse(Curr1Input.Text)) / double.Parse(ComboBoxCurr2.SelectedValue.ToString());
                    convertedValue = Math.Round(convertedValue, 3);
                    ConvertedValue.Content = convertedValue.ToString();
                }
            }
            else
            {
                ConvertedValue.Content = string.Empty;
                if (!string.IsNullOrEmpty(Curr1Input.Text))
                {
                    AmountContainsString.Content = "Can't contain letters";
                }
            }
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            ConvertedValue.Content = string.Empty;
            Curr1Input.Text = string.Empty;
            AmountContainsString.Content = string.Empty;
            Curr1Input.Focus();
        }

        private void Revert_Button_Clicked(object sender, RoutedEventArgs e)
        {
            int temp;
            temp = ComboBoxCurr1.SelectedIndex;
            ComboBoxCurr1.SelectedIndex = ComboBoxCurr2.SelectedIndex;
            ComboBoxCurr2.SelectedIndex = temp;
            Convert();
        }

        private void TextChangedEvent(object sender, TextChangedEventArgs e)
        {
            ConvertedValue.Content = string.Empty;
            AmountContainsString.Content = string.Empty;
            Convert();
        }

        private void PressedEnterEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Convert();
            }
        }

        private void FromCurrChanged(object sender, SelectionChangedEventArgs e)
        {
            Convert();
        }

        private void ToCurrChanged(object sender, SelectionChangedEventArgs e)
        {
            Convert();
        }
    }
}
