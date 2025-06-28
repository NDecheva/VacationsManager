# 🌴 VacationsManagerMVC

A professional and modern web application for managing employee vacation requests within a company. Built using the ASP.NET MVC framework, the system supports multiple user roles, automated workflows, and rich administrative capabilities.

---

## 📌 Overview

**VacationManagerMVC** simplifies the process of handling vacation requests by providing:

- Role-based access control
- Project and team management
- Automated vacation approvals
- Notification system
- Clean and responsive UI with Bootstrap Lux Theme

---

## 👤 User Roles

- **CEO** – Full administrative rights
- **Team Lead** – Manages team - requests, projects and members
- **Developer** – Can request vacations

---

## 🔧 Core Features

- ✍️ Submit and manage vacation requests (half-day, full-day, sick leave, etc.)
- 🛠 Approve/decline requests based on role
- 🧑‍💼 Manage users, teams, and roles
- 📁 Project and team assignments
- 🔔 Notifications for leave status updates
- 📄 Attachment support
- 🛡 Secure authentication and authorization system

---

## 🧪 Testing

Unit tests are included for critical service logic using xUnit and Moq.

---

## 🧠 Technologies Used

- ⚙️ **ASP.NET Core MVC** – Framework for the web layer
- 🧵 **Entity Framework Core** – ORM for database operations
- 🧭 **AutoMapper** – Object-object mapping
- 💾 **SQL Server** – Relational database
- 🎨 **Bootstrap Lux Theme** – Stylish and responsive UI
- 🧪 **xUnit** + **Moq** – Unit testing and mocking

---

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/NDecheva/VacationsManager.git
   ```
2. Update connection string in `appsettings.json`
3. Apply migrations and seed the database
4. Run the application:
   ```bash
   dotnet run
   ```

---

## 🗂 Project Structure

```
├── VacationsManagerMVC/       
├── VacationsManager.Data/     
├── VacationsManager.Services/ 
├── VacationsManager.Shared/   
├── VacationManager.Tests/     
```

---

## 📸 UI Highlights

- Responsive layout with Lux theme 🎨
- Clean admin dashboard
- Intuitive form flows for requests 📅
- Visual notification system 🔔

---

## 🙌 Contributions

Feel free to fork, contribute, and suggest improvements!

---

## 📝 License

MIT License

---

## 📬 Contact

Built with ❤️ by Team KIMP, led by Petkata.

> “Streamline your leave management with ease.”
