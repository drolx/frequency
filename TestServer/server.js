var express = require('express');
var bodyParser = require('body-parser');
var multer = require('multer');
var upload = multer();
var app = express();

app.use(express.json())
// app.use(express.urlencoded({ extended: true }))

// for parsing application/xwww-
app.use(bodyParser.urlencoded({ extended: true })); 

// for parsing multipart/form-data
app.use(upload.array()); 
app.use(express.static('public'));


app.get('/IOT', function (req, res) {
   res.send('GET REQUEST');

   if(req.body.length > 0)
   {
      console.log('---- GET ----')
      console.log(req.body)
   }
})

app.post('/IOT', function (req, res) {
   res.send('POST REQUEST');
   console.log('---- POST ----')
   console.log(req.body.ScanData)
})

var server = app.listen(8075, function () {
   var host = server.address().address
   var port = server.address().port
   
   console.log("Listening on http://%s:%s", host, port)
   
})