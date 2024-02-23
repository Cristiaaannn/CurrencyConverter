using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;

namespace CurrencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Root val = new Root();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            GetValue();
        }

        public async void GetValue()
        {
            val = await GetData<Root>("https://openexchangerates.org/api/latest.json?app_id=38917be4333a4e44b92fbb3661626272");
            BindCurrency();
        }

        public static async Task<Root> GetData<T>(string url)
        {
            var myRoot = new Root();
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage respone = await client.GetAsync(url);
                    if (respone.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var ResponceString = await respone.Content.ReadAsStringAsync();
                        var ResponceObject = JsonConvert.DeserializeObject<Root>(ResponceString);

                        return ResponceObject;
                    }
                    return myRoot;
                }
            }
            catch
            {
                return myRoot;
            }
        }

        private void BindCurrency()
        {
            DataTable dtCurrency = new DataTable();
            dtCurrency.Columns.Add("Text");
            dtCurrency.Columns.Add("Value");

            API_License.Content = val.license;
            dtCurrency.Rows.Add("INR", val.rates.INR);
            dtCurrency.Rows.Add("USD", val.rates.USD);
            dtCurrency.Rows.Add("NZD", val.rates.NZD);
            dtCurrency.Rows.Add("JPY", val.rates.JPY);
            dtCurrency.Rows.Add("EUR", val.rates.EUR);
            dtCurrency.Rows.Add("CAD", val.rates.CAD);
            dtCurrency.Rows.Add("ISK", val.rates.ISK);
            dtCurrency.Rows.Add("PHP", val.rates.PHP);
            dtCurrency.Rows.Add("DKK", val.rates.DKK);
            dtCurrency.Rows.Add("CZK", val.rates.CZK);

            ComboBoxCurr1.ItemsSource = dtCurrency.DefaultView;
            ComboBoxCurr1.DisplayMemberPath = "Text";
            ComboBoxCurr1.SelectedValuePath = "Value";
            ComboBoxCurr1.SelectedIndex = 4;

            ComboBoxCurr2.ItemsSource = dtCurrency.DefaultView;
            ComboBoxCurr2.DisplayMemberPath = "Text";
            ComboBoxCurr2.SelectedValuePath = "Value";
            ComboBoxCurr2.SelectedIndex = 1;
        }

        private void Convert()
        {
            double convertedValue;
            double a;
            if (Double.TryParse(Curr1Input.Text, out a))
            {
                if (a < 0)
                {
                    ConvertedValue.Content = string.Empty;
                    AmountContainsString.Content = "Can't convert negative";
                    return;
                }

                if (ComboBoxCurr1.SelectedIndex == ComboBoxCurr2.SelectedIndex)
                {
                    ConvertedValue.Content = a.ToString();
                }
                else
                {
                    convertedValue = (double.Parse(ComboBoxCurr2.SelectedValue.ToString()) * double.Parse(Curr1Input.Text)) / double.Parse(ComboBoxCurr1.SelectedValue.ToString());
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
