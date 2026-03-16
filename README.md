StaritsinLibrary — Подсистема учёта партнёров

Учебный проект по производственной практике ПП.02.01
Специальность 09.02.07 «Информационные системы и программирование»
Пермский институт (филиал) РЭУ им. Г.В. Плеханова, 2026 г.


О проекте
Подсистема для работы с партнёрами компании, занимающейся производством и реализацией продукции через партнёров. Позволяет вести учёт партнёров, просматривать историю реализации продукции и автоматически рассчитывать индивидуальную скидку.
Функционал

 Просмотр списка партнёров с карточками (тип, наименование, директор, телефон, рейтинг, скидка)
 Добавление и редактирование данных о партнёре
 Просмотр истории реализации продукции партнёром
 Автоматический расчёт индивидуальной скидки по объёму продаж
 Управление продажами с расчётом цены в реальном времени
 Поиск партнёров по наименованию
 Автоматическое создание базы данных при первом запуске


Архитектура
Проект состоит из трёх сборок:
StaritsinLibrary/
├── StaritsinLibrary/   # Библиотека классов — бизнес-логика и доступ к данным
│   ├── Models/                 # Модели данных (Partner, PartnerType, Product, PartnerSale)
│   ├── Repositories/           # PartnerRepository — CRUD операции
│   ├── Services/               # DiscountService, SaleService
│   ├── StaritsinDbContext.cs   # Контекст Entity Framework Core
│   └── StaritsinDbContextFactory.cs  # Фабрика контекста (WPF не знает про EF Core)
│
├── StaritsinWPF/      # WPF-приложение — пользовательский интерфейс
│   ├── App.xaml                # Глобальные стили
│   ├── App.xaml.cs             # Точка входа, инициализация сервисов
│   ├── MainWindow.xaml/.cs     # Главное окно (список партнёров + история)
│   ├── PartnerFormWindow.xaml/.cs  # Форма добавления/редактирования партнёра
│   ├── SalesWindow.xaml/.cs    # Окно управления продажами
│   └── SaleEditWindow.xaml/.cs # Форма добавления/редактирования продажи
│
└── StaritsinTests/    # Модульные тесты (MSTest)
    ├── DiscountServiceTests.cs
    ├── PartnerModelTests.cs
    ├── SaleCalculationTests.cs
    └── ...
Ключевой принцип: WPF-проект не содержит прямых зависимостей от Entity Framework Core — вся работа с БД инкапсулирована в StaritsinPartners.Domain.

Стек технологий
КомпонентТехнологияЯзыкC# (.NET 8)UIWPF (Windows Presentation Foundation)ORMEntity Framework Core 6База данныхPostgreSQL 15Провайдер PostgreSQLNpgsqlТестыMSTestКонтроль версийGit / GitHubIDEVisual Studio 2022

Алгоритм расчёта скидки
Скидка рассчитывается на основе суммарного количества реализованной продукции партнёром за весь период:
Суммарное количествоСкидкадо 10 0000%10 000 — 49 9995%50 000 — 299 99910%от 300 00015%

База данных
Схема: app, база данных: staritsin
partner_types          partners
─────────────          ────────────────────────────────────
id (PK)           ←─── partner_type_id (FK)
type_name              id (PK)
                       company_name
                       director_name
                       email, phone, address
                       rating (≥ 0)
                            │
                            │ 1:N (CASCADE DELETE)
                            ▼
products          partner_sales
────────          ───────────────────────────
id (PK)      ←── product_id (FK)            
product_name     id (PK)
unit             partner_id (FK)
price            quantity (> 0)
                 sale_date
                 base_price
                 discount_percent
                 unit_price
                 total_amount
                 comment

База данных в третьей нормальной форме (3НФ) с обеспечением ссылочной целостности.


Быстрый старт
1. Требования

Windows 10/11
Visual Studio 2022 (Community или выше)
PostgreSQL 15+
.NET 8 SDK

2. Настройка PostgreSQL
Выполнить от суперпользователя postgres:
sqlCREATE ROLE app WITH LOGIN PASSWORD '123456789';
CREATE DATABASE staritsin OWNER app;
3. Клонирование и сборка
bashgit clone https://github.com/<ваш-логин>/StaritsinPartners.git
cd StaritsinPartners
Открыть StaritsinPartners.sln в Visual Studio 2022.
Нажать Ctrl+Shift+B — NuGet-пакеты восстановятся автоматически.
4. Запуск
Установить стартовым проектом StaritsinPartners.WPF, нажать F5.
При первом запуске приложение автоматически создаст схему app, все таблицы и заполнит их тестовыми данными.

Запуск тестов
Test → Run All Tests (Ctrl+R, A)
Всего 72 модульных теста в 9 тестовых классах. Все тесты не требуют базы данных.
КлассТестовЧто тестируетсяDiscountServiceTests10Все граничные значения расчёта скидкиPartnerModelTests10Модель PartnerPartnerSaleModelUpdatedTests10Поля BasePrice, UnitPrice, TotalAmountSaleCalculationTests13Расчёт цены с учётом скидки, округлениеSaleEditModelTests7Модель SaleEditModelSaleListItemDTOTests7DTO списка продажSaleCalculationResultDTOTests7DTO результата расчётаPartnerTypeModelTests4Модель PartnerTypeProductModelUpdatedTests4Поле Price в модели Product

Структура проекта (файлы)
<details>
<summary>Развернуть</summary>
```
StaritsinLibrary.sln
StaritsinDB.sql                              ← скрипт создания БД вручную
StaritsinLibrary/
Models/
Partner.cs
PartnerType.cs
Product.cs
PartnerSale.cs
SaleEditModel.cs
SaleListItemDTO.cs
SaleCalculationResultDTO.cs
SaleLookupDTOs.cs
Repositories/
PartnerRepository.cs
Services/
DiscountService.cs
SaleService.cs
StaritsinDbContext.cs
StaritsinDbContextFactory.cs
StaritsinLibrary.csproj
StaritsinWPF/
App.xaml
App.xaml.cs
MainWindow.xaml
MainWindow.xaml.cs
PartnerFormWindow.xaml
PartnerFormWindow.xaml.cs
SalesWindow.xaml
SalesWindow.xaml.cs
SaleEditWindow.xaml
SaleEditWindow.xaml.cs
StaritsinWPF.csproj
StaritsinTests/
DiscountServiceTests.cs
PartnerModelTests.cs
PartnerTypeModelTests.cs
PartnerSaleModelUpdatedTests.cs
ProductModelUpdatedTests.cs
SaleCalculationTests.cs
SaleEditModelTests.cs
SaleListItemDTOTests.cs
SaleCalculationResultDTOTests.cs
StaritsinPartners.Tests.csproj
