var webpack = require('webpack');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
var extractLESS = new ExtractTextPlugin('style.css');

module.exports = {
    module: {
        rules: [
            { test: /\.less$/, loader: extractLESS.extract(['css-loader', 'less-loader']) },
        ]
    },
    plugins: [
        extractLESS
    ]
};