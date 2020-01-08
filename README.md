# UHF RFID CATCH
This is a UHF RFID Reader application for linux / IOT devices. It is able to identify commercial EPC Gen2 and ISO RFID Tags, and extract their card/reader details.


## Devices supported

Support is available for the devices listed below with link of device details or purchase store.

### `Build for Specific OS`

`dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=false --interactive`

`dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true /p:PublishTrimmed=false --interactive`

`dotnet publish -c Release -r osx-x64 /p:PublishSi**ngleFile=true /p:PublishTrimmed=false --interactive`

#### For all major platform use

`dotnet publish --interactive -c release -r $RUNTIME`

### `For Configuration override on excecution run this:`

uhf_rfid_catch.exe --ConnectionStrings:DefaultConnection app.db --Logging:Enabled True --Logging:LogLevel Debug

#### `Or run`

./uhf_rfid_catch --ConnectionStrings:DefaultConnection app.db --Logging:Enabled True --Logging:LogLevel Debug


