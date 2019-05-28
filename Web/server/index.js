const config = require('../config.json');
const express = require('express');
const app = express();
const server = require('http').Server(app);
const io = require('socket.io')(server);
const path = require('path');

const morgan = require('morgan');
const bodyParser = require('body-parser');
const session = require('express-session');

const Sequelize = require('sequelize');
const bcrypt = require('bcrypt');

const db = new Sequelize(
    config.database.name,
    config.database.user,
    config.database.password,
    config.database.config
);

app.use(morgan('common'));
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

app.set('trust proxy', 1);
app.use(
    session({
        secret: 'distillerie',
        resave: true,
        saveUninitialized: true,
        cookie: { secure: false }
    })
);

app.use('/assets/img', express.static(path.resolve('client/src/img')));
app.use('/assets', express.static(path.resolve('client/dist')));

app.get('/', (req, res) => {
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
    xp: {
        type: Sequelize.INTEGER,
        defaultValue: 0
    },
    password: {
        type: Sequelize.STRING
    },
    winCount: {
        type: Sequelize.INTEGER,
        defaultValue: 0
    },
    gameCount: {
        type: Sequelize.INTEGER,
        defaultValue: 0
    },
    shotCount: {
        type: Sequelize.INTEGER,
        defaultValue: 0
    },
    accurateShotCount: {
        type: Sequelize.INTEGER,
        defaultValue: 0
    }
});

app.use('*', async (req, res, next) => {
    if (req.session.userId) req.user = await User.findByPk(req.session.userId);
    next();
});

app.get('/auth/register', (req, res) => {
    res.sendFile(path.resolve('./client/dist/register.html'));
});

app.post('/auth/register', async (req, res) => {
    try {
        const hash = await bcrypt.hash(req.body.password, 10);

        const user = await User.create({
            username: req.body.username,
            email: req.body.email,
            password: hash
        });

        req.session.userId = user.id;
        res.redirect('/load');
    } catch (e) {
        res.sendStatus(403);
    }
});

app.get('/auth/login', (req, res) => {
    res.sendFile(path.resolve('./client/dist/login.html'));
});

app.post('/auth/login', async (req, res) => {
    const user = await User.findOne({
        where: {
            username: req.body.username
        }
    });

    if (!user) return res.sendStatus(403);

    const same = await bcrypt.compare(req.body.password, user.password);

    if (same) {
        req.session.userId = user.id;
        res.redirect('/load');
    } else {
        res.sendStatus(403);
    }
});

app.get('/auth/data', (req, res) => {
    if (!req.user) return res.json(null);

    res.send({
        id: req.user.id,
        username: req.user.username,
        xp: req.user.xp
    });
});

app.get('/load', (req, res) => {
    res.sendFile(path.resolve('./client/dist/load.html'));
});

app.get('/presentation', (req, res) => {
    res.sendFile(path.resolve('./client/dist/presentation.html'));
});

app.get('/game', (req, res) => {
    if (req.user) res.sendFile(path.resolve('./client/dist/game.html'));
    else res.redirect('/auth/register');
});

app.post('/game', async (req, res) => {
    console.log(req.body);

    req.user.xp = req.body.xp;
    req.user.gameCount++;
    req.user.shotCount += req.body.shotCount;
    req.user.accurateShotCount += req.body.accurateShotCount;
    if (req.body.winner) req.user.winner++;

    req.user.save();

    res.sendStatus(200);
});

async function main() {
    console.log('Connecting to database...');
    await db.authenticate();

    console.log('Connected to database. Synchronizing models...');
    await db.sync({ alter: true });
    console.log('Models synchronized.');

    server.listen(port, () => {
        console.log(`Web server started on port ${port} and is ready to operate.`);
    });
}

main();
