var express = require('express');
var router = express.Router();
const fileWatcherController = require('../controllers/file-watcher');
router.all('/', fileWatcherController.searchFiles);
router.post('/view', fileWatcherController.viewFile);
module.exports = router;
