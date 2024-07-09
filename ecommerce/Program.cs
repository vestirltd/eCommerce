

using ecommerce.Interfaces;
using ecommerce.Models;
using ecommerce.Services;

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
// builder.Services.AddTransient<IUpdateStockInterface, UpdateStocksService>();

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

