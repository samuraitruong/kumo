jQuery(function ($) {
    $(document).ready(function () {

        $('#calendar').fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,basicWeek,basicDay'
            },
            defaultDate: new Date(),
            eventLimit: true,
            eventMouseover: function (event, jsEvent, view) {
                $('a[href$="' + event.url + '"]').each(function (index) {
                    $(this).css('background', '#9BD5EE');
                });
            },
            eventMouseout: function (event, jsEvent, view) {
                $('a[href$="' + event.url + '"]').each(function (index) {
                    $(this).css('background', '#3a87ad');
                });
            }
        })

        var serviceRoot = window.location.protocol + '//' + window.location.host + '/';
        $.ajax({
            type: "POST",
            url: serviceRoot + "/erp/ERPCalendarFull?q=" + new Date().getTime(),
            cache: false,
            data: '',
            success: function (events) {
                var data = events.map(function (item) {
                    var duedate = new Date(new Date(item.DueDate).getTime());
                    return {
                        title: item.JobScope,
                        start: moment(item.StartDate).format('MM/DD/YYYY'),
                        end: moment(item.DueDate).add(1, 'days'),
                        url: '/erp/Details/' + item.Id + "?src=calendar",
                    };
                });
                $('#calendar').fullCalendar('addEventSource', data);

                //$('.fc-day-number').hover(function () {
                //    var link = "<a href='/erp/Add?StartDate=" + moment($(this).data('date')).format('DD/MM/YYYY') + "' id='addToggle' class='addToggle'>+ Add</span>";
                //    $(this).prepend(link);
                //},
                //function () {
                //    $(this).find('#addToggle').remove();
                //});

                //$('.fc-day').hover(function () {
                //    var link = "<a href='/erp/Add?StartDate=" + moment($(this).data('date')).format('DD/MM/YYYY') + "' id='addToggle' class='addToggle'>+ Add</span>";
                //    $(this).prepend(link);
                //},
                //function () {
                //    $(this).find('#addToggle').remove();
                //});

                $(document).on('mouseenter', '.fc-day', function () {
                    var link = "<a href='/erp/Add?StartDate=" + moment($(this).data('date')).format('DD/MM/YYYY') + "' id='addToggle' class='addToggle'>+ Add</span>";
                    $(this).prepend(link);
                }).on('mouseleave', '.fc-day', function () {
                    $(this).find('#addToggle').remove();
                });
            }
        });
    });
});
