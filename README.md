**📘 Todo App – Full Stack Documentation**
**🚀 Overview**
This project is a Full Stack Todo Application built with:
• 	Backend: ASP.NET Core Web API (deployed on SmarterASP.NET)
• 	Frontend: Static HTML, CSS, and JavaScript (deployed on Netlify / Cloudflare Pages)
• 	Database: SQL Server (remote hosting)
It demonstrates CRUD operations (Create, Read, Update, Delete) with a clean separation of backend and frontend.

**📂 Folder Structure**
  ToDoApp-FullStack/
│
├── backend/                # ASP.NET Core Web API
│   ├── Controllers/        # TodoController
│   ├── Models/             # Todo entity
│   ├── Data/               # EF Core DbContext
│   ├── Program.cs          # App startup & CORS config
│   └── ...                 # Other backend files
│
├── frontend/               # Frontend app
│   ├── index.html          # Main UI
│   ├── style.css           # Styling
│   ├── app.js              # JS logic (API calls, UI updates)
│   └── assets/             # (optional) images/icons
│
└── README.md               # Documentation

**⚙️ Backend Setup**
1. 	Clone repo and open  in Visual Studio.
2. 	Configure SQL Server connection string in .
3. 	Apply migrations:

4. 	Run locally:

5. 	Deploy to SmarterASP.NET (Release mode).
**🔒 CORS Configuration
In :Program.cs:**
  builder.Services.AddCors(options =>
  {
      options.AddPolicy("AllowFrontend",
          policy => policy
              .WithOrigins(
                  "https://<your-netlify-site>.netlify.app",
                  "https://<your-cloudflare-site>.pages.dev",
                  "http://localhost:5500"
              )
              .AllowAnyHeader()
              .AllowAnyMethod());
  });

var app = builder.Build();
app.UseCors("AllowFrontend");
app.MapControllers();
app.Run();


**🎨 Frontend Setup**
1. 	Place , , and  inside .
2. 	In , set API base:
const API_BASE = "yourapibase";
3. 	Deploy to Netlify or Cloudflare Pages:
• 	Root directory: 
• 	Build command: 
• 	Publish directory: 

**🌐 Deployment**
• 	Backend → SmarterASP.NET
• 	Frontend → Netlify () or Cloudflare Pages ()
• 	Ensure backend CORS allows both frontend domains.

**🛠️ Features**
• 	✅ List all Todos
• 	✅ Get Todo by ID
• 	✅ Create new Todo
• 	✅ Update Todo status
• 	✅ Delete Todo

**🔮 Future Roadmap**
This project will evolve into Read Todo App with:
• 	🔑 Authentication: Login with Gmail (OAuth 2.0 / Google Identity)
• 	👤 User Accounts: Each user manages their own Todos
• 	📖 Read Mode: Ability to mark Todos as “read” or “archived”
• 	📱 Responsive UI: Mobile‑friendly design
• 	🗄️ Improved Folder Structure:
frontend/
  src/
    index.html
    style.css
    app.js
    auth.js        # Gmail login logic
    dashboard.js   # User dashboard
  assets/
backend/
  Controllers/
  Models/
  Services/
  Data/
