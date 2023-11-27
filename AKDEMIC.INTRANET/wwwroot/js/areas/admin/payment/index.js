_app.datatable.elements = {
    'payment-datatable': {
        selector: '#payment-datatable',
        settings: {
            data: {
                source: {
                    read: {
                        method: 'GET',
                        url: '/admin/tramites/get'
                    }
                }
            },
            columns: [
                {
                    field: 'name',
                    title: 'Nombre del Trámite'
                },
                {
                    field: 'duration',
                    title: 'Duración',
                    template: function (row) {
                        var template = '';
                        template += row.duration;
                        template += row.duration !== 1 ? ' días' : ' día';

                        return template;
                    }
                },
                {
                    field: 'options',
                    title: 'Opciones',
                    sortable: false,
                    filterable: false,
                    template: function (row) {
                        var template = '';
                        template += '<a class="btn btn-primary btn-sm m-btn m-btn--icon" href="';
                        template += _app.url.parse('/admin/tramites/editar');
                        template += '/';
                        template += row.id;
                        template += '"><span><i class="la la-edit"></i><span> Editar </span></span></a> ';
                        //template += '<button class="btn btn-primary btn-sm m-btn m-btn--icon" data-action="update" data-url="';
                        //template += _app.url.parse('/admin/tramites/editar/post');
                        //template += '" data-value="';
                        //template += row.app().encode();
                        //template += '" onclick="payment.modal.update.show(this, event)"><span><i class="la la-edit"></i><span> Editar </span></span></button> ';
                        template += '<button class="btn btn-danger m-btn btn-sm m-btn--icon m-btn--icon-only" data-action="delete" data-url="';
                        template += _app.url.parse('/admin/tramites/eliminar/post');
                        template += '" data-value="';
                        template += row.app().encode();
                        template += '" onclick="payment.swal.delete.show(this, event)"><i class="la la-trash"></i></button>';

                        return template;
                    }
                }
            ]
        }
    }
};
_app.validate.elements = {
    'payment-modal-form': {
        selector: '#payment-modal-form',
        settings: {
            submitHandler: function (form, event) {
                var action = form.getAttribute('data-action');

                payment.form[action].async.send(form, event);
            }
        }
    },
    'payment-category-modal-form': {
        selector: '#payment-category-modal-form',
        settings: {
            submitHandler: function (form, event) {
                var action = form.getAttribute('data-action');

                paymentCategory.form[action].async.send(form, event);
            }
        }
    }
};

var payment = {
    form: {
        create: {
            async: {
                send: function (element, event) {
                    var formElements = element.elements;
                    var url = element.getAttribute('data-url');

                    this.settings.data = {
                        Name: formElements['Name'].value,
                        ProcedureCategoryId: formElements['ProcedureCategoryId'].value,
                        Duration: formElements['Duration'].value,
                        Score: formElements['Score'].value
                    };
                    this.settings.url = url;
                    this.settings.beforeSend = function (jqXHR, settings) {
                        _app.form.loader.show(formElements['Send']);
                    };
                    this.settings.complete = function (jqXHR, textStatus) {
                        _app.form.loader.hide(formElements['Send']);
                    };

                    _app.form.async.send(element, event, this);
                },
                settings: {
                    error: function (jqXHR, textStatus, errorThrown) {
                        toastr.error(_app.constant.toastr.message.error.create, _app.constant.toastr.title.error);
                    },
                    success: function (data, textStatus, jqXHR) {
                        _app.datatable.reload.all();
                        $('#payment-modal').modal('hide');
                        toastr.success(_app.constant.toastr.message.success.create, _app.constant.toastr.title.success);
                    }
                }
            }
        },
        update: {
            async: {
                send: function (element, event) {
                    var formElements = element.elements;
                    var url = element.getAttribute('data-url');

                    this.settings.data = {
                        Id: formElements['Id'].value,
                        Name: formElements['Name'].value,
                        ProcedureCategoryId: formElements['ProcedureCategoryId'].value,
                        Duration: formElements['Duration'].value,
                        Score: formElements['Score'].value
                    };
                    this.settings.url = url;
                    this.settings.beforeSend = function (jqXHR, settings) {
                        _app.form.loader.show(formElements['Send']);
                    };
                    this.settings.complete = function (jqXHR, textStatus) {
                        _app.form.loader.hide(formElements['Send']);
                    };

                    _app.form.async.send(element, event, this);
                },
                settings: {
                    error: function (jqXHR, textStatus, errorThrown) {
                        toastr.error(_app.constant.toastr.message.error.update, _app.constant.toastr.title.error);
                    },
                    success: function (data, textStatus, jqXHR) {
                        _app.datatable.reload.all();
                        $('#payment-modal').modal('hide');
                        toastr.success(_app.constant.toastr.message.success.update, _app.constant.toastr.title.success);
                    }
                }
            }
        }
    },
    modal: {
        create: {
            selector: '#payment-modal',
            show: function (element, event) {
                var action = element.getAttribute('data-action');
                var url = element.getAttribute('data-url');

                var data = {};

                _app.form.reset('payment-modal-form');
                _app.form.fill('payment-modal-form', data, action, url);
                _app.modal.show(element, event, this);
            }
        },
        update: {
            selector: '#payment-modal',
            show: function (element, event) {
                var action = element.getAttribute('data-action');
                var url = element.getAttribute('data-url');
                var value = element.getAttribute('data-value');
                value = value.app().decode();

                var data = {
                    Id: value.id,
                    Name: value.name
                };

                _app.form.fill('payment-modal-form', data, action, url);

                var ajaxKey = 0;
                var selectKey = 'payment-category-select2';
                var paymentCategoryId = value.paymentCategory.id;

                if (_app.ajax.elements[ajaxKey].status !== _app.constant.ajax.status.success) {
                    _app.select.elements[selectKey].settings = {
                        data: [
                            {
                                id: paymentCategoryId,
                                name: value.paymentCategory.name
                            }
                        ]
                    };
                }

                _app.select.elements['payment-category-select2'].settings.selected = paymentCategoryId;
                _app.select.elements['payment-score-select2'].settings.selected = value.score;

                _app.select.load.multiple(['payment-category-select2', 'payment-score-select2']);
                _app.select2.load.multiple(['payment-category-select2', 'payment-score-select2']);
                _app.modal.show(element, event, this);
            }
        }
    },
    swal: {
        delete: {
            promise: {},
            settings: {
                title: '¿Desea eliminar el registro?',
                type: 'warning',
                showCancelButton: true,
                showLoaderOnConfirm: true
            },
            show: function (element, event) {
                var url = element.getAttribute('data-url');
                var value = element.getAttribute('data-value');
                value = value.app().decode();

                this.settings.text = value.name;
                this.settings.preConfirm = function () {
                    return new Promise(function (resolve, reject) {
                        var tmpAjaxSettings = {
                            data: {
                                Id: value.id
                            },
                            url: url,
                            complete: function (jqXHR, textStatus) {
                                resolve();
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                toastr.error(_app.constant.toastr.message.error.delete, _app.constant.toastr.title.error);
                            },
                            success: function (data, textStatus, jqXHR) {
                                _app.datatable.reload.all();
                                toastr.success(_app.constant.toastr.message.success.delete, _app.constant.toastr.title.success);
                            }
                        };
                        var ajaxSettings = _app.ajax.settings.app().merge(tmpAjaxSettings);

                        $.ajax(ajaxSettings);
                    });
                };

                _app.swal.show(element, event, this);
            }
        }
    }
};

window.onload = function () {
    _app.ajax.load.all();
    _app.datatable.load.all();
    _app.validate.load.all();
};
