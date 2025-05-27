const connection = new signalR.HubConnectionBuilder()
    .withUrl("/battleHub")
    .withAutomaticReconnect()
    .build();


    document.addEventListener('DOMContentLoaded', () => {

    // Підключення до хабу
    connection.start()
        .then(() => {
            console.log("Connected to hub");
            // Отримання списку кімнат після підключення
            connection.invoke("GetAvailableRooms")
                .catch(err => console.error(err));
        })
        .catch(err => console.error(err));

    // Обробник події отримання списку кімнат
    connection.on("AvailableRooms", (rooms) => {
        console.log("Received rooms:", rooms);
        updateRoomsList(rooms);
    });
    

 

    

    // Додаємо обробник для кнопки створення кімнати
    const createRoomButton = document.getElementById('createRoom');
    if (createRoomButton) {
        createRoomButton.addEventListener('click', () => {
            connection.invoke("CreateBattleRoom")
                    .catch(err => console.error(err));
        });
    } else {
        console.error('Create room button not found');
    }
});
window.addEventListener("beforeunload", async () => {
    const roomId = localStorage.getItem("roomId"); // або інший спосіб отримати roomId
    if (!roomId) return;

    const data = new FormData();
    data.append("roomId", roomId);

    navigator.sendBeacon("/Battle/EndBattleOnClose", data);
});


async function updateRoomsList(rooms) {
    const roomsList = document.getElementById('availableRooms');
    if (!roomsList) {
        console.error('Available rooms list not found');
        return;
    }

    roomsList.innerHTML = '';

    for (const room of rooms) {
        try {
            const st = await getStudentById(room.player1Id);
            const li = document.createElement('li');
            li.textContent = `${st.username}'s room`;

            const joinButton = document.createElement('button');
            joinButton.textContent = 'Join';
            joinButton.onclick = () => joinRoom(room.roomId);

            li.appendChild(joinButton);
            roomsList.appendChild(li);
        } catch (err) {
            console.error(`Не вдалося завантажити студента ${room.player1Id}`, err);
        }
    }
}

async function getStudentById(studentId) {
    try {
        const response = await fetch(`/Battle/GetStudentById?id=${studentId}`);
        if (!response.ok) {
            throw new Error("Помилка при запиті студента");
        }

        const student = await response.json();
        console.log("Отримано студента:", student);
        return student;
    } catch (error) {
        console.error("Помилка:", error);
        return null;
    }
}
async function joinRoom(roomId) {
    try {
        await connection.invoke("JoinBattleRoom", roomId);
   

    } catch (err) {
        console.error(err);
    }
}

connection.on("BattleRoomCreated", (room) => {
    console.log("Battle room created:", room);
    document.getElementById('createRoomContainer').innerHTML = 'Waiting for player...';
    localStorage.setItem("roomId", room.roomId);
    console.log("created room id:",localStorage.getItem("roomId"));


});

connection.on("PlayerJoined", (room) => {
    
    console.log("Player joined:", room);
    //window.location.href = "/Battle/ExecuteBattle";
    
    postAndRedirect('/Battle/ExecuteBattle', room)
    updateBattleUI(room);
    
});
function postAndRedirect(url, room) {
    const form = document.createElement('form');
    form.method = 'POST';
    form.action = url;

    // Для безпеки, якщо використовуєте [ValidateAntiForgeryToken]
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    if (token) {
        const tokenInput = document.createElement('input');
        tokenInput.type = 'hidden';
        tokenInput.name = '__RequestVerificationToken';
        tokenInput.value = token.value;
        form.appendChild(tokenInput);
    }

    // Додати всі поля room у форму
    for (const key in room) {
        if (room.hasOwnProperty(key)) {
            const input = document.createElement('input');
            input.type = 'hidden';
            input.name = key;
            input.value = room[key];
            form.appendChild(input);
        }
    }

    document.body.appendChild(form);
    form.submit();
}

function updateBattleUI(room) {
    document.getElementById('player1Score').textContent = room.player1Score;
    document.getElementById('player2Score').textContent = room.player2Score;

    if (room.status === 'InProgress') {
        startBattle(room);
    }
}


function deserializeTests(jsonString) {
    try {
        // Десеріалізуємо основний JSON
        const tests = JSON.parse(jsonString);

        // Обробляємо AnswerVariants для кожного тесту
        const parsedTests = tests.map(test => {
            try {
                // Очищаємо рядок і парсимо AnswerVariants у масив
                const cleanAnswerVariants = test.AnswerVariants.replace(/\n\s*/g, '');
                const answerVariants = JSON.parse(cleanAnswerVariants);
                return {
                    ...test,
                    AnswerVariants: answerVariants
                };
            } catch (error) {
                console.error(`Error parsing AnswerVariants for TestID ${test.TestID}:`, error);
                return test;
            }
        });

        return parsedTests;
    } catch (error) {
        console.error("Error parsing JSON:", error);
        return [];
    }
}

