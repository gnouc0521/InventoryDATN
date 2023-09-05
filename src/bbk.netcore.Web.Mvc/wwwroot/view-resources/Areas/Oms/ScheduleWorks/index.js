(function () {
    var _schedulework = abp.services.app.scheduleWork;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ScheduleWork/CreateScheduleWork',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ScheduleWorks/_CreateModal.js',
        modalClass: 'ScheduleWorkCreateModal',
        modalType: 'modal-xl'

    });

    var _editModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ScheduleWork/EditScheduleWorkModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ScheduleWorks/_EditModal.js',
        modalClass: 'ScheduleWorkEditModal',
        modalType: 'modal-xl'

    });

    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });
    

    var getFilter = function () {
        let idLevels = document.getElementById("list-level");
        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val().toLocaleLowerCase();
        dataFilter.idLevel = idLevels.options[idLevels.selectedIndex].value;
        return dataFilter;
    }

  


    function LoadCalender() {
        _schedulework.getAll(getFilter()).done(function (result) {
            var eventsArray = [];
            for (i = 0; i < result.totalCount; i++) {
                let Endday = result.items[i].endDay.slice(0, 10);
               

                if (result.items[i].color == 0) {
                    eventsArray.push({
                        id: result.items[i].id,
                        title: result.items[i].title,
                        start: result.items[i].startDay,
                        end: result.items[i].endDay,
                        backgroundColor: "lightblue"
                    })
                }
                if (result.items[i].color == 1) {
                    eventsArray.push({
                        id: result.items[i].id,
                        title: result.items[i].title,
                        start: result.items[i].startDay,
                        end: result.items[i].endDay,
                        backgroundColor: "lightgreen"
                    })
                }
                if (result.items[i].color == 2) {
                    eventsArray.push({
                        id: result.items[i].id,
                        title: result.items[i].title,
                        start: result.items[i].startDay,
                        end: result.items[i].endDay,
                        backgroundColor: "lightcoral"
                    })
                }
            }

            var calendarEl = document.getElementById('calendar');

            var calendar = new FullCalendar.Calendar(calendarEl,
                {
                    locale: 'vi',
                    plugins: ['dayGrid', 'list', 'timeGrid', 'interaction', 'bootstrap'],
                    themeSystem: 'bootstrap',
                    timeZone: 'UTC',
                    dateAlignment: "month", //week, month
                    buttonText:
                    {
                        today: 'Hôm nay',
                        month: 'Tháng',
                        week: 'Tuần',
                        day: 'Ngày',
                        list: 'Danh sách'
                    },
                    eventTimeFormat:
                    {
                        hour: 'numeric',
                        minute: '2-digit',
                        meridiem: 'short'
                    },
                    navLinks: true,
                    header:
                    {
                        left: 'prev,next today addEventButton',
                        center: 'title',
                        right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
                    },
                    footer:
                    {
                        left: '',
                        center: '',
                        right: ''
                    },
                    /* eventLimit: true, // allow "more" link when too many events*/
                    events: eventsArray,
                    eventClick:  function(info) {
                        //$('#calendarModal .modal-title .js-event-title').text(info.event.title);
                        //$('#calendarModal .js-event-description').text(info.event.description);
                        //$('#calendarModal .js-event-url').attr('href',info.event.url);
                        //$('#calendarModal').modal();
                        //console.log(info.event.className);
                        //console.log(info.event.title);
                        //console.log(info.event.description);
                        //console.log(info.event.url);
                        var dataFilter = { Id: info.event.id };
                        _editModal.open(dataFilter);
                    },
                    /*viewRender: function(view) {
                        localStorage.setItem('calendarDefaultView',view.name);
                        $('.fc-toolbar .btn-primary').removeClass('btn-primary').addClass('btn-outline-secondary');
                    },*/

                });
            
            calendar.render();

        })
    }

    LoadCalender();

    $('#Search').click(function (e) {
        $("#calendar").empty();
        LoadCalender();
    });

    abp.event.on('app.reloadCalendar', function () {
        LoadCalender();
    });

    
    

    //var todayDate = moment().startOf('day');
    //var YM = todayDate.format('YYYY-MM');
    //var YESTERDAY = todayDate.clone().subtract(1, 'day').format('YYYY-MM-DD');
    //var TODAY = todayDate.format('YYYY-MM-DD');
    //var TOMORROW = todayDate.clone().add(1, 'day').format('YYYY-MM-DD');

    
})(jQuery);