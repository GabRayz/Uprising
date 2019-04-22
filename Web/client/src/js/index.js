let header = document.getElementById('mainHeader');

document.body.onscroll = e => {
    let range = document.scrollingElement.scrollTop / 400;

    header.style.backgroundColor = 'rgba(40, 40, 40, ' + range + ')';
};

let playButton = document.getElementsByClassName("playButton")[0];

playButton.addEventListener("mouseenter", function(e) {
    document.getElementsByClassName("playButton_bg")[0].style.backgroundImage = "url(/assets/img/play_hover.png)";
})
playButton.addEventListener("mouseleave", function(e) {
    document.getElementsByClassName("playButton_bg")[0].style.backgroundImage = "url(/assets/img/play.png)";
})
playButton.addEventListener("click", function(e) {
    document.getElementsByClassName("playButton_bg")[0].style.backgroundImage = "url(/assets/img/play_clic.png)";
})

let popup = document.getElementsByClassName("popup")[0];

function displayLogin() {
    console.log("display login");
    popup.style.visibility = (popup.style.visibility == "visible") ? "hidden" : "visible";
}

playButton.onclick = e => {
    while (document.body.firstChild) {
        document.body.removeChild(document.body.firstChild);
    }

    const container = document.createElement("div");
    container.id = "container";
    container.style.width = "100%";
    container.style.height = "100%";
    document.body.appendChild(container);

    const instance = UnityLoader.instantiate("container", "/assets/game/Build/game.json", { onProgress: UnityProgress });

    instance.SetFullscreen(1);
};