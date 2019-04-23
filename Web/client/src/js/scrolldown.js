let header = document.getElementById('mainHeader');

document.body.onscroll = e => {
    let range = document.scrollingElement.scrollTop / 400;

    header.style.backgroundColor = 'rgba(40, 40, 40, ' + range + ')';
};