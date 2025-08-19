using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PedidosApi.Data;
using PedidosApi.Interfaces;
using PedidosApi.Services;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IProductoService, ProductService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

var app = builder.Build();
app.MapControllers();

// Middleware

app.Run();

//dotnet ef dbcontext scaffold
