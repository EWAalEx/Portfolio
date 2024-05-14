import { dropdownClicked, expandNavMobile } from './modules/topnav.js';
import { setCSSColours } from './modules/colourCSS.js';

function navSizeCheck() {
    const nav = document.getElementsByClassName("nav");
    const mainContent = document.getElementById("main-content");
    mainContent.style.paddingTop = nav[0].clientHeight.toString() + "px";
    mainContent.style.minHeight = (window.innerHeight - nav[0].clientHeight).toString() + "px";

}
window.onload = navSizeCheck;

window.setCSSColours = setCSSColours;
window.dropdownClicked = dropdownClicked;
window.navSizeCheck = navSizeCheck;
window.expandNavMobile = expandNavMobile;

document.getElementById("loader").click();
window.addEventListener('resize', navSizeCheck);
