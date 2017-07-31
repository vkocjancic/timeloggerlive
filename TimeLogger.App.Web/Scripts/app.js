/// <reference path="jquery-1.9.1.js" />
/// <reference path="common.js" />
/// <reference path="Chart.bundle.min.js" />

(function ($) {
    var navigationBarDate,
        selector,
        timeLogList = new TimeLogList('entry-list'),
        dailyReport,
        insightsChart = new InsightsChart('insightsGraph'),
        insightsReport;

    // #region TimeLogStatus

    function TimeLogStatus(obj) {
        this.element = obj;
    }

    TimeLogStatus.prototype.displayNone = function () {
        this.resetClass(this.element.get(0), '');
    };

    TimeLogStatus.prototype.displayNew = function () {
        this.resetClass(this.element.get(0), 'fa fa-plus-circle');
    };

    TimeLogStatus.prototype.displayError = function () {
        this.resetClass(this.element.get(0), 'fa fa-exclamation-circle');
    };

    TimeLogStatus.prototype.displayLoading = function () {
        this.resetClass(this.element.get(0), 'fa fa-spinner fa-pulse fa-fw');
    };

    TimeLogStatus.prototype.displaySuccess = function () {
        this.resetClass(this.element.get(0), 'fa fa-check-circle');
    };

    TimeLogStatus.prototype.resetClass = function (obj, classes) {
        obj.className = classes;
    };

    // #endregion

    // #region DurationFormatter

    function DurationFormatter() {

    }

    DurationFormatter.prototype.format = function (totalInMinutes) {
        var hours = Math.floor(totalInMinutes / 60),
            minutes = totalInMinutes % 60;
        return hours + "h " + minutes + "m";
    };

    // #endregion

    // #region DateNavigationBar

    function DateNavigationBar(id) {
        this.element = $('#' + id);
        this.startDate = new Date();
        this.elementStatus = $('li.status i', this.element);
    }

    DateNavigationBar.prototype.dateOffset = 0;

    DateNavigationBar.prototype.selectedDate = function () { };

    DateNavigationBar.prototype.destroy = function () {
        $('li:eq(0) a', this.element).off('click');
        $('li:eq(1) a', this.element).off('click');
    };

    DateNavigationBar.prototype.init = function () {
        var obj = this;
        this.displayDate();
        this.executeAction();
        $('li:eq(0) a', this.element).click(function (e) {
            e.preventDefault();
            obj.step(-1);
        });
        $('li:eq(1) a', this.element).click(function (e) {
            e.preventDefault();
            obj.step(1);
        });
    };

    DateNavigationBar.prototype.step = function (stepIncrement) {
        this.dateOffset += stepIncrement;
        this.displayDate();
        this.toggleStatus(true);
        this.executeAction();
    };

    DateNavigationBar.prototype.executeAction = function () { };

    DateNavigationBar.prototype.addDays = function (days) {
        this.dateOffset += days;
        this.displayDate();
        this.toggleStatus(true);
        var obj = this;
        timeLogList.loadItemsForDate(this.selectedDate(), function () {
            obj.toggleStatus(false);
        });
    };

    DateNavigationBar.prototype.displayDate = function () {
        $('li.info', this.element).text(this.displayDateString(this.selectedDate()));
    };

    DateNavigationBar.prototype.displayDateString = function (dat) { };

    DateNavigationBar.prototype.toggleStatus = function (visible) {
        var status = new TimeLogStatus(this.elementStatus);
        if (visible) {
            status.displayLoading();
        }
        else {
            status.displayNone();
        }
    };

    // #endregion

    // #region DailyDateNavigationBar

    function DailyDateNavigationBar(id) {
        DateNavigationBar.call(this, id);
    }

    DailyDateNavigationBar.prototype = Object.create(DateNavigationBar.prototype);
    DailyDateNavigationBar.prototype.constructor = DailyDateNavigationBar;

    DailyDateNavigationBar.prototype.selectedDate = function () {
        return this.startDate.addDays(this.dateOffset);
    };

    DailyDateNavigationBar.prototype.executeAction = function () {
        var obj = this;
        timeLogList.loadItemsForDate(this.selectedDate(), function () {
            obj.toggleStatus(false);
        });
    };

    DailyDateNavigationBar.prototype.displayDateString = function (dat) {
        return dat.getMonthName() + ' ' + dat.getDate() + ', ' + dat.getFullYear();
    };

    // #endregion

    // #region RangeDateNavigationBar

    function RangeDateNavigationBar(id) {
        DateNavigationBar.call(this, id);
        this.id = id;
        this.interval = 'W';
    }

    RangeDateNavigationBar.prototype = Object.create(DateNavigationBar.prototype);
    RangeDateNavigationBar.prototype.constructor = RangeDateNavigationBar;

    RangeDateNavigationBar.prototype.executeRangeAction = function (startDate, endDate) {
        insightsChart.load(startDate, endDate, this.interval);
        insightsReport.load(startDate, endDate);
        this.toggleStatus(false);
    };

    RangeDateNavigationBar.prototype.displayDateRangeString = function (dateStart, dateEnd) {
        return dateStart.getMonthName() + ' ' + dateStart.getDate() + ', ' + dateStart.getFullYear() +
            ' - ' + dateEnd.getMonthName() + ' ' + dateEnd.getDate() + ', ' + dateEnd.getFullYear();
    };

    // #endregion

    // #region WeeklyDateNavigationBar

    function WeeklyDateNavigationBar(id) {
        RangeDateNavigationBar.call(this, id);
        this.interval = 'W';
    }

    WeeklyDateNavigationBar.prototype = Object.create(RangeDateNavigationBar.prototype);
    WeeklyDateNavigationBar.prototype.constructor = WeeklyDateNavigationBar;

    WeeklyDateNavigationBar.prototype.executeAction = function () {
        var endDate = this.selectedDate();
        var startDate = endDate.addDays(-6);
        this.executeRangeAction(startDate, endDate);
    };

    WeeklyDateNavigationBar.prototype.selectedDate = function () {
        return this.startDate.addDays(this.dateOffset * 7);
    };

    WeeklyDateNavigationBar.prototype.displayDateString = function (dat) {
        var datStart = dat.addDays(-6);
        return this.displayDateRangeString(datStart, dat);
    };

    // #endregion

    // #region MonthlyDateNavigationBar

    function MonthlyDateNavigationBar(id) {
        RangeDateNavigationBar.call(this, id);
        this.interval = 'M';
    }

    MonthlyDateNavigationBar.prototype = Object.create(RangeDateNavigationBar.prototype);
    MonthlyDateNavigationBar.prototype.constructor = MonthlyDateNavigationBar;

    MonthlyDateNavigationBar.prototype.executeAction = function () {
        var endDate = this.selectedDate();
        var startDate = endDate.addMonths(-1).addDays(1);
        this.executeRangeAction(startDate, endDate);
    };

    MonthlyDateNavigationBar.prototype.selectedDate = function () {
        return this.startDate.addMonths(this.dateOffset);
    };

    MonthlyDateNavigationBar.prototype.displayDateString = function (dat) {
        var datStart = dat.addMonths(-1).addDays(1);
        return this.displayDateRangeString(datStart, dat);
    };

    // #endregion

    // #region YearlyDateNavigationBar

    function YearlyDateNavigationBar(id) {
        RangeDateNavigationBar.call(this, id);
        this.interval = 'Y';
    }

    YearlyDateNavigationBar.prototype = Object.create(RangeDateNavigationBar.prototype);
    YearlyDateNavigationBar.prototype.constructor = YearlyDateNavigationBar;

    YearlyDateNavigationBar.prototype.executeAction = function () {
        var endDate = this.selectedDate();
        var startDate = endDate.addYears(-1).addDays(1);
        this.executeRangeAction(startDate, endDate);
    };

    YearlyDateNavigationBar.prototype.selectedDate = function () {
        return this.startDate.addYears(this.dateOffset);
    };

    YearlyDateNavigationBar.prototype.displayDateString = function (dat) {
        var datStart = dat.addYears(-1).addDays(1);
        return this.displayDateRangeString(datStart, dat);
    };

    // #endregion

    // #region TimeLogList
    function TimeLogList(className) {
        this.regexTimeFormat = /(\d{1,2})[:.](\d{2})\s{0,1}(am|AM|pm|PM)?/;
        this.element = $('.' + className);
        this.placeholderNewItem = $('.entry-ph', this.element);
        this.placeholderLog = '<tr data-id="{data-id}"><td class="time"><div contenteditable="true">{from}</div></td><td class="time"><div contenteditable="true">{to}</div></td><td class="description"><div contenteditable="true">{description}</div></td><td>{action}</td><td>{status}</td></tr>';
        this.placeholderTotal = $('.entry-total', this.element);
        this.dateNavigationBar = null;
        this.isUserActivated = glb_userRoles.indexOf("User") !== -1;
    }

    TimeLogList.prototype.init = function (navBarDate) {
        this.dateNavigationBar = navBarDate;
        var obj = this;
        if (obj.isUserActivated) {
            this.placeholderNewItem.click(function () {
                obj.newItem();
            });
            $('tbody', this.element).on('click', 'tr:not(.entry-ph)', function (e) {
                obj.editItem(this, $(e.target).get(0));
            });
            $('tbody', this.element).on('click', 'tr .action-save', function () {
                obj.saveItem(this);
            });
            $('tbody', this.element).on('click', 'tr .action-clear', function () {
                obj.removeItem(this);
            });
            $('tbody', this.element).on('keyup paste input', 'tr td.time div', function () {
                if (obj.isValidTime($(this).textOnly())) {
                    $(this).parent().removeClass('danger');
                }
                else {
                    $(this).parent().addClass('danger');
                }
            });
        }
        else { this.placeholderNewItem.hide(); }
        timeLogList.loadItemsForDate(this.dateNavigationBar.selectedDate());
    };

    TimeLogList.prototype.initTypeahead = function (row) {
        $('td.description div', row).typeahead({
            minLength: 1,
            items: 10,
            showHintOnFocus: true,
            source: function (query, process) {
                if (1 > query.length) return;
                return $.get('/App/api/assignmentsearch/', { query: query }, function (data) {
                    return process(data.assignments);
                });
            }
        });
    };

    TimeLogList.prototype.destroyTypeahead = function (row) {
        $('td.description div', row).typeahead('destroy');
    };

    TimeLogList.prototype.clearData = function () {
        $('tbody tr:not(.entry-ph)', this.element).remove();
        currentTimeLogs = [];
    };

    TimeLogList.prototype.createItems = function (logs) {
        var obj = this;
        $(logs).each(function () {
            var options = {
                '{data-id}': this.id,
                '{from}': this.from,
                '{to}': this.to ? this.to : '',
                '{description}': this.description,
                '{action}': (obj.isUserActivated) ? '<button class="action-clear"><i class="fa fa-times"></i></button>' : '',
                '{status}': '<i class="fa fa-check-circle"></i>'
            },
            formatter = new TemplateFormatter(options);
            obj.placeholderNewItem.before(formatter.format(obj.placeholderLog));
            currentTimeLogs.push({ id: this.id, duration: this.duration });
        });
        this.updateTotal();
        $('tbody tr:not(.entry-ph) td div', this.element).each(function () {
            $(this).prop('contenteditable', 'false');
        });
    };

    TimeLogList.prototype.editItem = function (obj, cell) {
        if (cell.tagName !== 'DIV' && cell.tagName !== 'TD') {
            return;
        }
        var id = $(obj).attr('data-id');
        if (id && id.length > 0) {
            $('td:lt(3) div', obj).each(function () {
                if ('false' === $(this).prop('contenteditable')) {
                    $(this).prop('contenteditable', 'true');
                    if (cell.tagName === 'TD') {
                        $('div', cell).focus();
                    }
                    else {
                        $(cell).focus();
                    }
                }
            });
            if (!$('.action-save', obj).get(0)) {
                $('<button class="action-save"><i class="fa fa-floppy-o"></i></button>').insertBefore($('td:eq(3) .action-clear', obj));
            }
            this.initTypeahead(obj);
        }
    };

    TimeLogList.prototype.newItem = function () {
        var options = {
            '{data-id}': '',
            '{from}': '',
            '{to}': '',
            '{description}': '',
            '{action}': '<button class="action-save"><i class="fa fa-floppy-o"></i></button><button class="action-clear"><i class="fa fa-times"></i></button>',
            '{status}': '<i class="fa fa-plus-circle"></i>'
        },
            formatter = new TemplateFormatter(options);
        this.placeholderNewItem.before(formatter.format(this.placeholderLog));
        var row = this.placeholderNewItem.prev();
        row.find('td:first-child div').focus();
        this.initTypeahead(row);
    };

    TimeLogList.prototype.removeItem = function (obj) {
        var tr = $(obj).parents('tr'),
            id = tr.attr('data-id'),
            status = new TimeLogStatus(tr.find('td:last-child i')),
            errorHandler = new ErrorHandler($('#entry-list-info')),
            timeLogList = this;
        if (id && id.length > 0) {
            status.displayLoading();
            $.ajax({
                url: '/App/api/timelog/' + id,
                method: 'DELETE',
                statusCode: {
                    403: function () {
                        errorHandler.redirectToLogin();
                    }
                }
            })
            .done(function (data) {
                timeLogList.destroyTypeahead(tr);
                tr.remove();
                currentTimeLogs.removeValue('id', id);
                timeLogList.updateTotal();
                errorHandler.displayMessage('success', 'Time log removed successfully');
            })
            .error(function (xhr, textStatus, errorThrown) {
                if (xhr.responseText) {
                    var json = $.parseJSON(xhr.responseText);
                    errorHandler.displayMessage('error', json.errorDescription ? json.errorDescription : json.Message);
                }
                else {
                    errorHandler.displayMessage('error', errorThrown);
                }
                $(obj).show();
            });
        }
        else {
            timeLogList.destroyTypeahead(tr);
            tr.remove();
            currentTimeLogs.removeValue('id', id);
            timeLogList.updateTotal();
        }
    };

    TimeLogList.prototype.saveItem = function (obj) {
        var tr = $(obj).parents('tr'),
            logId = tr.attr('data-id'),
            div = tr.find('td div[contenteditable]'),
            status = new TimeLogStatus(tr.find('td:last-child i')),
            errorHandler = new ErrorHandler($('#entry-list-info')),
            timeLogList = this;
        div.each(function (index) {
            $(this).prop('contenteditable', false);
        });
        var fromValue = $(div.get(0)).textOnly();
        var toValue = $(div.get(1)).textOnly();
        if (!this.isValidTime(fromValue) || !this.isValidTime(toValue)) {
            return;
        }
        $(obj).hide();
        status.displayLoading();
        if (logId && logId.length > 0) {
            $.ajax({
                url: '/App/api/timelog/',
                method: 'PUT',
                data: {
                    'id': logId,
                    'from': fromValue,
                    'to': toValue,
                    'description': $(div.get(2)).textOnly(),
                    'date': timeLogList.dateNavigationBar.selectedDate().toApiDateString()
                },
                statusCode: {
                    403: function () {
                        errorHandler.redirectToLogin();
                    }
                }
            })
            .done(function (data) {
                timeLogList.destroyTypeahead(tr.get(0));
                status.displaySuccess();
                currentTimeLogs.removeValue('id', logId);
                currentTimeLogs.push({ 'id': logId, 'duration': data.timelog.duration });
                timeLogList.updateTotal();
                errorHandler.displayMessage('success', 'Time log saved successfully');
                $(obj).remove();
            })
            .error(function (xhr, textStatus, errorThrown) {
                status.displayError();
                if (xhr.responseText) {
                    var json = $.parseJSON(xhr.responseText);
                    errorHandler.displayMessage('error', json.errorDescription ? json.errorDescription : json.Message);
                }
                else {
                    errorHandler.displayMessage('error', errorThrown);
                }
                $(obj).show();
            });
        }
        else {
            $.post('/App/api/timelog', {
                id: logId,
                from: fromValue,
                to: toValue,
                description: $(div.get(2)).textOnly(),
                date: timeLogList.dateNavigationBar.selectedDate().toApiDateString()
            })
            .success(function (data) {
                timeLogList.destroyTypeahead(tr.get(0));
                currentTimeLogs.push({ 'id': data.timelog.id, 'duration': data.timelog.duration });
                timeLogList.updateTotal();
                status.displaySuccess();
                tr.attr('data-id', data.timelog.id);
                $(div.get(0)).text(data.timelog.from);
                if (data.timelog.to) {
                    $(div.get(1)).text(data.timelog.to);
                }
                $(obj).remove();
                errorHandler.displayMessage('success', 'Time log saved successfully');
            })
            .fail(function (data) {
                if (data.status === 403) {
                    errorHandler.redirectToLogin();
                }
                status.displayError();
                if (data.responseText) {
                    errorHandler.displayMessage('error', $.parseJSON(data.responseText).errorDescription);
                }
                else {
                    errorHandler.displayMessage('error', data.statusText);
                }
                $(obj).show();
            });
        }
    };

    TimeLogList.prototype.loadItemsForDate = function (date, callback) {
        var timeLogList = this,
            errorHandler = new ErrorHandler($('#entry-list-info'));
        $.get('/App/api/timelog', { 'date': navigationBarDate.selectedDate().toApiDateString() })
        .success(function (data) {
            timeLogList.clearData();
            timeLogList.createItems(data.timelogs);
            if (callback) {
                callback();
            }
        })
        .fail(function (data) {
            if (data.status === 403) {
                errorHandler.redirectToLogin();
            }
            if (data.responseText) {
                errorHandler.displayMessage('error', $.parseJSON(data.responseText).errorDescription);
            }
            else {
                errorHandler.displayMessage('error', data.statusText);
            }
            if (callback) {
                callback();
            }
        });
    };

    TimeLogList.prototype.isValidTime = function (time) {
        return null === time
            || 0 === time.length
            || this.regexTimeFormat.test(time);
    };

    TimeLogList.prototype.updateTotal = function () {
        if (currentTimeLogs) {
            var total = 0,
                formatter = new DurationFormatter();
            for (var cnLog = 0; cnLog < currentTimeLogs.length; cnLog++) {
                total += currentTimeLogs[cnLog].duration;
            }
            this.placeholderTotal.text(formatter.format(total));
        }
    };
    // #endregion

    // #region Report

    function Report(id) {
        this.element = $('#' + id);
        this.body = $('#reportData tbody', this.element);
        this.footer = $('#reportData tfoot', this.element);
    }

    Report.prototype.init = function (isModal) {
        var report = this;
        if (isModal) {
            this.element.on('show.bs.modal', function (event) {
                var modal = $(this),
                    date = navigationBarDate.selectedDate();

                modal.find('.modal-title span').text(navigationBarDate.displayDateString(date));
                report.loadForDate(date, report.createItems);
            });
        }
    };

    Report.prototype.loadForDate = function (date, callback) { };

    Report.prototype.clearItems = function () {
        $('tr', this.body).remove();
    };

    Report.prototype.createItems = function (report, reportItems) {
        report.clearItems();
        var duration = 0,
            formatter = new DurationFormatter();
        for (var cnItem = 0; cnItem < reportItems.length; cnItem++) {
            var item = reportItems[cnItem];
            report.body.append('<tr><td>' + item.title + '</td><td>' + item.durationtext + '</td></tr>');
            duration += item.duration;
        }
        $('td:last', report.footer).text(formatter.format(duration));
    };

    Report.prototype.displayLoading = function () {
        this.body.append('<tr class="table-loading"><td colspan="2"><i class="fa fa-spinner fa-pulse fa-3x fa-fw"></i></td></tr>');
    };

    // #endregion

    // #region DailyReport

    function DailyReport(id) {
        Report.call(this, id);
    }

    DailyReport.prototype = Object.create(Report.prototype);
    DailyReport.prototype.constructor = DailyReport;

    DailyReport.prototype.loadForDate = function (date, callback) {
        var dailyReport = this,
            errorHandler = new ErrorHandler($('#entry-list-info'));
        dailyReport.clearItems();
        dailyReport.displayLoading();
        $.get('/App/api/report', { 'date': navigationBarDate.selectedDate().toApiDateString() })
        .success(function (data) {
            dailyReport.clearItems();
            if (callback) {
                callback(dailyReport, data.reportItems);
            }
        })
        .fail(function (data) {
            if (data.status === 403) {
                errorHandler.redirectToLogin();
            }
            if (data.responseText) {
                errorHandler.displayMessage('error', $.parseJSON(data.responseText).errorDescription);
            }
            else {
                errorHandler.displayMessage('error', data.statusText);
            }
        });
    };

    // #endregion

    // #region InsightsReport

    function InsightsReport(id) {
        Report.call(this, id);
        this.startDate = null;
    }

    InsightsReport.prototype = Object.create(Report.prototype);
    InsightsReport.prototype.constructor = InsightsReport;

    InsightsReport.prototype.load = function (startDate, endDate) {
        this.startDate = startDate;
        this.loadForDate(endDate, this.createItems);
    };
    
    InsightsReport.prototype.loadForDate = function (date, callback) {
        var report = this,
            errorHandler = new ErrorHandler($('#insights-list-info'));
        report.clearItems();
        report.displayLoading();
        $.post('/App/api/insightsreport', {
            'startDate': report.startDate.toApiDateString(),
            'endDate': date.toApiDateString()
        })
        .success(function (data) {
            report.clearItems();
            if (callback) {
                callback(report, data.reportItems);
            }
        })
        .fail(function (data) {
            if (data.status === 403) {
                errorHandler.redirectToLogin();
            }
            if (data.responseText) {
                errorHandler.displayMessage('error', $.parseJSON(data.responseText).errorDescription);
            }
            else {
                errorHandler.displayMessage('error', data.statusText);
            }
        });
    };

    // #endregion

    // #region DateRangeSelector

    function DateRangeSelector(id) {
        this.element = $('#' + id);
        this.buttons = $('.btn', this.element);
        this.defaultClass = 'btn-default';
        this.navigationBar = null;
    }

    DateRangeSelector.prototype.init = function (navDateBar) {
        var selector = this;
        this.navigationBar = navDateBar;
        selector.buttons.on('click', function () {
            selector.selectionChanged(this);
        });
    };

    DateRangeSelector.prototype.selectionChanged = function (button) {
        if (!$(button).hasClass(this.defaultClass)) {
            return;
        }
        $(this.buttons).addClass(this.defaultClass);
        $(button).removeClass(this.defaultClass);
        this.resetNavigationBar();
    };

    DateRangeSelector.prototype.resetNavigationBar = function () {
        this.navigationBar.destroy();
        if (this.isYearSelected()) {
            this.navigationBar = new YearlyDateNavigationBar(this.navigationBar.id);
        }
        else if (this.isMonthSelected()) {
            this.navigationBar = new MonthlyDateNavigationBar(this.navigationBar.id);
        }
        else {
            this.navigationBar = new WeeklyDateNavigationBar(this.navigationBar.id);
        }
        this.navigationBar.init();
    };

    DateRangeSelector.prototype.isWeekSelected = function () {
        return this.buttons.get(0).className.indexOf(this.defaultClass) === -1;
    };

    DateRangeSelector.prototype.isMonthSelected = function () {
        return this.buttons.get(1).className.indexOf(this.defaultClass) === -1;
    };

    DateRangeSelector.prototype.isYearSelected = function () {
        return this.buttons.get(2).className.indexOf(this.defaultClass) === -1;
    };

    // #endregion

    // #region InsightsChart

    function InsightsChart(id) {
        this.id = id;
        this.element = $('#' + id);
    }

    InsightsChart.prototype.init = function () {
        this.element.attr('height', '100');
    };

    InsightsChart.prototype.load = function (startDate, endDate, interval) {
        var chart = this,
            errorHandler = new ErrorHandler($('#insights-list-info'));
        $.post('/App/api/insightschart', {
            'startDate': startDate.toApiDateString(),
            'endDate': endDate.toApiDateString(),
            'interval': interval
        })
        .success(function (data) {
            chart.draw(data);
        })
        .fail(function (data) {
            if (data.status === 403) {
                errorHandler.redirectToLogin();
            }
            if (data.responseText) {
                errorHandler.displayMessage('error', $.parseJSON(data.responseText).errorDescription);
            }
            else {
                errorHandler.displayMessage('error', data.statusText);
            }
        });
    };

    InsightsChart.prototype.draw = function (data) {
        this.clear();
        var chart = new Chart(this.element, {
            type: 'bar',
            data: {
                datasets: [{
                    label: data.datasets[0].label,
                    data: data.datasets[0].data,
                    backgroundColor: 'rgba(126, 185, 20, 0.3)',
                    borderColor: 'rgba(126, 185, 20, 0.7)', 
                    borderWidth: 2
                },
                {
                    type: 'line',
                    label: data.datasets[1].label,
                    data: data.datasets[1].data,
                    backgroundColor: 'rgba(0, 0, 0, 0)',
                    borderColor: 'rgba(199, 31, 22, 1)' 
                }],
                labels: data.labels
            },
            options: {
                responsive: true,
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        });
    };

    InsightsChart.prototype.clear = function () {
        var parent = this.element.parent('div');
        this.element.remove();
        parent.append('<canvas id="insightsGraph"></canvas>');
        this.element = $('#' + this.id);
        this.init();
    };

    // #endregion

    // #region Site

    var siteEnum = {
        APP: {
            value: 0,
            init: function () {
                navigationBarDate = new DailyDateNavigationBar('date-nav');
                navigationBarDate.init();
                timeLogList.init(navigationBarDate);
                dailyReport = new DailyReport('dailyReportModal');
                dailyReport.init(true);
            }},
        INSIGHTS: {
            value: 1,
            init: function () {
                insightsChart.init();
                insightsReport = new InsightsReport('insightsReport');
                insightsReport.init(false);
                navigationBarDate = new WeeklyDateNavigationBar('date-range-nav');
                navigationBarDate.init();
                selector = new DateRangeSelector('date-range-selector');
                selector.init(navigationBarDate);
            }
        }
    };
    
    function Site() {
        this.type = siteEnum.APP;
        if (document.location.href.indexOf('/Insights') !== -1) {
            this.type = siteEnum.INSIGHTS;
        }
    }

    Site.prototype.init = function () {
        this.type.init();
    };

    // #endregion


    // event handlers
    $(document).ready(function () {
        var site = new Site();
        site.init();
    });

})(jQuery);