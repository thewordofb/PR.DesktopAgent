# PR.DesktopAgent

DesktopAgent is a simple Windows systray application written in .NET Core 3.1.  This application hosts a simple ASP.NET Core application server which can expose REST APIs to any website.  

## What's The Goal?

This is just an experiment to provide a simple downloadable service that can expose local desktop functionality to a web application.

## What Service is available?

At this point, all I've written is a REST wrapper for WIA compatible Scanners.

You can see the API [here](http://thewordofb.github.io/Scan-Api)

1. Enumerate all scanners
2. Create a scan job
3. Check scan job status
4. Download images acquired from scan job
