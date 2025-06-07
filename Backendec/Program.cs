using Backendec.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Добавь CORS СРАЗУ в сервисы
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {

        policy.WithOrigins(
                "http://localhost:5210",   // HTTP фронтенд
                "https://localhost:7199"   // HTTPS фронтенд
            )
            .AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36)),
        mysql => mysql.EnableRetryOnFailure()
    ));

var app = builder.Build();
builder.WebHost.UseUrls("https://localhost:5041");
app.UseRouting();
app.UseCors("AllowBlazor");
// 2. Автоматические миграции (оставляем как есть)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    Console.WriteLine("Попытка применить миграции...");
    
    try
    {
        db.Database.Migrate();
        Console.WriteLine("Миграции успешно применены");
        
        if (!db.Stores.Any())
        {
            SeedDatabase(db);
            Console.WriteLine("Добавлены тестовые данные");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ОШИБКА: {ex.Message}");
        if (ex.InnerException != null)
            Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
    }
}

app.UseSwagger();
app.UseSwaggerUI();
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.MapGet("/", () => 
{
    var env = app.Environment;
    return Results.Text($"BackendEC API работает!\n" +
                      $"Режим: {env.EnvironmentName}\n" +
                      $"Swagger: /swagger\n" +
                      $"API: /api/[controller]");
});

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