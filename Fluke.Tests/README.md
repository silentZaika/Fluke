Run with coverage
```
dotnet test --collect:"XPlat Code Coverage"
dotnet test --filter TestCategory!=Pact --logger:"console;verbosity=detailed" --logger "trx;LogFileName=test-results.trx" /p:CollectCoverage=true /p:CoverletOutput=./TestResults/coverage/ /p:CoverletOutputFormat=cobertura /p:ExcludeByFile="**/Migrations/*.cs"
dotnet test --filter "TestCategory=Pact|Integration" --logger:"console;verbosity=detailed" --logger "trx;LogFileName=test-results.trx"

Run tests to generate xml report file
```
dotnet test --logger:"console;verbosity=detailed" --logger "nunit;LogFileName=test-results.xml" --filter "FullyQualifiedName=FlukeTests.NunitXmlParserTests"
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