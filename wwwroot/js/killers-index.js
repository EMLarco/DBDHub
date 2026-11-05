(function () {
    const tiers = ['purple-tier', 'green-tier', 'yellow-tier'];
    let currentTierIndex = 0;

    const allPerks = document.querySelectorAll('.dbd-perk-background');
    if (allPerks.length === 0) return;

    function applyTierToAll() {
        const currentClass = tiers[currentTierIndex];
        allPerks.forEach(function (bg) {
            bg.classList.remove('purple-tier', 'green-tier', 'yellow-tier');
            bg.classList.add(currentClass);
        });
    }

    applyTierToAll();

    setInterval(function () {
        currentTierIndex = (currentTierIndex + 1) % tiers.length;
        applyTierToAll();
    }, 1000);
})();
