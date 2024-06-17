//for making the recording button blink
function blinkOverlay(odd, recording) {
    if (odd) {
        recording.style.visibility = "hidden";
        odd = false;
    } else {
        recording.style.visibility = "visible";
        odd = true;
    }

    return odd;
};

//for handling the theme toggle form light to dark
function toggleTheme(event) {
    const e = event.childNodes;

    for (i = 0; i < e.length; i++) {
        if (e[i].tagName == 'IMG') {
            const image = e[i].src.toUpperCase()
            if (image.includes('MOON')) {
                e[i].src = 'public/sun.svg';
            } else {
                e[i].src = 'public/moon.svg';
            }

        }
    }

    const theme = document.querySelector("#page-theme");

    if (theme.classList.contains("light")) {
        theme.classList.add("dark");
        theme.classList.remove("light");
    } else {
        theme.classList.add("light");
        theme.classList.remove("dark");
    }
}

//updates timer on overlay 
function startTimer() {
    tens++;

    if (tens <= 9) {
        appendTens.innerHTML = "0" + tens;
    }

    if (tens > 9) {
        appendTens.innerHTML = tens;

    }

    if (tens > 99) {
        seconds++;
        appendSeconds.innerHTML = "0" + seconds;
        tens = 0;
        appendTens.innerHTML = "0" + 0;
    }

    if (seconds <= 9) {
        appendSeconds.innerHTML = "0" + seconds;
    }

    if (seconds > 9) {
        appendSeconds.innerHTML = seconds;
    }

    if (seconds > 59) {
        minutes++;
        appendMinutes.innerHTML = "0" + minutes;
        seconds = 0;
        appendSeconds.innerHTML = "0" + 0;
    }

    if (minutes <= 9) {
        appendMinutes.innerHTML = "0" + minutes;
    }

    if (minutes > 9) {
        appendMinutes.innerHTML = minutes;
    }

    if (minutes > 59) {
        hours++;
        appendHours.innerHTML = "0" + hours;
        minutes = 0;
        appendMinutes.innerHTML = "0" + 0;
    }

    if (hours <= 9) {
        appendHours.innerHTML = "0" + hours;
    }

    if (hours > 9) {
        appendHours.innerHTML = hours;
    }

    if (hours > 99) {
        tens = "0";
        seconds = "0";
        minutes = "0";
        hours = "00";
        appendTens.innerHTML = tens;
        appendSeconds.innerHTML = seconds;
        appendMinutes.innerHTML = minutes;
        appendHours.innerHTML = hours;
    }

}

