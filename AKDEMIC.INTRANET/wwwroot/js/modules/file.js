var _app = (typeof _app !== "undefined") ? _app : {};
_app.modules = (typeof _app.modules !== "undefined") ? _app.modules : {};

_app.modules.file = {
    getFormattedFileSize: function (bytes) {
        if (bytes == 0) {
            return '0 Bytes';
        }

        var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
        var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
        var fileSize = Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];

        return fileSize;
    }
};
