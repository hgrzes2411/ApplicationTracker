using ApplicationTracker.Data;
using ApplicationTracker.Services.Dashboard;
using ApplicationTracker.Services.Dashboard.Calculators;
using ApplicationTracker.Services.Export;
using ApplicationTracker.Services.GhostDetection;
using ApplicationTracker.Services.GhostDetection.Rules;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationTracker")));
builder.Services.AddScoped<JobService>();
builder.Services.AddSingleton<GhostJobAnalyzer>();
builder.Services.AddSingleton<GhostDetector>();
builder.Services.AddSingleton<IGhostRule, CompanyHistoryRule>();
builder.Services.AddSingleton<IGhostRule, NoResponseRule>();
builder.Services.AddSingleton<IGhostRule, RepeatedPostingRule>();
builder.Services.AddSingleton<JobStatisticsService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<DashboardSummaryCalculator>();
builder.Services.AddScoped<CompanyAnalyticsCalculator>();
builder.Services.AddScoped<RecommendationCalculator>();
builder.Services.AddScoped<ResponseAnalyticsCalculator>();
builder.Services.AddScoped<RecentActivityCalculator>();
builder.Services.AddScoped<StatusChartCalculator>();
builder.Services.AddScoped<DashboardSummaryCalculator>();
builder.Services.AddScoped<SuccessRateCalculator>();
builder.Services.AddScoped<RecruitmentFunnelCalculator>();
builder.Services.AddScoped<RecruitmentTimelineCalculator>();
builder.Services.AddScoped<SourceAnalyticsCalculator>();
builder.Services.AddScoped<CompanyInsightsCalculator>();
builder.Services.AddScoped<CsvExportService>();
builder.Services.AddScoped<JobFilteringService>();
builder.Services.AddScoped<GhostAnalysisCacheService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
