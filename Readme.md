# Physiosoft

Physiosoft is a comprehensive web application tailored for physiotherapy facilities. It streamlines the process of managing physiotherapists, patients, and appointments, offering a centralized platform for scheduling and tracking sessions. The application is built with a focus on ease of use and efficiency in managing daily operations of a physiotherapy center.

## Features

- **Appointment Scheduling**:  Users can create, update, and delete appointments efficiently, facilitating smooth operation of the physiotherapy sessions.
- **Patient Management**: Allows for the registration and management of patient information, ensuring that patient records are easily accessible and well-organized.
- **Physiotherapist Profiles**: Manage and maintain detailed profiles for physiotherapists, including their schedules, specializations, and availability.
- **Appointment Tracking**: Provides a comprehensive view of upcoming and past appointments, aiding in better planning and resource allocation.

## Technical Architecture

Physiosoft employs a robust MVC layered architecture, ensuring separation of concerns and making the application scalable and maintainable. The architecture is outlined as follows:

- **Data Access Object (DAO) / Data Transfer Object (DTO)**: For handling data interactions and transfer between different application layers.
- **Data Layer**: Manages the application's data structures, it's logic and its interaction with the database.
- **View Layer**: Manages the user interface and presentation logic.
- **Controllers**: Serve as an intermediary between the Model and View layers, handling user input and responses.

In addition to this, Physiosoft integrates various other technologies and practices:

- **Repository Architecture**: Incorporating IBaseRepository and BaseRepository patterns for abstracted data operations, alongside IUserRepository and UserRepository for user-specific data handling.
- **Entity Framework**: Utilizes Entity Framework for object-relational mapping, simplifying data access and manipulation.
- **DbContext and Fluent API**: Manages database contexts and employs Fluent API for advanced configuration and mappings.
- **Validators**: Ensures data integrity and validation throughout the application.
- **Custom Exceptions**: Implements custom exception handling for clearer and more precise error management.
- **NLogger**: For efficient and effective logging of application activities and errors.
- **User Registration/Login/Validation**: Manages user accounts, authentication, and authorization securely.

## Tech Stack
- **C# and .NET 8**: Forms the core of backend development, providing a powerful and efficient programming environment.
- **SQL Database**: Utilizes SQL databases for robust and reliable data storage.
- **Razor, Bootstrap, and JQuery**: These technologies enhance the front-end development, ensuring a responsive and user-friendly interface.

## Installation and Setup
<ol>
  <li>Clone the repository:
    
     ```md
     git clone https://github.com/JohnNtirintis/Physiosoft.git
     ```
  </li>
  <li>Database Setup:
    <ul>
      <li>Navigate to the SQL folder within the cloned repository.</li>
      <li>Open SSMS.</li>
      <li>Run the CreateDatabase.sql script to create the necessary database schema.</li>
      <li>Execute the CreateUser.sql script to create user accounts.</li>
      <li>Finally, run the PopulateDatabase.sql script to populate the database with some example data.</li>
    </ul>
  </li>
  <li>Application Configuration
     <ul>
      <li>Ensure that the connection strings in the application configuration file (appsettings.json) are set to point to your newly created database.</li>
      <li>Verify that other configuration settings (like any API keys or external service configurations) are correctly set up.</li>
    </ul>
  </li>
</ol>


