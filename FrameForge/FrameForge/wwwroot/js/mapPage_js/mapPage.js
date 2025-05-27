document.addEventListener('DOMContentLoaded', () => {
    const islands = document.querySelectorAll('.firstIsland, .secondIsland, .thirdIsland, .fourthIsland, .fifthIsland');
    const sidebar = document.querySelector('#modalWindow');
    const islandImages = document.querySelectorAll('.islandFirstLvl, .islandSecondLvl, .islandThirdLvl, .islandFourthLvl, .islandFifthLvl');

    // Функція для визначення тексту залежно від острова
    const getModalContent = (islandClass) => {
        switch (islandClass) {
            case 'firstIsland':
                return {
                    title: 'Перший рівень',
                    description: 'Вступ до Компютерної графіки\n',
                    link: '/ProgressMap/ViewLevel?levelName=CG_IntroductionLevel'
                };
            case 'secondIsland':
                return {
                    title: 'Другий рівень',
                    description: 'Криві Безьє. \n' +
                        '    Задання та побудова кривих\n',
                    link: '/ProgressMap/ViewLevel?levelName=CG_Level2'
                };
            case 'thirdIsland':
                return {
                    title: 'Третій рівень',
                    description: 'Фрактали. \n' +
                        '    Класифікація фракталів та їх особливості\n',
                    link: '/ProgressMap/ViewLevel?levelName=CG_Level3'
                };
            case 'fourthIsland':
                return {
                    title: 'Четвертий рівень',
                    description: 'Колірні моделі. \n' +
                    'Основні колірні моделі, та задання кольору з їх допомогою',
                    link: '/ProgressMap/ViewLevel?levelName=CG_Level4'
                };
            case 'fifthIsland':
                return {
                    title: "П'ятий рівень",
                    description: "Рухомі зображення. \n" +
                    'Створення рухомих зображень за допомогою Афінних перетворень',
                    link: '/ProgressMap/ViewLevel?levelName=CG_Level5'
                }
        }
    };

    // Перевірка наявності елементів
    if (islandImages && sidebar) {
        islandImages.forEach((image, index) => {
            image.addEventListener('click', (e) => {
                e.preventDefault();
                const island = islands[index];
                const islandClass = island.classList[0];

                const link = island.querySelector('a');
                if (!link.classList.contains('completed') && !link.classList.contains('in-progress')) {
                    return;
                }
                
                //Стилі для виконаного рівня
                const img = document.getElementById('complete')
                if(link.classList.contains('completed')) {

                    img.style.display = 'inline-block';
                }
                else{
                    img.style.display = 'none'
                }
                
                //Для кожного острова свій текст
                const content = getModalContent(islandClass);
                document.querySelector('.modalWindowContent h1').textContent = content.title;
                document.querySelector('.modalWindowContent p').textContent = content.description;

                const startButton = document.querySelector('#startLevelButton');
                startButton.setAttribute('href', content.link);

                sidebar.classList.add('active');

                //Зміщення островів
                // islands.forEach(island => {
                //     const islandRect = island.getBoundingClientRect();
                //     if (islandRect.right > window.innerWidth - 320) {
                //         island.classList.add('shifted');
                //     }
                // });

                islands.forEach(island => {
                    island.classList.remove('zoomed');
                });

                island.classList.add('zoomed');
            });
        });
    }

    // Закриття модального вікна
    window.closeSidebar = function () {
        if (sidebar) {
            sidebar.classList.remove('active');
            islands.forEach(island => {
                island.classList.remove('shifted');
            });
        }
    };

    // Закриття при кліку поза модальним вікном
    document.addEventListener('click', (e) => {
        if (
            sidebar.classList.contains('active') &&
            !sidebar.contains(e.target) &&
            !e.target.closest('.islandFirstLvl, .islandSecondLvl, .islandThirdLvl, .islandFourthLvl, .islandFourthLvl, .islandFifthLvl')
        ) {
            closeSidebar();
            islands.forEach(img => {
                img.classList.remove('zoomed');
            });
        }
    });
});