//updates the overlay battery to reduce/ increase power as needed
function chargeStatus(flash, charge, battery) {
    const battery_components = battery.childNodes;
    var component_dict = {};

    for (i = 0; i < battery_components.length; i++) {

        if (battery_components[i].tagName == 'polygon') {
            var component_id = battery_components[i].id;

            component_dict[component_id] = battery_components[i];
        }

    }

    switch (charge) {
        case 5:
            battery.style.fill = "";
            battery.style.stroke = "";

            component_dict['batteryCell-1'].style.visibility = "visible";
            component_dict['batteryCell-2'].style.visibility = "visible";
            component_dict['batteryCell-3'].style.visibility = "visible";
            component_dict['batteryCell-4'].style.visibility = "visible";

            if (flash) {
                component_dict['batteryCell-5'].style.visibility = "visible";
                flash = false;
            } else {
                component_dict['batteryCell-5'].style.visibility = "hidden";
                flash = true;
            }
            break;

        case 4:
            battery.style.fill = "";
            battery.style.stroke = "";

            component_dict['batteryCell-1'].style.visibility = "visible";
            component_dict['batteryCell-2'].style.visibility = "visible";
            component_dict['batteryCell-3'].style.visibility = "visible";
            component_dict['batteryCell-5'].style.visibility = "hidden";

            if (flash) {
                component_dict['batteryCell-4'].style.visibility = "visible";
                flash = false;
            } else {
                component_dict['batteryCell-4'].style.visibility = "hidden";
                flash = true;
            }
            break;

        case 3:
            battery.style.fill = "";
            battery.style.stroke = "";

            component_dict['batteryCell-1'].style.visibility = "visible";
            component_dict['batteryCell-2'].style.visibility = "visible";
            component_dict['batteryCell-5'].style.visibility = "hidden";
            component_dict['batteryCell-4'].style.visibility = "hidden";

            if (flash) {
                component_dict['batteryCell-3'].style.visibility = "visible";
                flash = false;
            } else {
                component_dict['batteryCell-3'].style.visibility = "hidden";
                flash = true;
            }
            break;

        case 2:
            battery.style.fill = "";
            battery.style.stroke = "";

            component_dict['batteryCell-1'].style.visibility = "visible";
            component_dict['batteryCell-5'].style.visibility = "hidden";
            component_dict['batteryCell-3'].style.visibility = "hidden";
            component_dict['batteryCell-4'].style.visibility = "hidden";

            if (flash) {
                component_dict['batteryCell-2'].style.visibility = "visible";
                flash = false;
            } else {
                component_dict['batteryCell-2'].style.visibility = "hidden";
                flash = true;
            }
            break;

        case 1:
            battery.style.fill = "";
            battery.style.stroke = "";

            component_dict['batteryCell-5'].style.visibility = "hidden";
            component_dict['batteryCell-2'].style.visibility = "hidden";
            component_dict['batteryCell-3'].style.visibility = "hidden";
            component_dict['batteryCell-4'].style.visibility = "hidden";

            if (flash) {
                component_dict['batteryCell-1'].style.visibility = "visible";
                flash = false;
            } else {
                component_dict['batteryCell-1'].style.visibility = "hidden";
                flash = true;
            }

            break;

        default:
            battery.style.fill = "red";
            battery.style.stroke = "red";

            component_dict['batteryCell-1'].style.visibility = "hidden";
            component_dict['batteryCell-2'].style.visibility = "hidden";
            component_dict['batteryCell-3'].style.visibility = "hidden";
            component_dict['batteryCell-4'].style.visibility = "hidden";
            component_dict['batteryCell-5'].style.visibility = "hidden";
            break;
    }

    return flash;
}

//makes side bar overlay of pages 
function pageSetup(pageLen) {
    var page_content = document.querySelector('.page-content');
    var page_bar = document.querySelector('.page-bar');

    var pages = Math.ceil(page_content.scrollHeight / page_content.clientHeight);

    if (pages > pageLen) {
        for (i = pageLen + 1; i < pages + 1; i++) {

            //create seperator
            var newSeperator = document.createElement("div");
            newSeperator.classList.add("pos-seperator");

            //create div
            var newNode = document.createElement("a");
            newNode.classList = "pos";
            newNode.id = "pos-" + i;
            newNode.setAttribute("href", "#page-" + i);

            // and give it some content
            var newContent = document.createTextNode(i);

            // add the text node to the newly created div
            newNode.appendChild(newContent);

            // add the newly created element and its content into the DOM
            const currentNode = document.querySelector("#page-bar");

            currentNode.appendChild(newSeperator);
            currentNode.appendChild(newNode);
        }
        return pages;
    }
    return pageLen;
}

//highlights circle on side bar overlay with scroll
function pagePosition() {
    var page_content = document.querySelector('.page-content');

    var page_bar = document.querySelector("#page-bar");

    for (i = 0; i < page_bar.children.length; i++) {
        var child = page_bar.children[i];

        if (child.classList.contains("pos")) {
            child.classList.remove("current");
        }

    }

    var scroll_position = page_content.scrollTop + page_content.clientHeight;

    var page_num = Math.floor((scroll_position / page_content.clientHeight))

    if (scroll_position > 0) {
        page_id = "#pos-" + (page_num);
    }

    var highlight = document.querySelector(page_id);
    highlight.classList.add("current");

    if (page_num != pageLen) {
        charge = 5 - (Math.floor(page_num / (pageLen / 5)));
    } else {
        charge = 0;
    }

    flash = true;

}

function setUnderline() {
    const name = document.querySelector("#title-name");
    const job = document.querySelector("#title-job");

    if (name.clientWidth > job.clientWidth) {
        document.querySelector(".morse-underline").style.width = name.clientWidth + "px";
    } else {
        document.querySelector(".morse-underline").style.width = job.clientWidth + "px";
    }
}

