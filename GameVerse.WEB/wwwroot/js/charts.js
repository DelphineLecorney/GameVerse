window.charts = {};

window.renderPieChart = (canvasId, labels, data, colors) => {
    if (window.charts[canvasId]) {
        window.charts[canvasId].destroy();
    }

    const ctx = document.getElementById(canvasId).getContext('2d');
    window.charts[canvasId] = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: colors,
                borderColor: '#0e1f16',
                borderWidth: 2
            }]
        },
        options: {
            plugins: {
                legend: {
                    labels: { color: '#eafff2' }
                }
            }
        }
    });
};

window.renderBarChart = (canvasId, labels, data, color) => {
    if (window.charts[canvasId]) {
        window.charts[canvasId].destroy();
    }

    const ctx = document.getElementById(canvasId).getContext('2d');
    window.charts[canvasId] = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: color,
                borderRadius: 6
            }]
        },
        options: {
            plugins: { legend: { display: false } },
            scales: {
                x: { ticks: { color: '#a8c4b3' }, grid: { color: '#234d33' } },
                y: { ticks: { color: '#a8c4b3' }, grid: { color: '#234d33' } }
            }
        }
    });
};