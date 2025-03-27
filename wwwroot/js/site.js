function activateButton(button) {

    document.querySelectorAll(".menu-btn").forEach(btn => btn.classList.remove("active"));
    button.classList.add("active");
}




