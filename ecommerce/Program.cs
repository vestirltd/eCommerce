

using ecommerce.Interfaces;
using ecommerce.Models;
using ecommerce.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IDapperDBConnectInterface, DapperDBConnectService>();
builder.Services.AddTransient<IGetStockInterface, GetStockServices>();
builder.Services.AddTransient<IBillGenerate, BillGenerateService>();
builder.Services.AddTransient<IViewSalesReport, ViewSalesReport>();
builder.Services.AddTransient<IExportSalesReport, FileExportSalesReport>();
builder.Services.AddTransient<IUpdateStockInterface, UpdateStockService>();
builder.Services.AddTransient<ISendMail, SendMail>();
builder.Services.AddTransient<IGetInvoiceNumber, GetInvoiceNumber>();
// builder.Services.AddTransient<IUpdateStockInterface, UpdateStocksService>();
// Reddis connection
var redisConnectionString = builder.Configuration.GetSection("Redis:ConnectionString").Value;
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(redisConnectionString, true);
    configuration.AbortOnConnectFail = false;
    return ConnectionMultiplexer.Connect(configuration);
});

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();

// Docker docker exec -it redis redis-cli
// FLUSHALL
//dotnet add package StackExchange.Redis
