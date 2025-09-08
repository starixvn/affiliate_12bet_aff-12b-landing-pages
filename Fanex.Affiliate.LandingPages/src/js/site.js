import $ from 'jquery';
import 'jquery-validation';
import 'jquery-validation-unobtrusive'; 
import Swiper from 'swiper';
import { parsePhoneNumberFromString } from 'libphonenumber-js/core';
import metadata from 'libphonenumber-js/metadata.mobile.json';
import './turnstile.js';

// Register phone validation BEFORE DOM ready
$.validator.unobtrusive.adapters.add("phonenocountry", ["countryproperty"], function (options) {
    options.rules["phonenocountry"] = {
        countryProperty: options.params.countryproperty
    };
    options.messages["phonenocountry"] = options.message;
});

$.validator.addMethod("phonenocountry", function (value, element, params) {
    if (!value) return true;

    // Get country code from the specified property
    let countryCodeFieldName = "RegisterForm." + params.countryProperty;
    let countryCode = $("[name='" + countryCodeFieldName + "']").val();
    
    // If no country code found, skip validation
    if (!countryCode) return true;

    try {
        // Validate phone number using libphonenumber-js
        let phoneNumber = parsePhoneNumberFromString(value, countryCode, metadata);
        if (!phoneNumber || !phoneNumber.isValid()) {
            return false;
        }

        const type = phoneNumber.getType();
        return type === 'MOBILE' || type === 'FIXED_LINE_OR_MOBILE';
    } catch (e) {
        return false;
    }
}, "Invalid phone number for the selected country.");

$(function () {
    initGoTopBtn();
    initInputInfoToggle();
    initPasswordToggle();
    initFormScrollBack();
    initSwiper();

    enableRemoteValidationDebounce(500);

    const $form = $("#register-form");
    $form.on("submit", function () {
        if ($form.valid()) {
            const $passwordInput = $("#RegisterForm_UserPwd");
            if ($passwordInput.val()) {
                const encoded = btoa(unescape(encodeURIComponent($passwordInput.val())));
                $passwordInput.val("ZHNkc2RzZGRZA");
                $("#RegisterForm_Password").val(encoded);
            }
        }
    });

    let validator = $("#register-form").data("validator");

    if (validator) {
        validator.settings.highlight = function (element) {
            $(element).closest(".input-box")
                .removeClass("input-valid")
                .addClass("input-error");
        };
        validator.settings.unhighlight = function (element) {
            $(element).closest(".input-box")
                .removeClass("input-error")
                .addClass("input-valid");
        };
    }

    var $firstErrorField = $(".input-validation-error").first();

    if ($firstErrorField.length) {
        $('html, body').animate({
            scrollTop: $firstErrorField.offset().top - 100
        }, 500);
        $firstErrorField.focus();
    } else {
        var $summary = $(".validation-summary-errors, .validation-summary-valid.text-danger").first();
        if ($summary.length) {
            $('html, body').animate({
                scrollTop: $summary.offset().top - 100
            }, 500);
        }
    }
});

// Scroll To Top button
function initGoTopBtn() {
    var $goTopBtn = $("#goTopBtn");
    if ($goTopBtn.length === 0) return;

    function toggleGoTopBtn() {
        $goTopBtn.toggle($(window).scrollTop() > 200);
    }

    toggleGoTopBtn(); // Initial check
    $(window).on("scroll", toggleGoTopBtn);

    $goTopBtn.on("click", function () {
        $("html, body").animate({ scrollTop: 0 }, 400);
    });
}

// Toggle input-info
function initInputInfoToggle() {
    $(document).on("click", ".icon-information", function () {
        var $inputField = $(this).closest(".input-field");
        $inputField.find(".input-info").toggleClass("active");
    });
}

