.PHONY: all
all: build test

.PHONY: restore
restore:
	dotnet restore

.PHONY: build
build: restore
	dotnet build

.PHONY: test
test:
	dotnet test src/Turbocharged.Toolbox.Tests/Turbocharged.Toolbox.Tests.csproj

.PHONY: pack
pack:
	dotnet pack src/Turbocharged.Toolbox/Turbocharged.Toolbox.csproj --configuration Release
