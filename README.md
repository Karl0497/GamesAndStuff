# GamesAndStuff

A very simple web app used as a playground https://gamesandstuff.azurewebsites.net/

The app is hosted on Azure as part of the free plan. Within the web app, 2 discord bots are running as background threads. There's another background process that makes a web call to the same website to keep the web alive, so the bots will be active 24/7, otherwise, I would have to open the website every 24 hours.

I use this one mostly to learn dependency injection and entity framework's code first approach (not easy). The bots are for fun, one of them simulates gambling patterns and statistics.

It also includes a puzzle solver, called Tents and Trees. This is a puzzle game on app store and I'm too lazy (and dumb) to solve it manually.
