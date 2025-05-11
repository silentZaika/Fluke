Run with coverage
```
dotnet test --collect:"XPlat Code Coverage"

dotnet test /p:CollectCoverage=true /p:CoverletOutput=./TestResults/coverage/ /p:CoverletOutputFormat=cobertura /p:ExcludeByFile="**/Migrations/*.cs"
```

Generate HTML report
```
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
```