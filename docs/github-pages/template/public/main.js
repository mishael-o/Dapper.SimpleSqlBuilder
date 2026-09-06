/**
 * Plays the Quick Start hero animation: the interpolated string resolves, the
 * arrow draws, then the SQL Dapper actually receives lands.
 *
 * The start state lives behind `is-armed`, which is only added here. If this
 * never runs, or motion is reduced, the finished panels are what render.
 */
function initHeroDemo() {
    const demos = document.querySelectorAll('.hero-demo');

    if (!demos.length || !window.matchMedia || window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
        return;
    }

    const play = demo => {
        demo.classList.remove('is-playing');
        void demo.offsetWidth;
        demo.classList.add('is-playing');
    };

    demos.forEach(demo => demo.classList.add('is-armed'));

    if (!('IntersectionObserver' in window)) {
        demos.forEach(play);
        return;
    }

    const observer = new IntersectionObserver((entries, obs) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                play(entry.target);
                obs.unobserve(entry.target);
            }
        });
    }, { threshold: 0.35 });

    demos.forEach(demo => observer.observe(demo));
}

if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initHeroDemo);
} else {
    initHeroDemo();
}

export default {
    defaultTheme: 'auto',
    iconLinks: [
        {
            icon: 'github',
            href: 'https://github.com/mishael-o/Dapper.SimpleSqlBuilder',
            title: 'GitHub'
        }
    ]
}
