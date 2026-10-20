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

  const labels = ['Applied', 'Interview', 'Offer', 'Rejected', 'Ghost'];
  const backgroundColors = [
    '#38bdf8', '#a78bfa', '#22c55e', '#ef4444', '#f59e0b'
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
      maintainAspectRatio: false,
      plugins: {
        legend: {
          labels: {
            color: '#cbd5e1',
            font: { family: 'Inter, Segoe UI, sans-serif' }
          }
        }
      }
    }
  });
}
