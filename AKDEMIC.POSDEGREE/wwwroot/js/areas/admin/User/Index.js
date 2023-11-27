
var index = function () {
    var datatable = {
        User: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/user/getalluser",
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.search = $("#startDate").val();
                    }

                },
                columns: [
                    { data: "dni", title: "numero DNI" },
                    { data: "fullName", title: "Nombres completos" },
                    {data:  "email",title:"correo"}

                   
                   

                ]
            },
            reload: function () {
                datatable.User.object.ajax.reload();
                datatable.User.options.ajax.data();
            },
            init: function () {
               
                datatable.User.object = $("#datatable_data").DataTable(datatable.User.options);
            }

        }
    }
    var events = {
        onSearch: function () {

            $("#startDate").doneTyping(function () {
                datatable.User.reload();
            })
        },
        init: function () {
            this.onSearch();
        }
    }
    return {
        init: function () {
           
            datatable.User.init();
            events.init()
        }
    }
}();
$(function () {
    index.init();
});