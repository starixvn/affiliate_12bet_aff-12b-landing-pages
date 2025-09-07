/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./src/js/site.js":
/*!************************!*\
  !*** ./src/js/site.js ***!
  \************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("{__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var jquery_validation__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! jquery-validation */ \"./node_modules/jquery-validation/dist/jquery.validate.js\");\n/* harmony import */ var jquery_validation__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(jquery_validation__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var jquery_validation_unobtrusive__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! jquery-validation-unobtrusive */ \"./node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js\");\n/* harmony import */ var jquery_validation_unobtrusive__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(jquery_validation_unobtrusive__WEBPACK_IMPORTED_MODULE_2__);\n/* harmony import */ var swiper__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! swiper */ \"./node_modules/swiper/swiper.mjs\");\n/* harmony import */ var _turnstile_js__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./turnstile.js */ \"./src/js/turnstile.js\");\n\r\n\r\n \r\n\r\n\r\n\r\njquery__WEBPACK_IMPORTED_MODULE_0___default()(function () {\r\n    initGoTopBtn();\r\n    initInputInfoToggle();\r\n    initPasswordToggle();\r\n    initFormScrollBack();\r\n    initSwiper();\r\n\r\n    enableRemoteValidationDebounce(500);\r\n\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()(\"#register-form\").on(\"submit\", function () {\r\n        const $passwordInput = jquery__WEBPACK_IMPORTED_MODULE_0___default()(\"#RegisterForm_UserPwd\");\r\n        if ($passwordInput.val()) {\r\n            const encoded = btoa(unescape(encodeURIComponent($passwordInput.val())));\r\n            $passwordInput.val(encoded);\r\n        }\r\n    });\r\n\r\n    let validator = jquery__WEBPACK_IMPORTED_MODULE_0___default()(\"#register-form\").data(\"validator\");\r\n\r\n    if (validator) {\r\n        validator.settings.highlight = function (element) {\r\n            jquery__WEBPACK_IMPORTED_MODULE_0___default()(element).closest(\".input-box\")\r\n                .removeClass(\"valid\")\r\n                .addClass(\"error\");\r\n        };\r\n        validator.settings.unhighlight = function (element) {\r\n            jquery__WEBPACK_IMPORTED_MODULE_0___default()(element).closest(\".input-box\")\r\n                .removeClass(\"error\")\r\n                .addClass(\"valid\");\r\n        };\r\n    }\r\n});\r\n\r\n// Scroll To Top button\r\nfunction initGoTopBtn() {\r\n    var $goTopBtn = jquery__WEBPACK_IMPORTED_MODULE_0___default()(\"#goTopBtn\");\r\n    if ($goTopBtn.length === 0) return;\r\n\r\n    function toggleGoTopBtn() {\r\n        $goTopBtn.toggle(jquery__WEBPACK_IMPORTED_MODULE_0___default()(window).scrollTop() > 200);\r\n    }\r\n\r\n    toggleGoTopBtn(); // Initial check\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()(window).on(\"scroll\", toggleGoTopBtn);\r\n\r\n    $goTopBtn.on(\"click\", function () {\r\n        jquery__WEBPACK_IMPORTED_MODULE_0___default()(\"html, body\").animate({ scrollTop: 0 }, 400);\r\n    });\r\n}\r\n\r\n// Toggle input-info\r\nfunction initInputInfoToggle() {\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()(document).on(\"click\", \".icon-information\", function () {\r\n        var $inputField = jquery__WEBPACK_IMPORTED_MODULE_0___default()(this).closest(\".input-field\");\r\n        $inputField.find(\".input-info\").toggleClass(\"active\");\r\n    });\r\n}\r\n\r\n// Toggle password visibility\r\nfunction initPasswordToggle() {\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()(\".input-field\").each(function () {\r\n        var $field = jquery__WEBPACK_IMPORTED_MODULE_0___default()(this);\r\n        var $input = $field.find('input[type=\"password\"], input[type=\"text\"]');\r\n        var $eye = $field.find(\".icon-eye\");\r\n        var $eyeSlash = $field.find(\".icon-eye-slash\");\r\n\r\n        if ($input.length && $eye.length && $eyeSlash.length) {\r\n            $eye.add($eyeSlash).on(\"click\", function () {\r\n                var isPassword = $input.attr(\"type\") === \"password\";\r\n                $input.attr(\"type\", isPassword ? \"text\" : \"password\");\r\n                $eye.toggle(!isPassword);\r\n                $eyeSlash.toggle(isPassword);\r\n            });\r\n        }\r\n    });\r\n}\r\n\r\n// Scroll back to form when click outside\r\nfunction initFormScrollBack() {\r\n    var $form = jquery__WEBPACK_IMPORTED_MODULE_0___default()(\"#register-form\");\r\n    if ($form.length === 0) return;\r\n\r\n    var exceptions = [\r\n        \".goTopBtn\",\r\n        \".swiper-button-next\",\r\n        \".swiper-button-prev\",\r\n        \".swiper-pagination\"\r\n    ];\r\n\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()(document).on(\"click\", function (e) {\r\n        var $target = jquery__WEBPACK_IMPORTED_MODULE_0___default()(e.target);\r\n        var isInsideForm = $form.has($target).length > 0 || $form.is($target);\r\n        var isException = exceptions.some(function (selector) {\r\n            return $target.closest(selector).length > 0;\r\n        });\r\n\r\n        if (!isInsideForm && !isException) {\r\n            $form[0].scrollIntoView({ behavior: \"smooth\", block: \"start\" });\r\n        }\r\n    });\r\n}\r\n\r\n// Swiper init (multi-instance support)\r\nfunction initSwiper() {\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()(\".swiper-container\").each(function () {\r\n        var $container = jquery__WEBPACK_IMPORTED_MODULE_0___default()(this);\r\n        var $swiperEl = $container.find(\".swiper\").first();\r\n        if ($swiperEl.length === 0) return;\r\n\r\n        var slideCount = $swiperEl.find(\".swiper-slide\").length;\r\n\r\n        new swiper__WEBPACK_IMPORTED_MODULE_3__[\"default\"]($swiperEl[0], {\r\n            slidesPerView: 1,\r\n            spaceBetween: 20,\r\n            navigation: {\r\n                nextEl: $container.find(\".swiper-button-next\")[0],\r\n                prevEl: $container.find(\".swiper-button-prev\")[0],\r\n            },\r\n            pagination: {\r\n                el: $container.find(\".swiper-pagination\")[0],\r\n                clickable: true,\r\n            },\r\n            loop: slideCount > 1,\r\n            autoplay: slideCount > 1\r\n                ? { delay: 2500, disableOnInteraction: false }\r\n                : false,\r\n        });\r\n\r\n        var $pagination = $container.find(\".swiper-pagination\");\r\n        if (slideCount <= 1) {\r\n            $pagination.css(\"display\", \"none\");\r\n            $swiperEl.css(\"padding-bottom\", \"0\");\r\n        } else {\r\n            $swiperEl.css(\"padding-bottom\", \"30px\");\r\n        }\r\n    });\r\n}\r\n\r\nfunction enableRemoteValidationDebounce(delayMs) {\r\n    let debounceTimer;\r\n    let currentXhr;\r\n\r\n    (jquery__WEBPACK_IMPORTED_MODULE_0___default().validator).methods.remote = function (value, element, param) {\r\n        const previous = this.previousValue(element);\r\n\r\n        if (!value) {\r\n            if (previous) {\r\n                previous.old = null;\r\n                previous.valid = true; \r\n            }\r\n\r\n            if (currentXhr) {\r\n                currentXhr.abort();\r\n                currentXhr = null;\r\n            }\r\n\r\n            this.stopRequest(element, true);\r\n            return true;\r\n        }\r\n\r\n        if (!this.settings.messages[element.name]) {\r\n            this.settings.messages[element.name] = {};\r\n        }\r\n        if (previous.old === value) {\r\n            return previous.valid;\r\n        }\r\n\r\n        previous.old = value;\r\n\r\n        clearTimeout(debounceTimer);\r\n        const validator = this;\r\n\r\n        debounceTimer = setTimeout(() => {\r\n            if (currentXhr) {\r\n                currentXhr.abort();\r\n            }\r\n\r\n            validator.startRequest(element);\r\n\r\n            currentXhr = jquery__WEBPACK_IMPORTED_MODULE_0___default().ajax(jquery__WEBPACK_IMPORTED_MODULE_0___default().extend(true, {\r\n                url: param.url || param,\r\n                mode: \"abort\",\r\n                port: \"remote\" + element.name,\r\n                dataType: \"json\",\r\n                data: (() => {\r\n                    const data = {};\r\n                    data[element.name] = value;\r\n                    return data;\r\n                })(),\r\n                success: function (response) {\r\n                    const valid = response === true || response === \"true\";\r\n                    const errors = {};\r\n\r\n                    if (valid) {\r\n                        const submitted = validator.formSubmitted;\r\n                        validator.resetInternals();\r\n                        validator.toHide = validator.errorsFor(element);\r\n                        validator.formSubmitted = submitted;\r\n                        validator.successList.push(element);\r\n                        validator.invalid[element.name] = false;\r\n                        validator.showErrors();\r\n                    } else {\r\n                        const message = response || validator.defaultMessage(element, \"remote\");\r\n                        validator.settings.messages[element.name].remote = previous.originalMessage = message;\r\n                        errors[element.name] = message;\r\n                        validator.invalid[element.name] = true;\r\n                        validator.showErrors(errors);\r\n                    }\r\n                    previous.valid = valid;\r\n                    validator.stopRequest(element, valid);\r\n                }\r\n            }, param));\r\n        }, delayMs);\r\n\r\n        return \"pending\";\r\n    };\r\n}\r\n\r\n\n\n//# sourceURL=webpack://fanex-affiliate-landing-pages/./src/js/site.js?\n}");

