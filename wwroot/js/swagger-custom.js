// wwwroot/js/swagger-custom.js

window.onload = function () {
    // Wait until Swagger UI has fully loaded
    const uiInterval = setInterval(() => {
        const operations = document.querySelectorAll('.opblock');
        if (operations.length > 0) {
            clearInterval(uiInterval);
            addCustomButtons();
        }
    }, 1000);

    function addCustomButtons() {
        const operations = document.querySelectorAll('.opblock');

        operations.forEach(opblock => {
            // Create container for custom buttons
            const customButtonsContainer = document.createElement('div');
            customButtonsContainer.style.display = 'flex';
            customButtonsContainer.style.gap = '10px';
            customButtonsContainer.style.marginTop = '10px';

            // Create Download Button
            const downloadButton = document.createElement('button');
            downloadButton.innerHTML = 'Download JSON';
            downloadButton.className = 'btn btn-sm btn-primary';
            downloadButton.onclick = () => downloadJson(opblock);

            // Create Upload Button
            const uploadButton = document.createElement('button');
            uploadButton.innerHTML = 'Upload JSON';
            uploadButton.className = 'btn btn-sm btn-secondary';
            uploadButton.onclick = () => uploadJson(opblock);

            customButtonsContainer.appendChild(downloadButton);
            customButtonsContainer.appendChild(uploadButton);

            // Append buttons to the operation
            const opBlockBody = opblock.querySelector('.opblock-body');
            if (opBlockBody) {
                opBlockBody.prepend(customButtonsContainer);
            }
        });
    }

    function downloadJson(opblock) {
        const textarea = opblock.querySelector('.body-param .body-param__text, .body-param textarea, .request-body textarea');

        if (textarea) {
            const jsonContent = textarea.value;
            if (jsonContent) {
                const blob = new Blob([jsonContent], { type: 'application/json' });
                const link = document.createElement('a');
                link.href = URL.createObjectURL(blob);
                link.download = 'request-body.json';
                link.click();
                URL.revokeObjectURL(link.href);
            } else {
                alert('Request body is empty!');
            }
        } else {
            alert('No request body found for this operation!');
        }
    }

    function uploadJson(opblock) {
        const fileInput = document.createElement('input');
        fileInput.type = 'file';
        fileInput.accept = 'application/json';
        fileInput.onchange = event => {
            const file = event.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = e => {
                    const content = e.target.result;
                    try {
                        const json = JSON.stringify(JSON.parse(content), null, 2); // Beautify JSON
                        const textarea = opblock.querySelector('.body-param .body-param__text, .body-param textarea, .request-body textarea');
                        if (textarea) {
                            textarea.value = json;
                            // Trigger input event to update Swagger UI internal state
                            const event = new Event('input', { bubbles: true });
                            textarea.dispatchEvent(event);
                        } else {
                            alert('No request body field found for this operation!');
                        }
                    } catch (err) {
                        alert('Invalid JSON file!');
                    }
                };
                reader.readAsText(file);
            }
        };
        fileInput.click();
    }
};
