// Placeholder data for tasks, levels, and match history (replace with backend data)
const userData = {
    completedTasks: [
        { name: "Відредагуйте код", points: 10, coins: 250 },
        { name: "Відредагуйте код", points: 25, coins: 500 },
        { name: "Відредагуйте код", points: 50, coins: 1000 }
    ],
    completedLevels: [
        { name: "CG_IntroductionLevel", points: 20, coins: 300 },
        { name: "CG_Level2", points: 30, coins: 450 }
    ],
    inProgressLevels: [
        { name: "CG_Level3", points: 20, coins: 300 },
        { name: "CG_Level4", points: 20, coins: 300 }
    ],

    matchHistory: [
        { opponent: "User1", result: "won", date: "2025-05-20" },
        { opponent: "User2", result: "lost", date: "2025-05-19" },
        { opponent: "User3", result: "won", date: "2025-05-18" }
    ]
};

/*// Populate Completed Tasks
const completedTasksList = document.getElementById("completed-tasks");
userData.completedTasks.forEach(task => {
    const div = document.createElement("div");
    div.className = "stat-card";
    div.innerHTML = `
       <a asp-controller="Placeholder" asp-action="Placeholder">
        <span class="lvlMode easy">Easy</span>
            <span class="level-name">${task.name}</span>
            <div class="level-rewards">
            <span class="lvlMode easy">Easy</span>
            <p>dadasdadasjlkasfdlkjasfjlafjl</p>
          <span>${task.points} зірочок <img src="images/icons_mainPage/star.png" alt="star"></span>
                <span>${task.coins} монет <img src="images/icons_mainPage/coin.png" alt="coin"></span>
               </div>
        </a>
    `;
    completedTasksList.appendChild(div);
});*/

// Populate Completed Levels
const completedLevelsList = document.getElementById("completed-levels");
userData.completedLevels.forEach(level => {
    const div = document.createElement("div");
    div.className = "stat-card";
    div.innerHTML = `
        <a asp-controller="Placeholder" asp-action="Placeholder">
            <span class="level-name">${level.name}</span>
            <div class="level-rewards">
                <span>${level.points} зірочок <img src="images/icons_mainPage/star.png" alt="star"></span>
                <span>${level.coins} монет <img src="images/icons_mainPage/coin.png" alt="coin"></span>
            </div>
        </a>
    `;
    completedLevelsList.appendChild(div);
});

// Populate In-Progress Levels
const inProgressLevelsList = document.getElementById("in-progress-levels");
userData.inProgressLevels.forEach(level => {
    const div = document.createElement("div");
    div.className = "stat-card";
    div.innerHTML = `
        <a asp-controller="Placeholder" asp-action="Placeholder">
            <span class="level-name">${level.name}</span>
            <div class="level-rewards">
                <span>${level.points} зірочок <img src="images/icons_mainPage/star.png" alt="star"></span>
                <span>${level.coins} монет <img src="images/icons_mainPage/coin.png" alt="coin"></span>
            </div>
        </a>
    `;
    inProgressLevelsList.appendChild(div);
});

// Calculate and Populate Match History
const matchHistoryDiv = document.getElementById("match-history");
const totalMatches = userData.matchHistory.length;
const wins = userData.matchHistory.filter(match => match.result === "won").length;
const winPercentage = totalMatches > 0 ? ((wins / totalMatches) * 100).toFixed(2) : 0;
const lossPercentage = totalMatches > 0 ? (((totalMatches - wins) / totalMatches) * 100).toFixed(2) : 0;

document.getElementById("win-percentage").textContent = `${winPercentage}%`;
document.getElementById("loss-percentage").textContent = `${lossPercentage}%`;

userData.matchHistory.forEach(match => {
    const div = document.createElement("div");
    div.className = `match ${match.result}`;
    div.innerHTML = `
        <span>Матч проти ${match.opponent}</span>
        <span>${match.result === "won" ? "Перемога" : "Поразка"}</span>
        <span>${match.date}</span>
    `;
    matchHistoryDiv.appendChild(div);
});