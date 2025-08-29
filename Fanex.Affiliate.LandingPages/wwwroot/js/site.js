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
            const inputField = icon.closest(".input-field");
            const info = inputField?.querySelector(".input-info");
            if (info) info.classList.toggle("active");
        });
    });

    // Toggle password visibility
    document.querySelectorAll(".input-field").forEach((field) => {
        const input = field.querySelector(
            'input[type="password"], input[type="text"]'
        );
        const eye = field.querySelector(".icon-eye");
        const eyeSlash = field.querySelector(".icon-eye-slash");

        if (eye && eyeSlash && input) {
            eyeSlash.addEventListener("click", () => {
                input.type = "text";
                eyeSlash.style.display = "none";
                eye.style.display = "inline-block";
            });

            eye.addEventListener("click", () => {
                input.type = "password";
                eye.style.display = "none";
                eyeSlash.style.display = "inline-block";
            });
        }
    });

    // Scroll back to register form when click outside
    document.addEventListener("click", (e) => {
        const form = document.getElementById("register-form");
        if (
            form &&
            !form.contains(e.target) &&
            e.target.id !== "goTopBtn"
        ) {
            form.scrollIntoView({ behavior: "smooth", block: "start" });
        }
    });
});
