﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function autoSizeTextAreas() {
    document.querySelectorAll('textarea').forEach(element => {
        element.setAttribute('style', `height:${element.scrollHeight}px;overflow-y:hidden`);
        element.addEventListener('input', event => {
            event.target.style.height = 'auto';
            event.target.style.height = `${event.target.scrollHeight}px`;
        });
    });
}

function manuallyResizeTextArea(textArea) {
    textArea.dispatchEvent(new Event('input'));
}


function preventUnsavedChanges() {
    let isFormChanged = false;
    document.querySelector('form').addEventListener('input', () => {
        isFormChanged = true;
    });

    window.onbeforeunload = function() {
        if (isFormChanged) {
            return "";
        }
    }
}

preventUnsavedChanges();
autoSizeTextAreas();