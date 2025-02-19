using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Repository;
using RecruitmentProcessManagement.Repository.Interfaces;
using RecruitmentProcessManagement.Services;
using RecruitmentProcessManagement.Services.Intefaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// SQL Server Connection
builder.Services.AddDbContext<RecruitmentProcessManagement.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConncetion")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    options =>
            {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredUniqueChars = 4;
            })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Add DI Services.
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();

builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();

builder.Services.AddScoped<ICandidateReviewService, CandidateReviewService>();
builder.Services.AddScoped<ICandidateReviewRepository,CandidateReviewRepository>();

builder.Services.AddScoped<IInterviewerService, InterviewerService>();
builder.Services.AddScoped<IInterviewerRepository, InterviewerRepository>();

builder.Services.AddScoped<IBulkHiringService, BulkHiringService>();
builder.Services.AddScoped<IBulkHiringRepository, BulkHiringRepository>();

builder.Services.AddScoped<ICandidateDocumentService, CandidateDocumentService>();
builder.Services.AddScoped<ICandidateDocumentRepository, CandidateDocumentRepository>();

builder.Services.AddScoped<IDocumentVerificationService, DocumentVerificationService>();
builder.Services.AddScoped<IDocumentVerificationRepository, DocumentVerificationRepository>();

builder.Services.AddScoped<IFinalSelectionService, FinalSelectionService>();
builder.Services.AddScoped<IFinalSelectionRepository, FinalSelectionRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IInterviewService, InterviewService>();

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
