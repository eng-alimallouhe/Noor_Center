# Library Management System

This project is a **Library Management System** built with **ASP.NET Core** for the backend and **Angular** for the frontend. The system allows users to manage books, products, rentals, and handle other aspects of a library, such as inventory, employee management, payments, and reports.

## Technologies Used
- **Backend**: ASP.NET Core, Entity Framework, REST API
- **Frontend**: Angular, TypeScript, HTML, CSS
- **Database**: SQL Server
- **Authentication**: Firebase Authentication
- **Real-time Communication**: SignalR
- **Payment Integration**: Custom payment gateway

## Features
- **User Management**: Manage customer and employee profiles.
- **Book and Product Management**: Add, update, and delete books and products.
- **Inventory Management**: Track stock levels and generate alerts.
- **Order and Payment Management**: Process customer orders, handle payments, and generate invoices.
- **Rental System**: Manage book rentals and returns.
- **HR System**: Employee attendance, performance, and salary management.
- **Reports**: Generate various reports, including financial and inventory reports.
- **Loyalty System**: Customers can earn loyalty points.
- **Multi-payment Support**: Supports different payment methods.
- **Smart Recommendations**: Book recommendations based on customer preferences.

## Setup Instructions

### 1. Clone the repository
- To get started, clone the repository to your local machine:
```bash
git clone https://github.com/yourusername/your-repository.git
```
### 2. Backend Setup

#### Install Dependencies

- Navigate to the Backend folder and run the following command to install required dependencies:
- open the prject folder in CMD then write this command:
```bash
dotnet restore
```

#### Configure Database

- Update the appsettings.json file in the Backend project with your database connection string.
- Run the Application
```bash
dotnet run
```

- The backend API will be running at http://localhost:5000.

### 3. Frontend Setup

#### Install Dependencies

- Navigate to the Frontend folder and install the required Angular dependencies:
- open the UI folder in CMD then write this comman:
```bash
npm install
```

#### Run the Application

-To run the Angular application locally:
```bash
ng serve
```
or 
```bash
npm start
```

- The frontend will be running at http://localhost:4200

## Usage:

- Sign up for an account to access the system (email verification required).
- Login to manage your profile, rent books, and access reports.
- Admin users can manage inventory, employees, and view detailed reports.

## Contributing

- We welcome contributions! If you would like to contribute to the project, please follow these steps:

- Fork the repository.

- Create a new branch (git checkout -b feature-name).

- Commit your changes (git commit -am 'Add feature').

- Push to your branch (git push origin feature-name).

- Create a pull request.

## License

- This project is licensed under the MIT License - see the **LICENSE** file for details.