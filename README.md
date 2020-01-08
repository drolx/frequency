# UHF RFID CATCH
This is a UHF RFID Reader application for linux / IOT devices. It is able to identify commercial EPC Gen2 and ISO RFID Tags, and extract their card/reader details.


## Devices supported

Support is available for the devices listed below with link of device details or purchase store.

### `Build for Specific OS`
`dotnet publish -c Release /p:PublishProfile=linux-x64`

`dotnet publish -c Release /p:PublishProfile=win-x64`

`dotnet publish -c Release /p:PublishProfile=osx-x64`


### `For Configuration override on excecution run this:`

uhf_rfid_catch.exe --ConnectionStrings:DefaultConnection app.db --Logging:Enabled True --Logging:LogLevel Debug

#### `Or run for mac and linux`

./uhf_rfid_catch --ConnectionStrings:DefaultConnection app.db --Logging:Enabled True --Logging:LogLevel Debug


