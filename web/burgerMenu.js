document.addEventListener('DOMContentLoaded', () => {
    const burger = document.querySelector('.burger');
    const navLinks = document.querySelector('.nav-links');

    burger.addEventListener('click', () => {
        console.log('burger clicked');

        navLinks.classList.toggle('nav-links-show');
        burger.classList.toggle('burger-toggled');
    });

    let wasDownloadClicked = false;

    const downloadButtonInNav = document.querySelector('.download-nav');
    downloadButtonInNav.addEventListener('click', () => {
        console.log('download clicked');

        wasDownloadClicked = true;

        downloadButtonInNav.textContent = 'Redirecting..';
    });

    function updateButton()
    {
        if (wasDownloadClicked == false)
        {
            downloadButtonInNav.textContent = 'Download';
        }

        wasDownloadClicked = false;
    }

    setInterval(() => {
        updateButton();
    }, 1000);
});
