window.onload = async e => {
    const res = await fetch('/auth/data').then(res => res.json());
    localStorage.user = JSON.stringify(res);
    window.location = '/';
}