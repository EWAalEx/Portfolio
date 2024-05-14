export function setCSSColours(ps, ss, ts, pt, st, tt) {

    var root = document.querySelector(':root');
    root.style.setProperty('--primary-site-colour', ps);
    root.style.setProperty('--secondary-site-colour', ss);
    root.style.setProperty('--tertiary-site-colour', ts);
    root.style.setProperty('--primary-text-colour', pt);
    root.style.setProperty('--secondary-text-colour', st);
    root.style.setProperty('--tertiary-text-colour', tt);
}