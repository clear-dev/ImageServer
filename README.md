# Image Server

Very simple image server I created whilst toying with APIs & using DBContexts in ASP.NET

Intended for use with a program such as [ShareX](https://getsharex.com), the program I used to test / interact with the server

### Installing

Before beginning you will need an SQL Server Database within `(localdb)\MSSQLLocalDB`

Within the project, run the following in the `Package Manager Console` to update your database

    Update-Database

Once this is completed, you can Build / Run the solution

### Using with ShareX

To use with ShareX, you must set up a new custom destination for Images / Files with the following settings:

- Method: `POST`
- Request URL: `https://serverdomain:port/api/Images` (for example `https://localhost:7213/api/Images`)
- Body: `Form data (multipart/form-data)`
- URL: `{json:imageURI}`

## Authors

  - [Luke](https://github.com/clear-dev)