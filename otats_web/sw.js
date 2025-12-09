import { } from "https://www.gstatic.com/firebasejs/9.18.0/firebase-messaging.js"
var comm_url = "https://request.pushalert.co/";
var default_title = "";
var default_message = "";
var default_icon = "https://cdn.pushalert.co/icons/default_icon-53554.png";
var default_url = "https://ota.lscloud.vn";
var last_updated = "1677313984";
var client_id = 53554;
var domain_id = 1;
var pa_subdomain = "https://ota186.pushalert.co";
var appPublicKey = 'BLdd7O4bhTEv2PAyc8ynU8tzz2U5D+Y8jIAmfsU6UawbpA4kiLn1SsVWT47z7yUC+UJnwOZjCoPoVaSyTLFQTT4=';
const CACHE_NAME = 'cool-cache';

// Add whichever assets you want to pre-cache here:
const PRECACHE_ASSETS = [
    '/assets/',
    '/Content/',
    '/images/',
	'/Scripts/'
]

firebase.initializeApp({
    apiKey: 'api-key',
    authDomain: 'project-id.firebaseapp.com',
    databaseURL: 'https://project-id.firebaseio.com',
    projectId: 'project-id',
    storageBucket: 'project-id.appspot.com',
    messagingSenderId: 'sender-id',
    appId: 'app-id',
    measurementId: 'G-measurement-id',
});
const messaging = firebase.messaging();
messaging.onBackgroundMessage(function (payload) {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);
    // Customize notification here
    const notificationTitle = 'Background Message Title';
    const notificationOptions = {
        body: 'Background Message body.',
        icon: '/firebase-logo.png'
    };

    self.registration.showNotification(notificationTitle,
        notificationOptions);
});


