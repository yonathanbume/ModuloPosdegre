var _app = (typeof _app !== "undefined") ? _app : {};
_app.modules = (typeof _app.modules !== "undefined") ? _app.modules : {};

_app.modules.select = {
    fill: function (options) {
        options.data = options.data || {};
        options.name = options.name || "name";
        options.nullable = options.nullable || false;
        options.value = options.value || "id";

        var html = "";
        var selected = false;
        var optionsNames = options.name.split(".");
        var optionsValues = options.value.split(".");

        if (options.data.length <= 0 || options.nullable) {
            html += "<option value";

            if (options.selectedValue == null) {
                html += " selected=\"selected\"";
                selected = true;
            }

            html += ">---</option>";
        }

        if (options.data.constructor === Array) {
            for (var i = 0; i < options.data.length; i++) {
                var dataRow = options.data[i];
                var name = dataRow, value = dataRow;

                for (var j = 0; j < optionsNames.length; j++) {
                    var optionsName = optionsNames[j];

                    name = name[optionsName];
                }

                for (var j = 0; j < optionsValues.length; j++) {
                    var optionValue = optionsValues[j];

                    value = value[optionValue];
                }

                html += "<option value=\"";
                html += value;
                html += "\"";

                if ((!selected && options.selectedValue == null && i == 0) || options.selectedValue == value) {
                    html += " selected=\"selected\"";
                    selected = true;
                }

                html += ">";
                html += name;
                html += "</option>";
            }
        } else if (options.data.constructor === Object) {
            var index = 0;

            for (var value in options.data) {
                var name = options.data[value];
                html += "<option value=\"";
                html += value;
                html += "\"";

                if ((!selected && options.selectedValue == null && index++ == 0) || options.selectedValue == value) {
                    html += " selected=\"selected\"";
                    selected = true;
                }

                html += ">";
                html += name;
                html += "</option>";
            }
        }

        options.element.innerHTML = html;
    }
};
