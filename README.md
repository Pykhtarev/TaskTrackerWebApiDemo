# TaskTrackerWebApiDemo
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

# Description
Simple web api for managing project, you can easily creating a project assign a task and manipulating data in this entitys


### `Features`

- Implemented base CRUD operation
- Unification response (help handl any situation)
- Sorting by project fields
- Filter by status and priority
- Possibility to reassigned task to different project 

# Tech
Tracker uses:
- Swagger - the most powerful and easiest tool to take full advantage of the OpenAPI Specification.
- Entity Framework - for work with database
- ASP.Net Core 

### My configuration
- Windows 11pro
- Visual Studio community 2022
- i7-3632QM
- 16 gb ddr3


# Architecture

I applied the MVC pattern for this project



# Docker

Tracker is very easy to install and deploy in a Docker container.

By default, the Docker will expose port 8080, so change this within the
Dockerfile if necessary. When ready, simply use the Dockerfile to
build the image.

>Note: I use `local` db in solution, so kindly config correct db connection berfore build image.



# FAQ
- This api support OAuth 2 authentication?
>No. This api doesn't support any kind of authentication
- This api support cookie or some another state saving?
>No. For now it's pure stateless protocol 
- Where I can see documentation for methods?
>Run the solution through swagger all methods well documented


## Afterword

Want contribute? Feel free to leave me feedback in [telegram][PlDb].

What next? I want write webapi using bearer token.

**Happy coding, Hell Yeah!**

[PlDb]:<https://t.me/tg0vk>
