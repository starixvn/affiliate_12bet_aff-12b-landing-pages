import $ from 'jquery';
import 'jquery-validation';
import 'jquery-validation-unobtrusive'; 
import Swiper from 'swiper';
import './turnstile.js';

$(function () {
    initGoTopBtn();
    initInputInfoToggle();
    initPasswordToggle();
    initFormScrollBack();
    initSwiper();

    enableRemoteValidationDebounce(500);

    $("#register-form").on("submit", function () {
        const $passwordInput = $("#RegisterForm_UserPwd");
        if ($passwordInput.val()) {
            const encoded = btoa(unescape(encodeURIComponent($passwordInput.val())));
            $passwordInput.val(encoded);
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

