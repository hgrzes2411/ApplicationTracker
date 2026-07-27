using ApplicationTracker.Components;
using ApplicationTracker.Services;
using ApplicationTracker.Services.Charts;
using ApplicationTracker.Services.Dashboard;
using ApplicationTracker.Services.GhostDetection.Rules;
using ApplicationTracker.Services.Jobs;
using ApplicationTracker.Services.Statistics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<JsonRepository>();
builder.Services.AddSingleton<JobService>();
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
builder.Services.AddScoped<ChartService>();
builder.Services.AddScoped<DashboardSummaryCalculator>();
builder.Services.AddScoped<SuccessRateCalculator>();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
