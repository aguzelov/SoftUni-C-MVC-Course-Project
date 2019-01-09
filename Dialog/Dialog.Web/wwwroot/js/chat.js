"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("ReceiveMessage", function (chatName, user, message, date) {
    var currentOpenChatName = document.getElementById("chatInput").value;

    if (currentOpenChatName !== chatName) {
        return;
    }

    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    var incomingDiv = document.createElement("div");
    incomingDiv.className = "incoming_msg";

    //var imgDiv = document.createElement("div");
    //imgDiv.className = "incoming_msg_img";
    //imgDiv.innerHTML = '<img src="https://ptetutorials.com/images/user-profile.png" alt="sunil">';
    //incomingDiv.appendChild(imgDiv);

    var receivedDiv = document.createElement("div");
    receivedDiv.className = "received_msg";

    var receivedMsgDiv = document.createElement("div");
    receivedMsgDiv.className = "received_withd_msg";
    var p = document.createElement("p");
    p.innerHTML = msg;
    receivedMsgDiv.appendChild(p);

    var span = document.createElement("span");
    span.className = "time_date";

    span.innerHTML = convertUTCDateToLocalDate(new Date(date)) + " | " + user;
    receivedMsgDiv.appendChild(span);

    receivedDiv.appendChild(receivedMsgDiv);

    incomingDiv.appendChild(receivedDiv);

    var messagesList = document.getElementById("messagesList");
    messagesList.appendChild(incomingDiv);
    scrollToBottom("messagesList");
});

connection.on("GetRecentMessages", function (messages) {
    var user = document.getElementById("userInput").value;

    var messagesJson = JSON.parse(messages);
    var messagesList = document.getElementById("messagesList");

    messagesJson.forEach((item) => {
        var username = item["Username"];
        var text = item["Text"];
        var datetime = item["CreatedOn"];

        var date = convertUTCDateToLocalDate(new Date(datetime));

        //var date = item["Date"];

        if (username === user) {
            var outgoingDiv = document.createElement("div");
            outgoingDiv.className = "outgoing_msg";

            var sentMsgDiv = document.createElement("div");
            sentMsgDiv.className = "sent_msg";

            var p = document.createElement("p");
            p.innerHTML = text;
            sentMsgDiv.appendChild(p);

            var span = document.createElement("span");
            span.className = "time_date";
            span.innerHTML = date;
            sentMsgDiv.appendChild(span);

            outgoingDiv.appendChild(sentMsgDiv);

            messagesList.appendChild(outgoingDiv);
        } else {
            var incomingDiv = document.createElement("div");
            incomingDiv.className = "incoming_msg";

            //var imgDiv = document.createElement("div");
            //imgDiv.className = "incoming_msg_img";
            //imgDiv.innerHTML = '<img src="https://ptetutorials.com/images/user-profile.png" alt="sunil">';
            //incomingDiv.appendChild(imgDiv);

            var receivedDiv = document.createElement("div");
            receivedDiv.className = "received_msg";

            var receivedMsgDiv = document.createElement("div");
            receivedMsgDiv.className = "received_withd_msg";
            var incomingP = document.createElement("p");
            incomingP.innerHTML = text;
            receivedMsgDiv.appendChild(incomingP);

            var incomingSpan = document.createElement("span");
            incomingSpan.className = "time_date";
            incomingSpan.innerHTML = date + " | " + username;
            receivedMsgDiv.appendChild(incomingSpan);

            receivedDiv.appendChild(receivedMsgDiv);

            incomingDiv.appendChild(receivedDiv);

            messagesList.appendChild(incomingDiv);
        }
        scrollToBottom("messagesList");
    });
});

