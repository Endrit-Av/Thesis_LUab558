const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7219';

const PROXY_CONFIG = [
  {
    context: [
      //"/api/test/products",
      //"/api/test/users",
      //"/api/test/reviews",
      //"/api/test/images",
      "/api/mainpage/categories", // Mainpage-Kategorien hinzufügen
      "/api/mainpage/products",   // Mainpage-Produkte hinzufügen
      "/api/mainpage/banner-images", //Mainpage-Bilder hinzufügen
      "/api/mainpage/product" // Neue Route für Produktdetails
    ],
    target: "https://localhost:7219",
    retryOnError: true, // Wiederholt Anfragen bei Fehlern
    secure: false
  }
];

module.exports = PROXY_CONFIG;
