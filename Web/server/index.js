const config = require('../config.json');
const express = require('express');
const app = express();
const server = require('http').Server(app);
const io = require('socket.io')(server);
const path = require('path');

const morgan = require('morgan');

const Sequelize = require('sequelize');

const db = new Sequelize(
    config.database.name,
    config.database.user,
    config.database.password,
    config.database.config
);

app.use(morgan('common'));

app.use('/assets/img', express.static(path.resolve('client/src/img')));
app.use('/assets', express.static(path.resolve('client/dist')));

app.get('*', (req, res) => {
    res.sendFile(path.resolve('./client/dist/index.html'));
});

const port = process.env.PORT || config.port || 8080;

const User = db.define('user', {
    id: {
        type: Sequelize.INTEGER,
        primaryKey: true,
        autoIncrement: true
    },
    username: {
        type: Sequelize.STRING,
        allowNull: false
    },
    email: {
        type: Sequelize.STRING,
        validate: {
            isEmail: true
        }
    },
    password: {
        type: Sequelize.STRING
    }
});

async function main() {
    console.log('Connecting to database...');
    await db.authenticate();

    console.log('Connected to database. Synchronizing models...');
    await db.sync();
    console.log('Models synchronized.');

    server.listen(port, () => {
        console.log(`Web server started on port ${port} and is ready to operate.`);
    });
}

main();
