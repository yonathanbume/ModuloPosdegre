var Events = function () {

    var home_accordion = {
        init: function () { 
            $.ajax({
                url: ('/home/get').proto().parseURL(),
                type: 'GET',
                success: function (result) { 
                    $("#accordion-events").empty();
                    var events = result.events;
                    var divevents = '';
                    var divEmptyEvents = 'No se han registrado eventos próximos';
                    
                    for (var key = 0; key < events.length; key++) { 
                        divevents += '<div class="item eventos">';
                        divevents += '<i class="far fa-calendar icon"></i>';
                        divevents += '<label class="calendar-text">';
                        divevents += events[key].name;
                        divevents += '\n'; 
                        divevents += '<i class="fas fa-map-marker-alt" style="font-size:15px;font-style: normal;"></i>';
                        divevents += '\n';
                        divevents += events[key].place;
                        divevents += '</label>';
                        divevents += '<div class="date">';
                        divevents += events[key].day + ' de ' + events[key].monthname;
                        divevents += '</div>';
                        divevents += '</div>';
                    }
                    if (events.length > 0) {
                        $("#accordion-events").append(divevents);
                    }
                    else {
                        $("#accordion-events").append(divEmptyEvents);
                    }

                    var announcements = result.announcements;
                    var divannouncements = '';
                    var divEmptyAnnouncements = 'No tiene anuncios';
                    for (key = 0; key < announcements.length; key++) {
                        divannouncements += '<div class="item">';
                        divannouncements += '    <label class="code">';
                        divannouncements += announcements[key].title;
                        divannouncements += '</label >';
                        divannouncements += '<label class="text">';
                        divannouncements += announcements[key].description;
                        divannouncements += '</label > ';
                        divannouncements += ' </div>';
                    }
                    if (announcements.length > 0) {
                        $("#accordion-announcements").append(divannouncements);
                    }
                    else {
                        $("#accordion-announcements").append(divEmptyAnnouncements);
                    }
                   

                    var payments = result.payments;
                    var divpayments = '';
                    var divEmptyPayments = 'No tiene deudas pendientes';
                    for (key = 0; key < payments.length; key++) {
                        divpayments += '<div class="item">';
                        divpayments += '    <i class="fas fa-circle" style="color:#f2b81a"></i>';
                        divpayments += '  <div class="pago">';
                        divpayments += ' <div class="pago-name">';
                        divpayments += payments[key].description;
                        divpayments += ' </div>';
                        divpayments += ' <div style=" display:inline-block;">';
                        divpayments += payments[key].total;
                        divpayments += ' </div>';
                        divpayments += ' </div>';
                        divpayments += ' </div>'; 
                    }
                    if (payments.length > 0) {
                        $("#accordion-payments").append(divpayments);
                    }
                    else {
                        $("#accordion-payments").append(divEmptyPayments);
                    }
                    


                    
                    var topics = result.topics;
                    var divtopics = '';
                    var divEmptyTopics = 'No se han creado foros recientemente';
                    for (key = 0; key < topics.length; key++) { 

                        divtopics += '<div class="row foro">';
                        divtopics += '<div style="width:80%">';
                        divtopics += '<i class="fas fa-users"></i>';
                        
                        divtopics += '<label class="foro-text" style="font-weight:500;">';
                        divtopics += '<a href="';
                        divtopics += (`/foro/tema/${topics[key].forumid}/${topics[key].categoryid}`).proto().parseURL()
                        divtopics += '">';
                        divtopics += topics[key].title;
                        divtopics += '</a>';
                        divtopics += '</label>'; 
                        
                        divtopics += '<div class="foro-category">';
                        divtopics += topics[key].category;
                        divtopics += '</div > ';
                        divtopics += '</div>';
                        divtopics += '<div style="width:20%;font-weight:500;">';
                        divtopics += topics[key].count;
                        divtopics += '</div>';
                        divtopics += '</div>';
                    }
                    if (topics.length > 0) {
                        $("#foro-content").append(divtopics);
                    } else {
                        $("#foro-content").append(divEmptyTopics);
                    }
                    var schedules = result.schedules;
                    var divschedules = '';

                    var divTemplate = '';
                    divTemplate += '<table width="100%">';
                    divTemplate += '<thead style="text-align:center;color:#949494">';
                    divTemplate += '<tr>';
                    divTemplate += '<th>Hora</th>';
                    divTemplate += '<th>Curso</th>';
                    divTemplate += '<th>Aula</th>';
                    divTemplate += '</tr>';
                    divTemplate += '</thead>';
                    divTemplate += '<tbody>';

                    var divCloseTemplate = '';
                    divCloseTemplate += '</tbody >';
                    divCloseTemplate += '</table >';

                    var divScheduleMonday = divTemplate;
                    var divScheduleTuesday = divTemplate;
                    var divScheduleWednesday = divTemplate;
                    var divScheduleThursday = divTemplate;
                    var divScheduleFriday = divTemplate;
                    var divScheduleSatuday = divTemplate;

                    console.log(schedules);
                    for (key = 0; key < schedules.length; key++) {

                        switch (schedules[key].day) {
                            case "lunes":
                                divScheduleMonday += '<tr>';
                                divScheduleMonday += '<td>';
                                divScheduleMonday += schedules[key].start;
                                divScheduleMonday += '</td > ';
                                divScheduleMonday += '<td>';
                                divScheduleMonday += schedules[key].title;
                                divScheduleMonday += '</td > ';
                                divScheduleMonday += '<td>';
                                divScheduleMonday += schedules[key].description;
                                divScheduleMonday += '</td > ';
                                divScheduleMonday += '</tr>';
                                break;
                            case "martes":

                                divScheduleTuesday += '<tr>';
                                divScheduleTuesday += '<td>';
                                divScheduleTuesday += schedules[key].start;
                                divScheduleTuesday += '</td > ';
                                divScheduleTuesday += '<td>';
                                divScheduleTuesday += schedules[key].title;
                                divScheduleTuesday += '</td > ';
                                divScheduleTuesday += '<td>';
                                divScheduleTuesday += schedules[key].description;
                                divScheduleTuesday += '</td > ';
                                divScheduleTuesday += '</tr>'; 
                                break;
                            case "miércoles":

                                divScheduleWednesday += '<tr>';
                                divScheduleWednesday += '<td>';
                                divScheduleWednesday += schedules[key].start;
                                divScheduleWednesday += '</td > ';
                                divScheduleWednesday += '<td>';
                                divScheduleWednesday += schedules[key].title;
                                divScheduleWednesday += '</td > ';
                                divScheduleWednesday += '<td>';
                                divScheduleWednesday += schedules[key].description;
                                divScheduleWednesday += '</td > ';
                                divScheduleWednesday += '</tr>';
                                break; 
                            case "jueves":

                                divScheduleThursday += '<tr>';
                                divScheduleThursday += '<td>';
                                divScheduleThursday += schedules[key].start;
                                divScheduleThursday += '</td > ';
                                divScheduleThursday += '<td>';
                                divScheduleThursday += schedules[key].title;
                                divScheduleThursday += '</td > ';
                                divScheduleThursday += '<td>';
                                divScheduleThursday += schedules[key].description;
                                divScheduleThursday += '</td > ';
                                divScheduleThursday += '</tr>';
                                break; 
                            case "viernes":
                                divScheduleFriday += '<tr>';
                                divScheduleFriday += '<td>';
                                divScheduleFriday += schedules[key].start;
                                divScheduleFriday += '</td > ';
                                divScheduleFriday += '<td>';
                                divScheduleFriday += schedules[key].title;
                                divScheduleFriday += '</td > ';
                                divScheduleFriday += '<td>';
                                divScheduleFriday += schedules[key].description;
                                divScheduleFriday += '</td > ';
                                divScheduleFriday += '</tr>';
                                break;
                            case "sábado":
                                divScheduleSatuday += '<tr>';
                                divScheduleSatuday += '<td>';
                                divScheduleSatuday += schedules[key].start;
                                divScheduleSatuday += '</td > ';
                                divScheduleSatuday += '<td>';
                                divScheduleSatuday += schedules[key].title;
                                divScheduleSatuday += '</td > ';
                                divScheduleSatuday += '<td>';
                                divScheduleSatuday += schedules[key].description;
                                divScheduleSatuday += '</td > ';
                                divScheduleSatuday += '</tr>';
                                break;
                        }
                    }
                    divScheduleMonday += divCloseTemplate;
                    divScheduleTuesday += divCloseTemplate;
                    divScheduleWednesday += divCloseTemplate;
                    divScheduleThursday += divCloseTemplate;
                    divScheduleFriday += divCloseTemplate;
                    divScheduleSatuday += divCloseTemplate;

                    $("#monday").append(divScheduleMonday);
                    $("#tuesday").append(divScheduleTuesday);
                    $("#wednesday").append(divScheduleWednesday);
                    $("#thursday").append(divScheduleThursday);
                    $("#friday").append(divScheduleFriday);
                    $("#saturday").append(divScheduleSatuday);  


                    var schedule = `Horario del ${result.sheduleWeekViewModel.start} al ${result.sheduleWeekViewModel.end}`;
                    $("#schedule").append(schedule);
                }
            });
        }
    };

    return {
        init: function () { 
            home_accordion.init(); 
        }
    }
}();


var Horario = function () {

    var slideIndex = 1;
    
    var loadHorario = {
        init: function () { 
            showSlides(slideIndex);  
        }
    }

    function showSlides(n) {
        var i;
        var slides = document.getElementsByClassName("mySlides");
        var schedules = document.getElementsByClassName("mySchedule");

        if (n > slides.length) { slideIndex = 1}
        if (n < 1) { slideIndex = slides.length }

        for (i = 0; i < slides.length; i++) {
            slides[i].style.display = "none";
            schedules[i].style.display = "none";
        } 
        slides[slideIndex - 1].style.display = "block";
        schedules[slideIndex - 1].style.display = "block"; 
    }
 


    var events = {
        init: function () {
            $(".prev").on('click', function () {
                showSlides(slideIndex -= 1);
            });

            $(".next").on('click', function () {
                showSlides(slideIndex += 1);
            });
        }
    }


    return {
        init: function () {
            loadHorario.init();
            events.init();
        }
    }
}();

$(function () {
    Events.init();
    Horario.init(); 

});

