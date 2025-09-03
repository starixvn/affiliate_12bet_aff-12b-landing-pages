document.addEventListener("DOMContentLoaded", () => {
    const goTopBtn = document.getElementById("goTopBtn");

    // Scroll to Top button
    if (goTopBtn) {
        window.addEventListener("scroll", () => {
            goTopBtn.style.display = window.scrollY > 200 ? "block" : "none";
        });

        goTopBtn.addEventListener("click", () => {
            window.scrollTo({ top: 0, behavior: "smooth" });
        });
    }

    // Toggle input-info when click icon
    document.querySelectorAll(".icon-information").forEach((icon) => {
        icon.addEventListener("click", () => {
            const info = icon.closest(".input-field")?.querySelector(".input-info");
            info?.classList.toggle("active");
        });
    });

    // Toggle password visibility
    document.querySelectorAll(".input-field").forEach((field) => {
        const input = field.querySelector('input[type="password"], input[type="text"]');
        const eye = field.querySelector(".icon-eye");
        const eyeSlash = field.querySelector(".icon-eye-slash");

        if (eye && eyeSlash && input) {
            [eye, eyeSlash].forEach((el) =>
                el.addEventListener("click", () => {
                    const isPassword = input.type === "password";
                    input.type = isPassword ? "text" : "password";
                    eye.style.display = isPassword ? "inline-block" : "none";
                    eyeSlash.style.display = isPassword ? "none" : "inline-block";
                })
            );
        }
    });

    // Scroll back to form when click outside
    document.addEventListener("click", (e) => {
        const form = document.getElementById("register-form");
        if (!form) return;

        const exceptions = [
            "goTopBtn",
            "swiper-button-next",
            "swiper-button-prev",
            "swiper-pagination",
        ];

        const isInsideForm = form.contains(e.target);
        const isException = exceptions.some((cls) => e.target.closest(`.${cls}`));

        if (!isInsideForm && !isException) {
            form.scrollIntoView({ behavior: "smooth", block: "start" });
        }
    });

    // ✅ Swiper init
    const swiperEl = document.querySelector(".swiper");
    if (swiperEl) {
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

        if (slideCount <= 1) {
            swiperEl.querySelector(".swiper-pagination")?.style.setProperty("display", "none");
            swiperEl.style.paddingBottom = "0";
        } else {
            swiperEl.style.paddingBottom = "30px";
        }
    }
});
