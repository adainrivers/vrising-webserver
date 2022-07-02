# vrising-webserver
A Sample Web Server Project for V Rising Private Servers

This is still work in progress.

## How to use

Edit `VRising.WebServer.csproj` and change the `ServerPath` to your local development server path

Edit the port in Plugin.cs

`WebServer = new WebHostServer(80);`

Add more mappings to the router as needed.

`WebServer.Router.MapGet("/users", DataFactory.GetUsers);`