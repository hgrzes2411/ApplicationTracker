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
        await _js.InvokeVoidAsync(
            "dashboardCharts.createStatusChart",
            canvasId,
            data);
    }
}