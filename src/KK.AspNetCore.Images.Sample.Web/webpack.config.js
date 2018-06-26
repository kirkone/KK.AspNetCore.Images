var path = require('path');
var merge = require('extendify')({ isDeep: true, arrays: 'concat' });
var devConfig = require('./webpack.config.dev');
var prodConfig = require('./webpack.config.prod');
var isDevelopment = process.env.ASPNETCORE_ENVIRONMENT === 'Development';

module.exports = merge({
    resolve: {
        extensions: [ '.js', '.ts' ]
    },
    module: {
        rules: [
            { test: /\.ts(x?)$/, exclude: /node_modules/, loader: 'ts-loader?silent' }
        ],
    },
    entry: {
        main: ['./ClientApp/App.ts']
    },
    output: {
        path: path.join(__dirname, 'wwwroot', 'dist'),
        filename: '[name].js',
        publicPath: '/dist/'
    },
    plugins: []
}, isDevelopment ? devConfig : prodConfig);