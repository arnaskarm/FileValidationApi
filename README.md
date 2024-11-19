# File Validation API

The **File Validation API** is designed to validate uploaded file content.

---

## Getting Started

Follow the instructions below to run the API.

### Prerequisites

Ensure you have the following installed:

- [Docker](https://www.docker.com/get-started)

### How to Run

1. **Clone the Repository**

   Clone the main branch of the repository to your local machine:

   ```bash
   git clone https://github.com/arnaskarm/FileValidationApi.git
   ```

2. **Ensure Docker is installed and running**

   Make sure Docker is properly installed and running on your machine.

3. **Navigate to the project directory**

   Open PowerShell (or any terminal of your choice) and navigate to the root directory of the project, where the `docker-compose.yml` file is located:

   ```bash
   cd path-to-the-project/FileValidationApi
   ```

4. **Run the API with Docker Compose**

   Start the application by running the following command in PowerShell:

   ```bash
   docker-compose up
   ```

5. **Access the API via Swagger UI**

   Once the container is up and running, open your browser and navigate to the following URL:

   ```
   http://localhost:8000/swagger/index.html
   ```

   This will give you access to the API documentation and allow you to test the endpoint directly from the browser.

---   
