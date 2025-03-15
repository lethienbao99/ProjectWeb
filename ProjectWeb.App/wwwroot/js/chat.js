var connectionChat = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/chat").build();
connectionChat.on("MessageRevieced", function (user, message, type) {
    debugger
    if (type == "Text") {
        document.getElementById(
            "messageBox"
        ).innerHTML += `<div class="second-chat">
              <div class="circle" id="circle-mar"></div>
              <p>${message}</p>
              <div class="arrow"></div>
            </div>
            <input type="hidden" value="${user}" id="userReply" />
            `;
        var objDiv = document.getElementById("messageBox");
        objDiv.scrollTop = objDiv.scrollHeight;
    }
    else {
        document.getElementById(
            "messageBox"
        ).innerHTML += `<div class="second-chat">
              <img src="${message}" style="max-width: 200px;"/>
            </div>
            <input type="hidden" value="${user}" id="userReply" />
            `;
        var objDiv = document.getElementById("messageBox");
        objDiv.scrollTop = objDiv.scrollHeight;
    }
});

// Handle image upload
document.getElementById("imageInput").addEventListener("change", function () {
    debugger
    var base64Image = '';
    var UserIDLogin = document.getElementById("UserIDLogin").value;
    var receiver = document.getElementById("UserCreateID").value 
    if (UserIDLogin == receiver)
        receiver = document.getElementById("userReply").value;
    const file = this.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (event) {
            base64Image = event.target.result;
            connectionChat.send("SendMesaageToReceiver", receiver, base64Image, "Image").catch(function (err) {
                console.error("Error sending image:", err);
            });
        };
        reader.readAsDataURL(file);
        setTimeout(() => {
            document.getElementById(
                "messageBox"
            ).innerHTML += `<div class="first-chat">
              <img src="${base64Image}" style="max-width: 200px;"/>
            </div>
            `;
            document.getElementById("imageInput").value = "";
            var objDiv = document.getElementById("messageBox");
            objDiv.scrollTop = objDiv.scrollHeight;
        }, 1000);

    }
});


document.getElementById("sendMessage").addEventListener("click", function (event) {
    debugger
    var UserIDLogin = document.getElementById("UserIDLogin").value;
    var message = document.getElementById("textInput").value;
    var receiver = document.getElementById("UserCreateID").value 

    if (UserIDLogin == receiver)
        receiver = document.getElementById("userReply").value;

    if (receiver.length > 0) {
        connectionChat.send("SendMesaageToReceiver", receiver, message, "Text").catch(function (err) {
            return console.log(err);
        });
        let userText = message;
        if (userText == "") {
            alert("Please type something!");
        } else {
            document.getElementById("messageBox").innerHTML += `<div class="first-chat">
          <p>${userText}</p>
          <div class="arrow"></div>
        </div>`;
   
            document.getElementById("textInput").value = "";
            var objDiv = document.getElementById("messageBox");
            objDiv.scrollTop = objDiv.scrollHeight;
        }
    }
    else {
        connectionChat.send("SendMesaageToAll", sender, message).catch(function (err) {
            return console.log(err);
        });
    }

    event.preventDefault();
});

function fulfilled() {
}
function rejected() {
}

let audio1 = new Audio(
    "https://s3-us-west-2.amazonaws.com/s.cdpn.io/242518/clickUp.mp3"
);
function chatOpen() {
    document.getElementById("chat-open").style.display = "none";
    document.getElementById("chat-close").style.display = "block";

}
function chatClose() {
    document.getElementById("chat-open").style.display = "block";
    document.getElementById("chat-close").style.display = "none";
    document.getElementById("chat-window2").style.display = "none";


}
function openConversation() {
    chatOpen();
    document.getElementById("chat-window2").style.display = "block";


}

//Gets the text from the input box(user)
/*function userResponse() {
    console.log("response");
    let userText = document.getElementById("textInput").value;

    if (userText == "") {
        alert("Please type something!");
    } else {
        document.getElementById("messageBox").innerHTML += `<div class="first-chat">
          <p>${userText}</p>
          <div class="arrow"></div>
        </div>`;
        let audio3 = new Audio(
            "https://prodigits.co.uk/content/ringtones/tone/2020/alert/preview/4331e9c25345461.mp3"
        );
        audio3.load();
        audio3.play();

        document.getElementById("textInput").value = "";
        var objDiv = document.getElementById("messageBox");
        objDiv.scrollTop = objDiv.scrollHeight;

        setTimeout(() => {
            adminResponse();
        }, 1000);
    }
}*/

//admin Respononse to user's message
/*function adminResponse() {


    fetch("https://api.adviceslip.com/advice")
        .then((response) => {
            return response.json();
        })
        .then((adviceData) => {
            let Adviceobj = adviceData.slip;
            document.getElementById(
                "messageBox"
            ).innerHTML += `<div class="second-chat">
              <div class="circle" id="circle-mar"></div>
              <p>${Adviceobj.advice}</p>
              <div class="arrow"></div>
            </div>`;






            let audio3 = new Audio(
                "https://downloadwap.com/content2/mp3-ringtones/tone/2020/alert/preview/56de9c2d5169679.mp3"
            );
            audio3.load();
            audio3.play();

            var objDiv = document.getElementById("messageBox");
            objDiv.scrollTop = objDiv.scrollHeight;
        })
        .catch((error) => {
            console.log(error);
        });
}*/

//press enter on keyboard and send message
/*addEventListener("keypress", (e) => {
    if (e.keyCode === 13) {

        const e = document.getElementById("textInput");
        if (e === document.activeElement) {
            userResponse();
        }
    }
});*/







//start connection
connectionChat.start().then(fulfilled, rejected);