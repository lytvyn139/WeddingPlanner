# Wedding Planner
Planning to get married soon? Good news, I've made a full-stack app for you, yes for you! And yes it's absolutely free and mobile-friendly! 
Stack: C#/ASP.DOTNET/Entity/SQL


## Install Entity Framework
```cs
dotnet tool install dotnet-ef --global
```
## Edit appsettings.json
You have to replace XXXX with your SQL server login and password
```
cd WeddingPlanner
vim appsettings.json
# on line 13
# ...userid=XXXX;password=XXX;port=3306;...
```

## Reslore project
```
dotnet restore
```

## Migrate SQL db's
Create appropriate schema in our db
```
dotnet ef migrations add initial -v
dotnet ef database update -v
```

## Run
```
dotnet watch run
```

## LOCALHOST
to open app go to 
```
http://localhost:5000 
```
