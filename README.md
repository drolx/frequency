# UHF RFID IOT / SERVER DAEMON

This is a UHF RFID Reader application for linux / IOT devices. It is able to identify commercial EPC Gen2 and ISO RFID Tags, and extract their card/reader details.

## Devices supported

Support is available for the devices listed below with link of device details or purchase store.

### `Build for Specific OS`

```bash
dotnet publish -c Release -r linux-x64 /p:SelfContained=true /p:PublishSingleFile=true /p:PublishReadyToRun=false
```

```bash
dotnet publish -c Release -r win-x64 /p:SelfContained=true /p:PublishSingleFile=true /p:PublishReadyToRun=true
```

```bash
dotnet publish -c Release -r osx-x64 /p:SelfContained=true /p:PublishSingleFile=true /p:PublishReadyToRun=true
```

### `For Configuration override on excecution run this:`

```bash
./Iot.Rfid --ConnectionStrings:DefaultConnection app.db --Logging:Enabled True --Logging:LogLevel Info
```

#### `Or run for mac and linux`

```bash
./Iot.Rfid --ConnectionStrings:DefaultConnection app.db --Logging:Enabled True --Logging:LogLevel Info
```

## Lincense

`Copyright (c) 2020 Godwin peter .O`

`Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:`

`The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.`

`THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.`
