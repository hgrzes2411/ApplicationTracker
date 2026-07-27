export function createStatusChart(canvasId, data) {
  const el = document.getElementById(canvasId);
  if (!el) {
    console.warn(`createStatusChart: element with id '${canvasId}' not found`);
    return;
  }

  const ctx = el.getContext('2d');
  if (!ctx) {
    console.warn('createStatusChart: 2D context not available');
    return;
  }

  if (typeof Chart === 'undefined') {
    console.warn('createStatusChart: Chart.js is not loaded. Please include Chart.js before this module.');
    return;
  }

  // Destroy existing chart instance attached to element to avoid duplicates
  if (el._chartInstance) {
    try { el._chartInstance.destroy(); } catch { }
  }

  const labels = data.map((_, i) => `Item ${i + 1}`);
  const backgroundColors = [
    '#4CAF50', '#F44336', '#FFC107', '#2196F3', '#9C27B0', '#00BCD4', '#8BC34A'
  ];

  el._chartInstance = new Chart(ctx, {
    type: 'doughnut',
    data: {
      labels,
      datasets: [{
        data,
        backgroundColor: data.map((_, i) => backgroundColors[i % backgroundColors.length])
      }]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false
    }
  });
}
