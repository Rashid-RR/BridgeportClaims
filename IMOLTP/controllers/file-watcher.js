var path = require('path');
var fs = require('fs');
var cheerio = require("cheerio");
var getFileMeta = function (name, file, callback) {
    var stats = fs.statSync(file);
    let thefile = {
        size: stats.size,
        atime: stats.atime,
        ctime: stats.ctime,
        mtime: stats.mtime,
        birthtime: stats.birthtime,
        filePath: file,
        name: name,
        type: stats.isFile() ? 'file' : 'directory'
    }
    if (stats.isFile() && thefile.filePath.indexOf('.html') == -1) {
        callback(null);
    } else if (stats.isFile()) {
        fs.readFile(thefile.filePath, 'utf8', (err, text) => {
            var $ = cheerio.load(text);
            var title = $("title"); //get hold of title text
            if(title.html()) thefile.name = title.html().substring(title.html().lastIndexOf("[") + 1, title.html().lastIndexOf("]"));
            callback(thefile);            
        });
    } else {
        callback(thefile);
    }
}
var walkSync = function (dir, callback) {
    var fs = fs || require('fs'),
        files = fs.readdirSync(dir);
    var filelist = [];
    var itemsProcessed = 0;
    files.forEach(file => {
        getFileMeta(file, dir + '/' + file, (newFile) => {
            itemsProcessed++;
            if (newFile) {
                file = newFile;
                filelist.push(file);
            }
            if (itemsProcessed === files.length) {
                callback(filelist);
            }
        })
    });
};
exports.searchFiles = (req, res) => {
    let filePath = req.body.path || global.config.watchpath;
    walkSync(filePath, (files) => {
        res.render('index', { title: 'Express', files: files });
    });
}
exports.viewFile = (req, res) => {
    if (!req.body.path) {
        res.redirect("/");
    } else {
        fs.readFile(req.body.path, 'utf8', function (err, text) {
            res.send(text);
        });
    }
}