/// <reference path="jquery-1.9.1.js" />

(function ($) {

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
    }

    Date.prototype.addMonths = function (months) {
        var dat = new Date(this.valueOf());
        dat.setMonth(dat.getMonth() + months);
        return dat;
    }

    Date.prototype.addYears = function (years) {
        var dat = new Date(this.valueOf());
        dat.setFullYear(dat.getFullYear() + years);
        return dat;
    }

    Date.prototype.getMonthName = function (lang) {
        lang = lang && (lang in this.locale) ? lang : 'en';
        return this.locale[lang].month_names[this.getMonth()];
    };

    Date.prototype.toApiDateString = function () {
        return this.getFullYear() + '-' + ('0' + (this.getMonth() + 1)).slice(-2) + '-' + ('0' + this.getDate()).slice(-2);
    }

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

    window.TemplateFormatter = TemplateFormatter;

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
        }
        else if ('error' === type) {
            alert.icon = '<i class="fa fa-exclamation-circle text-danger"></i>';
            alert.bgcolor = 'bg-danger';
        }
        this.displayAlert(alert);
    };

    ErrorHandler.prototype.displayAlert = function (alert) {
        var obj = this.element;
        obj.addClass(alert.bgcolor);
        obj.html(alert.icon + '&nbsp;' + alert.message);
        obj.show();
        setTimeout(function () {
            obj.removeClass(alert.bgcolor);
            obj.hide();
        }, this.timeout);
    };

    window.ErrorHandler = ErrorHandler;

    // #endregion

})(jQuery);