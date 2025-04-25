document.addEventListener('DOMContentLoaded', () => {
    const islands = document.querySelectorAll('.firstIsland, .secondIsland, .thirdIsland, .fourthIsland');
    const sidebar = document.querySelector('#modalWindow');
    const islandImages = document.querySelectorAll('.islandFirstLvl, .islandSecondLvl, .islandThirdLvl, .islandFourthLvl');

    // Функція для визначення тексту залежно від острова
    const getModalContent = (islandClass) => {
        switch (islandClass) {
            case 'firstIsland':
                return {
                    title: 'Перший рівень',
                    description: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Doloremque mollitia quisquam quo sequi similique! Earum hic\n' +
                        '    iure odio sit. Ex rerum soluta voluptas. Culpa eaque hic illum molestiae optio tenetur voluptate voluptatum? Aut\n' +
                        '    consequuntur distinctio enim eveniet exercitationem illum incidunt inventore ipsam iure, maxime officiis\n'
                };
            case 'secondIsland':
                return {
                    title: 'Другий рівень',
                    description: 'Опис другого рівня'
                };
            case 'thirdIsland':
                return {
                    title: 'Третій рівень',
                    description: 'Опис третього рівня'
                };
            case 'fourthIsland':
                return {
                    title: 'Четвертий рівень',
                    description: 'Опис четвертого рівня'
                };
        }
    };

    // Перевірка наявності елементів
    if (islandImages && sidebar) {
        islandImages.forEach((image, index) => {
            image.addEventListener('click', (e) => {
                e.preventDefault();
                const island = islands[index];
                const islandClass = island.classList[0];
                
                //Для кожного остврова свій текст
                const content = getModalContent(islandClass);
                document.querySelector('.modalWindowContent h1').textContent = content.title;
                document.querySelector('.modalWindowContent p').textContent = content.description;
                sidebar.classList.add('active');

                //Зміщення островів
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
    window.closeSidebar = function () {
        if (sidebar) {
            sidebar.classList.remove('active');
            islands.forEach(island => {
                island.classList.remove('shifted');
            });
        }
    };

    //Закриття при кліку поза модальним вікном
    document.addEventListener('click', (e) => {
        if (
            sidebar.classList.contains('active') &&
            !sidebar.contains(e.target) &&
            !e.target.closest('.firstIsland, .secondIsland, .thirdIsland, .fourthIsland')
        ) {
            closeSidebar();
        }
    });
});