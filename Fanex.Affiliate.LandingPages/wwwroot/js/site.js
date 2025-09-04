document.addEventListener("DOMContentLoaded", () => {
    initGoTopBtn();
    initInputInfoToggle();
    initPasswordToggle();
    initFormScrollBack();
    initSwiper();
});

// Scroll To Top button
function initGoTopBtn() {
    const goTopBtn = document.getElementById("goTopBtn");
    if (!goTopBtn) return;

    const toggleGoTopBtn = () => {
        goTopBtn.style.display = window.scrollY > 200 ? "block" : "none";
    };

    toggleGoTopBtn();

    window.addEventListener("scroll", toggleGoTopBtn);

    goTopBtn.addEventListener("click", () => {
        window.scrollTo({ top: 0, behavior: "smooth" });
    });
}

// Toggle input-info
function initInputInfoToggle() {
    document.querySelectorAll(".icon-information").forEach((icon) => {
        icon.addEventListener("click", () => {
            icon.closest(".input-field")
                ?.querySelector(".input-info")
                ?.classList.toggle("active");
        });
    });
}

// Toggle password visibility
function initPasswordToggle() {
    document.querySelectorAll(".input-field").forEach((field) => {
        const input = field.querySelector('input[type="password"], input[type="text"]');
        const eye = field.querySelector(".icon-eye");
        const eyeSlash = field.querySelector(".icon-eye-slash");

        if (input && eye && eyeSlash) {
            [eye, eyeSlash].forEach((icon) =>
                icon.addEventListener("click", () => {
                    const isPassword = input.type === "password";
                    input.type = isPassword ? "text" : "password";
                    eye.style.display = isPassword ? "none" : "inline-block";
                    eyeSlash.style.display = isPassword ? "inline-block" : "none";
                })
            );
        }
    });
}

// Scroll back to form when click outside
function initFormScrollBack() {
    const form = document.getElementById("register-form");
    if (!form) return;

    const exceptions = [
        "goTopBtn",
        "swiper-button-next",
        "swiper-button-prev",
        "swiper-pagination",
    ];

    document.addEventListener("click", (e) => {
        const isInsideForm = form.contains(e.target);
        const isException = exceptions.some((cls) => e.target.closest(`.${cls}`));

        if (!isInsideForm && !isException) {
            form.scrollIntoView({ behavior: "smooth", block: "start" });
        }
    });
}

// Swiper init
function initSwiper() {
    const swiperEl = document.querySelector(".swiper");
    if (!swiperEl) return;

    const slideCount = swiperEl.querySelectorAll(".swiper-slide").length;
    const swiper = new Swiper(".swiper", {
        slidesPerView: 1,
        spaceBetween: 20,
        navigation: {
            nextEl: ".swiper-button-next",
            prevEl: ".swiper-button-prev",
        },
        pagination: {
            el: ".swiper-pagination",
            clickable: true,
        },
        loop: slideCount > 1,
        autoplay: slideCount > 1 ? {
            delay: 2500,
            disableOnInteraction: false,
        } : false,
    });

    const pagination = swiperEl.querySelector(".swiper-pagination");
    if (slideCount <= 1) {
        pagination?.style.setProperty("display", "none");
        swiperEl.style.paddingBottom = "0";
    } else {
        swiperEl.style.paddingBottom = "30px";
    }
}