/***/ }),

/***/ "./src/js/turnstile.js":
/*!*****************************!*\
  !*** ./src/js/turnstile.js ***!
  \*****************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("{__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n\r\n\r\njquery__WEBPACK_IMPORTED_MODULE_0___default()(function () {\r\n    let captchaType = parseInt(jquery__WEBPACK_IMPORTED_MODULE_0___default()('#RegisterForm_CaptchaType').val(), 10);\r\n    if (captchaType === 0) {\r\n        loadToken();\r\n    }\r\n\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()('#tokenBox').on('click', function () {\r\n        loadToken();\r\n    });\r\n});\r\n\r\nfunction loadToken() {\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default().getJSON(\"/api/SignUp/GetNumberCaptcha\", function (res) {\r\n        jquery__WEBPACK_IMPORTED_MODULE_0___default()(\"#tokenBox\").attr(\"src\", \"data:image/png;base64,\" + res.data.picture);\r\n        setTimeout(loadToken, res.data.seconds * 1000);\r\n    }).fail(function () {\r\n        console.log(\"get numbercapcha fail\");\r\n        setTimeout(loadToken, 5000);\r\n    });\r\n}\r\n\r\n// Tạo global functions để RegistrationForm.cshtml có thể gọi\r\nwindow.onTurnstileSuccess = function (token) {\r\n    console.log(\"Turnstile Success:\", token);\r\n\r\n    // Enable submit button\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()('#submit-btn').prop('disabled', false);\r\n    //// Lưu token vào hidden input\r\n    //const $hidden = $('input[name=\"cf-turnstile-response\"]');\r\n    //if ($hidden.length === 0) {\r\n    //    $('#signupForm').append(\r\n    //        $('<input>', { type: 'hidden', name: 'cf-turnstile-response', value: token })\r\n    //    );\r\n    //} else {\r\n    //    $hidden.val(token);\r\n    //}\r\n\r\n    // Ẩn number captcha nếu có\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()('#numberCapcha').hide();\r\n};\r\n\r\nwindow.onTurnstileError = function (errorCode) {\r\n    console.error(\"Turnstile Error:\", errorCode);\r\n\r\n    // Disable submit button\r\n    //$('#submit-btn').prop('disabled', true);\r\n\r\n    // Hiển thị number captcha\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()('#numberCapcha').show();\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()('#turnstile_widget').hide();\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()('#RegisterForm_CaptchaType').val(0);\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()(\"#turnstile_widget\").remove();\r\n\r\n    loadToken();\r\n};\r\n\r\nwindow.onTurnstileExpired = function () {\r\n    console.log(\"Turnstile Token Expired\");\r\n\r\n    // Disable submit button\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()('#submit-btn').prop('disabled', true);\r\n\r\n    turnstile.reset();\r\n};\n\n//# sourceURL=webpack://fanex-affiliate-landing-pages/./src/js/turnstile.js?\n}");

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = __webpack_modules__;
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/chunk loaded */
/******/ 	(() => {
/******/ 		var deferred = [];
/******/ 		__webpack_require__.O = (result, chunkIds, fn, priority) => {
/******/ 			if(chunkIds) {
/******/ 				priority = priority || 0;
/******/ 				for(var i = deferred.length; i > 0 && deferred[i - 1][2] > priority; i--) deferred[i] = deferred[i - 1];
/******/ 				deferred[i] = [chunkIds, fn, priority];
/******/ 				return;
/******/ 			}
/******/ 			var notFulfilled = Infinity;
/******/ 			for (var i = 0; i < deferred.length; i++) {
/******/ 				var [chunkIds, fn, priority] = deferred[i];
/******/ 				var fulfilled = true;
/******/ 				for (var j = 0; j < chunkIds.length; j++) {
/******/ 					if ((priority & 1 === 0 || notFulfilled >= priority) && Object.keys(__webpack_require__.O).every((key) => (__webpack_require__.O[key](chunkIds[j])))) {
/******/ 						chunkIds.splice(j--, 1);
/******/ 					} else {
/******/ 						fulfilled = false;
/******/ 						if(priority < notFulfilled) notFulfilled = priority;
/******/ 					}
/******/ 				}
/******/ 				if(fulfilled) {
/******/ 					deferred.splice(i--, 1)
/******/ 					var r = fn();
/******/ 					if (r !== undefined) result = r;
/******/ 				}
/******/ 			}
/******/ 			return result;
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/compat get default export */
/******/ 	(() => {
/******/ 		// getDefaultExport function for compatibility with non-harmony modules
/******/ 		__webpack_require__.n = (module) => {
/******/ 			var getter = module && module.__esModule ?
/******/ 				() => (module['default']) :
/******/ 				() => (module);
/******/ 			__webpack_require__.d(getter, { a: getter });
/******/ 			return getter;
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/jsonp chunk loading */
/******/ 	(() => {
/******/ 		// no baseURI
/******/ 		
/******/ 		// object to store loaded and loading chunks
/******/ 		// undefined = chunk not loaded, null = chunk preloaded/prefetched
/******/ 		// [resolve, reject, Promise] = chunk loading, 0 = chunk loaded
/******/ 		var installedChunks = {
/******/ 			"site": 0
/******/ 		};
/******/ 		
/******/ 		// no chunk on demand loading
/******/ 		
/******/ 		// no prefetching
/******/ 		
/******/ 		// no preloaded
/******/ 		
/******/ 		// no HMR
/******/ 		
/******/ 		// no HMR manifest
/******/ 		
/******/ 		__webpack_require__.O.j = (chunkId) => (installedChunks[chunkId] === 0);
/******/ 		
/******/ 		// install a JSONP callback for chunk loading
/******/ 		var webpackJsonpCallback = (parentChunkLoadingFunction, data) => {
/******/ 			var [chunkIds, moreModules, runtime] = data;
/******/ 			// add "moreModules" to the modules object,
/******/ 			// then flag all "chunkIds" as loaded and fire callback
/******/ 			var moduleId, chunkId, i = 0;
/******/ 			if(chunkIds.some((id) => (installedChunks[id] !== 0))) {
/******/ 				for(moduleId in moreModules) {
/******/ 					if(__webpack_require__.o(moreModules, moduleId)) {
/******/ 						__webpack_require__.m[moduleId] = moreModules[moduleId];
/******/ 					}
/******/ 				}
/******/ 				if(runtime) var result = runtime(__webpack_require__);
/******/ 			}
/******/ 			if(parentChunkLoadingFunction) parentChunkLoadingFunction(data);
/******/ 			for(;i < chunkIds.length; i++) {
/******/ 				chunkId = chunkIds[i];
/******/ 				if(__webpack_require__.o(installedChunks, chunkId) && installedChunks[chunkId]) {
/******/ 					installedChunks[chunkId][0]();
/******/ 				}
/******/ 				installedChunks[chunkId] = 0;
/******/ 			}
/******/ 			return __webpack_require__.O(result);
/******/ 		}
/******/ 		
/******/ 		var chunkLoadingGlobal = self["webpackChunkfanex_affiliate_landing_pages"] = self["webpackChunkfanex_affiliate_landing_pages"] || [];
/******/ 		chunkLoadingGlobal.forEach(webpackJsonpCallback.bind(null, 0));
/******/ 		chunkLoadingGlobal.push = webpackJsonpCallback.bind(null, chunkLoadingGlobal.push.bind(chunkLoadingGlobal));
/******/ 	})();
/******/ 	
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module depends on other loaded chunks and execution need to be delayed
/******/ 	var __webpack_exports__ = __webpack_require__.O(undefined, ["vendor"], () => (__webpack_require__("./src/js/site.js")))
/******/ 	__webpack_exports__ = __webpack_require__.O(__webpack_exports__);
/******/ 	
/******/ })()
;