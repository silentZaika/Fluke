Run with coverage
```
dotnet test --collect:"XPlat Code Coverage"

dotnet test --logger:"console;verbosity=detailed" --logger "trx;LogFileName=test-results.trx" /p:CollectCoverage=true /p:CoverletOutput=./TestResults/coverage/ /p:CoverletOutputFormat=cobertura /p:ExcludeByFile="**/Migrations/*.cs"
```

Generate HTML report
```
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
```

Re-create test database
```
dotnet ef migrations add InitialCreate --project Fluke.Core --startup-project Fluke.CollectorAPI
dotnet ef database update --project Fluke.Core --startup-project Fluke.CollectorAPI
```