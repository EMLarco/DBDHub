(function () {
    'use strict';

    const feed = document.getElementById('live-feed');
    const statusBadge = document.getElementById('live-connection');
    const clientsElement = document.getElementById('live-clients');
    const eventCountElement = document.getElementById('live-event-count');

    if (!feed || !window.signalR) return;

    const maxItems = 20;
    let eventCount = 0;
    let myServer = null;

    function setStatus(text, className) {
        if (!statusBadge) return;
        statusBadge.textContent = text;
        statusBadge.className = 'badge ' + className;
    }

    function typeClass(type) {
        if (type === 'match.created') return 'bg-success';
        if (type === 'match.simulated') return 'bg-warning text-dark';
        return 'bg-info text-dark';
    }

    function addEvent(event) {
        eventCount += 1;
        if (eventCountElement) eventCountElement.textContent = eventCount;
        if (feed.firstElementChild && feed.firstElementChild.classList.contains('text-muted')) {
            feed.innerHTML = '';
        }

        const item = document.createElement('li');
        item.className = 'live-item';

        const time = document.createElement('span');
        time.className = 'live-time';
        time.textContent = new Date().toLocaleTimeString();

        const badge = document.createElement('span');
        badge.className = 'badge live-type ' + typeClass(event.type);
        badge.textContent = event.type;

        const message = document.createElement('span');
        message.className = 'live-message';
        message.textContent = event.message || '';

        const server = document.createElement('span');
        server.className = 'live-server';
        server.textContent = event.server || '';

        item.append(time, badge, message, server);
        feed.prepend(item);

        while (feed.children.length > maxItems) {
            feed.removeChild(feed.lastElementChild);
        }
    }

    const connection = new signalR.HubConnectionBuilder()
        .withUrl('/hubs/live')
        .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
        .configureLogging(signalR.LogLevel.Warning)
        .build();

    connection.on('connected', function (info) {
        myServer = info.server;
        setStatus('Conectado: ' + info.server, 'bg-success');
    });

    connection.on('clientsOnline', function (info) {
        if (clientsElement && (!myServer || info.server === myServer)) {
            clientsElement.textContent = info.count;
        }
    });

    connection.on('liveEvent', addEvent);

    connection.onreconnecting(function () {
        setStatus('Reconectando…', 'bg-warning text-dark');
    });

    connection.onreconnected(function () {
        setStatus('Conectado', 'bg-success');
    });

    connection.onclose(function () {
        setStatus('Desconectado', 'bg-danger');
    });

    connection.start().catch(function (error) {
        console.error('SignalR:', error);
        setStatus('Sin conexión', 'bg-danger');
    });
})();
