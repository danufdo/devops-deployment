
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Database connection string is missing.");

var dataSource = NpgsqlDataSource.Create(connectionString);

// Initialise the database and create sample records.
await using (var connection = await dataSource.OpenConnectionAsync())
{
    await using var command = new NpgsqlCommand("""
        CREATE TABLE IF NOT EXISTS products (
            id INTEGER PRIMARY KEY,
            name VARCHAR(100) NOT NULL,
            price NUMERIC(12,2) NOT NULL
        );

        INSERT INTO products (id, name, price)
        VALUES
            (1, 'Laptop', 250000),
            (2, 'Keyboard', 12000),
            (3, 'Mouse', 5000)
        ON CONFLICT (id) DO NOTHING;
        """, connection);

    await command.ExecuteNonQueryAsync();
}

var app = builder.Build();

app.MapGet("/", () => "Product API is running!");

app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy",
    Service = "Product API"
}));

app.MapGet("/api/products", async () =>
{
    var products = new List<Product>();

    await using var connection =
        await dataSource.OpenConnectionAsync();

    await using var command = new NpgsqlCommand(
        "SELECT id, name, price FROM products ORDER BY id",
        connection);

    await using var reader = await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        products.Add(new Product(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetDecimal(2)));
    }

    return Results.Ok(products);
});

app.Run();

record Product(int Id, string Name, decimal Price);

