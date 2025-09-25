import $ from 'jquery';

// Create MetaPixel namespace to avoid conflicts
window.MetaPixel = window.MetaPixel || {};
const MetaPixel = window.MetaPixel;

$(document).ready(function () {

    // Standard Events enum
    const StandardEvents = {
        CompleteRegistration: 'CompleteRegistration',
        Purchase: 'Purchase',
        StartTrial: 'StartTrial'
    };

    // Skip Events enum
    const SkipEvents = {
        redeposit: 'redeposit',
        Purchase: 'Purchase',
        StartTrial: 'StartTrial'
    };

    // Check if Meta Pixel SDK is available
    MetaPixel.isSDKAvailable = function() {
        return window.fbq !== undefined && typeof window.fbq === 'function';
    }

    // Meta Pixel Tracker function
    MetaPixel.metaPixelTracker = function(eventName, currency, amount, eventValue) {
        if (SkipEvents[eventName]) return;
        if (!MetaPixel.isSDKAvailable()) return;

        const action = StandardEvents[eventName] ? 'track' : 'trackCustom';
        const params = {
            currency: currency,
            value: amount
        };

        try {
            window.fbq(action, eventName, params, { eventID: eventValue });
            console.log('Meta Pixel Event Sent:', eventName, params);
        } catch (error) {
            console.error('MetaPixelTracker error:', error);
        }
    }

    // Main tracking function
    MetaPixel.tracking = function(trackerParams) {
        const { eventName, eventValue, amount, currency, firstDeposit, userName } = trackerParams;

        const trackEvent = (eventName) => {
            try {
                MetaPixel.metaPixelTracker(eventName, currency, amount, eventValue);
            } catch (error) {
                console.error('MetaPixelTracker error:', error);
            }
        };

        try {
            switch (eventName) {
                case 'Purchase': {
                    const purchaseEvent = firstDeposit ? 'StartTrial' : 'redeposit';
                    trackEvent(purchaseEvent);
                    trackEvent(eventName);
                    break;
                }
                case 'Login': {
                    // GTM event pusher simulation
                    try {
                        if (window.dataLayer) {
                            window.dataLayer.push({
                                'event': eventName,
                                'currency': currency,
                                'eventValue': eventValue,
                                'amount': amount
                            });
                        }
                    } catch (error) {
                        console.error('GTM Event Pusher error:', error);
                    }
                    break;
                }
                default: {
                    trackEvent(eventName);
                    break;
                }
            }
        } catch (error) {
            console.error('Tracking error:', error);
        }
    }

    // Cookie helper functions
    MetaPixel.setCookie = function(name, value, days = 7) {
        const expires = new Date();
        expires.setTime(expires.getTime() + (days * 24 * 60 * 60 * 1000));
        document.cookie = name + '=' + value + ';expires=' + expires.toUTCString() + ';path=/';
    }

    // Check Meta Pixel status
    MetaPixel.checkStatus = function() {
        if (MetaPixel.isSDKAvailable()) {
            console.log("Meta Pixel SDK is ready!");
            // Get pixel IDs
            try {
                const pixelIds = window.fbq.getState().pixels.map(pixel => pixel.id).join(',');
                MetaPixel.setCookie('pixelIds', pixelIds);
                console.log("pixelIds:", pixelIds);
                
            } catch (error) {
                console.log("Error when getting Pixel IDs.");
            }
        } else {
            console.log("Meta Pixel SDK is not ready.");
        }
    }

    // Show result message
    MetaPixel.showResult = function(message, isError = false) {
        const className = isError ? 'error' : 'success';
        console.log(message);
    }

    // Check Meta Pixel status on page load
    setTimeout(MetaPixel.checkStatus, 2000);
});