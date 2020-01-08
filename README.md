# UHF RFID CATCH
This is a UHF RFID Reader application for linux / IOT devices. It is able to identify commercial EPC Gen2 and ISO RFID Tags, and extract their card/reader details.


## Devices supported

Support is available for the devices listed below with link of device details or purchase store.

### `Build for Specific OS`

`dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true --interactive`

`dotnet publish -c Release -r win10-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true --interactive`

`dotnet publish -c Release -r osx-x64 /p:PublishSi**ngleFile=true /p:PublishTrimmed=true --interactive`

#### For all major platform use

`dotnet publish --interactive -c release -r $RUNTIME`

#### Or Run

`dotnet warp`

### `Run command`

package.exe --ConnectionStrings:DefaultConnection app.db --Logging:Enabled True --Logging:LogLevel Debug


