clean:
	rm -rf ./nupkg
	clear

install:
	dotnet pack
	dotnet tool install --global --add-source ./nupkg kronos

uninstall:
	dotnet tool uninstall -g kronos
	make clean

update:
	make uninstall
	make install
	