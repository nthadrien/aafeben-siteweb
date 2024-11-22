// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const objectObs = document.querySelectorAll("[data-observer]");

const observer = new IntersectionObserver(entries => {
    entries.forEach( entry => {
        entry.target.classList.toggle("observe", entry.isIntersecting);
        if ( entry.isIntersecting ) observer.unobserve(entry.target);
    })
},  { threshold: 0.2 } );

objectObs.forEach( Element => observer.observe(Element) );

// Navbar observer
const navbar = document.querySelector("#home-nav");
const hero = document.querySelector("#header-hero");

const observerNav = new IntersectionObserver(entries => {
    entries.forEach( entry => {
        entry.target.classList.toggle("observe", entry.isIntersecting);
        if ( !entry.isIntersecting ) {
            navbar.classList.add("bg-primary");
            navbar.classList.add("fixed-top");
            navbar.classList.remove("position-absolute");
        } else {
            navbar.classList.remove("bg-primary");
            navbar.classList.add("position-absolute");
            console.log("back");
        }
    })
},  { rootMargin: "16px 0px 0px 0px" } );

observerNav.observe(hero);


// going back to previous page
document.getElementById("go-back")?.addEventListener("click", () => {
    history.back();
});


// toolstips 

const tooltipTriggerList = document?.querySelectorAll('[data-bs-toggle="tooltip"]');
const tooltipList = [...tooltipTriggerList]?.map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));