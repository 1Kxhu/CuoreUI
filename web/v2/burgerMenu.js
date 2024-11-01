document.addEventListener('DOMContentLoaded', () => {
    const burger = document.querySelector('.burger');
    const navLinks = document.querySelector('.nav-links');

    burger.addEventListener('click', () => {
        console.log('burger clicked');

        navLinks.classList.toggle('nav-links-show');
        burger.classList.toggle('burger-toggled');
    });

    const downloadButtonInNav = document.querySelector('.download-nav');
    downloadButtonInNav.addEventListener('click', () => {
        console.log('download clicked');
        downloadButtonInNav.textContent = 'Redirecting..';

        setTimeout(() => {
            downloadButtonInNav.textContent = 'Download';
        }, 1000);
    });
});
