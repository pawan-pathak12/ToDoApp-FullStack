// Change this if you expose an HTTP endpoint for dev (e.g., http://localhost:5013)
//const API_BASE = "https://localhost:5012/api/Todo";
const API_BASE = "https://pawandev12-001-site1.anytempurl.com/api/Todo";

// Utilities
const $ = (sel) => document.querySelector(sel);
const $$ = (sel) => document.querySelectorAll(sel);

function showMsg(text) {
  const el = $("#msg");
  if (!text) { el.hidden = true; el.textContent = ""; return; }
  el.hidden = false; el.textContent = text;
}

async function request(method, path = "", body) {
  const opts = { method, headers: { "Content-Type": "application/json", Accept: "application/json" } };
  if (body !== undefined) opts.body = JSON.stringify(body);
  const res = await fetch(`${API_BASE}${path}`, opts);
  if (!res.ok) {
    let msg = `HTTP ${res.status}`;
    try { const data = await res.json(); msg = data?.message || data?.title || msg; } catch {}
    throw new Error(msg);
  }
  if (res.status === 204) return null;
  return res.json();
}

const api = {
  list: () => request("GET"),
  get: (id) => request("GET", `/${id}`),
  create: (title) => request("POST", "", { title, isCompleted: false }),
  // We read first (to keep title unchanged), then PUT full object
  updateCompleted: async (id, isCompleted) => {
    const item = await api.get(id);
    return request("PUT", `/${id}`, { id, title: item.title, isCompleted });
  },
  remove: (id) => request("DELETE", `/${id}`),
};

// Tabs
function switchTab(name) {
  $$(".tab").forEach(b => b.classList.toggle("active", b.dataset.tab === name));
  $$(".panel").forEach(p => p.classList.toggle("active", p.id === `tab-${name}`));
  showMsg("");
}

$$(".tab").forEach(btn => btn.addEventListener("click", () => switchTab(btn.dataset.tab)));

// List panel
async function loadList() {
  try {
    showMsg("");
    const todos = await api.list();
    const ul = $("#todos");
    ul.innerHTML = "";
    if (!todos || todos.length === 0) {
      ul.innerHTML = '<li class="item"><div class="title" style="color:#6b7280">No todos yet.</div></li>';
      return;
    }
    todos.forEach(t => {
      const li = document.createElement("li");
      li.className = `item ${t.isCompleted ? "completed" : ""}`;
      li.innerHTML = `
        <input type="checkbox" ${t.isCompleted ? "checked" : ""} aria-label="toggle" />
        <div class="title">${t.title}</div>
        <button class="btn danger" aria-label="delete">Delete</button>
      `;
      // Toggle
      li.querySelector('input[type="checkbox"]').addEventListener("change", async (e) => {
        try {
          await api.updateCompleted(t.id, e.target.checked);
          await loadList();
        } catch (err) { showMsg(`Update failed: ${err.message}`); e.target.checked = !e.target.checked; }
      });
      // Delete
      li.querySelector(".btn.danger").addEventListener("click", async () => {
        if (!confirm("Delete this todo?")) return;
        try { await api.remove(t.id); await loadList(); }
        catch (err) { showMsg(`Delete failed: ${err.message}`); }
      });
      $("#todos").appendChild(li);
    });
  } catch (err) {
    showMsg(`Failed to load: ${err.message}`);
  }
}
$("#btn-reload").addEventListener("click", loadList);

// Get by ID
$("#form-get").addEventListener("submit", async (e) => {
  e.preventDefault();
  const id = Number($("#get-id").value);
  if (!Number.isFinite(id)) return;
  try {
    showMsg("");
    const data = await api.get(id);
    $("#get-result").textContent = JSON.stringify(data, null, 2);
  } catch (err) {
    $("#get-result").textContent = "";
    showMsg(`Get failed: ${err.message}`);
  }
});

// Add
$("#form-add").addEventListener("submit", async (e) => {
  e.preventDefault();
  const title = $("#add-title").value.trim();
  if (!title) return;
  try {
    showMsg("");
    await api.create(title);
    $("#add-title").value = "";
    showMsg("Todo created.");
    await loadList();
  } catch (err) {
    showMsg(`Create failed: ${err.message}`);
  }
});

// Update status
let loadedForUpdate = null;
$("#form-load").addEventListener("submit", async (e) => {
  e.preventDefault();
  const id = Number($("#upd-id").value);
  if (!Number.isFinite(id)) return;
  try {
    showMsg("");
    const item = await api.get(id);
    loadedForUpdate = item;
    $("#upd-title").textContent = item.title;
    $("#upd-completed").checked = !!item.isCompleted;
    $("#upd-area").hidden = false;
  } catch (err) {
    loadedForUpdate = null;
    $("#upd-area").hidden = true;
    showMsg(`Load failed: ${err.message}`);
  }
});

$("#btn-save").addEventListener("click", async () => {
  if (!loadedForUpdate) return;
  try {
    showMsg("");
    await api.updateCompleted(loadedForUpdate.id, $("#upd-completed").checked);
    showMsg("Updated.");
    await loadList();
  } catch (err) {
    showMsg(`Update failed: ${err.message}`);
  }
});

// Delete
$("#form-del").addEventListener("submit", async (e) => {
  e.preventDefault();
  const id = Number($("#del-id").value);
  if (!Number.isFinite(id)) return;
  if (!confirm("Delete this todo?")) return;
  try {
    showMsg("");
    await api.remove(id);
    $("#del-id").value = "";
    showMsg("Deleted.");
    await loadList();
  } catch (err) {
    showMsg(`Delete failed: ${err.message}`);
  }
});

// Init
switchTab("list");
loadList();