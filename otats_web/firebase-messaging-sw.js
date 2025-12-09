// Import the functions you need from the SDKs you need
//import { initializeApp } from "https://www.gstatic.com/firebasejs/9.18.0/firebase-app.js";
//import { getAnalytics } from "https://www.gstatic.com/firebasejs/9.18.0/firebase-analytics.js";
importScripts('https://www.gstatic.com/firebasejs/9.2.0/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/9.2.0/firebase-messaging-compat.js');
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries
const CACHE_NAME = 'cool-cache';

// Add whichever assets you want to pre-cache here:
const PRECACHE_ASSETS = [
    '/assets/',
    '/Content/',
    '/images/',
    '/Scripts/'
]
// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional

self.addEventListener('install', event => {
    event.waitUntil((async () => {
        const cache = await caches.open(CACHE_NAME);
        cache.addAll(PRECACHE_ASSETS);
    })());
})

const firebaseConfig = {
    apiKey: "AIzaSyBR6M1t6FQ4WUHLL-aoQ2lNqRNTl-rsUOI",
    authDomain: "lscloud-9e8a4.firebaseapp.com",
    projectId: "lscloud-9e8a4",
    storageBucket: "lscloud-9e8a4.appspot.com",
    messagingSenderId: "890857056215",
    appId: "1:890857056215:web:53cd2c55e637af1e0a0d92",
    measurementId: "G-L0ZZ0CLGWB"
};
firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();

self.addEventListener('push', function (event) {

    console.log("event:push")
    let messageTitle = "MESSAGETITLE"
    let messageBody = "MESSAGEBODY"
    let messageTag = "MESSAGETAG"

    const notificationPromise = self.registration.showNotification(
        messageTitle,
        {
            body: messageBody,
            tag: messageTag
        });

    event.waitUntil(notificationPromise);

}, false);

// If the web application is in the background, setBackGroundMessageHandler is called.
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