function getBrowserInfo() {
    return function (i, e) {
        "undefined" != typeof module && module.exports ? module.exports = e() : "function" == typeof define && define.amd ? define(e) : this.browser_info = e()
    }(0, function () {
        function i(i) {
            function o(e) {
                var o = i.match(e);
                return o && o.length > 1 && o[1] || ""
            }
            var n, t = o(/(ipod|iphone|ipad)/i).toLowerCase(), r = !/like android/i.test(i) && /android/i.test(i), a = /CrOS/.test(i), s = /silk/i.test(i), d = /sailfish/i.test(i), c = /tizen/i.test(i), m = /(web|hpw)os/i.test(i), f = /windows phone/i.test(i), l = !f && /windows/i.test(i), p = !t && !s && /macintosh/i.test(i), u = !r && !d && !c && !m && /linux/i.test(i), w = o(/edge\/(\d+(\.\d+)?)/i), h = o(/version\/(\d+(\.\d+)?)/i), b = /tablet/i.test(i) || /android/i.test(i) && !/mobile/i.test(i), v = !b && /[^-]mobi/i.test(i);
            /opera mini/i.test(i) ? (n = {
                name: "Opera Mini",
                operamini: e,
                majorVersion: o(/(?:opera mini)[\s\/](\d+(\.\d+)?)/i) || h,
                version: o(/(?:opera mini)\/([\d\.]+)/i)
            },
                v = e,
                b = !1) : /opera|opr/i.test(i) ? n = {
                    name: "Opera",
                    opera: e,
                    majorVersion: h || o(/(?:opera|opr)[\s\/](\d+(\.\d+)?)/i),
                    version: o(/(?:opera|opr)\/([\d\.]+)/i)
                } : /ucbrowser/i.test(i) ? n = {
                    name: "UC Browser",
                    ucbrowser: e,
                    majorVersion: o(/(?:ucbrowser)[\s\/](\d+(\.\d+)?)/i) || h,
                    version: o(/(?:ucbrowser)\/([\d\.]+)/i)
                } : /acheetahi/i.test(i) ? n = {
                    name: "CM Browser",
                    cmbrowser: e,
                    majorVersion: o(/(?:acheetahi)[\s\/](\d+(\.\d+)?)/i) || h,
                    version: o(/(?:acheetahi)\/([\d\.]+)/i)
                } : /yabrowser/i.test(i) ? n = {
                    name: "Yandex Browser",
                    yandexbrowser: e,
                    version: h || o(/(?:yabrowser)[\s\/](\d+(\.\d+)?)/i)
                } : f ? (n = {
                    name: "Windows Phone",
                    windowsphone: e
                },
                    w ? (n.msedge = e,
                        n.version = w) : (n.msie = e,
                            n.version = o(/iemobile\/(\d+(\.\d+)?)/i))) : /msie|trident/i.test(i) ? n = {
                                name: "Internet Explorer",
                                msie: e,
                                version: o(/(?:msie |rv:)([\.\d]+)/i),
                                majorVersion: o(/(?:msie |rv:)(\d+(\.\d+)?)/i)
                            } : a ? n = {
                                name: "Chrome",
                                chromeos: e,
                                chromeBook: e,
                                chrome: e,
                                version: o(/(?:chrome|crios|crmo)\/(\d+(\.\d+)?)/i)
                            } : /chrome.+? edge/i.test(i) ? n = {
                                name: "Microsoft Edge",
                                msedge: e,
                                version: w,
                                majorVersion: o(/(?:edge)\/(\d+(\.\d+)?)/i)
                            } : /chrome|crios|crmo/i.test(i) ? n = {
                                name: "Chrome",
                                chrome: e,
                                version: o(/(?:chrome|crios|crmo)\/([\d\.]+)/i),
                                majorVersion: o(/(?:chrome|crios|crmo)\/(\d+(\.\d+)?)/i)
                            } : t ? (n = {
                                name: "iphone" == t ? "iPhone" : "ipad" == t ? "iPad" : "iPod"
                            },
                                h && (n.version = h)) : d ? n = {
                                    name: "Sailfish",
                                    sailfish: e,
                                    version: o(/sailfish\s?browser\/(\d+(\.\d+)?)/i)
                                } : /seamonkey\//i.test(i) ? n = {
                                    name: "SeaMonkey",
                                    seamonkey: e,
                                    version: o(/seamonkey\/(\d+(\.\d+)?)/i)
                                } : /firefox|iceweasel/i.test(i) ? (n = {
                                    name: "Firefox",
                                    firefox: e,
                                    version: o(/(?:firefox|iceweasel)[ \/]([\d\.]+)/i),
                                    majorVersion: o(/(?:firefox|iceweasel)[ \/](\d+(\.\d+)?)/i)
                                },
                                    /\((mobile|tablet);[^\)]*rv:[\d\.]+\)/i.test(i) && (n.firefoxos = e)) : s ? n = {
                                        name: "Amazon Silk",
                                        silk: e,
                                        version: o(/silk\/(\d+(\.\d+)?)/i)
                                    } : r ? n = {
                                        name: "Android",
                                        version: h
                                    } : /phantom/i.test(i) ? n = {
                                        name: "PhantomJS",
                                        phantom: e,
                                        version: o(/phantomjs\/(\d+(\.\d+)?)/i)
                                    } : /blackberry|\bbb\d+/i.test(i) || /rim\stablet/i.test(i) ? n = {
                                        name: "BlackBerry",
                                        blackberry: e,
                                        version: h || o(/blackberry[\d]+\/(\d+(\.\d+)?)/i)
                                    } : m ? (n = {
                                        name: "WebOS",
                                        webos: e,
                                        version: h || o(/w(?:eb)?osbrowser\/(\d+(\.\d+)?)/i)
                                    },
                                        /touchpad\//i.test(i) && (n.touchpad = e)) : n = /bada/i.test(i) ? {
                                            name: "Bada",
                                            bada: e,
                                            version: o(/dolfin\/(\d+(\.\d+)?)/i)
                                        } : c ? {
                                            name: "Tizen",
                                            tizen: e,
                                            version: o(/(?:tizen\s?)?browser\/(\d+(\.\d+)?)/i) || h
                                        } : /safari/i.test(i) ? {
                                            name: "Safari",
                                            safari: e,
                                            version: h
                                        } : {
                                            name: o(/^(.*)\/(.*) /),
                                            version: function (e) {
                                                var o = i.match(e);
                                                return o && o.length > 1 && o[2] || ""
                                            }(/^(.*)\/(.*) /)
                                        },
                !n.msedge && /(apple)?webkit/i.test(i) ? (n.name = n.name || "Webkit",
                    n.webkit = e,
                    !n.version && h && (n.version = h)) : !n.opera && /gecko\//i.test(i) && (n.name = n.name || "Gecko",
                        n.gecko = e,
                        n.version = n.version || o(/gecko\/(\d+(\.\d+)?)/i)),
                n.msedge || !r && !n.silk ? t ? (n[t] = e,
                    n.ios = e) : l ? n.windows = e : p ? n.mac = e : u && (n.linux = e) : n.android = e;
            var g = "";
            n.windowsphone ? g = o(/windows phone (?:os)?\s?(\d+(\.\d+)*)/i) : t ? (g = o(/os (\d+([_\s]\d+)*) like mac os x/i),
                g = g.replace(/[_\s]/g, ".")) : r ? g = o(/android[ \/-](\d+(\.\d+)*)/i) : n.webos ? g = o(/(?:web|hpw)os\/(\d+(\.\d+)*)/i) : n.blackberry ? g = o(/rim\stablet\sos\s(\d+(\.\d+)*)/i) : n.bada ? g = o(/bada\/(\d+(\.\d+)*)/i) : n.tizen ? g = o(/tizen[\/\s](\d+(\.\d+)*)/i) : n.windows ? g = o(/windows nt[\/\s](\d+(\.\d+)*)/i) : n.mac && (g = o(/mac os x[\/\s](\d+(_\d+)*)/i)),
                g && (n.osversion = g);
            var _ = g.split(".")[0];
            return b || "ipad" == t || r && (3 == _ || 4 == _ && !v) || n.silk ? n.tablet = e : (v || "iphone" == t || "ipod" == t || r || n.blackberry || n.webos || n.bada) && (n.mobile = e),
                n.msedge || n.msie && n.version >= 10 || n.yandexbrowser && n.version >= 15 || n.chrome && n.version >= 20 || n.firefox && n.version >= 20 || n.safari && n.version >= 6 || n.opera && n.version >= 10 || n.ios && n.osversion && n.osversion.split(".")[0] >= 6 || n.blackberry && n.version >= 10.1 ? n.a = e : n.msie && n.version < 10 || n.chrome && n.version < 20 || n.firefox && n.version < 20 || n.safari && n.version < 6 || n.opera && n.version < 10 || n.ios && n.osversion && n.osversion.split(".")[0] < 6 ? n.c = e : n.x = e,
                n
        }
        var e = !0
            , o = i("undefined" != typeof navigator ? navigator.userAgent : "");
        o.test = function (i) {
            for (var e = 0; e < i.length; ++e) {
                var n = i[e];
                if ("string" == typeof n && n in o)
                    return !0
            }
            return !1
        }
            ,
            o._detect = i;
        var n = {};
        return o.mobile ? n.type = "mobile" : o.tablet ? n.type = "tablet" : n.type = "desktop",
            o.android ? n.os = "android" : o.ios ? n.os = "ios" : o.windows ? n.os = "windows" : o.mac ? n.os = "mac" : o.linux ? n.os = "linux" : o.windowsphone ? n.os = "windowsphone" : o.webos ? n.os = "webos" : o.blackberry ? n.os = "blackberry" : o.bada ? n.os = "bada" : o.tizen ? n.os = "tizen" : o.sailfish ? n.os = "sailfish" : o.firefoxos ? n.os = "firefoxos" : o.chromeos ? n.os = "chromeos" : n.os = "unknown",
            o.osversion && (n.osVer = o.osversion),
            o.chrome ? n.browser = "chrome" : o.firefox ? n.browser = "firefox" : o.opera ? n.browser = "opera" : o.operamini ? n.browser = "operamini" : o.ucbrowser ? n.browser = "ucbrowser" : o.cmbrowser ? n.browser = "cmbrowser" : o.safari || o.iosdevice && ("ipad" == o.iosdevice || "ipod" == o.iosdevice || "iphone" == o.iosdevice) ? n.browser = "safari" : o.msie ? n.browser = "ie" : o.yandexbrowser ? n.browser = "yandexbrowser" : o.msedge ? n.browser = "edge" : o.seamonkey ? n.browser = "seamonkey" : o.blackberry ? n.browser = "blackberry" : o.touchpad ? n.browser = "touchpad" : o.silk ? n.browser = "silk" : n.browser = "unknown",
            o.version && (n.browserVer = o.version),
            o.majorVersion && (n.browserMajor = o.majorVersion),
            n.language = navigator.language || "",
            n.engine = navigator.product || "",
            n.userAgent = navigator.userAgent,
            n
    }),
        browser_info
}
var last_url_id = -1
    , endpoint = ""
    , endpoint_full = "";
