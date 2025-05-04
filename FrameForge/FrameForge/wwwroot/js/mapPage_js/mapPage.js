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
                        '    consequuntur distinctio enim eveniet exercitationem illum incidunt inventore ipsam iure, maxime officiis\n',
                    link: '/ProgressMap/ViewLevel?levelName=CG_IntroductionLevel'
                };
            case 'secondIsland':
                return {
                    title: 'Другий рівень',
                    description: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Accusamus adipisci aliquam, debitis dignissimos dolor\n' +
                        '    dolores ex excepturi explicabo fugiat ipsum iusto laborum magnam molestias nihil obcaecati perferendis quibusdam\n' +
                        '    recusandae rerum sed totam. Aliquam consequuntur eligendi eos eum illo minus nihil totam vitae? Consectetur cum ea,\n' +
                        '    est id incidunt ipsum minima, necessitatibus neque nihil nobis obcaecati odit tempore temporibus ut veniam.',
                    link: '/ProgressMap/ViewLevel?levelName=CG_Level2'
                };
            case 'thirdIsland':
                return {
                    title: 'Третій рівень',
                    description: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. A ad, deserunt ea esse iusto magni natus placeat\n' +
                        '    praesentium tempore. Accusantium alias dolorem illo inventore itaque iusto necessitatibus perspiciatis porro\n' +
                        '    veritatis',
                    link: '/ProgressMap/ViewLevel?levelName=CG_Level3'
                };
            case 'fourthIsland':
                return {
                    title: 'Четвертий рівень',
                    description: 'Опис четвертого рівня',
                    link: '/ProgressMap/ViewLevel?levelName=CG_Level4'
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
                
                const link = island.querySelector('a');
                if (!link.classList.contains('completed') && !link.classList.contains('in-progress')) {
                    return;
                }
                
                //Для кожного остврова свій текст
                const content = getModalContent(islandClass);
                document.querySelector('.modalWindowContent h1').textContent = content.title;
                document.querySelector('.modalWindowContent p').textContent = content.description;

                const startButton = document.querySelector('#startLevelButton');
                startButton.setAttribute('href', content.link);
                
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



    document.querySelectorAll('.firstIsland, .secondIsland, .thirdIsland, .fourthIsland').forEach(img => {
        img.addEventListener('click', () => {
            img.classList.toggle('zoomed');
        });
    });
    //Закриття при кліку поза модальним вікном
    document.addEventListener('click', (e) => {
        if (
            sidebar.classList.contains('active') &&
            !sidebar.contains(e.target) &&
            !e.target.closest('.islandFirstLvl, .islandSecondLvl, .islandThirdLvl, .islandFourthLvl')
        ) {
            closeSidebar();
                // Видаляємо клас zoomed з усіх зображень
                document.querySelectorAll('.firstIsland.zoomed , .secondIsland, .thirdIsland, .fourthIsland').forEach(img => {
                    img.classList.remove('zoomed');
                });
        }
    });
});