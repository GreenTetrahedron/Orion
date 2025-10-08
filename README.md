# Orion

## Project Overview
Orion is an intranet chat app that runs without needing to connect any component to the internet. The project is split into 3 main components: Router, Server and Clients. The Router handles sending messages between the Server and Clients. The Server stores all data about messages and users into a database while also providing an interface for administrators to manage user details and user groups. An instance of the Client provides an interface for a single user to send messages to other users and to any groups they are part of.

There is [documentation](Docs/Orion_Documentation.pdf) that details the project's ideation, design and development stages among others. The document is stored in the repository at [Docs/Orion_Documentation.pdf](Docs/Orion_Documentation.pdf).


## Setup Instructions
Shortcuts to executable files for each application are located in ["Final Executables/Backup Shortcuts"]("Final Executables/Backup Shortcuts").

To run the application, you will need to setup the database as described in the [documentation](Docs/Orion_Documentation.pdf) and change the connection strings in [Orion.Server/App.config](Orion.Server/App.config) and [Orion.Server.App/App.config](Orion.Server.App/App.config) accordingly. The database will also have to be seeded with an Administrator account.

Currently, the Router and Server expect to be run on a computer with IP Address 192.168.0.26, otherwise they will not be able to connect to eachother or to the Clients. In future iterations the IP Address will be configurable. 

To start the system:
- Run the Router and then the Server on a system with IP Address 192.168.0.26
- Ensure the Router outputs its connection to the Server
- Then, Clients can be run on any systems connected to the same network
- Every Client connection should be displayed on the Router

If you wish to view how the application looks when running, there are screenshots in the [documentation](Docs/Orion_Documentation.pdf).
