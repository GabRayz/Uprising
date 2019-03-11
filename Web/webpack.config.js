// webpack v4
const HtmlWebpackPlugin = require('html-webpack-plugin');
const path = require('path');

const config = {
    entry: {
        app: ['./client/src/js/index.js', './client/src/css/index.css']
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
            hash: true
        })
    ]
};

module.exports = config;
