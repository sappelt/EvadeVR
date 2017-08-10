'use strict';

// --------------- Helpers that build all of the responses -----------------------

function buildSpeechletResponse(title, output, repromptText, shouldEndSession) {
    return {
        outputSpeech: {
            type: 'PlainText',
            text: output,
        },
        card: {
            type: 'Simple',
            title: `SessionSpeechlet - ${title}`,
            content: `SessionSpeechlet - ${output}`,
        },
        reprompt: {
            outputSpeech: {
                type: 'PlainText',
                text: repromptText,
            },
        },
        shouldEndSession,
    };
}

function buildSpeechletResponseWithDirectiveNoIntent() {
    console.log("in buildSpeechletResponseWithDirectiveNoIntent");
    return {
        "outputSpeech" : null,
        "card" : null,
        "directives" : [ {
        "type" : "Dialog.Delegate"
        } ],
        "reprompt" : null,
        "shouldEndSession" : false
    }
}

function buildSpeechletResponseDelegate(shouldEndSession) {
    return {
        outputSpeech:null,
        directives: [
                {
                    "type": "Dialog.Delegate",
                    "updatedIntent": null
                }
            ],
        reprompt: null,
        shouldEndSession: shouldEndSession
    }
}

function buildResponse(sessionAttributes, speechletResponse) {
    return {
        version: '1.0',
        sessionAttributes,
        response: speechletResponse,
    };
}

function errorCallback(callback, sessionAttributes, cardTitle) {
    //Return anonymous function that gets called
    return function() {
        callback(sessionAttributes,
            buildSpeechletResponse(cardTitle, "Sorry, I could not connect to the VR application", "", true));
    }
}


// --------------- Functions that control the skill's behavior -----------------------

function getWelcomeResponse(callback) {
    // If we wanted to initialize the session to have some attributes we could add those here.
    const sessionAttributes = {};
    const cardTitle = 'Welcome';
    const speechOutput = 'Welcome to EvadeVR. ' +
        'Please start analyzing your data by saying Start, Resume, ' +
        'Pause, Show Item Details, Jump To Timestep or Show Heatmap.';
    // If the user either does not reply to the welcome message or says something that is not
    // understood, they will be prompted again with this text.
    const repromptText = speechOutput;
    const shouldEndSession = false;

    sendCommand({ 
        "commandType": "Start", 
    }, function() {
        callback(sessionAttributes,
        buildSpeechletResponse(cardTitle, speechOutput, repromptText, shouldEndSession));
    });    
}

function getPauseResponse(intentRequest, session,callback) {
    // If we wanted to initialize the session to have some attributes we could add those here.
    const sessionAttributes = {};
    const cardTitle = 'Pause';
    const speechOutput = 'Simulation pause';
    // If the user either does not reply to the welcome message or says something that is not
    // understood, they will be prompted again with this text.
    const repromptText = speechOutput;
    const shouldEndSession = false;

    sendCommand({ 
        "commandType": "Pause", 
    }, function() {
        callback(sessionAttributes,
        buildSpeechletResponse(cardTitle, speechOutput, repromptText, shouldEndSession));
    });    
}

function getResumeResponse(intentRequest, session,callback) {
    // If we wanted to initialize the session to have some attributes we could add those here.
    const sessionAttributes = {};
    const cardTitle = 'Resume';
    const speechOutput = 'Simulation resumed';
    // If the user either does not reply to the welcome message or says something that is not
    // understood, they will be prompted again with this text.
    const repromptText = speechOutput;
    const shouldEndSession = false;

    sendCommand({ 
        "commandType": "Resume", 
    }, function() {
        callback(sessionAttributes,
        buildSpeechletResponse(cardTitle, speechOutput, repromptText, shouldEndSession));
    });    
}

function getResetResponse(intentRequest, session,callback) {
    // If we wanted to initialize the session to have some attributes we could add those here.
    const sessionAttributes = {};
    const cardTitle = 'Reset';
    const speechOutput = 'Simulation reset';
    // If the user either does not reply to the welcome message or says something that is not
    // understood, they will be prompted again with this text.
    const repromptText = speechOutput;
    const shouldEndSession = false;

    sendCommand({ 
        "commandType": "Reset", 
    }, function() {
        callback(sessionAttributes,
        buildSpeechletResponse(cardTitle, speechOutput, repromptText, shouldEndSession));
    });    
}

