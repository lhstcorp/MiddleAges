const playerAvatarsUrl = '../img/avatars/';
var category = "Total";

window.addEventListener("load", openRating('Total'));

function openRating(_category) {    
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
        if (obj.length > 0) {
        populateGrid(obj);
            alert("All good!");
        }
        else {
            alert("Not good!");
        }
    })
    .fail(function (data) {
        alert("Something went wrong");
    });
}

function prevPage() {
    const pageNumCtrl = document.getElementById("pageNum");

    page = pageNumCtrl.data("page") - 1;

    alert(page);
}

function nextPage() {
    const pageNumCtrl = document.getElementById("pageNum");

    pageNumCtrl.dataset.page = parseInt(pageNumCtrl.dataset.page) + 1;


}

function populateGrid(ratingList) {

    const ratingGrid = document.getElementById("ratinglines");
    ratingGrid.innerHTML = "";

    for (let i = 0; i < ratingList.length; i++) {

        //<div class="row">
        //    <div>
        //        <p class="mb-0 mt-2 lhst_rating-small-ints">@Model[i].TotalPlace</p>
        //    </div>
        //    <div class="mr-2">
        //        <img class="country_card-population-img" src="../img/avatars/@(Model[i].Player.ImageURL).webp" alt="" title="Lords" loading="lazy" />
        //    </div>
        //    <div style="width: 100px">
        //        <p class="mb-0 mt-2">@Model[i].Player.UserName</p>
        //    </div>
        //    <div class="lhst_rating-small-ints mr-2">
        //        <p class="mb-0 mt-2">@Model[i].ExpPlace</p>
        //    </div>
        //    <div class="lhst_rating-small-ints mr-2">
        //        <p class="mb-0 mt-2">@Model[i].MoneyPlace</p>
        //    </div>
        //    <div class="lhst_rating-small-ints">
        //        <p class="mb-0 mt-2">@Model[i].WarPowerPlace</p>
        //    </div>
        //</div>
        const ratingNode = document.createElement("div");
        ratingNode.classList.add("row");
        ratingGrid.appendChild(ratingNode);

        const totalPlaceDiv = document.createElement("div");
        ratingNode.appendChild(totalPlaceDiv);

        const totalPlaceP = document.createElement("p");
        totalPlaceP.classList.add("mb-0", "mt-2", "lhst_rating-small-ints");
        totalPlaceP.innerText = ratingList[i].TotalPlace;
        totalPlaceDiv.appendChild(totalPlaceP);

        const playerImgDiv = document.createElement("div");
        playerImgDiv.classList.add("mr-2");
        ratingNode.appendChild(playerImgDiv);

        const playerImg = document.createElement("img");
        playerImg.src = playerAvatarsUrl + ratingList[i].Player.ImageURL + '.webp';
        playerImg.classList.add("country_card-population-img");
        playerImg.loading = "lazy";
        playerImg.title = ratingList[i].Player.UserName;
        playerImgDiv.appendChild(playerImg);

        /*
        const armyNode = document.createElement("div");

        const playerImg = document.createElement("img");
        playerImg.src = playerAvatarsUrl + armyList[i].Player.ImageURL + '.webp';
        playerImg.height = 32;
        playerImg.loading = "lazy";
        playerImg.classList.add("lhst_country_history_img");
        playerImg.title = armyList[i].Player.UserName;
        armyNode.appendChild(playerImg);

        const soldiersInArmy = document.createElement("p");
        soldiersInArmy.innerHTML = armyList[i].SoldiersCount;
        soldiersInArmy.classList.add("lhst_country_info_region_value");
        soldiersInArmy.classList.add("pl-1");
        armyNode.appendChild(soldiersInArmy);

        if (armyList[i].Player.Id === currentPlayer.Id) {
            const disbandArmyImg = document.createElement("img");
            disbandArmyImg.src = interfaceIconsUrl + 'red-diagonal-cross.png'
            disbandArmyImg.height = 20;
            disbandArmyImg.loading = "lazy";
            disbandArmyImg.classList.add("lhst_country_history_img");
            disbandArmyImg.classList.add("pl-1");
            disbandArmyImg.dataset.armyid = armyList[i].ArmyId;
            disbandArmyImg.classList.add("lhst_cursor_pointer");
            disbandArmyImg.onclick = disbandPlayerArmy;
            armyNode.appendChild(disbandArmyImg);
        }
        */

    }    
}