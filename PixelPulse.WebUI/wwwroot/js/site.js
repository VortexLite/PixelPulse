window.addEventListener("load", toggleMenuClasses);
window.addEventListener("resize", toggleMenuClasses);

document.addEventListener("DOMContentLoaded", function () {
    setupCatalogMenu(".catalog", ".catalogMenu");
    const categories = document.querySelectorAll(".mainCategory li");

    categories.forEach(category => {
        category.addEventListener("click", function () {
            if (!isMobile()) {
                toggleCatalog(category.id);
            }
        });
    });

    setupMenuToggle(".menu1", ".menu2");
});

function isMobile() {
    return window.matchMedia("(max-width: 767px)").matches;
}

function setupCatalogMenu(buttonSelector, menuSelector) {
    const catalogButton = document.querySelector(buttonSelector);
    const catalogMenu = document.querySelector(menuSelector);

    if (!catalogButton || !catalogMenu) return;

    catalogButton.addEventListener("click", function (event) {
        event.stopPropagation();

        if (catalogMenu.classList.contains("active")) {
            closeMenu(catalogMenu);
        } else {
            openMenu(catalogMenu);
        }
    });

    document.addEventListener("click", function (event) {
        if (!catalogButton.contains(event.target) && !catalogMenu.contains(event.target)) {
            closeMenu(catalogMenu);
        }
    });
}

function openMenu(menu) {
    menu.classList.add("active");
}

function closeMenu(menu) {
    menu.classList.add("hiding");
    setTimeout(() => {
        menu.classList.remove("active", "hiding");
    }, 300);
}

function toggleCatalog(categoryId) {
    const catalogs = document.querySelectorAll(".catalogMenu > div");

    catalogs.forEach(catalog => {
        catalog.style.display = "none";
    });

    const activeCatalog = document.querySelector("." + categoryId + "Catalog");
    if (activeCatalog) {
        activeCatalog.style.display = "grid";
    }
}

function toggleMenuClasses() {
    const menu = document.querySelector("ul.menu");
    const menu1 = document.querySelector("ul.menu1");

    if (!menu || !menu1) return;

    if (window.matchMedia("(max-width: 767px)").matches) {
        menu.style.display = "none";
        menu1.style.display = "grid";
    } else {
        menu.style.display = "grid";
        menu1.style.display = "none";
    }
}

function setupMenuToggle(toggleSelector, menuSelector) {
    const toggleButton = document.querySelector(toggleSelector);
    const menu = document.querySelector(menuSelector);

    if (!toggleButton || !menu) return;

    toggleButton.addEventListener("click", function (event) {
        event.stopPropagation();

        if (menu.classList.contains("active")) {
            closeMenu(menu);
        } else {
            openMenu(menu);
        }
    });

    document.addEventListener("click", function (event) {
        if (!toggleButton.contains(event.target) && !menu.contains(event.target)) {
            closeMenu(menu);
        }
    });
}