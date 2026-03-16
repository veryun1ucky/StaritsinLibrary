using Microsoft.Extensions.DependencyInjection;
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

namespace StaritsinWPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            const string connectionString =
                "Host=localhost;Port=5432;Database=staritsin;Username=app;Password=123456789;";

            services.AddDbContext<StaritsinDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddTransient<PartnerRepository>();
            services.AddTransient<DiscountService>();

            ServiceProvider = services.BuildServiceProvider();

            try
            {
                using (var scope = ServiceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<StaritsinDbContext>();

                    if (!db.Database.CanConnect())
                    {
                        ShowConnectionError("Сервер PostgreSQL недоступен или база данных 'staritsin' не существует.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowConnectionError(ex.Message);
            }
        }

        private static void ShowConnectionError(string detail)
        {
            MessageBox.Show(
                "Не удалось подключиться к базе данных.\n\n" + detail + "\n\n" +
                "Убедитесь, что:\n" +
                "  1. PostgreSQL запущен на localhost:5432\n" +
                "  2. База данных 'staritsin' создана\n" +
                "  3. Роль 'app' с паролем '123456789' существует\n" +
                "  4. Скрипт StaritsinDB.sql выполнен",
                "Ошибка подключения к БД",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
