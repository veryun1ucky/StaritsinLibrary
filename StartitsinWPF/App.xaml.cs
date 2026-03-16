using StaritsinLibrary;
using StaritsinLibrary.Repositories;
using StaritsinLibrary.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StartitsinWPF
{
    public partial class App : Application
    {
        public static PartnerRepository PartnerRepository { get; private set; }
        public static DiscountService DiscountService { get; private set; }
        public static SaleService SaleService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            const string connectionString =
                "Host=localhost;Port=5432;Database=staritsin;Username=app;Password=123456789;";

            var factory = new StaritsinDbContextFactory(connectionString);

            DiscountService = new DiscountService();
            PartnerRepository = new PartnerRepository(factory);
            SaleService = new SaleService(factory, DiscountService);

            try
            {
                bool wasCreated = factory.EnsureCreated();

                if (wasCreated)
                {
                    MessageBox.Show(
                        "База данных 'staritsin' была создана автоматически и заполнена начальными данными.",
                        "База данных создана",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось подключиться к серверу PostgreSQL.\n\n" + ex.Message + "\n\n" +
                    "Убедитесь, что:\n" +
                    "  1. PostgreSQL запущен на localhost:5432\n" +
                    "  2. Роль 'app' с паролем '123456789' существует\n\n" +
                    "Команды для создания роли (выполнить от postgres):\n" +
                    "  CREATE ROLE app WITH LOGIN PASSWORD '123456789';\n" +
                    "  CREATE DATABASE staritsin OWNER app;",
                    "Ошибка подключения к БД",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
