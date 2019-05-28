let header = document.getElementById('mainHeader');

document.body.onscroll = e => {
    let range = document.scrollingElement.scrollTop / 400;

    header.style.backgroundColor = 'rgba(40, 40, 40, ' + range + ')';
};

let playButton = document.querySelector('.playButton');

playButton.addEventListener('mouseenter', e => {
    document.querySelector('.playButton_bg').style.backgroundImage =
        'url(/assets/img/play_hover.png)';
});

playButton.addEventListener('mouseleave', e => {
    document.querySelector('.playButton_bg').style.backgroundImage = 'url(/assets/img/play.png)';
});

playButton.addEventListener('click', e => {
    document.querySelector('.playButton_bg').style.backgroundImage =
        'url(/assets/img/play_clic.png)';
});

let popup = document.querySelector('.popup');

// document.querySelector('#navBar > li.nav_login > a').onclick = e => {
//     console.log('display login');
//     popup.style.visibility = popup.style.visibility == 'visible' ? 'hidden' : 'visible';
// };

playButton.onclick = e => {
    window.location = '/game';
};

window.onload = async e => {
    const res = await fetch('/auth/data').then(res => res.json());
    if (res) {
        console.log('logged');
        const element = document.querySelector('.nav_login');
        element.parentElement.removeChild(element);
    }

    const leaderboard = await fetch('/leaderboard').then(res => res.json());

    if (!leaderboard) return;

    const parent = document.querySelector('#rankingFrame > div > article > table > tbody');

    leaderboard.forEach(player => {
        const main = document.createElement('tr');

        const appendStat = stat => {
            const td = document.createElement('td');
            td.textContent = stat;
            main.appendChild(td);
        };

        appendStat(player.rank);
        appendStat(player.username);
        appendStat(0);
        appendStat(player.xp);
        appendStat(player.gameCount);
        appendStat(player.winCount);
        appendStat(Math.floor(player.accuracy * 100) + ' %');

        parent.appendChild(main);
    });
};
