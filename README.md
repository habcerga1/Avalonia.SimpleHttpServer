# Avalonia.SimpleHttpServer

When are you adding new server:
1. Add folder to ~/ProjectName/www/SITE_NAME
2. The name of the new server should be the same with folder Example: SITE_NAME
You can see default 2 folders inside the project

You can run app from Rider or VisualStudio or from desktop (run as Administrator)

On Widows:
dotnet ~/Avalonia.SimpleHttpServer/Avalonia.SimpleHttpServer/bin/Release/net6.0/Avalonia.SimpleHttpServer.dll

On MacOs:
sudo dotnet ~/Avalonia.SimpleHttpServer/Avalonia.SimpleHttpServer/bin/Release/net6.0/Avalonia.SimpleHttpServer.dll

For test connection use: 
http://127.0.0.1:10100/index.html  -> www/site1/index.html  Server Settings[ Name: site1 , Port: 10100]
http://127.0.0.1:10101/index.html  -> www/site2/index.html  Server Settings[ Name: site2 , Port: 10101]
