# My_POS - Point of Sale System

A modern Point of Sale (POS) system built with a separated Frontend (Vue 3) and Backend (ASP.NET Core 8.0) architecture.

## 🏗️ Project Structure

```
My_POS/
├── Backend_API/          # ASP.NET Core 8.0 API Backend
│   ├── Controllers/      # API Controllers
│   ├── Data/             # Database Context
│   ├── Models/           # Data Models
│   ├── Migrations/       # EF Core Migrations
│   └── README.md         # Backend documentation
│
├── Frontend-vue/         # Vue 3 + Vite Frontend
│   ├── src/
│   │   ├── js/           # Vue components & app logic
│   │   └── css/          # Stylesheets
│   ├── public/           # Static files
│   ├── package.json      # Node dependencies
│   ├── vite.config.js    # Vite configuration
│   └── README.md         # Frontend documentation
│
└── My_POS.sln            # Visual Studio Solution
```

## 🎯 Architecture

This project uses a **fully separated client-server architecture**:

### Backend
- **Technology**: ASP.NET Core 8.0
- **Port**: 5000 (HTTP), 5001 (HTTPS)
- **Database**: SQL Server
- **Role**: RESTful API serving business logic and data

### Frontend
- **Technology**: Vue 3 with Vite
- **Port**: 3000
- **Role**: Interactive user interface

### Communication
- Frontend makes API calls to backend via HTTP/CORS
- No server-side rendering or static file serving from backend
- Fully independent development and deployment

## 🚀 Quick Start

### Backend Setup

```bash
cd Backend_API

# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update

# Run the backend
dotnet run
```

Backend will be available at: `http://localhost:5000/api`

### Frontend Setup

```bash
cd Frontend-vue

# Install dependencies
npm install

# Start development server
npm run dev
```

Frontend will be available at: `http://localhost:3000`

## 📦 Installation & Development

### Prerequisites
- **.NET 8.0 SDK** - [Download](https://dotnet.microsoft.com/download)
- **Node.js v16+** - [Download](https://nodejs.org/)
- **SQL Server** - Local instance or configured connection string

### Step 1: Configure Backend

1. Edit `Backend_API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=POS_DB;Integrated Security=true;"
  }
}
```

2. Run migrations:
```bash
cd Backend_API
dotnet ef database update
```

3. Start backend:
```bash
dotnet run
```

### Step 2: Setup Frontend

1. Install dependencies:
```bash
cd Frontend-vue
npm install
```

2. Create `.env` file:
```bash
cp .env.example .env
```

3. Start frontend:
```bash
npm run dev
```

## 📚 Features

### POS Terminal
- 🛒 Product search and quick add to cart
- 📊 Real-time cart totals with tax calculation
- 👥 Cashier selection
- 💳 Customer information capture
- ✅ Order completion and checkout

### Product Management
- View all available products
- Manage product inventory
- Product pricing

### Order Management
- Create and record sales orders
- View order history
- Order details and line items

### User Management
- Manage cashier/staff accounts
- Track who processed each order

## 🔌 API Endpoints

### Products
```
GET    /api/products           - Get all products
GET    /api/products/{id}      - Get product by ID
POST   /api/products           - Create new product
PUT    /api/products/{id}      - Update product
DELETE /api/products/{id}      - Delete product
```

### Orders
```
GET    /api/orders             - Get all orders
GET    /api/orders/{id}        - Get order by ID
POST   /api/orders             - Create new order
PUT    /api/orders/{id}        - Update order
DELETE /api/orders/{id}        - Delete order
```

### Users
```
GET    /api/users              - Get all users
GET    /api/users/{id}         - Get user by ID
POST   /api/users              - Create new user
PUT    /api/users/{id}         - Update user
DELETE /api/users/{id}         - Delete user
```

## 🛠️ Build & Deployment

### Frontend Build
```bash
cd Frontend-vue
npm run build
```
Output: `Frontend-vue/dist/` - Ready for deployment to any static hosting

### Backend Build
```bash
cd Backend_API
dotnet publish -c Release
```
Output: `Backend_API/bin/Release/net8.0/publish/` - Ready for deployment

## 🔐 CORS Configuration

The backend is configured to accept requests from:
- `http://localhost:3000` (Development frontend)
- `http://localhost:8080` (Alternative)
- `http://localhost:5000` (Same origin)

Modify `Backend_API/Program.cs` to add production domains.

## 📊 Database Schema

### Users Table
- Id (PK)
- FirstName
- LastName
- Email
- CreatedAt

### Products Table
- Id (PK)
- Name
- Description
- Price
- Stock
- CreatedAt

### Orders Table
- Id (PK)
- OrderDate
- Total
- CustomerId
- UserId (FK → Users)

### Order_Items Table
- Id (PK)
- OrderId (FK → Orders)
- ProductId (FK → Products)
- Quantity
- Price

## ⚙️ Environment Variables

### Frontend (`.env`)
```
VITE_API_URL=http://localhost:5000
```

### Backend (`appsettings.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_Connection_String"
  }
}
```

## 📝 Migration Guide

The original monolithic structure has been refactored:

**Before:**
- Frontend files were in `Backend_API/wwwroot/`
- Backend served static files
- Tightly coupled architecture

**After:**
- Frontend is now in `Frontend-vue/` directory
- Backend removed static file serving
- Fully decoupled architecture
- Easier to scale and deploy independently

## 🐛 Troubleshooting

### Frontend can't connect to backend
- Verify backend is running on port 5000
- Check CORS configuration in `Backend_API/Program.cs`
- Ensure `VITE_API_URL` in `.env` is correct

### Database connection fails
- Verify SQL Server is running
- Check connection string in `appsettings.json`
- Ensure database exists

### Port conflicts
- Backend port: Change in `Program.cs`
- Frontend port: Change in `vite.config.js`

## 📦 Technologies Used

### Backend
- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
- RESTful API

### Frontend
- Vue 3
- Vue Router 4
- Vite
- Modern JavaScript (ES6+)

## 📄 License

This project is proprietary software.

## 👥 Support

For issues or questions, please contact the development team.

---

**Last Updated**: April 2026  
**Status**: ✅ Separated Architecture Complete
