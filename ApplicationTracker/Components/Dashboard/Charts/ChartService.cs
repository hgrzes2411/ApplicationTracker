using Microsoft.JSInterop;

namespace ApplicationTracker.Services.Charts;

public class ChartService
{
    private readonly IJSRuntime _js;

    public ChartService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task RenderStatusChartAsync(
        string canvasId,
        int[] data)
    {
        // Import the JS module dynamically to ensure the code is loaded
        var module = await _js.InvokeAsync<IJSObjectReference>("import", "./js/dashboardCharts.js");
        await module.InvokeVoidAsync("createStatusChart", canvasId, data);
    }
}