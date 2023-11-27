var Notifications = function () {

    var notifications = [];
    var pending_notifications = [];

    var push = function (n) {
        pending_notifications.push(n);

        $(".m_topbar_notification_icon .m-nav__link-icon").addClass("m-animate-shake");
        $(".m_topbar_notification_icon .m-nav__link-badge").addClass("m-badge");

        if (n.sound) {
            var snd = new Audio('/audio/institutional-alarm.mp3');
            snd.play();
        }

        setTimeout(function () {
            $(".m_topbar_notification_icon .m-nav__link-icon").removeClass("m-animate-shake")
        }, 3e3);


        load_slopes();
    };

    var events = {
        init: function () {
            $(".sendAlert").on('click', function () {
                var alertButton = $(this);
                alertButton.addLoader();
                $.ajax({
                    url: ("/alertaInstitucional/enviar").proto().parseURL(),
                    type: "POST",
                    success: function (result) {
                        // console.log('success');
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        alertButton.removeLoader();
                    },
                    error: function () {
                        toastr.error("Error al enviar la alerta", _app.constants.toastr.title.error);
                        alertButton.removeLoader();
                    }
                });
            });

            $(".m_topbar_notification_icon").click(function () {
                if (pending_notifications.length === 0 && open) {
                    setTimeout(function () {
                        $(".m-list-timeline__item.new").removeClass("new");
                    }, 3e3);
                }
            });
        }
    };

    var load = {
        init: function () {
            $.ajax({
                type: "GET",
                url: "/notification/get",
                success: function (data) {
                    notifications = data.result;
                    $("#topbar_notifications_notifications_1 .m-list-timeline__items").empty();
                    $("#topbar_notifications_notifications_1 .m-stack--ver.m-stack--general").remove();

                    $("#topbar_notifications_messages_1 .m-list-timeline__items").empty();
                    $("#topbar_notifications_messages_1 .m-stack--ver.m-stack--general").remove();

                    $("#topbar_notifications_notifications_2 .m-list-timeline__items").empty();
                    $("#topbar_notifications_notifications_2 .m-stack--ver.m-stack--general").remove();

                    $("#topbar_notifications_messages_2 .m-list-timeline__items").empty();
                    $("#topbar_notifications_messages_2 .m-stack--ver.m-stack--general").remove();

                    $(".number-notification").text(data.pending);
                    if (data.pending > 0) {
                        $(".m_topbar_notification_icon .m-nav__link-badge").addClass("m-badge");
                    }
                    var divMobile = $("#topbar_notifications_notifications_1 .m-scrollable");
                    var divDesktop = $("#topbar_notifications_notifications_2 .m-scrollable");

                    var messageDivMobile = $("#topbar_notifications_messages_1 .m-scrollable");
                    var messageDivDesktop = $("#topbar_notifications_messages_2 .m-scrollable");


                    if (notifications.length <= 0) {
                        $("#topbar_notifications_notifications_1 .m-list-timeline .m-list-timeline--skin-light").remove();
                        $("#topbar_notifications_notifications_2 .m-list-timeline .m-list-timeline--skin-light").remove();
                        divMobile.prepend(
                            "<div class='m-stack m-stack--ver m-stack--general' style='min-height: 180px;'>" +
                            "<div class='m-stack__item m-stack__item--center m-stack__item--middle'>" +
                            "<span>No existen notificaciones.</span>" +
                            "</div>" +
                            "</div>"
                        );

                        divDesktop.prepend(
                            "<div class='m-stack m-stack--ver m-stack--general' style='min-height: 180px;'>" +
                            "<div class='m-stack__item m-stack__item--center m-stack__item--middle'>" +
                            "<span>No existen notificaciones.</span>" +
                            "</div>" +
                            "</div>"
                        );
                    }
                    else {
                        $.each(notifications, function (key, value) {
                            $("#topbar_notifications_notifications_1 .m-list-timeline__items").append(
                                "<div class='m-list-timeline__item new" + (value.readed === true ? ' m-list-timeline__item--read' : '') + "'>" +
                                "<span class='m-list-timeline__badge'></span>" +
                                "<span class='m-list-timeline__text'>" + (value.url !== '' ? "<a href='/redirect/" + value.id + "' class='m-link'>" : '') + value.text + "</a>" + (value.state_text !== '' ? " <span class='m-badge " + value.background_state_class + " m-badge--wide'>" + value.state_text + "</span>" : '') + "</span>" +
                                "<span class='m-list-timeline__time'>" + value.date + "</span>" +
                                "</div>");

                            $("#topbar_notifications_notifications_2 .m-list-timeline__items").append(
                                "<div class='m-list-timeline__item new" + (value.readed === true ? ' m-list-timeline__item--read' : '') + "'>" +
                                "<span class='m-list-timeline__badge'></span>" +
                                "<span class='m-list-timeline__text'>" + (value.url !== '' ? "<a href='/redirect/" + value.id + "' class='m-link'>" : '') + value.text + "</a>" + (value.state_text !== '' ? " <span class='m-badge " + value.background_state_class + " m-badge--wide'>" + value.state_text + "</span>" : '') + "</span>" +
                                "<span class='m-list-timeline__time'>" + value.date + "</span>" +
                                "</div>");
                        });
                    }

                    if (data.messages.length <= 0) {
                        $("#topbar_notifications_messages_1 .m-list-timeline .m-list-timeline--skin-light").remove();
                        $("#topbar_notifications_messages_2 .m-list-timeline .m-list-timeline--skin-light").remove();
                        messageDivMobile.prepend(
                            "<div class='m-stack m-stack--ver m-stack--general' style='min-height: 180px;'>" +
                            "<div class='m-stack__item m-stack__item--center m-stack__item--middle'>" +
                            "<span>No existen mensajes.</span>" +
                            "</div>" +
                            "</div>"
                        );
                        messageDivDesktop.prepend(
                            "<div class='m-stack m-stack--ver m-stack--general' style='min-height: 180px;'>" +
                            "<div class='m-stack__item m-stack__item--center m-stack__item--middle'>" +
                            "<span>No existen mensajes.</span>" +
                            "</div>" +
                            "</div>"
                        );
                    }
                    else {
                        data.messages.forEach(function (e, i) {
                            let tmp = `<div class="m-list-timeline__item">`;
                            tmp += `         <span class="m-list-timeline__badge -m-list-timeline__badge--state-success"></span>`
                            tmp += `         <span class="m-list-timeline__text"><strong>${e.title}</strong><p>${e.message}</p></span>`
                            tmp += `         <span class="m-list-timeline__time">${e.createdAt}</span>`
                            tmp += `   </div>`;
                            $("#topbar_notifications_messages_1 .m-list-timeline__items").append(`${tmp}`);
                        });

                        data.messages.forEach(function (e, i) {
                            let tmp = `<div class="m-list-timeline__item">`;
                            tmp += `         <span class="m-list-timeline__badge -m-list-timeline__badge--state-success"></span>`
                            tmp += `         <span class="m-list-timeline__text"><strong>${e.title}</strong><p>${e.message}</p></span>`
                            tmp += `         <span class="m-list-timeline__time">${e.createdAt}</span>`
                            tmp += `   </div>`;
                            $("#topbar_notifications_messages_2 .m-list-timeline__items").append(`${tmp}`);
                        });
                    }

                },
                complete: function () {
                }
            });
        }
    };

    var load_slopes = function () {
        $.ajax({
            type: "GET",
            url: "/notification/get",
            success: function (data) {
                notifications = data.result;
                $("#topbar_notifications_notifications_1 .m-list-timeline__items").empty();
                $("#topbar_notifications_notifications_1 .m-stack--ver.m-stack--general").remove();

                $("#topbar_notifications_messages_1 .m-list-timeline__items").empty();
                $("#topbar_notifications_messages_1 .m-stack--ver.m-stack--general").remove();

                $("#topbar_notifications_notifications_2 .m-list-timeline__items").empty();
                $("#topbar_notifications_notifications_2 .m-stack--ver.m-stack--general").remove();

                $("#topbar_notifications_messages_2 .m-list-timeline__items").empty();
                $("#topbar_notifications_messages_2 .m-stack--ver.m-stack--general").remove();

                $(".number-notification").text(data.pending);
                if (data.pending > 0) {
                    $(".m_topbar_notification_icon .m-nav__link-badge").addClass("m-badge");
                }
                var divMobile = $("#topbar_notifications_notifications_1 .m-scrollable");
                var divDesktop = $("#topbar_notifications_notifications_2 .m-scrollable");

                var messageDivMobile = $("#topbar_notifications_messages_1 .m-scrollable");
                var messageDivDesktop = $("#topbar_notifications_messages_2 .m-scrollable");


                if (notifications.length <= 0) {
                    $("#topbar_notifications_notifications_1 .m-list-timeline .m-list-timeline--skin-light").remove();
                    $("#topbar_notifications_notifications_2 .m-list-timeline .m-list-timeline--skin-light").remove();
                    divMobile.prepend(
                        "<div class='m-stack m-stack--ver m-stack--general' style='min-height: 180px;'>" +
                        "<div class='m-stack__item m-stack__item--center m-stack__item--middle'>" +
                        "<span>No existen notificaciones.</span>" +
                        "</div>" +
                        "</div>"
                    );

                    divDesktop.prepend(
                        "<div class='m-stack m-stack--ver m-stack--general' style='min-height: 180px;'>" +
                        "<div class='m-stack__item m-stack__item--center m-stack__item--middle'>" +
                        "<span>No existen notificaciones.</span>" +
                        "</div>" +
                        "</div>"
                    );
                }
                else {
                    $.each(notifications, function (key, value) {
                        $("#topbar_notifications_notifications_1 .m-list-timeline__items").append(
                            "<div class='m-list-timeline__item new" + (value.readed === true ? ' m-list-timeline__item--read' : '') + "'>" +
                            "<span class='m-list-timeline__badge'></span>" +
                            "<span class='m-list-timeline__text'>" + (value.url !== '' ? "<a href='/redirect/" + value.id + "' class='m-link'>" : '') + value.text + "</a>" + (value.state_text !== '' ? " <span class='m-badge " + value.background_state_class + " m-badge--wide'>" + value.state_text + "</span>" : '') + "</span>" +
                            "<span class='m-list-timeline__time'>" + value.date + "</span>" +
                            "</div>");

                        $("#topbar_notifications_notifications_2 .m-list-timeline__items").append(
                            "<div class='m-list-timeline__item new" + (value.readed === true ? ' m-list-timeline__item--read' : '') + "'>" +
                            "<span class='m-list-timeline__badge'></span>" +
                            "<span class='m-list-timeline__text'>" + (value.url !== '' ? "<a href='/redirect/" + value.id + "' class='m-link'>" : '') + value.text + "</a>" + (value.state_text !== '' ? " <span class='m-badge " + value.background_state_class + " m-badge--wide'>" + value.state_text + "</span>" : '') + "</span>" +
                            "<span class='m-list-timeline__time'>" + value.date + "</span>" +
                            "</div>");
                    });
                }

                if (data.messages.length <= 0) {
                    $("#topbar_notifications_messages_1 .m-list-timeline .m-list-timeline--skin-light").remove();
                    $("#topbar_notifications_messages_2 .m-list-timeline .m-list-timeline--skin-light").remove();
                    messageDivMobile.prepend(
                        "<div class='m-stack m-stack--ver m-stack--general' style='min-height: 180px;'>" +
                        "<div class='m-stack__item m-stack__item--center m-stack__item--middle'>" +
                        "<span>No existen mensajes.</span>" +
                        "</div>" +
                        "</div>"
                    );
                    messageDivDesktop.prepend(
                        "<div class='m-stack m-stack--ver m-stack--general' style='min-height: 180px;'>" +
                        "<div class='m-stack__item m-stack__item--center m-stack__item--middle'>" +
                        "<span>No existen mensajes.</span>" +
                        "</div>" +
                        "</div>"
                    );
                }
                else {
                    data.messages.forEach(function (e, i) {
                        let tmp = `<div class="m-list-timeline__item">`;
                        tmp += `         <span class="m-list-timeline__badge -m-list-timeline__badge--state-success"></span>`
                        tmp += `         <span class="m-list-timeline__text"><strong>${e.title}</strong><p>${e.message}</p></span>`
                        tmp += `         <span class="m-list-timeline__time">${e.createdAt}</span>`
                        tmp += `   </div>`;
                        $("#topbar_notifications_messages_1 .m-list-timeline__items").append(`${tmp}`);
                    });

                    data.messages.forEach(function (e, i) {
                        let tmp = `<div class="m-list-timeline__item">`;
                        tmp += `         <span class="m-list-timeline__badge -m-list-timeline__badge--state-success"></span>`
                        tmp += `         <span class="m-list-timeline__text"><strong>${e.title}</strong><p>${e.message}</p></span>`
                        tmp += `         <span class="m-list-timeline__time">${e.createdAt}</span>`
                        tmp += `   </div>`;
                        $("#topbar_notifications_messages_2 .m-list-timeline__items").append(`${tmp}`);
                    });
                }
            },
            complete: function () {
            }
        });
        /*
        if (notifications.length === 0) {

            var div = $("#topbar_notifications_notifications .m-scrollable #mCSB_1_container");

            $("#topbar_notifications_notifications .m-stack--ver.m-stack--general").remove();

            div.prepend(
                "<div class='m-list-timeline m-list-timeline--skin-light'>" +
                    "<div class='m-list-timeline__items'></div>" +
                "</div>"
            );
        }

        $.ajax({
            type: "GET",
            url: "/notification/noreaded/count/get",
            success: function (data) {
                $(".number-notification").text(data); 
            }
        });

        $.each(pending_notifications, function (key, value) {
            notifications.unshift(value);
            $("#topbar_notifications_notifications .m-list-timeline__items")
                .prepend(
                    "<div class='m-list-timeline__item new" + (value.readed === true ? ' m-list-timeline__item--read' : '') + "'>" +
                    "<span class='m-list-timeline__badge'></span>" +
                    "<span class='m-list-timeline__text'>" + (value.url !== '' ? "<a href='/redirect/" + value.id + "' class='m-link'>" : '') + value.text + "</a>" + (value.state_text !== '' ? " <span class='m-badge " + value.background_state_class + " m-badge--wide'>" + value.state_text + "</span>" : '') + "</span>" +
                    "<span class='m-list-timeline__time'>" + value.date + "</span>" +
                    "</div>");
        });

        pending_notifications = [];
        $(".m-scrollable").animate({ scrollTop: 0 }, 'slow');
        $("#mCSB_1_container").animate({ top: 0 });
        $("#mCSB_1_dragger_vertical").animate({ top: 0 });*/
    };

    return {
        init: function () {
            load.init();
            events.init();
            /*SIGNALR - NOTIFICATIONS */
            //"use strict";
            //var connection = new signalR.HubConnectionBuilder().withUrl("/Hubs/Akdemic").build();
            //connection.start().then(function () {
            //    //document.getElementById("sendButton").disabled = false;
            //})
            //    .catch(function (err) {
            //        return console.error(err.toString());
            //    });
            //connection.on("ReceiveNotification", function (NotificationType) {
            //    if (NotificationType == _app.constants.toastr.notificationType.surveys) {
            //        toastr.info(_app.constants.toastr.message.info.surveys, _app.constants.toastr.title.info);
            //    } else if (NotificationType == _app.constants.toastr.notificationType.grades) {
            //        toastr.info(_app.constants.toastr.message.info.grades, _app.constants.toastr.title.info);
            //    }
            //    load_slopes();
            //});
        },

        load_slopes: load_slopes,
        push: push
    };
}();

$(function () {
    Notifications.init();
});