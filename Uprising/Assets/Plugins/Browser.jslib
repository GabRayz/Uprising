mergeInto(LibraryManager.library, {
    GetUsername: function () {
        console.log("get username");
        var user = JSON.parse(localStorage.user);
        console.log("js : " + user.username);
        return user.username;
    }
});