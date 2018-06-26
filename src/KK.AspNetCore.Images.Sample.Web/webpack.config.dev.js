module.exports = {
    devtool: 'inline-source-map',
    module: {
        rules: [
            { test: /\.less$/, loader: 'style-loader!css-loader!less-loader' }
        ]
    }
};