document.addEventListener('DOMContentLoaded', () => {
    
    //Відкривання модального вікна при натисканні на осрови
    const islands = document.querySelectorAll('.firstIsland, .secondIsland, .thirdIsland, .fourthIsland');
    const sidebar = document.querySelector('#modalWindow');
    const islandImages = document.querySelectorAll('.islandFirstLvl, .islandSecondLvl, .islandThirdLvl, .islandFourthLvl');

    if (islandImages && sidebar) {
        islandImages.forEach(image => {
            image.addEventListener('click', (e) => {
                e.preventDefault();
                sidebar.classList.add('active');
                islands.forEach(island => {
                    const islandRect = island.getBoundingClientRect();
                    if (islandRect.right > window.innerWidth - 300) {
                        island.classList.add('shifted');
                    }
                });
            });
        });
    }

    //Закриття модального вікна
    window.closeSidebar = function() {
        if (sidebar) {
            sidebar.classList.remove('active');
            // Повертаємо острови на місце
            islands.forEach(island => {
                island.classList.remove('shifted');
            });
        }
    };
});