function randomStars() {
    return Math.floor(Math.random() * 5) + 1;
}

function randomMoney() {
    return Math.floor(Math.random() * 11) + 10;
}

function displayStars(count) {
    const starsDiv = document.getElementById('stars');
    starsDiv.innerHTML = '';
    for (let i = 0; i < count; i++) {
        starsDiv.innerHTML += '<span class="star">★</span>';
    }
    document.getElementById('starsInput').value = count;
}

function animateMoney(count) {
    const moneySpan = document.getElementById('money');
    let current = 0;
    const interval = setInterval(() => {
        if (current >= count) {
            clearInterval(interval);
        }
        moneySpan.textContent = current;
        current++;
    }, 50);
    document.getElementById('moneyInput').value = count;
}

const stars = randomStars();
const money = randomMoney();
displayStars(stars);
animateMoney(money);

const form = document.getElementById('completeLevelForm');
const modal = document.getElementById('modal');
const closeModal = document.getElementById('closeModal');
const toggleButtons = document.querySelectorAll('.toggle-btn');

form.addEventListener('submit', (e) => {
    e.preventDefault();
    modal.style.display = 'flex';
});

closeModal.addEventListener('click', () => {
    modal.style.display = 'none';
    form.submit();
});

toggleButtons.forEach(button => {
    button.addEventListener('click', () => {
        const targetId = button.getAttribute('data-target');
        const content = document.getElementById(targetId);
        const isExpanded = content.classList.toggle('active');
        button.setAttribute('aria-expanded', isExpanded);
    });
});