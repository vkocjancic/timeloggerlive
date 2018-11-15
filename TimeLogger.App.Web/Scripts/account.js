(function ($) {
    var usernameField = $('#inUsername'),
        passwordField = $('#inPassword'),
        passwordConfirmField = $('#inPasswordConfirmation'),
        passwordResetIdField = $('#inPasswordResetRequestId'),
        paymentPlanField = $('#inPaymentPlan'),
        btnRegister = $('#btnRegister'),
        btnResetPassword = $('#btnResetPassword'),
        warningField = $('#app-warning'),

        init = function () {
            if (passwordConfirmField[0]) {
                passwordConfirmField[0].oninput = inputPasswordCheck;
                passwordField[0].oninput = passwordConfirmField[0].oninput;
            }
        },

        inputPasswordCheck = function (e) {
            passwordConfirmField.removeClass('form-input-success');
            passwordConfirmField.removeClass('form-input-error');
            if ((0 === passwordField.val().length) && (0 === passwordConfirmField.val().length)) {
                return;
            }
            if (passwordField.val() === passwordConfirmField.val()) {
                passwordConfirmField.addClass('form-input-success');
            }
            else {
                passwordConfirmField.addClass('form-input-error');
            }
        },

        displayErrorMessage = function (message) {
            warningField.html('');
            warningField.append('<p class="bg-danger"><i class="fa fa-exclamation-triangle" aria-hidden="true"></i>&nbsp;' + message + '</p>');
        };
            
    // events
    $(btnRegister).click(function (e) {
        e.preventDefault();
        btnRegister.addClass('disabled');
        $.post("/App/api/register", {
            email: usernameField.val(),
            password: passwordField.val(),
            plan: paymentPlanField.val()
        })
        .success(function (data) {
            if (global_paypal_link) {
                window.location.href = global_paypal_link;
            }
            btnRegister.removeClass('disabled');
        })
        .fail(function (data) {
            if (data.responseText) {
                displayErrorMessage($.parseJSON(data.responseText).errorDescription);
            }
            else {
                displayErrorMessage(data.statusText);
            }
            btnRegister.removeClass('disabled');
        });
    });

    $(btnResetPassword).click(function (e) {
        e.preventDefault();
        btnResetPassword.addClass('disabled');
        $.post("/App/api/resetpassword", {
            id: passwordResetIdField.val(),
            password: passwordField.val()
        })
        .success(function (data) {
            if (global_login_link) {
                window.location.href = global_login_link;
            }
            btnResetPassword.removeClass('disabled');
        })
        .fail(function (data) {
            if (data.responseText) {
                displayErrorMessage($.parseJSON(data.responseText).errorDescription);
            }
            else {
                displayErrorMessage(data.statusText);
            }
            btnResetPassword.removeClass('disabled');
        });
    });

    $(document).ready(function () {
        init();
    });

})(jQuery);