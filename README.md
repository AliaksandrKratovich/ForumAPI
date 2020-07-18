# Description

A simple backend implementation of website for users, who can create/submit articles and post comments.

## Technical description

- Application was written with .NET CORE 3.1 by using REST archihecture as a microservice;
- Was added authentication by using JWT token. Only authenticated users can post articles and comments;
- User can't create a post with already existing article title;
- Was added articles search by partial title occurrence, username or category;
- Was provided CRUD operations for entities;
- As a database was used MongoDB.
- Was added unit testing (XUnit).

## Start application

- Launch MongoDB.
- - *If you didn't install MongoDB client, you can get it [here](https://www.mongodb.com/try/download/community)*
- Ensure you have such directories: C:\data\db -  on Windows OS and \data\db on Linux or MacOS.
- Start solution (set Forum.WebApi as a startup poject).
- Use swagger UI to interact with application.
