mergeInto(LibraryManager.library, {
    GetUsername: function () {
        var user = JSON.parse(localStorage.user);
        return user.username;
        console.log("get username");
    }
});