if (void 0 === domain_id)
    var domain_id = 1;

self.addEventListener('install', event => {
    event.waitUntil((async () => {
        const cache = await caches.open(CACHE_NAME);
        cache.addAll(PRECACHE_ASSETS);
    })());
}),
self.addEventListener("push", function (i) {
        i.waitUntil(self.registration.pushManager.getSubscription().then(function (i) {
            endpoint = i.endpoint.slice(i.endpoint.lastIndexOf("/") + 1);
            endpoint_full = i.endpoint;
            var e = comm_url + "getNotification.php?t=" + (new Date).getTime() + "&endpoint=" + encodeURIComponent(endpoint) + "&client_id=" + client_id + "&domain_id=" + domain_id + "&endpoint_full=" + encodeURIComponent(endpoint_full);
            return fetch(e).then(function (i) {
                if (200 !== i.status)
                    throw console.log("Looks like there was a problem. Status Code: " + i.status),
                    new Error("Looks like there was a problem. Status Code: " + i.status);
                return i.json().then(function (i) {
                    var e = /firefox/i.test(navigator.userAgent)
                        , o = 0;
                    if (e && (o = Math.floor(1e3 * Math.random() + 1)),
                        !e) {
                        if (i.error || !i.notification)
                            throw new Error("Data not received: " + JSON.stringify(i));
                        var n = i.notification.title
                            , t = i.notification.message
                            , r = i.notification.icon
                            , a = i.notification.badge_icon
                            , s = i.notification.url
                            , d = i.notification.uid
                            , c = i.notification.eid
                            , m = i.notification.type
                            , f = i.notification.url_id
                            , l = ""
                            , p = i.notification.tag
                            , u = !0;
                        i.notification.hasOwnProperty("requireInteraction") && !1 === i.notification.requireInteraction && (u = !1);
                        var w = []
                            , h = [];
                        return h.url = s,
                            h.url_id = f,
                            h.uid = d,
                            h.eid = c,
                            h.type = m,
                            void 0 !== i.notification.action1 && (w[0] = {
                                action: "action1",
                                title: i.notification.action1.title
                            },
                                h.action1 = i.notification.action1.url),
                            void 0 !== i.notification.action2 && (w[1] = {
                                action: "action2",
                                title: i.notification.action2.title
                            },
                                h.action2 = i.notification.action2.url),
                            void 0 !== i.notification.image && (l = i.notification.image),
                            self.registration.showNotification(n, {
                                body: t,
                                icon: r,
                                image: l,
                                badge: a,
                                tag: p,
                                requireInteraction: u,
                                data: h,
                                actions: w
                            })
                    }
                    setTimeout(function () {
                        if (i.error || !i.notification)
                            throw new Error("Data not received: " + JSON.stringify(i));
                        var e = i.notification.title
                            , o = i.notification.message
                            , n = i.notification.icon
                            , t = i.notification.url
                            , r = i.notification.uid
                            , a = i.notification.eid
                            , s = i.notification.type
                            , d = i.notification.url_id
                            , c = i.notification.tag
                            , m = !0;
                        return i.notification.hasOwnProperty("requireInteraction") && !1 === i.notification.requireInteraction && (m = !1),
                            self.registration.showNotification(e, {
                                body: o,
                                icon: n,
                                tag: c,
                                requireInteraction: m,
                                data: {
                                    url: t,
                                    url_id: d,
                                    uid: r,
                                    eid: a,
                                    type: s
                                }
                            })
                    }, o)
                })
            }).catch(function (i) {
                fetch(comm_url + "error.php?txt=" + encodeURIComponent(i.toString()) + "&endpoint=" + encodeURIComponent(endpoint) + "&client_id=" + client_id + "&domain_id=" + domain_id + "&endpoint_full=" + encodeURIComponent(endpoint_full)),
                    fetch("https://push.pushalert.co/error.php?txt=" + encodeURIComponent(i.toString()) + "&endpoint=" + encodeURIComponent(endpoint) + "&client_id=" + client_id + "&domain_id=" + domain_id + "&endpoint_full=" + encodeURIComponent(endpoint_full), {
                        mode: "no-cors"
                    })
            })
        }).catch(function (i) {
            fetch(comm_url + "error.php?txt=" + encodeURIComponent(i.toString()) + "&endpoint=" + encodeURIComponent(endpoint) + "&client_id=" + client_id + "&domain_id=" + domain_id + "&endpoint_full=" + encodeURIComponent(endpoint_full)),
                fetch("https://push.pushalert.co/error.php?txt=" + encodeURIComponent(i.toString()) + "&endpoint=" + encodeURIComponent(endpoint) + "&client_id=" + client_id + "&domain_id=" + domain_id + "&endpoint_full=" + encodeURIComponent(endpoint_full), {
                    mode: "no-cors"
                })
        }))
    }),