// Toggle password visibility
function initPasswordToggle() {
    $(".input-field").each(function () {
        var $field = $(this);
        var $input = $field.find('input[type="password"], input[type="text"]');
        var $eye = $field.find(".icon-eye");
        var $eyeSlash = $field.find(".icon-eye-slash");

        if ($input.length && $eye.length && $eyeSlash.length) {
            $eye.add($eyeSlash).on("click", function () {
                var isPassword = $input.attr("type") === "password";
                $input.attr("type", isPassword ? "text" : "password");
                $eye.toggle(!isPassword);
                $eyeSlash.toggle(isPassword);
            });
        }
    });
}

// Scroll back to form when click outside
function initFormScrollBack() {
    var $form = $("#register-form");
    if ($form.length === 0) return;

    var exceptions = [
        ".goTopBtn",
        ".swiper-button-next",
        ".swiper-button-prev",
        ".swiper-pagination"
    ];

    $(document).on("click", function (e) {
        var $target = $(e.target);
        var isInsideForm = $form.has($target).length > 0 || $form.is($target);
        var isException = exceptions.some(function (selector) {
            return $target.closest(selector).length > 0;
        });

        if (!isInsideForm && !isException) {
            $form[0].scrollIntoView({ behavior: "smooth", block: "start" });
        }
    });
}

// Swiper init (multi-instance support)
function initSwiper() {
    $(".swiper-container").each(function () {
        var $container = $(this);
        var $swiperEl = $container.find(".swiper").first();
        if ($swiperEl.length === 0) return;

        var slideCount = $swiperEl.find(".swiper-slide").length;

        new Swiper($swiperEl[0], {
            slidesPerView: 1,
            spaceBetween: 20,
            navigation: {
                nextEl: $container.find(".swiper-button-next")[0],
                prevEl: $container.find(".swiper-button-prev")[0],
            },
            pagination: {
                el: $container.find(".swiper-pagination")[0],
                clickable: true,
            },
            loop: slideCount > 1,
            autoplay: slideCount > 1
                ? { delay: 2500, disableOnInteraction: false }
                : false,
        });

        var $pagination = $container.find(".swiper-pagination");
        if (slideCount <= 1) {
            $pagination.css("display", "none");
            $swiperEl.css("padding-bottom", "0");
        } else {
            $swiperEl.css("padding-bottom", "30px");
        }
    });
}

function enableRemoteValidationDebounce(delayMs) {
    let debounceTimer;
    let currentXhr;

    $.validator.methods.remote = function (value, element, param) {
        const previous = this.previousValue(element);

        if (!value) {
            if (previous) {
                previous.old = null;
                previous.valid = true; 
            }

            if (currentXhr) {
                currentXhr.abort();
                currentXhr = null;
            }

            this.stopRequest(element, true);
            return true;
        }

        if (!this.settings.messages[element.name]) {
            this.settings.messages[element.name] = {};
        }
        if (previous.old === value) {
            return previous.valid;
        }

        previous.old = value;

        clearTimeout(debounceTimer);
        const validator = this;

        debounceTimer = setTimeout(() => {
            if (currentXhr) {
                currentXhr.abort();
            }

            validator.startRequest(element);

            currentXhr = $.ajax($.extend(true, {
                url: param.url || param,
                mode: "abort",
                port: "remote" + element.name,
                dataType: "json",
                data: (() => {
                    const data = {};
                    data[element.name] = value;
                    return data;
                })(),
                success: function (response) {
                    const valid = response === true || response === "true";
                    const errors = {};

                    if (valid) {
                        const submitted = validator.formSubmitted;
                        validator.resetInternals();
                        validator.toHide = validator.errorsFor(element);
                        validator.formSubmitted = submitted;
                        validator.successList.push(element);
                        validator.invalid[element.name] = false;
                        validator.showErrors();
                    } else {
                        const message = response || validator.defaultMessage(element, "remote");
                        validator.settings.messages[element.name].remote = previous.originalMessage = message;
                        errors[element.name] = message;
                        validator.invalid[element.name] = true;
                        validator.showErrors(errors);
                    }
                    previous.valid = valid;
                    validator.stopRequest(element, valid);
                }
            }, param));
        }, delayMs);

        return "pending";
    };
}

