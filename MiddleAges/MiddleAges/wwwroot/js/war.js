function openCountryWarsTab() {
    const countryWarsTab = document.getElementById("countryWarsTab");
    const worldWarsTab = document.getElementById("worldWarsTab");
    const countryWarsBtn = document.getElementById("countryWarsBtn");
    const worldWarsBtn = document.getElementById("worldWarsBtn");

    countryWarsTab.classList.add("d-block");
    countryWarsTab.classList.remove("d-none");
    worldWarsTab.classList.add("d-none");
    worldWarsTab.classList.remove("d-block");

    countryWarsBtn.classList.add("active");
    worldWarsBtn.classList.remove("active");
}

function openWorldWarsTab() {
    const countryWarsTab = document.getElementById("countryWarsTab");
    const worldWarsTab = document.getElementById("worldWarsTab");
    const countryWarsBtn = document.getElementById("countryWarsBtn");
    const worldWarsBtn = document.getElementById("worldWarsBtn");

    countryWarsTab.classList.add("d-none");
    countryWarsTab.classList.remove("d-block");
    worldWarsTab.classList.add("d-block");
    worldWarsTab.classList.remove("d-none");

    countryWarsBtn.classList.remove("active");
    worldWarsBtn.classList.add("active");
}