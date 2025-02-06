const pageLinesCount = 50;
var pageCount = 1
var category = "Total";

window.addEventListener("load", openRating('Total'));

function openRating(_category) { 
    clearPlayerNameFilter()
    // Получить все элементы с помощью class="tablinks" и удалить class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    document.getElementById(_category + "Btn").className += " active";

    category = _category;

    populateRatingGrid();
}

function populateRatingGrid() {
    var page = document.getElementById("pageNum").dataset.page;

    $.ajax({
        url: 'Rating/GetRatingByCategoryAndPage',
        type: 'post',
        datatype: 'json',
        data: {
            category: category,
            page: page
        },
        success: function (response) {
            if (response == null || response == undefined || response.length == 0) {
                return 'Error';
            }
            else {
                return response;
            }
        },
        error: function (response) {
            return 'Error';
        }
    }).done(function (data) {
        let obj = JSON.parse(data);
        if (obj.Ratings.length > 0) {
            populateGrid(obj);
            pageCount = obj.LastPageNum;
            checkPageButtonsEnableability();
            changePageNumText();
        }
    })
    .fail(function (data) {
        alert("Something went wrong");
    });
}

function prevPage() {
    const pageNumCtrl = document.getElementById("pageNum");

    if (pageNumCtrl.dataset.page > 1) {
        pageNumCtrl.dataset.page = parseInt(pageNumCtrl.dataset.page) - 1;
        populateRatingGrid();        
    }
}

function nextPage() {
    const pageNumCtrl = document.getElementById("pageNum");

    if (pageNumCtrl.dataset.page < pageCount) {
        pageNumCtrl.dataset.page = parseInt(pageNumCtrl.dataset.page) + 1;
        populateRatingGrid();
    }
}

function populateGrid(ratingList) {

    const ratingGrid = document.getElementById("ratinglines");
    ratingGrid.innerHTML = "";

    for (let i = 0; i < ratingList.Ratings.length; i++) {

        const ratingNode = document.createElement("div");
        ratingNode.classList.add("row", "mb-1");
        ratingGrid.appendChild(ratingNode);

        const totalPlaceDiv = document.createElement("div");
        ratingNode.appendChild(totalPlaceDiv);        

        const totalPlaceP = document.createElement("p");
        totalPlaceP.classList.add("mb-0", "mt-2", "lhst_rating-small-ints");
        totalPlaceP.innerText = ratingList.Ratings[i].TotalPlace;
        totalPlaceDiv.appendChild(totalPlaceP);

        const playerImgDiv = document.createElement("div");
        playerImgDiv.classList.add("mr-2", "lhst_scale");
        ratingNode.appendChild(playerImgDiv);

        const playerImg = document.createElement("img");
        playerImg.src = playerAvatarsUrl + ratingList.Ratings[i].Player.ImageURL;
        playerImg.classList.add("country_card-population-img", "lhst_cursor_pointer", "m_playerBtn");
        playerImg.dataset.playerid = ratingList.Ratings[i].Player.Id;
        playerImg.loading = "lazy";
        playerImg.title = ratingList.Ratings[i].Player.UserName;
        playerImgDiv.appendChild(playerImg);

        const playerNameDiv = document.createElement("div");
        playerNameDiv.style.width = "100px";
        ratingNode.appendChild(playerNameDiv);

        const playerNameP = document.createElement("p");
        playerNameP.classList.add("mb-0", "mt-2", "lhst_cursor_pointer", "m_playerBtn", "lhst_rating_username");
        playerNameP.dataset.playerid = ratingList.Ratings[i].Player.Id;
        playerNameP.innerText = ratingList.Ratings[i].Player.UserName;
        playerNameDiv.appendChild(playerNameP);

        const expDiv = document.createElement("div");
        expDiv.classList.add("lhst_rating-small-ints", "mr-2");
        ratingNode.appendChild(expDiv);

        const expP = document.createElement("p");
        expP.classList.add("mb-0", "mt-2");
        expP.innerText = ratingList.Ratings[i].ExpPlace;
        expDiv.appendChild(expP);

        const moneyDiv = document.createElement("div");
        moneyDiv.classList.add("lhst_rating-small-ints", "mr-2");
        ratingNode.appendChild(moneyDiv);

        const moneyP = document.createElement("p");
        moneyP.classList.add("mb-0", "mt-2");
        moneyP.innerText = ratingList.Ratings[i].MoneyPlace;
        moneyDiv.appendChild(moneyP);

        const powerDiv = document.createElement("div");
        powerDiv.classList.add("lhst_rating-small-ints");
        ratingNode.appendChild(powerDiv);

        const powerP = document.createElement("p");
        powerP.classList.add("mb-0", "mt-2");
        powerP.innerText = ratingList.Ratings[i].WarPowerPlace;
        powerDiv.appendChild(powerP);        
    }    
}

function checkPageButtonsEnableability() {
    const pageNumCtrl = document.getElementById("pageNum");

    pageLeftBtn = document.getElementById("pageLeft");
    pageRightBtn = document.getElementById("pageRight");

    pageLeftBtn.disabled = false;
    pageRightBtn.disabled = false;

    if (pageNumCtrl.dataset.page == 1) {
        pageLeftBtn.disabled = true;
    }

    if (pageNumCtrl.dataset.page == pageCount) {
        pageRightBtn.disabled = true;
    }
}

function changePageNumText() {
    const pageNumCtrl = document.getElementById("pageNum");
    pageNumCtrl.innerText = 'Page ' + pageNumCtrl.dataset.page + '/' + pageCount;
}

function findPlayer() {
    const playerNameInput = document.getElementById("playerNameInput");

    var playerName = playerNameInput.value;

    if (playerName.length > 2) {
        searchRatingsByPlayerName(playerName);
    }
}

function searchRatingsByPlayerName(playerName) {
    $.ajax({
        url: 'Rating/SearchRatingsByPlayerName',
        type: 'post',
        datatype: 'json',
        data: {
            playerName, playerName
        },
        success: function (response) {
            if (response == null || response == undefined || response.length == 0) {
                return 'Error';
            }
            else {
                return response;
            }
        },
        error: function (response) {
            return 'Error';
        }
    }).done(function (data) {
        let obj = JSON.parse(data);
        if (obj.Ratings.length > 0) {
            populateGrid(obj);
            pageCount = obj.LastPageNum;
            checkPageButtonsEnableability();
            changePageNumText();
        }
        else {
            showNoResults();
        }
    })
    .fail(function (data) {
        alert("Something went wrong");
    });
}

function showNoResults() {
    const ratingGrid = document.getElementById("ratinglines");
    ratingGrid.innerHTML = "";

    const noResultsP = document.createElement("p");
    noResultsP.classList.add("ml-2", "mb-0", "mt-2");
    noResultsP.style.fontWeight = "400";
    noResultsP.innerText = "No results found";
    ratingGrid.appendChild(noResultsP);
}

function clearPlayerNameFilter() {
    document.getElementById("playerNameInput").value = "";
}