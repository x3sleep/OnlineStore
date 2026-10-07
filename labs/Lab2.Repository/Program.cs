using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.BLL;
using OnlineStore.BLL.DTOs;
using OnlineStore.BLL.Exceptions;
using OnlineStore.BLL.Services;
using OnlineStore.DAL.Data;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Repositories.Interfaces;

Console.OutputEncoding = Encoding.UTF8;
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

var dbPath = Path.Combine(AppContext.BaseDirectory, "lab2_repository.db");
var services = new ServiceCollection()
    .AddBusinessLogic($"Data Source={dbPath}")
    .BuildServiceProvider();

using (var scope = services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    Header("Задание 1. Структура таблиц в базе данных SQLite");
    var tables = context.Database
        .SqlQueryRaw<string>("SELECT name AS Value FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%' ORDER BY name")
        .ToList();
    Console.WriteLine($"Создано таблиц: {tables.Count}");
    Console.WriteLine(string.Join(", ", tables));
}

using (var scope = services.CreateScope())
{
    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

    Header("Задание 3. CRUD через репозитории: Create");
    var electronics = new Category { Name = "Электроника" };
    var phones = new Category { Name = "Смартфоны", Parent = electronics };
    var laptops = new Category { Name = "Ноутбуки", Parent = electronics };
    uow.Categories.Add(electronics);
    uow.Categories.Add(phones);
    uow.Categories.Add(laptops);
    var apple = new Brand { Name = "Apple", Country = "США" };
    var xiaomi = new Brand { Name = "Xiaomi", Country = "Китай" };
    uow.Brands.Add(apple);
    uow.Brands.Add(xiaomi);
    uow.Products.Add(new Product { Sku = "PH-001", Name = "iPhone 16", Price = 89_990m, StockQuantity = 10, Category = phones, Brand = apple });
    uow.Products.Add(new Product { Sku = "PH-002", Name = "Xiaomi 14T Pro", Price = 54_990m, StockQuantity = 7, Category = phones, Brand = xiaomi });
    uow.Products.Add(new Product { Sku = "PH-003", Name = "Redmi Note 13", Price = 21_490m, StockQuantity = 25, Category = phones, Brand = xiaomi });
    uow.Products.Add(new Product { Sku = "NB-001", Name = "MacBook Air 13", Price = 119_990m, StockQuantity = 4, Category = laptops, Brand = apple });
    uow.Products.Add(new Product { Sku = "NB-002", Name = "Xiaomi RedmiBook Pro", Price = 74_990m, StockQuantity = 3, Category = laptops, Brand = xiaomi });
    uow.Products.Add(new Product { Sku = "TMP-001", Name = "Тестовый товар", Price = 1m, StockQuantity = 1, Category = phones });
    var saved = uow.SaveChanges();
    Console.WriteLine($"SaveChanges(): сохранено записей — {saved}");

    Header("Задание 3. CRUD через репозитории: Read");
    foreach (var product in uow.Products.GetAll())
    {
        PrintProduct(product);
    }

    var first = uow.Products.GetById(1)!;
    Console.WriteLine($"GetById(1): {first.Name}");

    Header("Задание 3. CRUD через репозитории: Update");
    first.Price = 84_990m;
    uow.Products.Update(first);
    uow.SaveChanges();
    PrintProduct(uow.Products.GetById(1)!);

    Header("Задание 3. CRUD через репозитории: Delete");
    var temp = (await uow.Products.GetBySkuAsync("TMP-001"))!;
    uow.Products.Delete(temp.Id);
    uow.SaveChanges();
    Console.WriteLine($"Delete({temp.Id}): товар «{temp.Name}» удален, осталось товаров: {uow.Products.GetAll().Count()}");

    Header("Задание 2. Специализированный репозиторий IProductRepository");
    Console.WriteLine("GetByPriceRangeAsync(20 000, 80 000):");
    foreach (var product in await uow.Products.GetByPriceRangeAsync(20_000m, 80_000m))
    {
        PrintProduct(product);
    }

    Console.WriteLine($"GetByCategoryAsync({laptops.Id}) — ноутбуки:");
    foreach (var product in await uow.Products.GetByCategoryAsync(laptops.Id))
    {
        PrintProduct(product);
    }

    Console.WriteLine("SearchByNameAsync(\"Pro\"):");
    foreach (var product in await uow.Products.SearchByNameAsync("Pro"))
    {
        PrintProduct(product);
    }

    Header("Задание 4. Асинхронные методы GenericRepository");
    var customer = new Customer
    {
        FullName = "Иван Петров",
        Email = "ivan@example.com",
        PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
        Addresses = [new Address { City = "Москва", Street = "ул. Тверская, д. 1", PostalCode = "125009", IsDefault = true }]
    };
    await uow.Customers.AddAsync(customer);
    await uow.SaveChangesAsync();
    var found = await uow.Customers.GetByEmailAsync("ivan@example.com");
    Console.WriteLine($"AddAsync + GetByEmailAsync: {found!.FullName} (Id = {found.Id})");
    Console.WriteLine($"GetAllAsync<Category>: {string.Join(", ", (await uow.Categories.GetAllAsync()).Select(c => c.Name))}");
}

using (var scope = services.CreateScope())
{
    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    var customer = (await uow.Customers.GetByEmailAsync("ivan@example.com"))!;
    var addressId = (await uow.Customers.GetWithAddressesAsync(customer.Id))!.Addresses[0].Id;

    Header("Задание 4. Unit of Work: успешный заказ (Orders + Products + Payments в одной транзакции)");
    var summary = await orderService.PlaceOrderAsync(customer.Id, addressId, [new OrderLine(1, 1), new OrderLine(3, 2)], PaymentMethod.Card);
    Console.WriteLine($"Заказ №{summary.Id}: статус {summary.Status}, позиций {summary.ItemsCount}, сумма {summary.TotalAmount:C}");
    await PrintStock(uow, 1, 3);

    Header("Задание 4. Unit of Work: откат транзакции");
    try
    {
        await orderService.PlaceOrderAsync(customer.Id, addressId, [new OrderLine(4, 1)], PaymentMethod.CashOnDelivery);
    }
    catch (BusinessRuleException ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }

    Console.WriteLine($"Заказов в базе: {(await uow.Orders.GetAllAsync()).Count}");
    await PrintStock(uow, 4);
}

static async Task PrintStock(IUnitOfWork uow, params int[] productIds)
{
    foreach (var id in productIds)
    {
        var product = await uow.Products.GetByIdAsync(id);
        Console.WriteLine($"Остаток «{product!.Name}»: {product.StockQuantity} шт.");
    }
}

static void PrintProduct(Product product) =>
    Console.WriteLine($"  [{product.Id}] {product.Sku,-8} {product.Name,-22} {product.Price,12:C}  остаток: {product.StockQuantity}");

static void Header(string title)
{
    Console.WriteLine();
    Console.WriteLine($"=== {title} ===");
}
