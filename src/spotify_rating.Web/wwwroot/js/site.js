$(document).ready(function () {

    toastr.options = {
        timeOut: 2500,
        hideDuration: 300,
        showDuration: 300,
        showEasing: "swing",
        hideEasing: "linear"
    };

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

    $("#btnClearCache").click(() => {
        localStorage.removeItem("likedTracks");
        toastr.warning("Cache cleared.");
        location.reload();
    });

    $("#btnRefresh").click(() => {
        currentIndex = 0;
        if (typeof loadTracks === "function") {
            loadTracks();
            toastr.success("Refreshed tracks.");
        }
    });

    function updateThemeIcon(theme) {
        const icon = $("#themeIcon");
        if (theme === "dark") {
            icon.removeClass().addClass("fa-solid fa-moon");
        } else {
            icon.removeClass().addClass("fa-solid fa-sun");
        }
    }
});
