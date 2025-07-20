# Human Capital Management System (HCMS)

A modular and secure Human Capital Management System built with ASP.NET Core, Entity Framework Core, and a clean multi-layered architecture. This system allows administrators to manage departments, employees, and user accounts, with role-based access and JWT authentication.

---

## 📂 Project Structure

- Core: Business logic, DTOs, service contracts

- Infrastructure: EF Core models, context, and repositories

- BackendAPI: API controllers, API service, auth, Swagger

- UI: Razor views, controllers, UI services, JWT session handling

---

## 🚀 Features

- 🔐 **Authentication & Authorization** using **JWT** and **ASP.NET Identity**
- 👥 Role-based access control (Employee, Manager, HRAdmin)
- 🧑‍💼 Employee & Department Management
- 🌐 RESTful API with full **Swagger** documentation
- 🎨 MVC UI with session-based JWT storage
- 📦 Clean separation of concerns via layered architecture

---

## 🛠️ Tech Stack

- **Backend**: ASP.NET Core Web API (.NET 9)
- **Frontend**: ASP.NET Core MVC
- **Authentication**: JWT, ASP.NET Identity
- **Database**: SQL Server with EF Core
- **Documentation**: Swagger / Swashbuckle
- **Dependency Injection** throughout

---

## 👥 Roles and Permissions

### HR Admin
- Full access to managing employees and departments.
- Can create, update, and delete records.
- Has access to all features.

### Manager
- Can view and update employees within their own department.
- Cannot manage other departments or perform system-wide admin tasks.

### Employee
- Can only view their own profile details.
- No administrative rights.

---

## 📸 Screenshots

### Login Page

![Login Page](https://github.com/user-attachments/assets/bf0d44c3-c295-400e-a583-0f2b4047f8a8)

---

### HR Admin Dashboard

![HR Admin Dashboard](https://github.com/user-attachments/assets/ef788d1f-4bdc-4635-a4e0-f5d3c5a02969)

---

## 🔧 Setup Instructions

### 1. Clone the repository

```bash
git clone https://github.com/MarioTonchev/human-capital-management-system.git hcms
cd hcms
```

### 2. Configure your connection strings
Update appsettings.json in HCMS.BackendAPI with your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=HCMS;Trusted_Connection=True;"
}
```

### 3. Run Database Migrations & Seed
Ensure your database is created and seeded:

```bash
cd HCMS.BackendAPI
dotnet ef database update
```

The app seeds initial users, roles, and departments on startup.

### 4. Run the application
You have to run both projects in parallel:

- HCMS.BackendAPI (API at https://localhost:7248/)
- HCMS.UI (MVC at https://localhost:7039/)

Use Visual Studio or run with the CLI commands:
```bash
dotnet run --project HCMS.BackendAPI
dotnet run --project HCMS.UI
```

---

## 🔑 Seeded HRAdmin Account

The application seeds several user accounts on startup.  
You can log in as an HRAdmin with the following credentials:

- **Username:** admin  
- **Password:** a12345

---

## 🧪 API Documentation
Once the API is running, open:

```bash
https://localhost:7248/
```

From there you can:

- View all endpoints

- Authorize via JWT

- Send test requests

---

## 🔐 Authentication Notes

- You must first register and login to obtain a JWT token

- Use the token with Authorization: Bearer <your_token> in Swagger or your frontend

- JWT is stored in session on the MVC app and auto-injected
