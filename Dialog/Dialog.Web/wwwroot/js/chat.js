"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;

    var incomingDiv = document.createElement("div");
    incomingDiv.className = "incoming_msg";

    var imgDiv = document.createElement("div");
    imgDiv.className = "incoming_msg_img";
    imgDiv.innerHTML = '<img src="https://ptetutorials.com/images/user-profile.png" alt="sunil">';
    incomingDiv.appendChild(imgDiv);

    var receivedDiv = document.createElement("div");
    receivedDiv.className = "received_msg";

    var receivedMsgDiv = document.createElement("div");
    receivedMsgDiv.className = "received_withd_msg";
    var p = document.createElement("p");
    p.innerHTML = msg;
    receivedMsgDiv.appendChild(p);

    var span = document.createElement("span");
    span.className = "time_date";
    var date = new Date();
    span.innerHTML = date.toTimeString().split(' ')[0] + " | " + user;
    receivedMsgDiv.appendChild(span);

    receivedDiv.appendChild(receivedMsgDiv);

    incomingDiv.appendChild(receivedDiv);

    document.getElementById("messagesList").appendChild(incomingDiv);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var messageInput = document.getElementById("messageInput");
    var message = messageInput.value;

    messageInput.value = '';

    connection.invoke("SendMessage", user, message).catch(function (err) {
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