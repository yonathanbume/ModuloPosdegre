var _app = (typeof _app !== "undefined") ? _app : {};
_app.modules = (typeof _app.modules !== "undefined") ? _app.modules : {};

_app.modules.form = {
    fill: function (options) {
        for (var key in options.data) {
            if (options.data.hasOwnProperty(key)) {
                var value = options.data[key];

                if (value != null) {
                    options.element[key].value = value;
                }
            }
        }
    },
    post: function (path, params) {
        var form = document.createElement("form");
        form.setAttribute("method", "POST");
        form.setAttribute("action", path);

        var token = document.createElement("input");
        token.setAttribute("type", "hidden");
        token.setAttribute("name", "_token");
        token.setAttribute("value", $("meta[name=\"csrf-token\"]").attr("content"));
        form.appendChild(token);

        for (var key in params) {
            if (params.hasOwnProperty(key)) {
                var hiddenField = document.createElement("input");
                hiddenField.setAttribute("type", "hidden");
                hiddenField.setAttribute("name", key);
                hiddenField.setAttribute("value", params[key]);

                form.appendChild(hiddenField);
            }
        }

        document.body.appendChild(form);
        form.submit();
    },
    reset: function (options) {
        options.removeAria = options.removeAria || false;
        options.removeClass = options.removeClass || false;
        options.removeData = options.removeData || false;
        options.removeId = options.removeId || false;
        options.removeRole = options.removeRole || false;
        options.removeStyle = options.removeStyle || false;

        options.element.reset();

        var elementAttributes = options.element.attributes;
        var elementAttributesLength = elementAttributes.length;

        for (var i = elementAttributesLength - 1; i >= 0; i--) {
            var elementAttribute = elementAttributes[i];
            var elementAttributeName = elementAttribute.name.toLowerCase();

            if (
                (elementAttributeName.indexOf("aria") !== -1 && options.removeAria) ||
                (elementAttributeName.indexOf("class") !== -1 && options.removeClass) ||
                (elementAttributeName.indexOf("data") !== -1 && options.removeData) ||
                (elementAttributeName.indexOf("id") !== -1 && options.removeId) ||
                (elementAttributeName.indexOf("role") !== -1 && options.removeRole) ||
                (elementAttributeName.indexOf("style") !== -1 && options.removeStyle)
            ) {
                options.element.removeAttribute(elementAttributeName)
            }
        }
    }
};
