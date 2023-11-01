# 🚀 Personal Management Application Backend API

Welcome to the **Personal Management Application Backend API** project. This robust backend, developed during an internship, is powered by .NET Core and boasts Docker containerization for the database. It leverages Postman for automated testing and Newman CLI for generating test reports.

## Basic Features

### 💼 Import Transactions from CSV (BE-B1)

- Streamline the import of transactions from CSV files.
- Transactions are rich in details: Transaction ID, Beneficiary Name, Transaction Date, Direction, Amount, Currency, Kind, and Category.

### 📜 Paginated Transaction Listing

- Enjoy the luxury of paginated transaction listings with customizable filters.
- Expose convenient `GET /transactions` API endpoints.
- Apply filters for specific date ranges and transaction kinds.

### 📂 Import Categories from CSV (BE-B3)

- Simplify the import of spending categories from CSV.
- Create a robust DB schema supporting primary and foreign key relationships.
- Validate input per OAS3 spec and persist categories into the database.

### 🏷️ Categorize Single Transaction (BE-B4)

- Empower yourself with the ability to categorize a single transaction.
- Utilize `POST /transactions/{id}/categorize` to assign categories.
- Ensures the existence of both category and transaction in the database.

### 📊 Analytical View of Spending (BE-B5)

- Dive into analytical views of spending by categories and subcategories.
- Access data through `GET /spending-analytics`.
- Implement optional filters for categories, date ranges, and transaction directions.

### 🔄 Split Transactions (BE-B6)

- Unleash the potential to split transactions into multiple spending categories.
- The powerful `POST /transactions/{id}/split` endpoint supports splitting and management of splits.
- A well-structured DB schema ensures data integrity and extends the transaction list endpoint.

## Advanced Features

### 🚀 Automatically Assign Categories (BE-A2)

- Automate category assignments based on predefined rules.
- The `POST /transactions/auto-categorize` endpoint handles automatic category assignments.
- Configure and define rules that categorize transactions based on SQL-compliant predicates.

### 🌐 Create a Basic Web UI (BE-A3)

- Enhance the user experience with a functional web UI for transaction management.
- Choose your preferred technology stack (e.g., plain JS+HTML, ASP.NET, Angular, React).
- Prioritize functionality while keeping design in check.

### 🤝 Backend Interoperability (BE-A4)

- Foster collaboration by ensuring seamless integration with frontend implementations.
- Collaborate with colleagues to resolve integration issues and achieve t
