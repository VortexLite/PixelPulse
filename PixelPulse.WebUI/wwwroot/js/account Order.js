document.addEventListener("DOMContentLoaded", function () {
    const orderRows = document.querySelectorAll(".order-row");

    orderRows.forEach(row => {
        row.addEventListener("click", function () {
            let nextRow = row.nextElementSibling;

            while (nextRow && nextRow.classList.contains("customerOrdersDetails")) {
                nextRow.classList.toggle("active");
                nextRow.classList.toggle("hidden");
                nextRow = nextRow.nextElementSibling;
            }
        });
    });
});