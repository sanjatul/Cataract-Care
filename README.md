# Cataract Care System

## Overview
The Cataract Care System is an automated cataract diagnosis platform that uses deep learning algorithms to analyze fundus images for cataract detection. It is designed to provide accessible healthcare solutions, particularly in regions with limited access to ophthalmologists. The platform supports both admin and general users with a subscription-based model, allowing users to upload images and receive diagnosis reports.

## Features

### 1. User Roles
- **Admin**:
  - Manage subscription packages (create, update, enable/disable).
  - Set pricing, duration, and maximum photo limits for packages.
  - View user activity and reports.
  
- **General Users**:
  - Subscribe to diagnostic services.
  - Upload fundus images for cataract analysis.
  - View previous diagnosis reports.

### 2. Image Analysis & Diagnosis
- Upload fundus images for real-time cataract diagnosis.
- Diagnosis reports include:
  - **Normal** or **Cataract** result.
  - Probability score (e.g., “Cataract with probability 99.93%”).
  - Detailed report including patient information (name, age, gender, eye analyzed).

### 3. Subscription Management
- General users can purchase subscription packages to access diagnosis services.
- Subscription details include:
  - Pricing and duration.
  - Maximum image upload limit (can be unlimited).
  
- Admin can manage subscription plans and enable/disable packages.

### 4. Authentication & Authorization
- User login and registration.
- Token-based authentication using **JWT**.
- Redirect users to login when accessing protected features (e.g., subscription purchase).

### 5. Diagnosis History
- Users can view all previous diagnosis reports.
- Downloadable reports for healthcare professionals or personal reference.

### 6. Payment Integration
- Online payment gateway for purchasing subscription packages.
- Payment confirmation and transaction history for users.

## Technologies Used

### Frontend
- **React** - Version 18
  - **React-Router** for navigation between pages.
  - **Redux** for state management (user sessions, subscriptions, image uploads).
  - **Bootstrap** for responsive UI design.

### Backend
- **ASP.NET Core** - Version 7 (Web API)
  - Controllers for handling subscription, user management, and image analysis.
  - RESTful API services to interact with the frontend.

### Deep Learning Model
- **Flask** API for deep learning model deployment.
  - The model processes fundus images and returns diagnosis results.
  - Integrated into the backend via REST API for real-time analysis.

### Database
- **MSSQL**
  - Database for storing user data, subscription packages, and diagnosis reports.
  - Entity Framework Core for ORM and database management.


## Setup and Installation

### Prerequisites
- .NET SDK 7.0+
- Node.js 16.x or higher
- MSSQL Server
- Python environment (for Flask model API)