connection.on("GetUserChats", function (chats) {
    var user = document.getElementById("userInput").value;

    var chatsJson = JSON.parse(chats);

    document.getElementById("inbox_chats").innerHTML = "";

    chatsJson.forEach((item) => {
        var chatId = item["ChatId"];
        var chatName = item["ChatName"];

        var chatListDiv = document.createElement("div");
        chatListDiv.className = "chat_list";
        chatListDiv.id = chatName;

        chatListDiv.onclick = function () {
            changeChat(chatName);
        };

        var chatPeopleDiv = document.createElement("div");
        chatPeopleDiv.className = "chat_people";

        var chatIbDiv = document.createElement("div");
        chatIbDiv.className = "chat_ib";

        var h = document.createElement("h");
        h.innerHTML = chatName;
        chatIbDiv.appendChild(h);

        chatPeopleDiv.appendChild(chatIbDiv);

        var closeDiv = document.createElement("div");
        closeDiv.className = "chat_img";

        var closeButton = document.createElement("button");
        closeButton.innerHTML = "&times;";

        closeButton.onclick = function () {
            removeFromChat(chatName, user);
        };

        closeDiv.appendChild(closeButton);
        chatPeopleDiv.appendChild(closeDiv);

        chatListDiv.appendChild(chatPeopleDiv);

        document.getElementById("inbox_chats").appendChild(chatListDiv);
    });
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

window.onload = function () {
    //var user = document.getElementById("userInput").value;
    //connection.invoke("SendUserChats", user).catch(function (err) {
    //    return console.error(err.toString());
    //});

    getRecentMessages("Global");
};

document.getElementById("sendButton").addEventListener("click", function (event) {
    var chatName = document.getElementById("chatInput").value;
    var messageInput = document.getElementById("messageInput");
    var message = messageInput.value;

    if (isEmptyOrSpaces(message)) {
        messageInput.value = '';
        return;
    }

    messageInput.value = '';

    connection.invoke("SendMessage", chatName, message).catch(function (err) {
        return console.error(err.toString());
    });

    var outgoingDiv = document.createElement("div");
    outgoingDiv.className = "outgoing_msg";

    var sentMsgDiv = document.createElement("div");
    sentMsgDiv.className = "sent_msg";

    var p = document.createElement("p");
    p.innerHTML = message;
    sentMsgDiv.appendChild(p);

    var date = new Date();

    var span = document.createElement("span");
    span.className = "time_date";
    span.innerHTML = date.toTimeString().split(' ')[0];
    sentMsgDiv.appendChild(span);

    outgoingDiv.appendChild(sentMsgDiv);

    document.getElementById("messagesList").appendChild(outgoingDiv);
    scrollToBottom("messagesList");
    event.preventDefault();
});

// Submit message when press Enter
document.getElementById("messageInput")
    .addEventListener("keydown", function (event) {
        if (event.keyCode === 13 && !event.shiftKey) {
            event.preventDefault();
            document.getElementById("sendButton").click();
        }
    });

function getRecentMessages(chatName) {
    connection.start().then(() => {
        connection.invoke("SendRecentMessages", chatName).catch(function (err) {
            return console.error(err.toString());
        })
    });

    scrollToBottom("messagesList");
}

function changeChat(chatName) {
    document.getElementById("messagesList").innerHTML = "";
    var oldCahtName = document.getElementById("chatInput").value;
    document.getElementById(oldCahtName).className = "chat_list";

    getRecentMessages(chatName);

    document.getElementById("chatInput").setAttribute("value", chatName);
    document.getElementById(chatName).className = "chat_list active_chat";
}

function isEmptyOrSpaces(str) {
    return str === null || str.match(/^ *$/) !== null;
}

function scrollToBottom(id) {
    var messagesList = document.getElementById("messagesList");
    messagesList.scrollTop = messagesList.scrollHeight;
}

function convertUTCDateToLocalDate(date) {
    var newDate = new Date(date.toString() + " UTC");

    var currentDate = new Date();

    var dateString;

    if (newDate.getDate() !== currentDate.getDate()) {
        dateString = newDate.getDate() + "." + (newDate.getMonth() + 1) + "." + newDate.getFullYear() + " " +
            newDate.getHours() + ":" + newDate.getMinutes() + ":" + newDate.getSeconds();
    } else {
        dateString = newDate.getHours() + ":" + newDate.getMinutes() + ":" + newDate.getSeconds();
    }

    return dateString;
}