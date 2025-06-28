// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Prevent flash on page load by applying dark mode immediately
(function() {
    // Check if dark mode is enabled in localStorage
    if (localStorage.getItem('darkModeEnabled') === 'true') {
        // Apply dark mode class to the document element before the page renders
        document.documentElement.classList.add('dark-mode');
        document.documentElement.setAttribute('data-theme', 'dark');
        document.body.classList.add('dark-mode');
    }
})();

// Dark Mode Functionality
document.addEventListener('DOMContentLoaded', function() {
    // Проверяваме дали има запазена настройка
    const darkModeEnabled = localStorage.getItem('darkModeEnabled') === 'true';
    
    // Прилагаме запазената настройка
    if (darkModeEnabled) {
        document.body.classList.add('dark-mode');
        document.documentElement.classList.add('dark-mode');
        document.documentElement.setAttribute('data-theme', 'dark');
        document.getElementById('darkModeToggle').innerHTML = '<i class="bi bi-sun-fill"></i>';
    }
    
    // Добавяме обработчик на събитието за бутона
    const darkModeToggle = document.getElementById("darkModeToggle");
    if (darkModeToggle) {
        darkModeToggle.addEventListener("click", function () {
            // Превключваме класа
            document.body.classList.toggle("dark-mode");
            document.documentElement.classList.toggle("dark-mode");
            
            // Превключваме data-theme атрибута
            const isDarkMode = document.body.classList.contains('dark-mode');
            if (isDarkMode) {
                document.documentElement.setAttribute('data-theme', 'dark');
            } else {
                document.documentElement.removeAttribute('data-theme');
            }
            
            // Запазваме настройката
            localStorage.setItem('darkModeEnabled', isDarkMode);
            
            // Променяме иконата на бутона
            this.innerHTML = isDarkMode ? '<i class="bi bi-sun-fill"></i>' : '<i class="bi bi-moon-fill"></i>';
        });
    }
});