function getSwitchCameraResponse(intentRequest, session,callback) {
    // If we wanted to initialize the session to have some attributes we could add those here.
    const sessionAttributes = {};
    const cardTitle = 'SwitchCamera';
    const speechOutput = 'Camera switched';
    // If the user either does not reply to the welcome message or says something that is not
    // understood, they will be prompted again with this text.
    const repromptText = speechOutput;
    const shouldEndSession = false;

    sendCommand({ 
        "commandType": "SwitchCamera", 
    }, function() {
        callback(sessionAttributes,
        buildSpeechletResponse(cardTitle, speechOutput, repromptText, shouldEndSession));
    });    
}

function getToggleHeatmapResponse(intentRequest, session,callback) {
    // If we wanted to initialize the session to have some attributes we could add those here.
    const sessionAttributes = {};
    const cardTitle = 'Toggle heatmap';
    const speechOutput = 'Heatmap toggled';
    // If the user either does not reply to the welcome message or says something that is not
    // understood, they will be prompted again with this text.
    const repromptText = speechOutput;
    const shouldEndSession = false;

    sendCommand({ 
        "commandType": "ToggleHeatmap", 
    }, function() {
        callback(sessionAttributes,
        buildSpeechletResponse(cardTitle, speechOutput, repromptText, shouldEndSession));
    });    
}

function getShowItemListResponse(intentRequest, session,callback) {
    // If we wanted to initialize the session to have some attributes we could add those here.
    const sessionAttributes = {};
    const cardTitle = 'Show item menu';
    const speechOutput = 'Item menu displayed';
    // If the user either does not reply to the welcome message or says something that is not
    // understood, they will be prompted again with this text.
    const repromptText = speechOutput;
    const shouldEndSession = false;

    sendCommand({ 
        "commandType": "ShowItemMenu", 
    }, function() {
        callback(sessionAttributes,
        buildSpeechletResponse(cardTitle, speechOutput, repromptText, shouldEndSession));
    });    
}

function getJumpToTimeDetailsResponse(intentRequest, session, callback) {
    const sessionAttributes = {};
    const cardTitle = 'Jump to timestep';
    const speechOutput = 'You travelled through time and now are at timestep ';
    // If the user either does not reply to the welcome message or says something that is not
    // understood, they will be prompted again with this text.
    const repromptText = 'You could go back- or forwards by saying, travel to step 5';
    const shouldEndSession = false;

    var step = undefined;
    if (intentRequest.intent.slots.Step) {
        step = intentRequest.intent.slots.Step.value;
    }

    if(step !== undefined) {
        sendCommand({ 
            "commandType": "JumpToTime", 
            "step": step
        }, function() {
            callback(sessionAttributes,
            buildSpeechletResponse(cardTitle, speechOutput + step, repromptText, shouldEndSession));
        });    
    } else {
        buildSpeechletResponse(cardTitle, speechOutput, repromptText, shouldEndSession);
    }
}

function getShowItemDetailsResponse(intentRequest, session, callback) {
    const sessionAttributes = {};
    const cardTitle = 'Show Item details';
    const speechOutput = 'You can now see the details of item ';
    // If the user either does not reply to the welcome message or says something that is not
    // understood, they will be prompted again with this text.
    const repromptText = 'You could ask for the details of an item by saying show me item 1 details';
    const shouldEndSession = false;

    var item = undefined;
    if (intentRequest.intent.slots.ItemId) {
        item = intentRequest.intent.slots.ItemId.value;
    }

    if(item !== undefined) {
        sendCommand({ 
            "commandType": "ShowItemDetails", 
            "itemId": item
        }, function() {
            callback(sessionAttributes,
            buildSpeechletResponse(cardTitle, speechOutput + item, repromptText, shouldEndSession));
        });    
    } else {
        buildSpeechletResponse(cardTitle, speechOutput, repromptText, shouldEndSession);
    }
}

function handleSessionEndRequest(callback) {
    const cardTitle = 'Session Ended';
    const speechOutput = 'Thank you for trying EvadeVR!';
    // Setting this to true ends the session and exits the skill.
    const shouldEndSession = true;

    callback({}, buildSpeechletResponse(cardTitle, speechOutput, null, shouldEndSession));
}

