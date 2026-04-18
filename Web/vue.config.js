module.exports = {
  devServer: {
    proxy: {
      '/api/policies': {
        target: 'http://localhost:5050',
        changeOrigin: true
      },
      '/api/offers': {
        target: 'http://localhost:5050',
        changeOrigin: true
      },
      '/api': {
        target: 'http://localhost:5030',
        changeOrigin: true
      },
      '/login': {
        target: 'http://localhost:6060',
        changeOrigin: true,
        pathRewrite: {
          '^/login': '/api'
        }
      }
    }
  }
}
