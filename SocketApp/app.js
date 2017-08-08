'use strict';

var port = 8080;

var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http)

//io.attach(port);
//console.log("Starting");

io.on('connection', function(socket){
  console.log('someone connected');
	socket.on('command', function(msg){
    console.log("command", msg);

    //Send to all listeners
		io.emit('command', msg);
	});
})

app.get('/', function(req, res){
  res.sendFile(__dirname + '/index.html');
});

http.listen(port, function() {
  console.log('listening on *:', port);
});
