# 🎨 FrameForge

**FrameForge** is a modern, digitalized and gamified web-based educational platform designed to help students explore and understand key concepts in **Computer Graphics** through interactive tools, visualizations, and coding experiments.

---

## 🚀 Features

- 📊 Learn the basics of Computer Graphocs with our progress map system
- ✏️ Complete daily tasks to receive points and stars
- ⚔️ Fight with other students in CG knowledge
- 📈 Move up in the leaderboard to become a geek in Computer Graphics
- 📁 Organized modules covering:
  - Introduction to Computer Graphics
  - Bezier Curve
  - Fractals
  - Colour Models
  - Moving Images
- 📷 Upload and manipulate images (coming soon)
- 💬 Collaboration and comment system for students and instructors (coming soon)

---

## 🌐 Live Demo

👉 *Coming soon*

---

## 🏗️ Tech Stack

- **Backend**: ASP.NET Core MVC
- **Frontend**: Razor Pages, JavaScript, HTML5 Canvas
- **Database**: Entity Framework Core with MySQL Server
- **Testing**: xUnit
- **Build Tooling**: MSBuild, GitHub Actions (CI/CD – planned)

---

## 📦 Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/nazarsokal/FrameForge.git
   cd FrameForge
   ```

2. **Set up the database**
   - Configure your `appsettings.json` or `secrets.json` with your connection string.
   - Run the initial migration:
     ```bash
     dotnet ef database update
     ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Open in browser**
   Navigate to `https://localhost:5118`

---

## 🧪 Running Tests

```bash
dotnet test
```

> ✅ Make sure your database is up and seeded before running tests.

---

## 🧑‍💻 Contributing

We welcome contributions from the community! Here's how you can help:

- Report bugs and request features via [Issues](https://github.com/nazarsokal/FrameForge/issues)
- Fork the repository, make changes in a separate branch, and open a **Pull Request**
- Follow our branch naming convention: `feature/*`, `bugfix/*`, `docs/*`

> 🔁 All changes should go through a PR review process before merging into `develop`.

---

## 🗺️ Project Structure

```
FrameForge/
├── Controllers/         # API and page logic
├── Views/               # Razor views
├── wwwroot/             # Static assets (JS, CSS, images)
├── Entities/            # Database models
├── Services/            # Business logic
├── ServiceContracts/    # Interfaces for DI
├── ServiceTests/        # xUnit tests
├── FrameForge.csproj
└── README.md
```

---

## 📜 License

MIT License. See `LICENSE` for more information.

---

## 📧 Contact

Built with 💻 by:   
[Nazar Sokalchuk](https://github.com/nazarsokal) - BackEnd      
[Nazar Novosilets](https://github.com/NazarNovosilets) - FrontEnd     
[Oleksandr Herhel](https://github.com/smurfik1488) - BackEnd     
[Serhiy Matrohin](https://github.com/serhiy28) - FrontEnd    
