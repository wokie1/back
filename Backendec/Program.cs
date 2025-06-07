using Backendec.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36)),
        mysql => mysql.EnableRetryOnFailure()
    ));

// Сборка приложения
var app = builder.Build();

// Автоматическое создание БД и миграций
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();


    try
    {
        // Применяем миграции
        db.Database.Migrate();

        // Если таблицы пустые — заполняем начальными данными
        if (!db.Stores.Any() && !db.Products.Any())
        {
            SeedDatabase(db);
        }
    }
    catch (Exception ex) { 
        Console.WriteLine($"error: {ex.Message}");
        Console.WriteLine(ex.ToString());
    }
}

// Swagger и маршруты
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


// Метод заполнения данными
void SeedDatabase(AppDbContext db)
{
    var store = new Store { Name = "Магазин №1", Address = "ул. Примерная, 1" };
    var product1 = new Product
    {
        Barcode = "1234567890",
        GroupCode = 1,
        Name = "Молоко",
        Weight = 1.0,
        Type = 0,
        Price = 89.99m,
        ExpiryDate = DateTime.Today.AddDays(7)
    };
    var product2 = new Product
    {
        Barcode = "9876543210",
        GroupCode = 1,
        Name = "Хлеб",
        Weight = 0.5,
        Type = 0,
        Price = 49.99m,
        ExpiryDate = DateTime.Today.AddDays(-1) // просрочен
    };

    var customer = new Customer
    {
        FullName = "Иван Иванов",
        CardNumber = "CARD123"
    };

    db.Stores.Add(store);
    db.Products.AddRange(product1, product2);
    db.Customers.Add(customer);
    db.SaveChanges();

    db.Availabilitys.Add(new Availability
    {
        StoreId = store.Id,
        ProductId = product1.Id,
        Quantity = 50
    });

    db.Sales.Add(new Sale
    {
        CustomerId = customer.Id,
        ProductId = product1.Id,
        SaleDate = DateTime.Today,
        TotalPrice = product1.Price
    });

    db.SaveChanges();
}