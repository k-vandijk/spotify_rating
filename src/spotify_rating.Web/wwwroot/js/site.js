$(document).ready(function () {

    const themeCacheKey = "theme";

    const savedTheme = localStorage.getItem(themeCacheKey) || "dark";
    $("body").addClass(savedTheme);
    updateThemeIcon(savedTheme);

    $("#btnToggleTheme").click(function () {
        $("body").toggleClass("dark light");
        const mode = $("body").hasClass("dark") ? "dark" : "light";
        localStorage.setItem(themeCacheKey, mode);
        updateThemeIcon(mode);
    });

    function updateThemeIcon(theme) {
        const icon = $("#themeIcon");
        if (theme === "dark") {
            icon.removeClass().addClass("fa-solid fa-moon me-2");
        } else {
            icon.removeClass().addClass("fa-solid fa-sun me-2");
        }
    }

});

const timeOptions = {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    hour12: false,
};