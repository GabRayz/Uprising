const express = require('express');
const app = express();
const server = require('http').Server(app);
const io = require('socket.io')(server);
const path = require('path');

app.use('/assets', express.static(path.resolve('client/dist')));

app.get('*', (req, res) => {
    res.sendFile(path.resolve('./client/dist/index.html'));
});

const port = process.env.PORT || 8080;

server.listen(port, () => {
    console.log(`Web server started on port ${port} and is ready to operate.`);
});