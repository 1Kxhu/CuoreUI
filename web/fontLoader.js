// fontLoader.js
document.addEventListener("DOMContentLoaded", () => {
    console.log("dom loaded");
    // Wait for the font to load
    document.fonts.ready.then(() => {
        console.log("fonts ready");
        // Add the class to switch to MiSans font
        document.body.classList.add('font-loaded');
        console.log("added .font-loaded to body");
    });
});
