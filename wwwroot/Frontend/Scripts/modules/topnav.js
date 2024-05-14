export function dropdownClicked(element) {
    const dropdowns = document.getElementsByClassName("dropdown");
    const width = window.innerWidth;
    if (element.classList.contains("nochild")) {
        window.location.href = element.id;
    } else {
        if (width > 1024) {
            window.location.href = element.id;
        } else {
            if (element.classList.contains("closed")) {
                for (let dropdown of dropdowns) {
                    if (dropdown.classList.contains("open")) {
                        dropdown.classList.add("closed");
                        dropdown.classList.remove("open");
                        element.children[0].style.paddingBottom = "initial";
                    }
                }
                element.classList.add("open");
                element.classList.remove("closed");

                element.children[0].style.paddingBottom = "8px";
            }
            else {
                window.location.href = element.id;
            }
        }
    }
}

export function expandNavMobile(state) {
    const nav = document.getElementsByClassName("mobile-header-collapse")[0];
    const dropdowns = document.getElementsByClassName("dropdown");

    if (nav.classList.contains("open")) {
        nav.classList.remove("open");
        nav.classList.add("closed");
    }
    else {
        nav.classList.remove("closed");
        nav.classList.add("open");

        for (let dropdown of dropdowns) {
            if (dropdown.classList.contains("open")) {
                dropdown.classList.add("closed");
                dropdown.classList.remove("open");
            }
        }
    }
}