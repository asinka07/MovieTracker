# MovieTracker

MovieTracker is a web application for movie enthusiasts.  
It allows users to browse movies, explore genres, directors, and share reviews.  
Registered users can add movies to their watchlist and submit movies for admin approval.  
Administrators have full control over the platform through a dedicated admin panel.

The platform helps users discover what to watch by displaying the latest published movies and ranking genres based on popularity.

## 1. Features

### For All Users (including non-registered)
- View latest published movies on the home page
- Show top genres based on movie count

### For Registered Users

#### Movies
- Add new movies (submitted for admin approval)
- Edit existing movies
- Delete movies (with confirmation)
- View detailed information about each movie
- Filter movies by genre
- Sort movies by publication date
- Add/remove movies from personal Watchlist

#### Reviews
- Add reviews to movies
- Display reviews inside movie details page

#### Genres
- Add new genres
- Prevent duplicate genres

#### Directors
- View all directors with biographies

### For Administrators
- Full CRUD on Movies, Genres, Directors
- Approve or reject pending movies submitted by users
- Manage users (view details, promote/demote roles, delete)
- Admin Dashboard with statistics (total users, movies, genres, directors, reviews)

## 2. Tech Stack

### Backend
- ASP.NET Core MVC (.NET 8)
- C#
- Entity Framework Core 8

### Database
- SQL Server Express

### Frontend
- HTML5
- CSS3
- Bootstrap 5
- Toastr (notifications)

### Development Tools
- Visual Studio 2022

### Architecture
- Model-View-Controller (MVC) pattern
- MVC Areas (Admin area for administration)
- Service layer with interfaces (Dependency Injection)
- Separate class libraries for Data, Services, ViewModels

## 3. Project Structure

```
MovieTracker/                    ← Main web project
MovieTracker.Data/               ← DbContext and Models
MovieTracker.Data.Models/        ← Entity models
MovieTracker.Services/           ← Business logic
MovieTracker.ViewModels/         ← ViewModels
MovieTracker.GCommon/            ← Shared constants and validations
MovieTracker.Tests/              ← Unit tests
```

## 4. Entity Models

| Model | Description |
|-------|-------------|
| Movie | Film with title, description, genre, director, approval status |
| Genre | Movie genre |
| Director | Film director with biography |
| Review | User review for a movie |
| Watchlist | User's personal movie watchlist |

## 5. Installation

> ### **Requirements**
>
> - .NET 8 SDK
> - SQL Server Express
> - Visual Studio 2022
> - Entity Framework Core 8 (via NuGet)

### Installation Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/asinka07/MovieTracker.git
   ```
2. Open the solution file in Visual Studio 2022.
3. Configure the connection string in `appsettings.json` if needed.
4. Open Package Manager Console and run:

   ```bash
   Update-Database
   ```
   This will apply the migrations and create the database.

5. Press F5 or Ctrl + F5 to run the application.
6. The database will be seeded automatically with genres, directors, and an admin account.

## 6. Configuration

> Before running the application, make sure the database connection is correct.

- The connection string is in `appsettings.json`:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=MovieTrackerDb;Trusted_Connection=True;"
}
```

- `.\\SQLEXPRESS` points to your local SQL Server Express instance.
- No additional environment variables are required.
- Make sure you apply migrations using `Update-Database` in the Package Manager Console before running the app.

### Default Admin Account

```
Email: admin@test.com
Password: MovieTracker2@26
```

## 7. Usage

### Home Page

- The navigation menu contains:
  - **MovieTracker** (click to return to home)
  - **Home**
  - For registered users: **Movies**, **Genres**, **Directors**, **Watchlist**
  - For admins: **Dashboard**, **Content** (Movies, Genres, Directors), **Pending**, **Users**

- Below the heading you can see:
  - The last 6 published movies
  - The 5 genres with most movies

- Clicking **View Details** opens a modal showing:
  - Movie title
  - Date of publishing
  - Description

- Clicking a genre redirects to the Movies page with a filter applied.

---

### Movies Page

- By default, all approved movies are displayed, ordered by oldest first.
- Filters:
  - Select a genre
  - Select sorting by newest first
  - Apply changes using the **Apply** button

### Add Movie

- Click **Add Movie** to open the form with 3 required fields and an optional Director field.
- The dropdown shows all existing genres. If the genre doesn't exist, you can add it from the **Add Genre** form.
- After submitting, the movie is sent for admin approval and a success message appears.

### Movie Details

- Options:
  - **Back** → returns to the Movies page
  - **Edit** → opens a form to modify title, genre, description or director
  - **Delete** → confirmation pop-up appears
  - **Add to Watchlist** / **Remove from Watchlist** → available for registered users
  - **Add Review** → adds a review and shows a success message

---

### Genres Page

- Lists all existing genres with movie count.
- If no genres exist, a message is displayed.

### Add Genre

- Click **Add Genre** to open the form.
- Adding a duplicate genre is not allowed.

---

### Directors Page

- Lists all directors with their biographies.
- Pagination is available for large lists.

---

### Watchlist Page

- Displays all movies added to the user's personal watchlist.
- Each entry shows title, genre and date added.
- Options: **Details** → view movie, **Remove** → remove from watchlist.

---

### Admin Panel

#### Dashboard
- Overview of platform statistics: total users, movies, genres, directors, reviews.
- Quick links to manage each section.

#### Movies
- Table view of all movies with status (Approved/Pending).
- Options: Edit, Approve, Delete.

#### Pending
- Lists all movies awaiting approval.
- Admin can Review (full details), Approve or Delete each submission.

#### Directors
- Full CRUD for directors.

#### Genres
- Full CRUD for genres with movie count.

#### Users
- List of all users with role, movie count, review count.
- Options: Details, Promote to Administrator, Demote to User, Delete.

## 8. Security

- CSRF protection with AntiForgeryToken on all forms
- Role-based authorization (`[Authorize(Roles = "Administrator")]`)
- Admin Area separated from public controllers
- SQL Injection prevention through Entity Framework Core
- XSS prevention through Razor's automatic HTML encoding
- Custom 404 and 500 error pages

## 9. Unit Tests

Unit tests cover the business logic in the service layer using xUnit and InMemory database.

| Service | Coverage |
|---------|---------|
| GenreService | ✅ |
| DirectorService | ✅ |
| MovieService | ✅ |
| WatchlistService | ✅ |
| HomeService | ✅ |

**Total coverage: 71%** (above the required 65%)

## 10. Seeded Data

The application seeds the following data on first run:

- **Admin account** – email: `admin@test.com`, password: `MovieTracker2@26`
- **Administrator role**
- **8 genres** – Action, Animation, Documentary, Romance, Comedy, Rom-Com, Drama, Sci-Fi
- **18 directors** – including Christopher Nolan, Steven Spielberg, Martin Scorsese and more
