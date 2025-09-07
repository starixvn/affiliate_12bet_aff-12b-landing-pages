const path = require('path');

module.exports = {
  entry: {
    site: './src/js/site.js'
  },
  output: {
    path: path.resolve(__dirname, 'wwwroot/js'),
    filename: '[name].bundle.js',
    clean: true
  },
  optimization: {
    splitChunks: {
      chunks: 'all',
      cacheGroups: {
        vendor: {
          test: /[\\/]node_modules[\\/]/,
          name: 'vendor',
          chunks: 'all',
        }
      }
    }
  },
  resolve: {
    extensions: ['.js']
  }
};
