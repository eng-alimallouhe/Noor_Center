using LMS.Application.Services.Users.Authentication;
using LMS.Domain.Entities.Financial;
using LMS.Domain.Entities.HR;
using LMS.Domain.Entities.Orders;
using LMS.Domain.Entities.Stock;
using LMS.Domain.Entities.Users;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.DbContexts;
using LMS.Infrastructure.Interfaces;
using LMS.Infrastructure.Repositories.Financial;
using LMS.Infrastructure.Repositories.HR;
using LMS.Infrastructure.Repositories.Order;
using LMS.Infrastructure.Repositories.OrderManagement;
using LMS.Infrastructure.Repositories.Orders;
using LMS.Infrastructure.Repositories.Stock;
using LMS.Infrastructure.Repositories.Users;
using LMS.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;


Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.File("logs\\logger.txt", rollingInterval: RollingInterval.Month)
            .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


//get JWT section from appsettigns.json:
var jwtSettings = builder.Configuration.GetSection("Jwt");

//adding authentication by JWTBearer:
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
    };
});

// Inject AutoMapper and assign the Profiles:
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Inject the dbcontext(AppDbContext): 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);



// Inject the Repositories:
// Users Repositories:
builder.Services.AddScoped<IRepository<Role>, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepositroy>();
builder.Services.AddScoped<IRepository<Department>, DepartmentRepository>();
builder.Services.AddScoped<IRepository<Employee>, EmployeeRepository>();
builder.Services.AddScoped<IRepository<EmployeeDepartment>, EmployeeDepartmentRepository>();
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
builder.Services.AddScoped<IRepository<Address>, AddressRepository>();
builder.Services.AddScoped<IRepository<OtpCode>, OtpCodeRepository>();
builder.Services.AddScoped<IRepository<Notification>, NotificationRepository>();
builder.Services.AddScoped<IRepository<Notification>, NotificationRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

//Stock Repositories: 
builder.Services.AddScoped<IRepository<Supplier>, SupplierRepository>();
builder.Services.AddScoped<IRepository<Purchase>, PurchaseRepository>();
builder.Services.AddScoped<IRepository<Publisher>, PublisherRepository>();
builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IRepository<InventoryLog>, InventoryLogRepository>();
builder.Services.AddScoped<IRepository<Genre>, GenreRepository>();
builder.Services.AddScoped<IRepository<Book>, BookRepository>();
builder.Services.AddScoped<IRepository<Author>, AuthorRepository>();

//Orders Repositories: 
builder.Services.AddScoped<IRepository<SellOrder>, SellOrderRepository>();
builder.Services.AddScoped<IRepository<RentalOrder>, RentalOrderRepository>();
builder.Services.AddScoped<IRepository<PrintOrder>, PrintOrderRepository>();
builder.Services.AddScoped<IRepository<DeliveryOrder>, DeliveryOrderRepository>();
builder.Services.AddScoped<IRepository<CartItem>, CartItemRepository>();
builder.Services.AddScoped<IRepository<Cart>, CartRepository>();

//HR Repositories:
builder.Services.AddScoped<IRepository<Salary>, SalaryRepository>();
builder.Services.AddScoped<IRepository<Penalty>, PenaltyRepository>();
builder.Services.AddScoped<IRepository<Leave>, LeaveRepository>();
builder.Services.AddScoped<IRepository<LeaveBalance>, LeaveBalanceRepository>();
builder.Services.AddScoped<IRepository<Incentive>, IncentiveRepository>();
builder.Services.AddScoped<IRepository<Attendance>, AttendanceRepository>();

//Financial Repositories: 
builder.Services.AddScoped<IRepository<LoyaltyLevel>, LevelRepository>();
builder.Services.AddScoped<IRepository<FinancialRevenue>, FinancialRevenueRepository>();
builder.Services.AddScoped<IRepository<Payment>, PaymentRepository>();

//Inject Email Service:
builder.Services.AddScoped<IEmailService, EmailService>(prvider =>
{
    var config = builder.Configuration;
    return new EmailService(
        email: config["EmailSettings:Email"]!,
        password: config["EmailSettings:Password"]!,
        host: config["EmailSettings:Host"]!,
        port: int.Parse(config["EmailSettings:Port"]!)
    );
});


//Inject Application Services:
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<CodeService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<TokenReaderService>();

builder.Services.AddCors(options =>
{ 
    options.AddPolicy("FronPlicy",
        bl =>
        {
            bl.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

builder.Host.UseSerilog();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FronPlicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
