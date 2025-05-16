// Enhanced animation system with performance optimizations
document.addEventListener('DOMContentLoaded', function () {
    // Import animejs
    const anime = window.anime;

    // Import confetti-js
    const confetti = window.confetti;

    // Cache DOM elements for better performance
    const animatedElements = document.querySelectorAll('.animated-element');

    // Initialize animations with staggered timing for smoother loading
    function initializeAnimations() {
        anime({
            targets: '.menu-btn',
            translateX: ['-100%', '0%'],
            opacity: [0, 1],
            easing: 'easeOutExpo',
            duration: 800,
            delay: anime.stagger(100, { start: 300 }),
            begin: function () {
                document.querySelectorAll('.menu-btn').forEach(btn => {
                    btn.style.visibility = 'visible';
                });
            }
        });

        // Animate main content with subtle fade-in
        anime({
            targets: 'main > *',
            opacity: [0, 1],
            translateY: ['20px', '0px'],
            easing: 'easeOutCubic',
            duration: 800,
            delay: anime.stagger(150, { start: 500 })
        });
    }

    // Button hover animations
    function setupButtonAnimations() {
        document.querySelectorAll('.menu-btn:not(.disabled)').forEach(btn => {
            btn.addEventListener('mouseenter', function () {
                anime({
                    targets: this,
                    scale: 1.05,
                    backgroundColor: 'var(--cor-principal3)',
                    color: '#fff',
                    duration: 300,
                    easing: 'easeOutQuad'
                });
            });

            btn.addEventListener('mouseleave', function () {
                anime({
                    targets: this,
                    scale: 1,
                    backgroundColor: '',
                    color: '',
                    duration: 300,
                    easing: 'easeOutQuad'
                });
            });
        });
    }

    // Optimize animations for scroll events
    let ticking = false;
    function optimizedScrollAnimation() {
        if (!ticking) {
            window.requestAnimationFrame(function () {
                // Your scroll-based animations here
                ticking = false;
            });
            ticking = true;
        }
    }

    // Initialize all animations
    initializeAnimations();
    setupButtonAnimations();

    // Add scroll listener with optimization
    window.addEventListener('scroll', optimizedScrollAnimation, { passive: true });

    // Expose functions for use in other scripts
    window.animationSystem = {
        runEntryAnimation: function (selector, delay = 0) {
            anime({
                targets: selector,
                opacity: [0, 1],
                translateY: ['20px', '0px'],
                easing: 'easeOutCubic',
                duration: 600,
                delay: delay
            });
        },
        runButtonClickAnimation: function (element) {
            anime({
                targets: element,
                scale: [1, 0.95, 1],
                duration: 400,
                easing: 'easeInOutQuad'
            });
        },
        runSuccessAnimation: function () {
            // Confetti animation for success events
            confetti({
                particleCount: 100,
                spread: 70,
                origin: { y: 0.6 }
            });
        }
    };
});

// Function to activate button (enhanced version)
function activateButton(element) {
    // Remove active class from all buttons
    document.querySelectorAll('.menu-btn').forEach(btn => {
        btn.classList.remove('active');
    });

    // Add active class to clicked button with animation
    if (!element.classList.contains('disabled')) {
        element.classList.add('active');

        anime({
            targets: element,
            backgroundColor: 'var(--cor-principal)',
            color: '#fff',
            scale: [1, 1.1, 1],
            duration: 500,
            easing: 'easeOutElastic(1, .6)'
        });
    }
}