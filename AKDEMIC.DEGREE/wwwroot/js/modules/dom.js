var _app = (typeof _app !== "undefined") ? _app : {};
_app.modules = (typeof _app.modules !== "undefined") ? _app.modules : {};

_app.modules.dom = {
    replace: function (options) {
        options.nodeType = options.nodeType || 1;
        options.data = options.data || {};

        if (options.element != null) {
            for (var i = 0; i < options.element.length; i++) {
                var childElement = options.element[i];
                var childNodes = childElement.childNodes;

                for (var j = 0; j < childNodes.length; j++) {
                    var childNode = childNodes[0];

                    if (childNode.nodeType == options.nodeType) {
                        //childNode.data = html;
                        childNode.nodeValue = html;
                        //childNode.textContent = html;
                        //childNode.wholeText = html;
                    }
                }
            }
        }
    }
};
