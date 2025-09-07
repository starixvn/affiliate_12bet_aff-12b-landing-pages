import $ from 'jquery';

$(function () {
    let captchaType = parseInt($('#RegisterForm_CaptchaType').val(), 10);
    if (captchaType === 0) {
        loadToken();
    }

    $('#tokenBox').on('click', function () {
        loadToken();
    });
});

function loadToken() {
    $.getJSON("/api/SignUp/GetNumberCaptcha", function (res) {
        $("#tokenBox").attr("src", "data:image/png;base64," + res.data.picture);
        setTimeout(loadToken, res.data.seconds * 1000);
    }).fail(function () {
        console.log("get numbercapcha fail");
        setTimeout(loadToken, 5000);
    });
}

// Tạo global functions để RegistrationForm.cshtml có thể gọi
window.onTurnstileSuccess = function (token) {
    console.log("Turnstile Success:", token);

    // Enable submit button
    $('#submit-btn').prop('disabled', false);
    //// Lưu token vào hidden input
    //const $hidden = $('input[name="cf-turnstile-response"]');
    //if ($hidden.length === 0) {
    //    $('#signupForm').append(
    //        $('<input>', { type: 'hidden', name: 'cf-turnstile-response', value: token })
    //    );
    //} else {
    //    $hidden.val(token);
    //}

    // Ẩn number captcha nếu có
    $('#numberCapcha').hide();
};

window.onTurnstileError = function (errorCode) {
    console.error("Turnstile Error:", errorCode);

    // Disable submit button
    //$('#submit-btn').prop('disabled', true);

    // Hiển thị number captcha
    $('#numberCapcha').show();
    $('#turnstile_widget').hide();
    $('#RegisterForm_CaptchaType').val(0);
    $("#turnstile_widget").remove();

    loadToken();
};

window.onTurnstileExpired = function () {
    console.log("Turnstile Token Expired");

    // Disable submit button
    $('#submit-btn').prop('disabled', true);

    turnstile.reset();
};