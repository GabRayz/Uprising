// webpack v4
const HtmlWebpackPlugin = require('html-webpack-plugin');
const path = require('path');

const config = {
    entry: {
        global: ['./client/src/css/index.css'],
        app: ['./client/src/js/index.js'],
        register: ['./client/src/css/register.css'],
        presentation: ['./client/src/css/presentation.css'],
        load: ['./client/src/js/load.js'],
        scroll: ['./client/src/js/scrolldown.js']
    },
    output: {
        filename: '[name].js',
        path: __dirname + '/client/dist',
        publicPath: '/assets'
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader'
                }
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader', // creates style nodes from JS strings
                    'css-loader', // translates CSS into CommonJS
                ]
            }
        ]
    },
    plugins: [
        new HtmlWebpackPlugin({
            template: './client/src/html/index.html',
            filename: 'index.html',
            minify: {
                collapseWhitespace: true
            },
            hash: true,
            chunks: ['global', 'app']
        }),
        new HtmlWebpackPlugin({
            template: './client/src/html/register.html',
            filename: 'register.html',
            minify: {
                collapseWhitespace: true
            },
            hash: true,
            chunks: ['global', 'register']
        }),
        new HtmlWebpackPlugin({
            template: './client/src/html/presentation.html',
            filename: 'presentation.html',
            minify: {
                collapseWhitespace: true
            },
            hash: true,
            chunks: ['global', 'presentation', 'scroll']
        }),
        new HtmlWebpackPlugin({
            template: './client/src/html/load.html',
            filename: 'load.html',
            minify: {
                collapseWhitespace: true
            },
            hash: true,
            chunks: ['load']
        })
    ]
};

module.exports = config;