self.addEventListener("notificationclose", function (i) {
        var e = i.notification.data.url_id
            , o = i.notification.data.uid;
        last_url_id !== e && fetch(comm_url + "notfClosed.php?url_id=" + e + "&uid=" + o + "&client_id=" + client_id + "&domain_id=" + domain_id)
    }),
self.addEventListener("notificationclick", function (i) {
        var e = i.notification.data.url_id
            , o = i.notification.data.uid
            , n = i.notification.data.eid
            , t = i.notification.data.type;
        last_url_id = e,
            i.notification.close();
        var r = i.notification.data.url
            , a = 0;
        if ("action1" === i.action ? (r = i.notification.data.action1,
            a = 1) : "action2" === i.action && (r = i.notification.data.action2,
                a = 2),
            0 == o || 0 == o)
            ;
        else {
            var s = []
                , d = getBrowserInfo();
            for (key in d)
                s.push(key + "=" + encodeURIComponent(d[key]));
            s = s.join("&"),
                fetch(comm_url + "trackClicked.php?url_id=" + e + "&uid=" + o + "&" + s + "&client_id=" + client_id + "&domain_id=" + domain_id + "&clicked_on=" + a + "&eid=" + n + "&type=" + t).then(function (i) {
                    if (200 !== i.status)
                        throw console.log("Looks like there was a problem. Status Code: " + i.status),
                        new Error
                }).catch(function (i) { })
        }
        i.waitUntil(clients.matchAll({
            type: "window"
        }).then(function (i) {
            for (var e = 0; e < i.length; e++) {
                var o = i[e];
                if (o.url === r && "focus" in o)
                    return o.focus()
            }
            if (clients.openWindow)
                return clients.openWindow(r)
        }))
    });
