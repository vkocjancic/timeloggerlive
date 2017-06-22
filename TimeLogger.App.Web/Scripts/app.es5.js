'use strict';

(function ($) {
    var navigationBarDate = new DateNavigationBar('date-nav');
    var timeLogList = new TimeLogList('entry-list');
    var dailyReport = new DailyReport('dailyReportModal');

    // #region jQuery extensions

    $.prototype.textOnly = function () {
        return $(this).clone().children().remove().end().text();
    };

    // #endregion

    // #region Array extensions

    Array.prototype.removeValue = function (name, value) {
        var array = $.map(this, function (v, i) {
            return v[name] === value ? null : v;
        });
        this.length = 0;
        this.push.apply(this, array);
    };

    // #endregion

    // #region Date extensions

    Date.prototype.locale = {
        en: {
            month_names: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
        }
    };

    Date.prototype.addDays = function (days) {
        var dat = new Date(this.valueOf());
        dat.setDate(dat.getDate() + days);
        return dat;
    };

    Date.prototype.getMonthName = function (lang) {
        lang = lang && lang in this.locale ? lang : 'en';
        return this.locale[lang].month_names[this.getMonth()];
    };

    Date.prototype.toApiDateString = function () {
        return this.getFullYear() + '-' + ('0' + (this.getMonth() + 1)).slice(-2) + '-' + ('0' + this.getDate()).slice(-2);
    };

    // #endregion

    // #region TemplateFormatter
    function TemplateFormatter(replacements) {
        this.replacements = replacements;
    }

    TemplateFormatter.prototype.format = function (template) {
        var replacements = this.replacements;
        var re = new RegExp(Object.keys(replacements).join("|"), "gi");
        template = template.replace(re, function (matched) {
            return replacements[matched];
        });
        return template;
    };

    // #endregion

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

    function DurationFormatter() {}

    DurationFormatter.prototype.format = function (totalInMinutes) {
        var hours = Math.floor(totalInMinutes / 60),
            minutes = totalInMinutes % 60;
        return hours + "h " + minutes + "m";
    };

    // #endregion

    // #region ErrorHandler

    function ErrorHandler(obj) {
        this.element = obj;
        this.timeout = 5000;
    }

    ErrorHandler.prototype.displayMessage = function (type, message) {
        var alert = { 'icon': '', 'bgcolor': '', 'message': message };
        if ('success' === type) {
            alert.icon = '<i class="fa fa-check-circle text-success"></i>';
            alert.bgcolor = 'bg-success';
        } else if ('error' === type) {
            alert.icon = '<i class="fa fa-exclamation-circle text-danger"></i>';
            alert.bgcolor = 'bg-danger';
        }
        this.displayAlert(alert);
    };

    ErrorHandler.prototype.displayAlert = function (alert) {
        var obj = this.element;
        obj.get(0).className = alert.bgcolor;
        obj.html(alert.icon + '&nbsp;' + alert.message);
        obj.show();
        setTimeout(function () {
            obj.hide();
        }, this.timeout);
    };

    // #endregion

    // #region DateNavigationBar

    function DateNavigationBar(id) {
        this.element = $('#' + id);
        this.startDate = new Date();
        //this.startDate.setTime(this.startDate.getTime() - this.startDate.getTimezoneOffset() * 60000);
        this.elementStatus = $('li.status i', this.element);
    }

    DateNavigationBar.prototype.dateOffset = 0;

    DateNavigationBar.prototype.selectedDate = function () {
        return this.startDate.addDays(this.dateOffset);
    };

    DateNavigationBar.prototype.init = function () {
        var obj = this;
        $('li:eq(0) a', this.element).click(function (e) {
            e.preventDefault();
            obj.addDays(-1);
        });
        $('li:eq(1) a', this.element).click(function (e) {
            e.preventDefault();
            obj.addDays(1);
        });
    };

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

    DateNavigationBar.prototype.displayDateString = function (dat) {
        return dat.getMonthName() + ' ' + dat.getDate() + ', ' + dat.getFullYear();
    };

    DateNavigationBar.prototype.toggleStatus = function (visible) {
        var status = new TimeLogStatus(this.elementStatus);
        if (visible) {
            status.displayLoading();
        } else {
            status.displayNone();
        }
    };

    // #endregion

    // #region TimeLogList
    function TimeLogList(className) {
        this.regexTimeFormat = /(\d{1,2})[:.](\d{2})\s{0,1}(am|AM|pm|PM)?/;
        this.element = $('.' + className);
        this.placeholderNewItem = $('.entry-ph', this.element);
        this.placeholderLog = '<tr data-id="{data-id}"><td class="time"><div contenteditable="true">{from}</div></td><td class="time"><div contenteditable="true">{to}</div></td><td><div contenteditable="true">{description}</div></td><td>{action}</td><td>{status}</td></tr>';
        this.placeholderTotal = $('.entry-total', this.element);
        this.dateNavigationBar = navigationBarDate;
    }

    TimeLogList.prototype.init = function () {
        var obj = this;
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
            } else {
                $(this).parent().addClass('danger');
            }
        });
        timeLogList.loadItemsForDate(this.dateNavigationBar.selectedDate());
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
                '{action}': '<button class="action-clear"><i class="fa fa-times"></i></button>',
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
        var id = $(obj).attr('data-id');
        if (id && id.length > 0) {
            $('td:lt(3) div', obj).each(function () {
                if ('false' === $(this).prop('contenteditable')) {
                    $(this).prop('contenteditable', 'true');
                    $('div', cell).focus();
                }
            });
            if (!$('.action-save', obj).get(0)) {
                $('<button class="action-save"><i class="fa fa-floppy-o"></i></button>').insertBefore($('td:eq(3) .action-clear', obj));
            }
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
        this.placeholderNewItem.prev().find('td:first-child div').focus();
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
                method: 'DELETE'
            }).done(function (data) {
                tr.remove();
                currentTimeLogs.removeValue('id', id);
                timeLogList.updateTotal();
                errorHandler.displayMessage('success', 'Time log removed successfully');
            }).error(function (xhr, textStatus, errorThrown) {
                if (xhr.responseText) {
                    var json = $.parseJSON(xhr.responseText);
                    errorHandler.displayMessage('error', json.errorDescription ? json.errorDescription : json.Message);
                } else {
                    errorHandler.displayMessage('error', errorThrown);
                }
                $(obj).show();
            });
        } else {
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
                }
            }).done(function (data) {
                status.displaySuccess();
                currentTimeLogs.removeValue('id', logId);
                currentTimeLogs.push({ 'id': logId, 'duration': data.timelog.duration });
                timeLogList.updateTotal();
                errorHandler.displayMessage('success', 'Time log saved successfully');
                $(obj).remove();
            }).error(function (xhr, textStatus, errorThrown) {
                status.displayError();
                if (xhr.responseText) {
                    var json = $.parseJSON(xhr.responseText);
                    errorHandler.displayMessage('error', json.errorDescription ? json.errorDescription : json.Message);
                } else {
                    errorHandler.displayMessage('error', errorThrown);
                }
                $(obj).show();
            });
        } else {
            $.post('/App/api/timelog', {
                id: logId,
                from: fromValue,
                to: toValue,
                description: $(div.get(2)).textOnly(),
                date: timeLogList.dateNavigationBar.selectedDate().toApiDateString()
            }).success(function (data) {
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
            }).fail(function (data) {
                status.displayError();
                if (data.responseText) {
                    errorHandler.displayMessage('error', $.parseJSON(data.responseText).errorDescription);
                } else {
                    errorHandler.displayMessage('error', data.statusText);
                }
                $(obj).show();
            });
        }
    };

    TimeLogList.prototype.loadItemsForDate = function (date, callback) {
        var timeLogList = this,
            errorHandler = new ErrorHandler($('#entry-list-info'));
        $.get('/App/api/timelog', { 'date': navigationBarDate.selectedDate().toApiDateString() }).success(function (data) {
            timeLogList.clearData();
            timeLogList.createItems(data.timelogs);
            if (callback) {
                callback();
            }
        }).fail(function (data) {
            if (data.responseText) {
                errorHandler.displayMessage('error', $.parseJSON(data.responseText).errorDescription);
            } else {
                errorHandler.displayMessage('error', data.statusText);
            }
            if (callback) {
                callback();
            }
        });
    };

    TimeLogList.prototype.isValidTime = function (time) {
        return null == time || 0 === time.length || this.regexTimeFormat.test(time);
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

    // #region DailyReport

    function DailyReport(id) {
        this.element = $('#' + id);
        this.body = $('.modal-body tbody', this.element);
    }

    DailyReport.prototype.init = function () {
        var report = this;
        this.element.on('show.bs.modal', function (event) {
            var modal = $(this);
            var date = navigationBarDate.selectedDate();
            modal.find('.modal-title span').text(navigationBarDate.displayDateString(date));
            report.loadForDate(date, function (reportItems) {
                report.clearItems();
                var duration = 0,
                    formatter = new DurationFormatter();
                for (var cnItem = 0; cnItem < reportItems.length; cnItem++) {
                    var item = reportItems[cnItem];
                    report.body.append('<tr><td>' + item.title + '</td><td>' + item.durationtext + '</td></tr>');
                    duration += item.duration;
                }
                $('td:last', report.footer).text(formatter.format(duration));
            });
        });
    };

    DailyReport.prototype.loadForDate = function (date, callback) {
        var dailyReport = this,
            errorHandler = new ErrorHandler($('#entry-list-info'));
        $.get('/App/api/report', { 'date': navigationBarDate.selectedDate().toApiDateString() }).success(function (data) {
            dailyReport.clearItems();
            if (callback) {
                callback(data.reportItems);
            }
        }).fail(function (data) {
            if (data.responseText) {
                errorHandler.displayMessage('error', $.parseJSON(data.responseText).errorDescription);
            } else {
                errorHandler.displayMessage('error', data.statusText);
            }
        });
    };

    DailyReport.prototype.clearItems = function () {
        $('tr', this.body).remove();
    };

    // #endregion

    // event handlers
    $(document).ready(function () {
        navigationBarDate.init();
        timeLogList.init();
        dailyReport.init();
    });
})(jQuery);

