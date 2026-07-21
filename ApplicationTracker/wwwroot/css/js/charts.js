window.dashboardCharts = {

    createStatusChart: function (canvasId, data) {

        const ctx = document
            .getElementById(canvasId)
            .getContext("2d");

        new Chart(ctx, {

            type: "bar",

            data: {

                labels: [
                    "Applied",
                    "Interview",
                    "Offer",
                    "Rejected",
                    "Ghost"
                ],

                datasets: [{
                    label: "Liczba aplikacji",
                    data: data
                }]
            },

            options: {
                responsive: true,
                maintainAspectRatio: false
            }

        });
    }
};