using StaritsinLibrary.Models;
using StaritsinLibrary.Services;
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

namespace StartitsinWPF
{
    /// <summary>
    /// Логика взаимодействия для SalesWindow.xaml
    /// </summary>
    public partial class SalesWindow : Window
    {
        private readonly SaleService _saleService;

        public SalesWindow()
        {
            InitializeComponent();
            _saleService = App.SaleService;
            Loaded += async (s, e) => await LoadDataAsync();
        }



        private async Task LoadDataAsync()
        {
            try
            {
                var data = await _saleService.GetSalesAsync();
                SalesDataGrid.ItemsSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка при загрузке продаж:\n\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SaleEditWindow(null) { Owner = this };

            if (window.ShowDialog() == true && window.IsSaved)
                await LoadDataAsync();
        }

   
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = SalesDataGrid.SelectedItem as SaleListItemDTO;

            if (selectedItem == null)
            {
                MessageBox.Show(
                    "Сначала выберите продажу из списка.",
                    "Продажа не выбрана",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var window = new SaleEditWindow(selectedItem.Id) { Owner = this };

            if (window.ShowDialog() == true && window.IsSaved)
                await LoadDataAsync();
        }


        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = SalesDataGrid.SelectedItem as SaleListItemDTO;

            if (selectedItem == null)
            {
                MessageBox.Show(
                    "Сначала выберите продажу из списка.",
                    "Продажа не выбрана",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var answer = MessageBox.Show(
                "Вы действительно хотите удалить продажу №" + selectedItem.Id + "?\n\n" +
                "Это действие нельзя отменить.",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (answer != MessageBoxResult.Yes) return;

            try
            {
                await _saleService.DeleteSaleAsync(selectedItem.Id);
                await LoadDataAsync();

                MessageBox.Show(
                    "Продажа успешно удалена.",
                    "Готово",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось удалить продажу:\n\n" + ex.Message,
                    "Ошибка удаления",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
