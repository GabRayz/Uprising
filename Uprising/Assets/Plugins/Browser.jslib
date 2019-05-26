mergeInto(LibraryManager.library, {
    GetUsername: function () {
        console.log("get username");
        var username = JSON.parse(localStorage.user).username;
        
        var buffer = _malloc(lengthBytesUTF8(username) + 1);
        writeStringToMemory(username, buffer);
        return buffer;
    },
    GetCookie: function () {
        console.log("get cookie");

        var cookie = document.cookie;

        var buffer = _malloc(lengthBytesUTF8(cookie) + 1);
        writeStringToMemory(cookie, buffer);
        return buffer;
    }
});