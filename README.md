# GSES2 BTC application
Genesis & KMA Software Engineering School 3.0

# Description

This application was implemented in C# based on the ASP.NET Core .NET Framework 7.0. This application is an Api that allows:
  - get the current exchange rate of Bitcoin (BTC) to Ukrainian Hryvnia (UAH);
  - subscribe to receive information about the current exchange rate via e-mail;
  - notify all subscribed users about the current rate.

Additionally, this application also implements the ability to create complex templates for email letters in HTML/CSS using Razor.

# Instructions
## Run in Docker:
1. Clone this repository
2. Create .env file inside the solution`s root folder and specify environment variables inside
  ```
  SMTP_USER_ADDRESS=<your-outlook-account>
  SMTP_USER_PASSWORD=<your-outlook-account-password>
  ```
3. Run Docker
4. Execute start.bat or run cmd from solution`s root folder and execute specified commands:
  ```
  copy /Y ".\gses\Dockerfile" ".\Dockerfile"
  docker build . -t gses
  docker run -d -p 5000:80 --rm --env-file .env gses
  ```
5. Open http://localhost:5000/api/swagger/index.html url in a browser to explore Swagger
