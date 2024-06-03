$(function () {

    var INDEX = 0;
    voiceToText();

    // ----------------------------- Voice to text ---------------------------------------

    function voiceToText() {
        const recognition = new webkitSpeechRecognition();
        recognition.continuous = true;
        recognition.lang = 'en-US';

        const startVoiceBtn = document.getElementById('start-voice-record');
        const stopVoiceBtn  = document.getElementById('stop-voice-record');
        const messageInput  = document.getElementById('chat-input');

        startVoiceBtn.addEventListener('click', () => {
            recognition.start();
            startVoiceBtn.style = "display:none;";
            stopVoiceBtn.style = "display:inline-block;";
        });

        stopVoiceBtn.addEventListener('click', () => {
            recognition.stop();
            stopVoiceBtn.style = "display:none;";
            startVoiceBtn.style = "display:inline-block;";
        });

        // Add a regular expression to filter out symbols
        const noSymbolsRegex = /[^\w\s]|_/g;

        recognition.onresult = (event) => {
            const lastResultIndex = event.results.length - 1;
            const lastTranscript = event.results[lastResultIndex][0].transcript;

            // Remove symbols from the transcript using the regex
            const transcriptWithoutSymbols = lastTranscript.replace(noSymbolsRegex, '');

            console.log(transcriptWithoutSymbols);

            messageInput.value = transcriptWithoutSymbols;
        };

    }

    // -------------------------- Submit -----------------------------------------------

    $("#chat-submit").click(function (e) {
        e.preventDefault();      

        // Get user input messege
        var msg = $("#chat-input").val();

        // If user enter nothing
        if (msg.trim() == '') {
            return false;
        }

        // From user side
        generateMessage(msg, 'self');

        // Get data via ajax request
        makeAjaxRequest("ChatBot/ChatMessege", "POST", "json", { userInput: msg  })
            .then(function (data) {

                // HttpStatusCode Ok
                if (data.status = 200) {
                    // From bot side
                    // Print all messages
                    if (data.type == 'Text') {
                        data.messeges.forEach(element => {
                            generateMessage(element, 'user');
                        });
                        
                    }
                    else if (data.type == 'Buttons') {
                        generateButtonMessage(msg, data.buttons)
                    }

                    console.log(data)
                }
                else
                    generateMessage("Server respond issue. Please try again", 'user');
            })
            .catch(function (error) {
                console.log(error)
                generateMessage("Something went wrong. Please try again later", 'user')
            });
    })

    /*
     * Add reply as a messege
     */
    function generateMessage(msg, type) {

        INDEX++;

        // Get messege as html (With user image and css class all)
        var str = generateChatMessage(msg, type, INDEX);

        // Add that generated html to chatbox
        $(".chat-logs").append(str);

        $("#cm-msg-" + INDEX).hide().fadeIn(300);

        // Clear chat input
        if (type == 'self') {
            $("#chat-input").val('');
        }

        $(".chat-logs").stop().animate({ scrollTop: $(".chat-logs")[0].scrollHeight }, 1000);
    }

    /*
     * Add reply as buttons
     */
    function generateButtonMessage(msg, buttons) {

        INDEX++;

        // Get button object as html
        var btn_obj = buttons.map(function (buttons) {
            return generateButtonObject(buttons.value, buttons.name, buttons.className, buttons.hrefUrl, buttons.buttonId);
        }).join('');

        // Get buttons as a messege in html
        var str = generateButtonChatMessage(msg, btn_obj, INDEX);

        // Add buttons to chat
        $(".chat-logs").append(str);

        // Message fade in
        $("#cm-msg-" + INDEX).hide().fadeIn(300);

        $(".chat-logs").stop().animate({ scrollTop: $(".chat-logs")[0].scrollHeight }, 1000);

        // Disable send button if we have buttons
        $("#chat-input").attr("disabled", true);
    }

    //$(document).delegate(".chat-btn", "click", function () {
    //    var value = $(this).attr("chat-value");
    //    var name = $(this).html();
    //    $("#chat-input").attr("disabled", false);
    //    generateMessage(name, 'self');
    //})

    $("#chat-circle").click(function () {
        $("#chat-circle").toggle('scale');
        $(".chat-box").toggle('scale');
        generateMessage("Hello! How may I help you?", 'user');
    })

    $(".chat-box-toggle").click(function () {
        $("#chat-circle").toggle('scale');
        $(".chat-box").toggle('scale');
    })

    // ----------------------------- Helpers ---------------------------------------------

    /*
     * Define a function that makes an AJAX request and returns a Promise object.
     */
    function makeAjaxRequest(url, method, dataType, data) {

        //  Create a new Promise object that encapsulates the AJAX request
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: url,
                method: method,
                dataType: dataType,
                data: data,
                success: function (response) {
                    resolve(response);
                },
                error: function (xhr, status, error) {
                    reject(xhr.responseText);
                }
            });
        });
    }


    /*
     * Get normal messege ib html format
     */
    function generateChatMessage(msg, type, index) {
        var strArr = [];
        strArr.push("<div id='cm-msg-" + index + "' class=\"chat-msg " + type + "\">");
        strArr.push("<span class=\"msg-avatar\">");
        if (type == 'user') {
            strArr.push("<img src='https://cdn-icons-png.freepik.com/512/8943/8943377.png'>");
        } else {
            strArr.push("<img src='https://picsum.photos/201'>");
        }
        strArr.push("<\/span>");
        strArr.push("<div class=\"cm-msg-text\">");
        strArr.push(msg);
        strArr.push("<\/div>");
        strArr.push("<\/div>");
        const speakerStatus = document.getElementById("speaker-status").value;
        if (speakerStatus == 1) {
            const synth = window.speechSynthesis;
            const utterance = new SpeechSynthesisUtterance(msg);
            const voices = synth.getVoices();
            utterance.voice = voices[0]
            synth.speak(utterance);

        }
        return strArr.join("");
    }

    /*
    * Get button in html format
    */
    function generateButtonObject(value, name, className, href, buttonId) {
        var strArr = [];
        strArr.push("<li class=\"button\">");
        strArr.push("<a target='_blank' href='" + href + "' id='" + buttonId + "' class=\"btn " + className + " chat-btn\" chat-value=\"" + value + "\">" + name + "<\/a>");
        strArr.push("<\/li>");

        return strArr.join("");
    }

    /*
     * Get normal messege ib html format
     */
    function generateButtonChatMessage(msg, btn_obj, index) {
        var strArr = [];
        strArr.push("<div id='cm-msg-" + index + "' class=\"chat-msg cm-button-container user\">");
        strArr.push("<span class=\"msg-avatar\">");
        strArr.push("<img src='https://picsum.photos/200'>");
        strArr.push("<\/span>");
        strArr.push("<div class=\"cm-msg-button\">");
        strArr.push("<ul>");
        strArr.push(btn_obj);
        strArr.push("<\/ul>");
        strArr.push("<\/div>");
        strArr.push("<\/div>");

        return strArr.join("");
    }

})
function TurnOnOffSpeaker() {
    const speakerStatus = document.getElementById("speaker-status").value;
    const startSpeaker = document.getElementById('start-speaker');
    const offSpeaker = document.getElementById('off-speaker');

    if (speakerStatus == 0) {
        document.getElementById("speaker-status").value = 1;
        offSpeaker.style = "display:inline;";
        startSpeaker.style = "display:none;";
    }
    else {
        document.getElementById("speaker-status").value = 0;
        startSpeaker.style = "display:inline;";
        offSpeaker.style = "display:none;";
    }
}