# Frontend - Vue 3 POS System

This is the Frontend for the Point of Sale System built with Vue 3 and Vite.

## Project Structure

```
Frontend-vue/
├── public/           # Static files
│   └── index.html   # Main HTML entry point
├── src/             # Source files
│   ├── css/         # Stylesheets
│   │   └── styles.css
│   └── js/          # JavaScript/Vue components
│       ├── main.js      # Entry point
│       └── pos-app.js   # Main Vue application
├── package.json     # Dependencies and scripts
├── vite.config.js   # Vite configuration
└── .env.example     # Environment variables template
```

## Setup & Installation

### Prerequisites
- Node.js (v16 or higher)
- npm or yarn

### Installation

1. Install dependencies:
```bash
npm install
```

2. Create `.env` file from template:
```bash
cp .env.example .env
```

3. Update `.env` if needed:
```
VITE_API_URL=http://localhost:5000
```

## Development

Start the development server:
```bash
npm run dev
```

The frontend will be available at `http://localhost:3000`

The dev server is configured to proxy API requests from `/api/*` to your backend.

## Building for Production

Build the project:
```bash
npm run build
```

Output will be in the `dist/` folder.

Preview the build:
```bash
npm run preview
```

## API Endpoints

The frontend communicates with the backend API at:
- Development: `http://localhost:5000/api`
- Production: Configure in `.env`

### Available API Routes
- `GET /api/products` - Get all products
- `GET /api/orders` - Get all orders
- `GET /api/users` - Get all users
- `POST /api/orders` - Create a new order

## Backend Separation

The Frontend and Backend are now completely separated:
- **Backend** runs on port 5000 (ASP.NET Core API)
- **Frontend** runs on port 3000 (Vue 3 + Vite)
- CORS is configured to allow requests between them

## Features

- 📦 Product Management
- 🛒 Shopping Cart
- 📋 Order Management
- 👥 User Management
- 💰 Point of Sale Terminal

## Notes

- The old `/wwwroot` folder has been removed from the Backend
- Frontend is now properly modularized with Vite
- CSS and JS are properly imported as ES modules
