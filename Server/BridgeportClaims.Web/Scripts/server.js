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
            broadCastMessage: function (msgFrom, msg) {
            /// <summary>Calls the BroadCastMessage method on the server-side DocumentsHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"msgFrom\" type=\"String\">Server side type is System.String</param>
            /// <param name=\"msg\" type=\"String\">Server side type is System.String</param>
                return proxies['DocumentsHub'].invoke.apply(proxies['DocumentsHub'], $.merge(["BroadCastMessage"], $.makeArray(arguments)));
             },

            sendMonitoringData: function (eventType, connectionId) {
            /// <summary>Calls the SendMonitoringData method on the server-side DocumentsHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"eventType\" type=\"String\">Server side type is System.String</param>
            /// <param name=\"connectionId\" type=\"String\">Server side type is System.String</param>
                return proxies['DocumentsHub'].invoke.apply(proxies['DocumentsHub'], $.merge(["SendMonitoringData"], $.makeArray(arguments)));
             },

            sendNewDocument: function (documentId, fileName, fileSize, created, lastAccess, lastWrite) {
            /// <summary>Calls the SendNewDocument method on the server-side DocumentsHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"documentId\" type=\"Number\">Server side type is System.Int32</param>
            /// <param name=\"fileName\" type=\"String\">Server side type is System.String</param>
            /// <param name=\"fileSize\" type=\"String\">Server side type is System.String</param>
            /// <param name=\"created\" type=\"Object\">Server side type is System.DateTime</param>
            /// <param name=\"lastAccess\" type=\"Object\">Server side type is System.DateTime</param>
            /// <param name=\"lastWrite\" type=\"Object\">Server side type is System.DateTime</param>
                return proxies['DocumentsHub'].invoke.apply(proxies['DocumentsHub'], $.merge(["SendNewDocument"], $.makeArray(arguments)));
             }
        };

        proxies['MonitorHub'] = this.createHubProxy('MonitorHub'); 
        proxies['MonitorHub'].client = { };
        proxies['MonitorHub'].server = {
        };

        return proxies;
    };

    signalR.hub = $.hubConnection("https://localhost:44398/", { useDefaultPath: false });
    $.extend(signalR, signalR.hub.createHubProxies());

}(window.jQuery, window));