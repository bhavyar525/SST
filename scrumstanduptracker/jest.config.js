const { defaults } = require("jest-config");
 
module.exports = {
  testEnvironment: "jsdom",
  setupFiles: ["<rootDir>/jest.setup.js"],
  transform: {
    "^.+\\.(js|jsx)$": "babel-jest"
  },
  moduleFileExtensions: [...defaults.moduleFileExtensions, "js", "jsx"]
};