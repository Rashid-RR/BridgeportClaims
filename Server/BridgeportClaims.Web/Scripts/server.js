/*!
 * ASP.NET SignalR JavaScript Library v2.2.2
 * http://signalr.net/
 *
 * Copyright (c) .NET Foundation. All rights reserved.
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 *
 */

/// <reference path="..\..\SignalR.Client.JS\Scripts\jquery-1.6.4.js" />
/// <reference path="jquery.signalR.js" />
(function ($, window, undefined) {
    /// <param name="$" type="jQuery" />
    "use strict";

    if (typeof ($.signalR) !== "function") {
        throw new Error("SignalR: SignalR is not loaded. Please ensure jquery.signalR-x.js is referenced before ~/signalr/js.");
    }

    var signalR = $.signalR;

    function makeProxyCallback(hub, callback) {
        return function () {
            // Call the client hub method
            callback.apply(hub, $.makeArray(arguments));
        };
    }

    function registerHubProxies(instance, shouldSubscribe) {
        var key, hub, memberKey, memberValue, subscriptionMethod;

        for (key in instance) {
            if (instance.hasOwnProperty(key)) {
                hub = instance[key];

                if (!(hub.hubName)) {
                    // Not a client hub
                    continue;
                }

                if (shouldSubscribe) {
                    // We want to subscribe to the hub events
                    subscriptionMethod = hub.on;
                } else {
                    // We want to unsubscribe from the hub events
                    subscriptionMethod = hub.off;
                }

                // Loop through all members on the hub and find client hub functions to subscribe/unsubscribe
                for (memberKey in hub.client) {
                    if (hub.client.hasOwnProperty(memberKey)) {
                        memberValue = hub.client[memberKey];

                        if (!$.isFunction(memberValue)) {
                            // Not a client hub function
                            continue;
                        }

                        subscriptionMethod.call(hub, memberKey, makeProxyCallback(hub, memberValue));
                    }
                }
            }
        }
    }

    $.hubConnection.prototype.createHubProxies = function () {
        var proxies = {};
        this.starting(function () {
            // Register the hub proxies as subscribed
            // (instance, shouldSubscribe)
            registerHubProxies(proxies, true);

            this._registerSubscribedHubs();
        }).disconnected(function () {
            // Unsubscribe all hub proxies when we "disconnect".  This is to ensure that we do not re-add functional call backs.
            // (instance, shouldSubscribe)
            registerHubProxies(proxies, false);
        });

        proxies['DocumentsHub'] = this.createHubProxy('DocumentsHub'); 
        proxies['DocumentsHub'].client = { };
        proxies['DocumentsHub'].server = {
            broadCastExternalMessage: function (msgFrom, msg) {
            /// <summary>Calls the BroadCastExternalMessage method on the server-side DocumentsHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"msgFrom\" type=\"String\">Server side type is System.String</param>
            /// <param name=\"msg\" type=\"String\">Server side type is System.String</param>
                return proxies['DocumentsHub'].invoke.apply(proxies['DocumentsHub'], $.merge(["BroadCastExternalMessage"], $.makeArray(arguments)));
             },

            broadCastMessage: function (msgFrom, msg) {
            /// <summary>Calls the BroadCastMessage method on the server-side DocumentsHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"msgFrom\" type=\"String\">Server side type is System.String</param>
            /// <param name=\"msg\" type=\"String\">Server side type is System.String</param>
                return proxies['DocumentsHub'].invoke.apply(proxies['DocumentsHub'], $.merge(["BroadCastMessage"], $.makeArray(arguments)));
             },

            getDocuments: function (model) {
            /// <summary>Calls the GetDocuments method on the server-side DocumentsHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"model\" type=\"Object\">Server side type is BridgeportClaims.Web.Models.DocumentViewModel</param>
                return proxies['DocumentsHub'].invoke.apply(proxies['DocumentsHub'], $.merge(["GetDocuments"], $.makeArray(arguments)));
             },

            joinRoom: function (room) {
            /// <summary>Calls the JoinRoom method on the server-side DocumentsHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"room\" type=\"String\">Server side type is System.String</param>
                return proxies['DocumentsHub'].invoke.apply(proxies['DocumentsHub'], $.merge(["JoinRoom"], $.makeArray(arguments)));
             },

            sendMessageToRoom: function (room, message) {
            /// <summary>Calls the SendMessageToRoom method on the server-side DocumentsHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"room\" type=\"String\">Server side type is System.String</param>
            /// <param name=\"message\" type=\"String\">Server side type is System.String</param>
                return proxies['DocumentsHub'].invoke.apply(proxies['DocumentsHub'], $.merge(["SendMessageToRoom"], $.makeArray(arguments)));
             },

            sendMonitoringData: function (eventType, connectionId) {
            /// <summary>Calls the SendMonitoringData method on the server-side DocumentsHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"eventType\" type=\"String\">Server side type is System.String</param>
            /// <param name=\"connectionId\" type=\"String\">Server side type is System.String</param>
                return proxies['DocumentsHub'].invoke.apply(proxies['DocumentsHub'], $.merge(["SendMonitoringData"], $.makeArray(arguments)));
             }
        };

        proxies['MonitorHub'] = this.createHubProxy('MonitorHub'); 
        proxies['MonitorHub'].client = { };
        proxies['MonitorHub'].server = {
            hello: function () {
            /// <summary>Calls the Hello method on the server-side MonitorHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
                return proxies['MonitorHub'].invoke.apply(proxies['MonitorHub'], $.merge(["Hello"], $.makeArray(arguments)));
             }
        };

        return proxies;
    };

    signalR.hub = $.hubConnection("https://localhost:44398/", { useDefaultPath: false });
    $.extend(signalR, signalR.hub.createHubProxies());

}(window.jQuery, window));