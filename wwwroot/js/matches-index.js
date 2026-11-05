(function () {
    const list = document.querySelector('.match-list[data-details-url]');
    const modalElement = document.getElementById('matchDetailModal');
    const content = document.getElementById('matchDetailContent');
    if (!list || !modalElement || !content) return;

    const detailsUrl = list.dataset.detailsUrl;
    const modal = new bootstrap.Modal(modalElement);

    async function loadMatchDetails(id) {
        modal.show();
        try {
            const response = await fetch(detailsUrl + '/' + encodeURIComponent(id));
            if (!response.ok) throw new Error('Error');
            content.innerHTML = await response.text();
        } catch {
            content.innerHTML = '<div class="alert alert-danger m-3">No se pudo cargar el detalle.</div>';
        }
    }

    list.addEventListener('click', function (event) {
        if (event.button !== 0 || event.metaKey || event.ctrlKey || event.shiftKey || event.altKey) return;

        const card = event.target.closest('.match-card[data-match-id]');
        if (!card) return;

        event.preventDefault();
        loadMatchDetails(card.dataset.matchId);
    });
})();

(function () {
    'use strict';

    const form = document.getElementById('simulateMatchForm');
    const result = document.getElementById('simulate-result');
    if (!form) return;

    form.addEventListener('submit', async function (event) {
        event.preventDefault();
        const button = form.querySelector('button[type="submit"]');
        if (button) button.disabled = true;
        if (result) {
            result.textContent = 'Simulando…';
            result.className = 'small text-muted';
        }

        try {
            const response = await fetch(form.action, {
                method: 'POST',
                body: new FormData(form)
            });
            if (!response.ok) throw new Error('HTTP ' + response.status);

            const data = await response.json();
            if (!data.ok) throw new Error(data.message || 'Error desconocido');

            if (result) {
                result.textContent = 'Partida #' + data.id + ' creada y emitida.';
                result.className = 'small text-success';
            }
        } catch (error) {
            if (result) {
                result.textContent = 'No se pudo simular: ' + error.message;
                result.className = 'small text-danger';
            }
        } finally {
            if (button) button.disabled = false;
        }
    });
})();
