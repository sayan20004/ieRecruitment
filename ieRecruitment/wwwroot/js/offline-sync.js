document.addEventListener("DOMContentLoaded", function () {
    const FORM_STORAGE_KEY = "ie_recruitment_draft";
    const CANDIDATE_ID_INPUT = document.getElementById("CandidateId") || document.getElementById("UserId");
    
    function saveToLocal() {
        const formData = {};
        const inputs = document.querySelectorAll("input, select, textarea");
        
        inputs.forEach(input => {
            if (input.name) {
                if (input.type === "checkbox") {
                    formData[input.name] = input.checked;
                } else if (input.type === "radio") {
                    if (input.checked) formData[input.name] = input.value;
                } else {
                    formData[input.name] = input.value;
                }
            }
        });

        localStorage.setItem(FORM_STORAGE_KEY, JSON.stringify(formData));
    }

    function restoreFromLocal() {
        const stored = localStorage.getItem(FORM_STORAGE_KEY);
        if (!stored) return;

        const formData = JSON.parse(stored);
        const inputs = document.querySelectorAll("input, select, textarea");

        inputs.forEach(input => {
            if (input.name && formData[input.name] !== undefined) {
                if (input.type === "checkbox") {
                    input.checked = formData[input.name];
                } else if (input.type === "radio") {
                    if (input.value === formData[input.name]) input.checked = true;
                } else {
                    input.value = formData[input.name];
                }
            }
        });
    }

    async function syncToDb() {
        const stored = localStorage.getItem(FORM_STORAGE_KEY);
        const candidateId = CANDIDATE_ID_INPUT ? CANDIDATE_ID_INPUT.value : null;

        if (stored && candidateId && navigator.onLine) {
            try {
                const response = await fetch('/api/Sync/save', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        candidateId: candidateId,
                        localStorageData: stored
                    })
                });
            } catch (e) {
                console.error(e);
            }
        }
    }

    restoreFromLocal();

    document.querySelectorAll("input, select, textarea").forEach(el => {
        el.addEventListener("input", saveToLocal);
        el.addEventListener("change", saveToLocal);
    });

    window.addEventListener('online', syncToDb);
    
    setInterval(function() {
        if(navigator.onLine) syncToDb();
    }, 30000);
});