function sendCommand(json, callback, errorCallback) {
    var socket = require('socket.io-client')("http://imaps17.us-east-1.elasticbeanstalk.com/")
    var error = false;
    socket.on('connect', function(){
        console.log('connected to chat server');
        
        socket.emit('command', json); 
        
        socket.disconnect();
    });

    socket.io.on('connect_error', function(err) {

        error = true;
        // handle server error here
        console.log('Error connecting to chat server');

        if(errorCallback)
            errorCallback();
    });

    if(!error)
        callback();
}

// --------------- Events -----------------------

/**
 * Called when the session starts.
 */
function onSessionStarted(sessionStartedRequest, session) {
    console.log(`onSessionStarted requestId=${sessionStartedRequest.requestId}, sessionId=${session.sessionId}`);
}

/**
 * Called when the user launches the skill without specifying what they want.
 */
function onLaunch(launchRequest, session, callback) {
    console.log(`onLaunch requestId=${launchRequest.requestId}, sessionId=${session.sessionId}`);

    // Dispatch to your skill's launch.
    getWelcomeResponse(callback);
}

/**
 * Called when the user specifies an intent for this skill.
 */
function onIntent(intentRequest, session, callback) {
    console.log(`onIntent requestId=${intentRequest.requestId}, sessionId=${session.sessionId}`);

    const intent = intentRequest.intent;
    const intentName = intentRequest.intent.name;

    // Dispatch to your skill's intent handlers
    if (intentName === 'Pause') {
        getPauseResponse(intentRequest, session, callback);
    }
    else if (intentName === 'Reset') {
        getResetResponse(intentRequest, session, callback);
    }
    else if (intentName === 'Resume') {
        getResumeResponse(intentRequest, session, callback);
    }
    else if (intentName === 'SwitchCamera') {
        getSwitchCameraResponse(intentRequest, session, callback);
    }
    else if (intentName === 'ShowHeatmap' || intentName === 'HideHeatmap') {
        getToggleHeatmapResponse(intentRequest, session, callback);
    }
    else if (intentName === 'JumpToTime') {
        getJumpToTimeDetailsResponse(intentRequest, session, callback);
    }
    else if(intentName === "ShowItemDetails") {
        getShowItemDetailsResponse(intentRequest, session, callback);
    }
    else if(intentName === "ShowItemList") {
        getShowItemListResponse(intentRequest, session, callback);
    }
    else if (intentName === 'AMAZON.HelpIntent') {
        getWelcomeResponse(callback);
    } else if (intentName === 'AMAZON.StopIntent' || intentName === 'AMAZON.CancelIntent') {
        handleSessionEndRequest(callback);
    } else {
        throw new Error('Invalid intent');
    }
}

/**
 * Called when the user ends the session.
 * Is not called when the skill returns shouldEndSession=true.
 */
function onSessionEnded(sessionEndedRequest, session) {
    console.log(`onSessionEnded requestId=${sessionEndedRequest.requestId}, sessionId=${session.sessionId}`);
    // Add cleanup logic here
}


// --------------- Main handler -----------------------

// Route the incoming request based on type (LaunchRequest, IntentRequest,
// etc.) The JSON body of the request is provided in the event parameter.
exports.handler = (event, context, callback) => {
    try {
        console.log(`event.session.application.applicationId=${event.session.application.applicationId}`);

        /**
         * Uncomment this if statement and populate with your skill's application ID to
         * prevent someone else from configuring a skill that sends requests to this function.
         */
        /*
        if (event.session.application.applicationId !== 'amzn1.echo-sdk-ams.app.[unique-value-here]') {
             callback('Invalid Application ID');
        }
        */

        if (event.session.new) {
            onSessionStarted({ requestId: event.request.requestId }, event.session);
        }

        if (event.request.type === 'LaunchRequest') {
            onLaunch(event.request,
                event.session,
                (sessionAttributes, speechletResponse) => {
                    callback(null, buildResponse(sessionAttributes, speechletResponse));
                });
        } else if (event.request.type === 'IntentRequest') {
            onIntent(event.request,
                event.session,
                (sessionAttributes, speechletResponse) => {
                    callback(null, buildResponse(sessionAttributes, speechletResponse));
                });
        } else if (event.request.type === 'SessionEndedRequest') {
            onSessionEnded(event.request, event.session);
            callback();
        }
    } catch (err) {
        callback(err);
    }
};
