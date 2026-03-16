using StaritsinLibrary.Models;
using StaritsinLibrary.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace StartitsinWPF
{
    /// <summary>
    /// Логика взаимодействия для SaleEditWindow.xaml
    /// </summary>
    public partial class SaleEditWindow : Window
    {
        private readonly SaleService _saleService;
        private readonly int? _editId;

        private bool _isLoading;

        public bool IsSaved { get; private set; }

        public SaleEditWindow(int? editId)
        {
            InitializeComponent();
            _saleService = App.SaleService;
            _editId = editId;

            Loaded += async (s, e) => await InitializeFormAsync();
        }

    

        private async Task InitializeFormAsync()
        {
            try
            {
                _isLoading = true;

                var partners = await _saleService.GetPartnersForLookupAsync();
                PartnerComboBox.ItemsSource = partners;

                var products = await _saleService.GetProductsForLookupAsync();
                ProductComboBox.ItemsSource = products;

                if (_editId.HasValue)
                {
                    Title = "Редактирование продажи — Staritsin";
                    FormTitleText.Text = "Редактирование продажи";

                    var model = await _saleService.GetSaleByIdAsync(_editId.Value);

                    if (model == null)
                        throw new InvalidOperationException("Продажа не найдена.");

                    PartnerComboBox.SelectedValue = model.PartnerId;
                    ProductComboBox.SelectedValue = model.ProductId;
                    QuantityBox.Text = model.Quantity.ToString();
                    SaleDatePicker.SelectedDate = model.SaleDate;
                    CommentBox.Text = model.Comment ?? string.Empty;
                }
                else
                {
                    Title = "Добавление продажи — Staritsin";
                    FormTitleText.Text = "Добавление продажи";
                    SaleDatePicker.SelectedDate = DateTime.Today;
                }

                _isLoading = false;
                await RefreshCalculationAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка при открытии формы:\n\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Close();
            }
            finally
            {
                _isLoading = false;
            }
        }



        private async void InputControl_Changed(object sender, EventArgs e)
        {
            if (_isLoading) return;

            await RefreshCalculationAsync();
        }


        private async Task RefreshCalculationAsync()
        {
           
            BasePriceText.Text = "0.00";
            DiscountText.Text = "0%";
            UnitPriceText.Text = "0.00";
            TotalAmountText.Text = "0.00";

            if (PartnerComboBox.SelectedValue == null || ProductComboBox.SelectedValue == null)
                return;

            int quantity;
            if (!int.TryParse(QuantityBox.Text.Trim(), out quantity) || quantity <= 0)
                return;

            try
            {
                var result = await _saleService.CalculateSaleAsync(
                    (int)PartnerComboBox.SelectedValue,
                    (int)ProductComboBox.SelectedValue,
                    quantity,
                    _editId);

                BasePriceText.Text = result.BasePrice.ToString("N2", CultureInfo.InvariantCulture);
                DiscountText.Text = result.DiscountPercent + "%";
                UnitPriceText.Text = result.UnitPrice.ToString("N2", CultureInfo.InvariantCulture);
                TotalAmountText.Text = result.TotalAmount.ToString("N2", CultureInfo.InvariantCulture);
            }
            catch
            {
               
            }
        }


        private void QuantityBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    return;
                }
            }
        }



        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveButton.IsEnabled = false;
            CancelButton.IsEnabled = false;

            try
            {
                if (PartnerComboBox.SelectedValue == null)
                    throw new ArgumentException("Выберите партнёра.");

                if (ProductComboBox.SelectedValue == null)
                    throw new ArgumentException("Выберите продукцию.");

                int quantity;
                if (!int.TryParse(QuantityBox.Text.Trim(), out quantity))
                    throw new ArgumentException("Количество должно быть целым числом.");

                if (quantity <= 0)
                    throw new ArgumentException("Количество должно быть больше нуля.");

                if (SaleDatePicker.SelectedDate == null)
                    throw new ArgumentException("Укажите дату продажи.");

                var model = new SaleEditModel
                {
                    Id = _editId ?? 0,
                    PartnerId = (int)PartnerComboBox.SelectedValue,
                    ProductId = (int)ProductComboBox.SelectedValue,
                    Quantity = quantity,
                    SaleDate = SaleDatePicker.SelectedDate.Value,
                    Comment = CommentBox.Text
                };

                if (_editId.HasValue)
                    await _saleService.UpdateSaleAsync(model);
                else
                    await _saleService.AddSaleAsync(model);

                IsSaved = true;
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка сохранения",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            finally
            {
                SaveButton.IsEnabled = true;
                CancelButton.IsEnabled = true;
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
