var _app = (typeof _app !== "undefined") ? _app : {};
_app.modules = (typeof _app.modules !== "undefined") ? _app.modules : {};

_app.modules.unit = {
    bytesToSize: function (bytes, digits) {
        if (bytes <= 0) {
            return "0B";
        }

        var digits = digits <= 0 ? 0 : digits || 2;
        var constant = 1024;
        var index = Math.floor(Math.log(bytes) / Math.log(constant));
        var size = parseFloat((bytes, Math.pow(constant, index)).toFixed(digits));
        var units = ["B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"];

        return size + units[index];
    }
}