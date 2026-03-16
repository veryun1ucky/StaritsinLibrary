using StaritsinLibrary.Models;
using StaritsinLibrary.Repositories;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StartitsinWPF
{
    public partial class MainWindow : Window
    {
        private readonly PartnerRepository _partnerRepository;
        private readonly DiscountService _discountService;

        private List<PartnerViewModel> _allPartners = new List<PartnerViewModel>();
        private PartnerViewModel _selectedPartner;

        public MainWindow()
        {
            InitializeComponent();
            _partnerRepository = App.PartnerRepository;
            _discountService = App.DiscountService;

            Loaded += async (s, e) => await LoadPartnersAsync();
        }


        private async Task LoadPartnersAsync()
        {
            try
            {
                var partners = await _partnerRepository.GetAllAsync();
                _allPartners = new List<PartnerViewModel>();

                foreach (var partner in partners)
                {
                    int totalQuantity = await _partnerRepository.GetTotalQuantityByPartnerAsync(partner.Id);
                    int discount = _discountService.CalculateDiscount(totalQuantity);
                    _allPartners.Add(new PartnerViewModel(partner, discount));
                }

                ApplySearchFilter(SearchBox.Text);
                PartnerCountText.Text = "Всего партнёров: " + _allPartners.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка при загрузке партнёров:\n\n" + ex.Message,
                    "Ошибка загрузки",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ApplySearchFilter(string searchText)
        {
            List<PartnerViewModel> result;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                result = _allPartners;
            }
            else
            {
                result = _allPartners
                    .Where(p => p.CompanyName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            PartnersItemsControl.ItemsSource = result;
        }


        private async void PartnerCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border == null) return;

            var vm = border.Tag as PartnerViewModel;
            if (vm == null) return;

            _selectedPartner = vm;
            await LoadSalesAsync(vm);
        }

        private async Task LoadSalesAsync(PartnerViewModel vm)
        {
            try
            {
                var sales = await _partnerRepository.GetSalesByPartnerAsync(vm.Id);
                SalesDataGrid.ItemsSource = sales;
                SalesHeaderText.Text = "История реализации — " + vm.DisplayTitle;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось загрузить историю продаж:\n\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        private async void AddPartner_Click(object sender, RoutedEventArgs e)
        {
            var form = new PartnerForm(null) { Owner = this };
            if (form.ShowDialog() == true)
                await LoadPartnersAsync();
        }

        private async void EditPartner_MenuClick(object sender, RoutedEventArgs e)
        {
            if (_selectedPartner == null)
            {
                MessageBox.Show(
                    "Сначала выберите партнёра из списка, нажав на его карточку.",
                    "Партнёр не выбран",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            await OpenEditFormAsync(_selectedPartner.Id);
        }

        private async void ContextMenu_Edit_Click(object sender, RoutedEventArgs e)
        {
            var vm = GetPartnerFromContextMenu(sender);
            if (vm != null)
                await OpenEditFormAsync(vm.Id);
        }

        private async Task OpenEditFormAsync(int partnerId)
        {
            var form = new PartnerForm(partnerId) { Owner = this };
            if (form.ShowDialog() == true)
            {
                await LoadPartnersAsync();

                if (_selectedPartner != null && _selectedPartner.Id == partnerId)
                {
                    var updated = _allPartners.FirstOrDefault(p => p.Id == partnerId);
                    if (updated != null)
                        await LoadSalesAsync(updated);
                }
            }
        }


        private async void ContextMenu_Delete_Click(object sender, RoutedEventArgs e)
        {
            var vm = GetPartnerFromContextMenu(sender);
            if (vm == null) return;

            var answer = MessageBox.Show(
                "Вы действительно хотите удалить партнёра:\n«" + vm.CompanyName + "»?\n\n" +
                "Все записи о продажах этого партнёра также будут удалены.\n" +
                "Это действие нельзя отменить.",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (answer != MessageBoxResult.Yes) return;

            try
            {
                await _partnerRepository.DeleteAsync(vm.Id);

                if (_selectedPartner != null && _selectedPartner.Id == vm.Id)
                {
                    _selectedPartner = null;
                    SalesDataGrid.ItemsSource = null;
                    SalesHeaderText.Text = "Выберите партнёра для просмотра истории реализации";
                }

                await LoadPartnersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось удалить партнёра «" + vm.CompanyName + "»:\n\n" + ex.Message,
                    "Ошибка удаления",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }



        private async void Sales_Click(object sender, RoutedEventArgs e)
        {
            var window = new SalesWindow { Owner = this };
            window.ShowDialog();

            await LoadPartnersAsync();

            if (_selectedPartner != null)
            {
                var updated = _allPartners.FirstOrDefault(p => p.Id == _selectedPartner.Id);
                if (updated != null)
                    await LoadSalesAsync(updated);
            }
        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplySearchFilter(SearchBox.Text);
        }

      
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Подсистема учёта партнёров\nВерсия 1.0\n\nАвтор: Staritsin",
                "О программе",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }



        private static PartnerViewModel GetPartnerFromContextMenu(object sender)
        {
            var menuItem = sender as MenuItem;
            if (menuItem == null) return null;

            var contextMenu = menuItem.Parent as ContextMenu;
            if (contextMenu == null) return null;

            var border = contextMenu.PlacementTarget as Border;
            if (border == null) return null;

            return border.Tag as PartnerViewModel;
        }
    }

    public class PartnerViewModel
    {
        private readonly Partner _partner;

        public PartnerViewModel(Partner partner, int discount)
        {
            _partner = partner;
            Discount = discount;
        }

        public int Id => _partner.Id;
        public string CompanyName => _partner.CompanyName;
        public string DirectorName => _partner.DirectorName;
        public string Phone => _partner.Phone;
        public int Rating => _partner.Rating;

        public string DisplayTitle =>
            (_partner.PartnerType != null ? _partner.PartnerType.TypeName : "—") + " | " + _partner.CompanyName;

        public int Discount { get; }
        public string DiscountText => Discount + "%";
    }
}
