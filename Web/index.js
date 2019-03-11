const express = require('express');
const app = express();
const server = require('http').Server(app);
const io = require('socket.io')(server);

const port = process.env.PORT || 8080;

app.get('*', (req, res) => {
    res.send('<h1>It works!</h1>');
});

server.listen(port, () => {
    console.log(`Web server started on port ${port} and is ready to operate.`);
});