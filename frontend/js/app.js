const apiUrl = "https://localhost:5001/api/todo"; // adjust port if needed

document.getElementById("todo-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    const title = document.getElementById("todo-title").value;

    const response = await fetch(apiUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ title, isCompleted: false })
    });

    if (response.ok) {
        document.getElementById("todo-title").value = "";
        loadTodos();
    }
});

async function loadTodos() {
    const response = await fetch(apiUrl);
    const todos = await response.json();

    const list = document.getElementById("todo-list");
    list.innerHTML = "";

    todos.forEach(todo => {
        const li = document.createElement("li");
        li.textContent = todo.title;
        if (todo.isCompleted) li.classList.add("completed");

        // Toggle button
        const toggleBtn = document.createElement("button");
        toggleBtn.textContent = todo.isCompleted ? "Undo" : "Complete";
        toggleBtn.onclick = async () => {
            await fetch(`${apiUrl}/${todo.id}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ ...todo, isCompleted: !todo.isCompleted })
            });
            loadTodos();
        };

        // Delete button
        const deleteBtn = document.createElement("button");
        deleteBtn.textContent = "Delete";
        deleteBtn.onclick = async () => {
            await fetch(`${apiUrl}/${todo.id}`, { method: "DELETE" });
            loadTodos();
        };

        li.appendChild(toggleBtn);
        li.appendChild(deleteBtn);
        list.appendChild(li);
    });
}

loadTodos();