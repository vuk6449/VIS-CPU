PROJECT := VukCPU.csproj
CONFIG ?= Release
ARGS ?=

.PHONY: all build run clean

all:
	@echo "Defaulting to build, args: build, run, clean"
	@$(MAKE) build

build:
	@dotnet build $(PROJECT) -c $(CONFIG)

run:
	@dotnet run --project $(PROJECT) -c $(CONFIG) -- $(ARGS)

clean:
	@dotnet clean $(PROJECT) -c $(CONFIG)