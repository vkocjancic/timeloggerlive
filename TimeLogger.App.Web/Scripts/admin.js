/// <reference path="jquery-1.9.1.js" />
/// <reference path="common.js" />

(function ($) {
    var accountList = new AccountList("accountList");

    // #region AccountStatus

    function AccountStatus(obj) {
        this.element = obj;
    }

    AccountStatus.prototype.displayNone = function () {
        this.resetClass(this.element.get(0), '');
    };

    AccountStatus.prototype.displayNew = function () {
        this.resetClass(this.element.get(0), 'fa fa-circle text-danger');
    };

    AccountStatus.prototype.displayError = function () {
        this.resetClass(this.element.get(0), 'fa fa-exclamation-circle');
    };

    AccountStatus.prototype.displayLoading = function () {
        this.resetClass(this.element.get(0), 'fa fa-spinner fa-pulse fa-fw');
    };

    AccountStatus.prototype.displaySuccess = function () {
        this.resetClass(this.element.get(0), 'fa fa-circle text-success');
    };

    AccountStatus.prototype.resetClass = function (obj, classes) {
        obj.className = classes;
    };

    // #endregion

    // #region AccountList

    function AccountList(id) {
        this.items = [];
        this.itemTemplate = '<tr data-id="{data-id}"><td>{email}</td><td>{created}</td><td>{billingoption}</td><td>{price}</td><td>{status}</td><td>{action}</td></tr>';
        this.element = $('#' + id);
    };

    AccountList.prototype.activateAccount = function (obj) {
        var tr = $(obj).parents('tr'),
           id = tr.attr('data-id'),
           errorHandler = new ErrorHandler($('#account-list-info')),
           status = new AccountStatus(this.element.find('td:eq(4) i')),
           list = this;
        if (id && id.length > 0) {
            $(obj).hide();
            $.ajax({
                url: '/App/api/accountlist/',
                method: 'PUT',
                data: {
                    'id': id,
                    'isActive': true
                }
            })
            .done(function (data) {
                status.displaySuccess();
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
    }

    AccountList.prototype.clearItems = function () {
        $("tbody tr", this.element).remove();
        this.items = [];
    };

    AccountList.prototype.createItems = function (items) {
        var list = this;
        $(items).each(function () {
            var replacements = {
                '{data-id}': this.id,
                '{email}': this.email,
                '{created}': this.created,
                '{billingoption}': this.billingoption,
                '{price}': this.price,
                '{action}': '<button class="action-activate"><i class="fa fa-check"></i></button>',
                '{status}': '<i></i>'
            },
            formatter = new TemplateFormatter(replacements);
            $("tbody", list.element).append(formatter.format(list.itemTemplate));
            var status = new AccountStatus(list.element.find('tr[data-id=' + this.id + '] td:eq(4) i'))
            status.displayNew();
        });
    };

    AccountList.prototype.init = function () {
        if (this.element) {
            this.clearItems();
            this.loadInactive();
            var list = this;
            $('tbody', this.element).on('click', 'tr .action-activate', function () {
                list.activateAccount(this);
            });
        }
    };

    AccountList.prototype.loadInactive = function (callback) {
        var list = this,
            errorHandler = new ErrorHandler($('#account-list-info'));
        $.get('/App/api/accountlist', { 'accounttype' : 'I' })
        .success(function (data) {
            list.clearItems();
            list.createItems(data.accounts);
            if (callback) {
                callback();
            }
        })
        .fail(function (data) {
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

    // #endregion

    // #region Initialization

    $(document).ready(function () {
        accountList.init();
    });

    // #endregion

})(jQuery);