//gets mouse position
function getMousePosition(event) {
    var eventDoc, doc, body;

    event = event || window.event;

    if (event.pageX == null && event.clientX != null) {
        eventDoc = (event.target && event.target.ownerDocument) || document;
        doc = eventDoc.documentElement;
        body = eventDoc.body;

        event.pageX = event.clientX +
            (doc && doc.scrollLeft || body && body.scrollLeft || 0) -
            (doc && doc.clientLeft || body && body.clientLeft || 0);
        event.pageY = event.clientY +
            (doc && doc.scrollTop || body && body.scrollTop || 0) -
            (doc && doc.clientTop || body && body.clientTop || 0);
    }

    followMousePosition(event.pageX, event.pageY)

    return [event.pageX, event.pageY];
}

function followMousePosition(mouseX, mouseY) {
    var mouseFollower = document.querySelector("#mouse-follow");

    var theme = document.querySelector("#page-theme").classList;

    if (theme.contains("dark")) {
        mouseFollower.style.background = "radial-gradient(400px at " + mouseX + "px " + mouseY + "px, rgb(56 216 29 / 15%), transparent 80%)";
    } else {
        mouseFollower.style.background = "radial-gradient(400px at " + mouseX + "px " + mouseY + "px, rgb(48 0 226 / 15%), transparent 80%)";
    }

}

//show and hide hidden pages
function showProjectPage(pageId) {
    var hidden_page_container = document.querySelector(".hidden-pages");

    var target_page = document.querySelector("#" + pageId);

    hidden_page_container.classList.toggle("closed");

    target_page.classList.toggle("closed");
}

function closeHiddenPages() {
    var hidden_page_container = document.querySelector(".hidden-pages");

    hidden_page_container.classList.toggle("closed");

    for (i = 0; i < hidden_page_container.childElementCount; i++) {
        var child = hidden_page_container.children[i];
        if (!child.classList.contains("closed")) {
            child.classList.toggle("closed");
        }
    }
}

//hide all pages
closeHiddenPages();

var page = document.getElementById('page'),
    ua = navigator.userAgent,
    iphone = ~ua.indexOf('iPhone') || ~ua.indexOf('iPod');

var setupScroll = window.onload = function () {
    // Start out by adding the height of the location bar to the width, so that
    // we can scroll past it
    if (iphone) {
        // iOS reliably returns the innerWindow size for documentElement.clientHeight
        // but window.innerHeight is sometimes the wrong value after rotating
        // the orientation
        var height = document.documentElement.clientHeight;
        // Only add extra padding to the height on iphone / ipod, since the ipad
        // browser doesn't scroll off the location bar.
        if (iphone && !fullscreen) height += 60;
        page.style.height = height + 'px';
    }
    // Scroll after a timeout, since iOS will scroll to the top of the page
    // after it fires the onload event
    setTimeout(scrollTo, 0, 0, 1);
};

/// overlay effects
//for recording
const recording_dot = document.querySelector("#recording-dot");
var odd = false;
setInterval(() => odd = blinkOverlay(odd, recording_dot), 1000);

//for battery
const battery = document.querySelector("#battery").firstElementChild;
var charge = 5;
var flash = true;
setInterval(() => flash = chargeStatus(flash, charge, battery), 750);

//for timer
var hours = 00;
var minutes = 00;
var seconds = 00;
var tens = 00;

var appendTens = document.getElementById("tens")
var appendSeconds = document.getElementById("seconds")
var appendMinutes = document.getElementById("minutes")
var appendHours = document.getElementById("hours")

var buttonStart = document.getElementById('button-start');
var buttonStop = document.getElementById('button-stop');
var buttonReset = document.getElementById('button-reset');

setInterval(startTimer, 10);

//side bar
var pageLen = 1
pageLen = pageSetup(pageLen);

//highighting relevant circle on side bar
pagePosition();
var page_content = document.querySelector('.page-content');
page_content.addEventListener("scroll", pagePosition);

//underline under name matches width
setUnderline();
window.onresize = () => (pageLen = pageSetup(pageLen), setUnderline());

//mouse hover effect
var mousePosition = [0, 0];

document.onmousemove = () => (mousePosition = getMousePosition());