self.addEventListener('activate', function (a) {
    a.waitUntil(clients.claim())
});
const WorkerMessengerCommand = {
    AMP_SUBSCRIPION_STATE: 'amp-web-push-subscription-state',
    AMP_SUBSCRIBE: 'amp-web-push-subscribe',
    AMP_UNSUBSCRIBE: 'amp-web-push-unsubscribe'
};
self.addEventListener("fetch", function (i) { });
self.addEventListener('message', a => {
    const { command: b } = a.data;
    b === WorkerMessengerCommand.AMP_SUBSCRIPION_STATE ? onMessageReceivedSubscriptionState() : b === WorkerMessengerCommand.AMP_SUBSCRIBE ? onMessageReceivedSubscribe() : b === WorkerMessengerCommand.AMP_UNSUBSCRIBE ? onMessageReceivedUnsubscribe() : void 0
}
);
function onMessageReceivedSubscriptionState() {
    let a = null;
    self.registration.pushManager.getSubscription().then(b => {
        return a = b,
            b ? self.registration.pushManager.permissionState(b.options) : null
    }
    ).then(b => {
        if (null == b)
            broadcastReply(WorkerMessengerCommand.AMP_SUBSCRIPION_STATE, !1);
        else {
            const c = !!a && 'granted' === b;
            broadcastReply(WorkerMessengerCommand.AMP_SUBSCRIPION_STATE, c)
        }
    }
    )
}
function onMessageReceivedSubscribe() {
    self.registration.pushManager.subscribe({
        userVisibleOnly: !0,
        applicationServerKey: urlBase64ToUint8Array(appPublicKey)
    }).then(function (a) {
        broadcastReply(WorkerMessengerCommand.AMP_SUBSCRIBE, null),
            sendSub(a)
    })
}
function onMessageReceivedUnsubscribe() {
    self.registration.pushManager.getSubscription().then(function (a) {
        a && a.unsubscribe().then(function () {
            broadcastReply(WorkerMessengerCommand.AMP_UNSUBSCRIBE, null),
                unsubscribe(a)
        })
    })
}
function broadcastReply(a, b) {
    self.clients.matchAll().then(c => {
        for (let d = 0; d < c.length; d++) {
            const f = c[d];
            f.postMessage({
                command: a,
                payload: b
            })
        }
    }
    )
}
function urlBase64ToUint8Array(a) {
    const b = '='.repeat((4 - a.length % 4) % 4)
        , c = (a + b).replace(/\-/g, '+').replace(/_/g, '/')
        , d = self.atob(c)
        , f = new Uint8Array(d.length);
    for (let g = 0; g < d.length; ++g)
        f[g] = d.charCodeAt(g);
    return f
}
function sendSub(a) {
    var b = []
        , c = getBrowserInfo()
        , d = c.browser;
    for (key in c)
        b.push(key + '=' + encodeURIComponent(c[key]));
    b = b.join('&'),
        b = b + '&pa_id=' + client_id + '&domain_id=' + domain_id,
        last_indexof_slash = a.endpoint.lastIndexOf('/'),
        b = b + '&endpoint_url=' + encodeURIComponent(a.endpoint.substr(0, last_indexof_slash)),
        b = b + '&subs_info=' + encodeURIComponent(JSON.stringify(a));
    var f;
    if ('edge' === d) {
        var g = new URL(a.endpoint);
        f = g.searchParams.get('token')
    } else
        f = a.endpoint.slice(last_indexof_slash + 1);
    b = b + '&endpoint=' + encodeURIComponent(f),
        b += '&amp=1',
        fetch(pa_subdomain + '/subscribe/' + f, {
            method: 'post',
            headers: {
                'Content-type': 'application/x-www-form-urlencoded; charset=UTF-8'
            },
            body: b
        }).then(function (h) {
            h.json().then(function (j) {
                if (j.welcome_enable) {
                    var k = !0;
                    j.hasOwnProperty('welcome_req_interaction') && !1 === j.welcome_req_interaction && (k = !1);
                    var l = []
                        , m = [];
                    m.url = j.welcome_url,
                        m.url_id = 0,
                        m.uid = 0,
                        'undefined' != typeof j.action1_title && (l[0] = {
                            action: 'action1',
                            title: j.action1_title
                        },
                            m.action1 = j.action1_url),
                        'undefined' != typeof j.action2_title && (l[1] = {
                            action: 'action2',
                            title: j.action2_title
                        },
                            m.action2 = j.action2_url),
                        self.registration.showNotification(j.welcome_title, {
                            body: j.welcome_msg,
                            icon: j.welcome_icon,
                            requireInteraction: k,
                            data: m,
                            actions: l
                        })
                }
            }).catch(function (j) {
                console.log('Error sending subscription to server:' + j.toString())
            })
        })
}
function unsubscribe(a) {
    var b = getBrowserInfo(), c = b.browser, d;
    if ('edge' === c) {
        var f = new URL(a.endpoint);
        d = f.searchParams.get('token')
    } else
        d = a.endpoint.slice(a.endpoint.lastIndexOf('/') + 1);
    fetch(pa_subdomain + '/unsubscribe/' + d, {
        method: 'post',
        headers: {
            'Content-type': 'application/x-www-form-urlencoded; charset=UTF-8'
        },
        body: 'pa_id=' + client_id + '&domain_id=' + domain_id + '&endpoint=' + encodeURIComponent(d)
    }).then(function (g) {
        g.json().then(function () { }).catch(function (h) {
            console.error('Error sending subscription to server:', h)
        })
    })
}
