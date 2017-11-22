// Copyright (c) Microsoft. All rights reserved.

/*global log*/
/*global updateState*/
/*global sleep*/
/*jslint node: true*/

"use strict";

// Default state
var state = {
    online: true
};


/**
 * Restore the global state using data from the previous iteration.
 *
 * @param previousState The output of main() from the previous iteration
 */
function restoreState(previousState) {
    // If the previous state is null, force a default state
    if (previousState !== undefined && previousState !== null) {
        state = previousState;
    } else {
        log("Using default state");
    }
}

/**
 * Entry point function called by the method.
 *
 * @param context        The context contains current time, device model and id
 * @param previousState  The device state since the last iteration
 */
/*jslint unparam: true*/
function main(context, previousState) {

    // Reboot - devices goes offline and comes online after 20 seconds
    log("Executing turn on/off lightbulb function.");

    restoreState(previousState);

    log("light is currently: " + state.LightOn);

    state.DeviceMethodStatus = "Turning lightbulb on or off...";
    // update the state 
    updateState(state);

    if (state.LightOn == true) {
        state.LightOn = false;
        state.DeviceMethodStatus = "Turned lightbulb off.";
    } else {
        state.LightOn = true;
        state.DeviceMethodStatus = "Turned lightbulb on.";
    }

    // update the state 
    updateState(state);

    log("light is now: " + state.LightOn);

}
