# Azure VM Manager

## Overview

Over the last couple of years I have been learning TDD, SOLID principles, React and Azure. I have decided to create this project to tie all these concepts together into a single application. The application will be a Single Page Application (SPA) frontend in React with a C# .Net WebAPI backend. The application will be hosted as an Azure App Service and will use the Microsoft Identity Provider for authentication. I will build the application using TDD for both the frontend and backend, using SOLID principles. The purpose of the application will be to allow users to manage virtual machines in Azure in a simplified way where an organisation has a need to provide self-contained, pre-configured environments for thing such as testing. I will be using the Azure SDK to do this. I will begin this project by starting the implementation of the business logic in the Web API and will be implementing the following features:

## Features

- **Simplified VM Provisioning**: Create virtual machines from default SKUs or your own custom images.
- **Create Custom Images**: Create an image from your customised virtual machines.
- **Security**: Control access to virtual machines, and users with admin privileges can control access to the application.
