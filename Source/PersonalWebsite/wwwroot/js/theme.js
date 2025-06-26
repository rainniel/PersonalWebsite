window.Theme = {
    init: function () {
        if (localStorage.theme == null || !(localStorage.theme != 'dark' || localStorage.theme != 'light')) {
            if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
                localStorage.theme = "dark";
            } else {
                localStorage.theme = "light";
            }
        }

        const isDark = localStorage.theme == 'dark';
        this.setTheme(isDark);
    },
    toggle: function () {
        const isDark = localStorage.theme != 'dark';

        this.setIcon(isDark);
        this.setTheme(isDark);
        localStorage.theme = isDark ? 'dark' : 'light';
    },
    showIcon: function () {
        this.setIcon(localStorage.theme == 'dark');
    },
    setIcon: function (isDark) {
        const lightIcon = document.getElementById('light-icon');
        const darkIcon = document.getElementById('dark-icon');

        if (lightIcon && darkIcon) {
            lightIcon.style.display = isDark ? 'none' : 'block';
            darkIcon.style.display = isDark ? 'block' : 'none';
        }
    },
    setTheme: function (isDark) {
        if (isDark) {
            document.documentElement.classList.add('dark');
        } else {
            document.documentElement.classList.remove('dark');
        }
    }
};

Theme.init();
document.addEventListener("DOMContentLoaded", function () {
    Theme.showIcon();
});
