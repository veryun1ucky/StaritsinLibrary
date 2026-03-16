using StaritsinLibrary.Models;
using StaritsinLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для PartnerForm.xaml
    /// </summary>
    public partial class PartnerForm : Window
    {
        private readonly PartnerRepository _partnerRepository;

        private readonly int? _editPartnerId;

        public PartnerForm(int? editPartnerId)
        {
            InitializeComponent();

            _partnerRepository = App.PartnerRepository;
            _editPartnerId = editPartnerId;

            Loaded += async (s, e) => await InitializeFormAsync();
        }


        private async Task InitializeFormAsync()
        {
            try
            {
                var types = await _partnerRepository.GetPartnerTypesAsync();
                PartnerTypeCombo.ItemsSource = types;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось загрузить типы партнёров:\n\n" + ex.Message,
                    "Ошибка загрузки",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Close();
                return;
            }

            if (_editPartnerId.HasValue)
            {
                Title = "Редактирование партнёра — Staritsin";
                FormTitleText.Text = "Редактирование партнёра";
                await LoadPartnerDataAsync(_editPartnerId.Value);
            }
            else
            {
                Title = "Добавление партнёра — Staritsin";
                FormTitleText.Text = "Добавление партнёра";
            }
        }

        private async Task LoadPartnerDataAsync(int partnerId)
        {
            try
            {
                var partner = await _partnerRepository.GetByIdAsync(partnerId);
                if (partner == null)
                {
                    MessageBox.Show(
                        "Партнёр не найден в базе данных. Возможно, он был удалён.",
                        "Запись не найдена",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    Close();
                    return;
                }

                CompanyNameBox.Text = partner.CompanyName;
                DirectorNameBox.Text = partner.DirectorName;
                PhoneBox.Text = partner.Phone;
                EmailBox.Text = partner.Email;
                AddressBox.Text = partner.Address;
                RatingBox.Text = partner.Rating.ToString();

                PartnerTypeCombo.SelectedValue = partner.PartnerTypeId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка при загрузке данных партнёра:\n\n" + ex.Message,
                    "Ошибка загрузки",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage;

            if (!ValidateForm(out errorMessage))
            {
                MessageBox.Show(
                    errorMessage,
                    "Ошибка ввода данных",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_editPartnerId.HasValue)
                    await UpdatePartnerAsync(_editPartnerId.Value);
                else
                    await CreatePartnerAsync();

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось сохранить данные партнёра:\n\n" + ex.Message,
                    "Ошибка сохранения",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task CreatePartnerAsync()
        {
            var partner = new Partner
            {
                CompanyName = CompanyNameBox.Text.Trim(),
                PartnerTypeId = (int)PartnerTypeCombo.SelectedValue,
                DirectorName = DirectorNameBox.Text.Trim(),
                Phone = PhoneBox.Text.Trim(),
                Email = EmailBox.Text.Trim(),
                Address = AddressBox.Text.Trim(),
                Rating = int.Parse(RatingBox.Text.Trim())
            };
            await _partnerRepository.AddAsync(partner);
        }

        private async Task UpdatePartnerAsync(int partnerId)
        {
            var partner = await _partnerRepository.GetByIdAsync(partnerId);
            if (partner == null) return;

            partner.CompanyName = CompanyNameBox.Text.Trim();
            partner.PartnerTypeId = (int)PartnerTypeCombo.SelectedValue;
            partner.DirectorName = DirectorNameBox.Text.Trim();
            partner.Phone = PhoneBox.Text.Trim();
            partner.Email = EmailBox.Text.Trim();
            partner.Address = AddressBox.Text.Trim();
            partner.Rating = int.Parse(RatingBox.Text.Trim());

            await _partnerRepository.UpdateAsync(partner);
        }


        private bool ValidateForm(out string errorMessage)
        {
            var errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(CompanyNameBox.Text))
                errors.AppendLine("• Укажите наименование компании.");

            if (PartnerTypeCombo.SelectedItem == null)
                errors.AppendLine("• Выберите тип партнёра из списка.");

            if (string.IsNullOrWhiteSpace(DirectorNameBox.Text))
                errors.AppendLine("• Укажите ФИО директора.");

            if (string.IsNullOrWhiteSpace(PhoneBox.Text))
                errors.AppendLine("• Укажите телефон компании.");

            if (string.IsNullOrWhiteSpace(EmailBox.Text))
            {
                errors.AppendLine("• Укажите email компании.");
            }
            else if (!IsValidEmail(EmailBox.Text.Trim()))
            {
                errors.AppendLine("• Email имеет неверный формат. Пример корректного: info@company.ru");
            }

            if (string.IsNullOrWhiteSpace(AddressBox.Text))
                errors.AppendLine("• Укажите юридический адрес.");

            if (string.IsNullOrWhiteSpace(RatingBox.Text))
            {
                errors.AppendLine("• Укажите рейтинг партнёра.");
            }
            else
            {
                int rating;
                if (!int.TryParse(RatingBox.Text.Trim(), out rating) || rating < 0)
                    errors.AppendLine("• Рейтинг должен быть целым неотрицательным числом (0 и выше).");
            }

            if (errors.Length > 0)
            {
                errorMessage = "Пожалуйста, исправьте следующие ошибки:\n\n" + errors.ToString().TrimEnd();
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }


        private static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }


        private void RatingBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
