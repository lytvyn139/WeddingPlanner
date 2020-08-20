# Wedding Planner

# How to:

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
