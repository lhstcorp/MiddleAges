function moveToLand() {
    let landId = $('#moveToBtn').data("selectedland").replace('_', ' ');

    $.ajax({
        url: 'Map/MoveToLand/' + landId,
        type: 'post',
        datatype: 'json',
        data: {
            landId: landId
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
    })
    .done(function (data) {
        let obj = JSON.parse(data);
        if (obj == 'Error') {
            alert("Unfortunately you haven't moved to the selected land");
        }
        else {
            changeIconPosition('currentLandIcon', landId);
            setLocationBtnEnableability(landId);
        }
    })
    .fail(function (data) {
        alert("Unfortunately you haven't moved to the selected land");
    });
}

function settleDown() {
    let landId = $('#settleBtn').data("selectedland").replace('_', ' ');

    $.ajax({
        url: 'Map/SettleDown/' + landId,
        type: 'post',
        datatype: 'json',
        data: {
            landId: landId
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
    })
    .done(function (data) {
        let obj = JSON.parse(data);
        if (obj == 'Error') {
            alert("Unfortunately you haven't settled down on this land");
        }
        else {
            changeIconPosition('residenceIcon', landId);
            setLocationBtnEnableability(landId);
        }
    })
    .fail(function (data) {
        alert("Unfortunately you haven't settled down on this land");
    });
}

function changeIconPosition(iconId, landId) {    
    const landPolygon = document.getElementById(landId.replace(' ', '_'));
    let landCenter = getPolygonCenter(landPolygon.animatedPoints);    

    var landCenterPoint = svg.createSVGPoint();

    landCenterPoint.x = landCenter[0] - 10;
    landCenterPoint.y = landCenter[1] - 15;

    const locationIcon = document.getElementById(iconId);
    locationIcon.setAttribute('x', landCenterPoint.x);
    locationIcon.setAttribute('y', landCenterPoint.y);
}

function startAnUprising() {
    let landId = $('#startAnUprisingBtn').data("selectedland").replace('_', ' ');

    $.ajax({
        url: 'Map/StartAnUprising/' + landId,
        type: 'post',
        datatype: 'json',
        data: {
            landId: landId
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
    })
        .done(function (data) {
            let obj = JSON.parse(data);
            if (obj == 'Error') {
                alert("Unfortunately you haven't started the rebellion on this land");
            }
            else {
                addRevoltIcon(landId);
                setLocationBtnEnableability(landId);
            }
        })
        .fail(function (data) {
            alert("Unfortunately you haven't settled down on this land");
        });
}

function addRevoltIcon(landId) {
    const landPolygon = document.getElementById(landId.replace(' ', '_'));
    let landCenter = getPolygonCenter(landPolygon.animatedPoints);

    var landCenterPoint = svg.createSVGPoint();

    landCenterPoint.x = landCenter[0] - 10;
    landCenterPoint.y = landCenter[1] - 15;

    var map = document.getElementById("global_region_map");
    const revoltSvg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
    revoltSvg.setAttribute('x', landCenterPoint.x);
    revoltSvg.setAttribute('y', landCenterPoint.y);

    var revoltIcon = document.createElementNS('http://www.w3.org/2000/svg', 'image');
    revoltIcon.setAttribute('href', '../img/interface-icons/map-icons/revolt.svg');
    revoltSvg.appendChild(revoltIcon);

    map.appendChild(revoltSvg);
}

function toggleTextures() {
    const isChecked = document.getElementById('texture-toggle').checked;
    const map = document.getElementById("global_region_map");
    if (isChecked) {
        map.style.filter = 'url(#filter3)';
    }
    else {
        map.style.filter = 'none'; // Отключаем текстуры
    }

    document.getElementById('texture-toggle').addEventListener('change', toggleTextures);
}

function addHeraldicIcon(landId, heraldicImagePath) {
    // Получаем полигон региона по ID
    const landPolygon = document.getElementById(landId.replace(' ', '_'));

    // Получаем центр полигона (региона)
    let landCenter = getPolygonCenter(landPolygon.animatedPoints);

    // Создаём SVGPoint для центра региона
    var landCenterPoint = svg.createSVGPoint();
    landCenterPoint.x = landCenter[0] - 15; // Корректируем позицию по X для выравнивания
    landCenterPoint.y = landCenter[1] - 15; // Корректируем позицию по Y для выравнивания

    // Создаём новый SVG элемент для изображения герба
    var map = document.getElementById("global_region_map");
    const heraldicSvg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
    heraldicSvg.setAttribute('x', landCenterPoint.x);
    heraldicSvg.setAttribute('y', landCenterPoint.y);
    heraldicSvg.setAttribute('width', '30');  // Указываем размеры изображения
    heraldicSvg.setAttribute('height', '30'); // Указываем размеры изображения

    // Создаём элемент image и указываем путь к изображению герба
    var heraldicIcon = document.createElementNS('http://www.w3.org/2000/svg', 'image');
    heraldicIcon.setAttribute('href', heraldicImagePath); // Путь к гербу
    heraldicIcon.setAttribute('width', '30');  // Ширина герба
    heraldicIcon.setAttribute('height', '30'); // Высота герба

    // Добавляем герб на карту
    heraldicSvg.appendChild(heraldicIcon);
    map.appendChild(heraldicSvg);
}

function toggleHeraldic() {
    const isChecked = document.getElementById('heraldic-toggle').checked;
    const map = document.getElementById("global_region_map");
    if (isChecked) {
        showHeraldicIcons();
    }
    else {
        hideHeraldicIcons();
    }

    document.getElementById('heraldic-toggle').addEventListener('change', toggleHeraldic);
}

function showHeraldicIcons() {
    // Логика для отображения гербов (например, добавление элементов SVG с гербами)
    addHeraldicIcon('Hrodna', '../img/map-regions-icons-middle-ages/Hrodna.png');
    addHeraldicIcon('Bierastavica', '../img/map-regions-icons-middle-ages/Bierastavica.png');
    addHeraldicIcon('Masty', '../img/map-regions-icons-middle-ages/Masty.png');
    addHeraldicIcon('Vaukavysk', '../img/map-regions-icons-middle-ages/Vaukavysk.png');
    addHeraldicIcon('Zelva', '../img/map-regions-icons-middle-ages/Zelva.png');
    addHeraldicIcon('Svislach', '../img/map-regions-icons-middle-ages/Svislach.png');
    addHeraldicIcon('Slonim', '../img/map-regions-icons-middle-ages/Slonim.png');
    addHeraldicIcon('Dzyatlava', '../img/map-regions-icons-middle-ages/Dzyatlava.png');
    addHeraldicIcon('Shchuchin', '../img/map-regions-icons-middle-ages/Shchuchin.png');
    addHeraldicIcon('Voranava', '../img/map-regions-icons-middle-ages/Voranava.png');
    addHeraldicIcon('Lida', '../img/map-regions-icons-middle-ages/Lida.png');
    addHeraldicIcon('Navahrudak', '../img/map-regions-icons-middle-ages/Navahrudak.png');
    addHeraldicIcon('Karelichy', '../img/map-regions-icons-middle-ages/Karelichy.png');
    addHeraldicIcon('Iue', '../img/map-regions-icons-middle-ages/Iue.png');
    addHeraldicIcon('Ashmjany', '../img/map-regions-icons-middle-ages/Ashmjany.png');
    addHeraldicIcon('Smargon', '../img/map-regions-icons-middle-ages/Smargon.png');
    addHeraldicIcon('Astravets', '../img/map-regions-icons-middle-ages/Astravets.png');
    addHeraldicIcon('Pastavy', '../img/map-regions-icons-middle-ages/Pastavy.png');
    addHeraldicIcon('Braslau', '../img/map-regions-icons-middle-ages/Braslau.png');
    addHeraldicIcon('Sharkaushchyna', '../img/map-regions-icons-middle-ages/Sharkaushchyna.png');
    addHeraldicIcon('Dokshytsy', '../img/map-regions-icons-middle-ages/Dokshytsy.png');
    addHeraldicIcon('Glubokae', '../img/map-regions-icons-middle-ages/Glubokae.png');
    addHeraldicIcon('Miyory', '../img/map-regions-icons-middle-ages/Miyory.png');
    addHeraldicIcon('Verhnyadzvinsk', '../img/map-regions-icons-middle-ages/Verhnyadzvinsk.png');
    addHeraldicIcon('Lepel', '../img/map-regions-icons-middle-ages/Lepel.png');
    addHeraldicIcon('Chashniki', '../img/map-regions-icons-middle-ages/Chashniki.png');
    addHeraldicIcon('Talachyn', '../img/map-regions-icons-middle-ages/Talachyn.png');
    addHeraldicIcon('Orsha', '../img/map-regions-icons-middle-ages/Orsha.png');
    addHeraldicIcon('Dubrouna', '../img/map-regions-icons-middle-ages/Dubrouna.png');
    addHeraldicIcon('Syanno', '../img/map-regions-icons-middle-ages/Syanno.png');
    addHeraldicIcon('Lyozna', '../img/map-regions-icons-middle-ages/Lyozna.png');
    addHeraldicIcon('Vitebsk', '../img/map-regions-icons-middle-ages/Vitebsk.png');
    addHeraldicIcon('Beshankovichy', '../img/map-regions-icons-middle-ages/Beshankovichy.png');
    addHeraldicIcon('Shumilina', '../img/map-regions-icons-middle-ages/Shumilina.png');
    addHeraldicIcon('Ushachi', '../img/map-regions-icons-middle-ages/Ushachi.png');
    addHeraldicIcon('Polatsk', '../img/map-regions-icons-middle-ages/Polatsk.png');
    addHeraldicIcon('Haradok', '../img/map-regions-icons-middle-ages/Haradok.png');
    addHeraldicIcon('Rasony', '../img/map-regions-icons-middle-ages/Rasony.png');
    addHeraldicIcon('Horki', '../img/map-regions-icons-middle-ages/Horki.png');
    addHeraldicIcon('Drybin', '../img/map-regions-icons-middle-ages/Drybin.png');
    addHeraldicIcon('Mstislaul', '../img/map-regions-icons-middle-ages/Mstislaul.png');
    addHeraldicIcon('Shklou', '../img/map-regions-icons-middle-ages/Shklou.png');
    addHeraldicIcon('Kruhlae', '../img/map-regions-icons-middle-ages/Kruhlae.png');
    addHeraldicIcon('Byalynichy', '../img/map-regions-icons-middle-ages/Byalynichy.png');
    addHeraldicIcon('Klichau', '../img/map-regions-icons-middle-ages/Klichau.png');
    addHeraldicIcon('Asipovichy', '../img/map-regions-icons-middle-ages/Asipovichy.png');
    addHeraldicIcon('Hlusk', '../img/map-regions-icons-middle-ages/Hlusk.png');
    addHeraldicIcon('Babruisk', '../img/map-regions-icons-middle-ages/Babruisk.png');
    addHeraldicIcon('Kirausk', '../img/map-regions-icons-middle-ages/Kirausk.png');
    addHeraldicIcon('Byhau', '../img/map-regions-icons-middle-ages/Byhau.png');
    addHeraldicIcon('Mahilou', '../img/map-regions-icons-middle-ages/Mahilou.png');
    addHeraldicIcon('Chausy', '../img/map-regions-icons-middle-ages/Chausy.png');
    addHeraldicIcon('Cherykau', '../img/map-regions-icons-middle-ages/Cherykau.png');
    addHeraldicIcon('Krychau', '../img/map-regions-icons-middle-ages/Krychau.png');
    addHeraldicIcon('Klimavichy', '../img/map-regions-icons-middle-ages/Klimavichy.png');
    addHeraldicIcon('Khotimsk', '../img/map-regions-icons-middle-ages/Khotimsk.png');
    addHeraldicIcon('Kastsukovichy', '../img/map-regions-icons-middle-ages/Kastsukovichy.png');
    addHeraldicIcon('Krasnapolle', '../img/map-regions-icons-middle-ages/Krasnapolle.png');
    addHeraldicIcon('Slauharad', '../img/map-regions-icons-middle-ages/Slauharad.png');
    addHeraldicIcon('Karma', '../img/map-regions-icons-middle-ages/Karma.png');
    addHeraldicIcon('Chachersk', '../img/map-regions-icons-middle-ages/Chachersk.png');
    addHeraldicIcon('Rahachou', '../img/map-regions-icons-middle-ages/Rahachou.png');
    addHeraldicIcon('Zhlobin', '../img/map-regions-icons-middle-ages/Zhlobin.png');
    addHeraldicIcon('Svetlahorsk', '../img/map-regions-icons-middle-ages/Svetlahorsk.png');
    addHeraldicIcon('Aktsyabrski', '../img/map-regions-icons-middle-ages/Aktsyabrski.png');
    addHeraldicIcon('Petrykau', '../img/map-regions-icons-middle-ages/Petrykau.png');
    addHeraldicIcon('Zhytkavichy', '../img/map-regions-icons-middle-ages/Zhytkavichy.png');
    addHeraldicIcon('Lelchytsy', '../img/map-regions-icons-middle-ages/Lelchytsy.png');
    addHeraldicIcon('Kalinkavichy', '../img/map-regions-icons-middle-ages/Kalinkavichy.png');
    addHeraldicIcon('Mazyr', '../img/map-regions-icons-middle-ages/Mazyr.png');
    addHeraldicIcon('Yelsk', '../img/map-regions-icons-middle-ages/Yelsk.png');
    addHeraldicIcon('Naroulya', '../img/map-regions-icons-middle-ages/Naroulya.png');
    addHeraldicIcon('Khoiniki', '../img/map-regions-icons-middle-ages/Khoiniki.png');
    addHeraldicIcon('Brahin', '../img/map-regions-icons-middle-ages/Brahin.png');
    addHeraldicIcon('Rechytsa', '../img/map-regions-icons-middle-ages/Rechytsa.png');
    addHeraldicIcon('Loeu', '../img/map-regions-icons-middle-ages/Loeu.png');
    addHeraldicIcon('Buda-Kashalyova', '../img/map-regions-icons-middle-ages/Buda-Kashalyova.png');
    addHeraldicIcon('Homel', '../img/map-regions-icons-middle-ages/Homel.png');
    addHeraldicIcon('Vetka', '../img/map-regions-icons-middle-ages/Vetka.png');
    addHeraldicIcon('Dobrush', '../img/map-regions-icons-middle-ages/Dobrush.png');
    addHeraldicIcon('Salihorsk', '../img/map-regions-icons-middle-ages/Salihorsk.png');
    addHeraldicIcon('Liuban', '../img/map-regions-icons-middle-ages/Liuban.png');
    addHeraldicIcon('Staryja_Darohi', '../img/map-regions-icons-middle-ages/Staryja Darohi.png');
    addHeraldicIcon('Puhavichy', '../img/map-regions-icons-middle-ages/Puhavichy.png');
    addHeraldicIcon('Cherven', '../img/map-regions-icons-middle-ages/Cherven.png');
    addHeraldicIcon('Berazino', '../img/map-regions-icons-middle-ages/Berazino.png');
    addHeraldicIcon('Krupki', '../img/map-regions-icons-middle-ages/Krupki.png');
    addHeraldicIcon('Barysau', '../img/map-regions-icons-middle-ages/Barysau.png');
    addHeraldicIcon('Smalyavichy', '../img/map-regions-icons-middle-ages/Smalyavichy.png');
    addHeraldicIcon('Lahoisk', '../img/map-regions-icons-middle-ages/Lahoisk.png');
    addHeraldicIcon('Myadzel', '../img/map-regions-icons-middle-ages/Myadzel.png');
    addHeraldicIcon('Vileika', '../img/map-regions-icons-middle-ages/Vileika.png');
    addHeraldicIcon('Maladzechna', '../img/map-regions-icons-middle-ages/Maladzechna.png');
    addHeraldicIcon('Minsk', '../img/map-regions-icons-middle-ages/Minsk.png');
    addHeraldicIcon('Valozhyn', '../img/map-regions-icons-middle-ages/Valozhyn.png');
    addHeraldicIcon('Staubcy', '../img/map-regions-icons-middle-ages/Staubcy.png');
    addHeraldicIcon('Dzyarzhynsk', '../img/map-regions-icons-middle-ages/Dzyarzhynsk.png');
    addHeraldicIcon('Uzda', '../img/map-regions-icons-middle-ages/Uzda.png');
    addHeraldicIcon('Slutsk', '../img/map-regions-icons-middle-ages/Slutsk.png');
    addHeraldicIcon('Kapyl', '../img/map-regions-icons-middle-ages/Kapyl.png');
    addHeraldicIcon('Nyasvizh', '../img/map-regions-icons-middle-ages/Nyasvizh.png');
    addHeraldicIcon('Kletsk', '../img/map-regions-icons-middle-ages/Kletsk.png');
    addHeraldicIcon('Baranavichy', '../img/map-regions-icons-middle-ages/Baranavichy.png');
    addHeraldicIcon('Lyahavichy', '../img/map-regions-icons-middle-ages/Lyahavichy.png');
    addHeraldicIcon('Hantsavichy', '../img/map-regions-icons-middle-ages/Hantsavichy.png');
    addHeraldicIcon('Luninec', '../img/map-regions-icons-middle-ages/Luninec.png');
    addHeraldicIcon('Stolin', '../img/map-regions-icons-middle-ages/Stolin.png');
    addHeraldicIcon('Pinsk', '../img/map-regions-icons-middle-ages/Pinsk.png');
    addHeraldicIcon('Ivatsevichy', '../img/map-regions-icons-middle-ages/Ivatsevichy.png');
    addHeraldicIcon('Ivanava', '../img/map-regions-icons-middle-ages/Ivanava.png');
    addHeraldicIcon('Byaroza', '../img/map-regions-icons-middle-ages/Byaroza.png');
    addHeraldicIcon('Pruzhany', '../img/map-regions-icons-middle-ages/Pruzhany.png');
    addHeraldicIcon('Kamyanec', '../img/map-regions-icons-middle-ages/Kamyanec.png');
    addHeraldicIcon('Brest', '../img/map-regions-icons-middle-ages/Brest.png');
    addHeraldicIcon('Zhabinka', '../img/map-regions-icons-middle-ages/Zhabinka.png');
    addHeraldicIcon('Malaryta', '../img/map-regions-icons-middle-ages/Malaryta.png');
    addHeraldicIcon('Kobryn', '../img/map-regions-icons-middle-ages/Kobryn.png');
    addHeraldicIcon('Drahichyn', '../img/map-regions-icons-middle-ages/Drahichyn.png');
    addHeraldicIcon('Ershichi', '../img/map-regions-icons-middle-ages/Ershichi.png');
    addHeraldicIcon('Shumyachi', '../img/map-regions-icons-middle-ages/Shumyachi.png');
    addHeraldicIcon('Roslavl', '../img/map-regions-icons-middle-ages/Roslavl.png');
    addHeraldicIcon('Hislavichi', '../img/map-regions-icons-middle-ages/Hislavichi.png');
    addHeraldicIcon('Monastyrschina', '../img/map-regions-icons-middle-ages/Monastyrschina.png');
    addHeraldicIcon('Krasny', '../img/map-regions-icons-middle-ages/Krasny.png');
    addHeraldicIcon('Rudnya', '../img/map-regions-icons-middle-ages/Rudnya.png');
    addHeraldicIcon('Pochinok', '../img/map-regions-icons-middle-ages/Pochinok.png');
    addHeraldicIcon('Smolensk', '../img/map-regions-icons-middle-ages/Smolensk.png');
    addHeraldicIcon('Demidov', '../img/map-regions-icons-middle-ages/Demidov.png');
    addHeraldicIcon('Velizh', '../img/map-regions-icons-middle-ages/Velizh.png');
    addHeraldicIcon('Elnya', '../img/map-regions-icons-middle-ages/Elnya.png');
    addHeraldicIcon('Glinka', '../img/map-regions-icons-middle-ages/Glinka.png');
    addHeraldicIcon('Kardymovo', '../img/map-regions-icons-middle-ages/Kardymovo.png');
    addHeraldicIcon('Duhovschina', '../img/map-regions-icons-middle-ages/Duhovschina.png');
    addHeraldicIcon('Yarcevo', '../img/map-regions-icons-middle-ages/Yarcevo.png');
    addHeraldicIcon('Dorogobuzh', '../img/map-regions-icons-middle-ages/Dorogobuzh.png');
    addHeraldicIcon('Safonovo', '../img/map-regions-icons-middle-ages/Safonovo.png');
    addHeraldicIcon('Holm-Zhirkovski', '../img/map-regions-icons-middle-ages/Holm-Zhirkovski.png');
    addHeraldicIcon('Ugra', '../img/map-regions-icons-middle-ages/Ugra.png');
    addHeraldicIcon('Vyazma', '../img/map-regions-icons-middle-ages/Vyazma.png');
    addHeraldicIcon('Tsyomkino', '../img/map-regions-icons-middle-ages/Tsyomkino.png');
    addHeraldicIcon('Novodugino', '../img/map-regions-icons-middle-ages/Novodugino.png');
    addHeraldicIcon('Sychyovka', '../img/map-regions-icons-middle-ages/Sychyovka.png');
    addHeraldicIcon('Gzhatsk', '../img/map-regions-icons-middle-ages/Gzhatsk.png');
    addHeraldicIcon('Salchininkai', '../img/map-regions-icons-middle-ages/Salchininkai.png');
    addHeraldicIcon('Varena', '../img/map-regions-icons-middle-ages/Varena.png');
    addHeraldicIcon('Druskininkai', '../img/map-regions-icons-middle-ages/Druskininkai.png');
    addHeraldicIcon('Lazdijai', '../img/map-regions-icons-middle-ages/Lazdijai.png');
    addHeraldicIcon('Alytus', '../img/map-regions-icons-middle-ages/Alytus.png');
    addHeraldicIcon('Trakai', '../img/map-regions-icons-middle-ages/Trakai.png');
    addHeraldicIcon('Vilnius', '../img/map-regions-icons-middle-ages/Vilnius.png');
    addHeraldicIcon('Svencionys', '../img/map-regions-icons-middle-ages/Svencionys.png');
    addHeraldicIcon('Ignalina', '../img/map-regions-icons-middle-ages/Ignalina.png');
    addHeraldicIcon('Zarasai', '../img/map-regions-icons-middle-ages/Zarasai.png');
    addHeraldicIcon('Kalvarija', '../img/map-regions-icons-middle-ages/Kalvarija.png');
    addHeraldicIcon('Marijampole', '../img/map-regions-icons-middle-ages/Marijampole.png');
    addHeraldicIcon('Prienai', '../img/map-regions-icons-middle-ages/Prienai.png');
    addHeraldicIcon('Elektrenai', '../img/map-regions-icons-middle-ages/Elektrenai.png');
    addHeraldicIcon('Kaisiadorys', '../img/map-regions-icons-middle-ages/Kaisiadorys.png');
    addHeraldicIcon('Sirvintos', '../img/map-regions-icons-middle-ages/Sirvintos.png');
    addHeraldicIcon('Moletai', '../img/map-regions-icons-middle-ages/Moletai.png');
    addHeraldicIcon('Utena', '../img/map-regions-icons-middle-ages/Utena.png');
    addHeraldicIcon('Rokiskis', '../img/map-regions-icons-middle-ages/Rokiskis.png');
    addHeraldicIcon('Kupiskis', '../img/map-regions-icons-middle-ages/Kupiskis.png');
    addHeraldicIcon('Anyksciai', '../img/map-regions-icons-middle-ages/Anyksciai.png');
    addHeraldicIcon('Ukmerge', '../img/map-regions-icons-middle-ages/Ukmerge.png');
    addHeraldicIcon('Ionava', '../img/map-regions-icons-middle-ages/Ionava.png');
    addHeraldicIcon('Kaunas', '../img/map-regions-icons-middle-ages/Kaunas.png');
    addHeraldicIcon('Kazlu_Ruda', '../img/map-regions-icons-middle-ages/Kazlu Ruda.png');
    addHeraldicIcon('Vilaviskis', '../img/map-regions-icons-middle-ages/Vilaviskis.png');
    addHeraldicIcon('Sakiai', '../img/map-regions-icons-middle-ages/Sakiai.png');
    addHeraldicIcon('Jurbarkas', '../img/map-regions-icons-middle-ages/Jurbarkas.png');
    addHeraldicIcon('Raseiniai', '../img/map-regions-icons-middle-ages/Raseiniai.png');
    addHeraldicIcon('Kedainiai', '../img/map-regions-icons-middle-ages/Kedainiai.png');
    addHeraldicIcon('Panevezys', '../img/map-regions-icons-middle-ages/Panevezys.png');
    addHeraldicIcon('Birzai', '../img/map-regions-icons-middle-ages/Birzai.png');
    addHeraldicIcon('Pasvalys', '../img/map-regions-icons-middle-ages/Pasvalys.png');
    addHeraldicIcon('Radviliskis', '../img/map-regions-icons-middle-ages/Radviliskis.png');
    addHeraldicIcon('Pakruojis', '../img/map-regions-icons-middle-ages/Pakruojis.png');
    addHeraldicIcon('Joniskis', '../img/map-regions-icons-middle-ages/Joniskis.png');
    addHeraldicIcon('Siauliai', '../img/map-regions-icons-middle-ages/Siauliai.png');
    addHeraldicIcon('Kelme', '../img/map-regions-icons-middle-ages/Kelme.png');
    addHeraldicIcon('Silale', '../img/map-regions-icons-middle-ages/Silale.png');
    addHeraldicIcon('Taurage', '../img/map-regions-icons-middle-ages/Taurage.png');
    addHeraldicIcon('Pagegiai', '../img/map-regions-icons-middle-ages/Pagegiai.png');
    addHeraldicIcon('Akmene', '../img/map-regions-icons-middle-ages/Akmene.png');
    addHeraldicIcon('Mazeikiai', '../img/map-regions-icons-middle-ages/Mazeikiai.png');
    addHeraldicIcon('Telsiai', '../img/map-regions-icons-middle-ages/Telsiai.png');
    addHeraldicIcon('Rietavas', '../img/map-regions-icons-middle-ages/Rietavas.png');
    addHeraldicIcon('Plunge', '../img/map-regions-icons-middle-ages/Plunge.png');
    addHeraldicIcon('Skuodas', '../img/map-regions-icons-middle-ages/Skuodas.png');
    addHeraldicIcon('Kretinga', '../img/map-regions-icons-middle-ages/Kretinga.png');
    addHeraldicIcon('Klaipeda', '../img/map-regions-icons-middle-ages/Klaipeda.png');
    addHeraldicIcon('Silute', '../img/map-regions-icons-middle-ages/Silute.png');
    addHeraldicIcon('Hajnowka', '../img/map-regions-icons-middle-ages/Hajnowka.png');
    addHeraldicIcon('Siemiatycze', '../img/map-regions-icons-middle-ages/Siemiatycze.png');
    addHeraldicIcon('Bielsk_Podlaski', '../img/map-regions-icons-middle-ages/Bielsk Podlaski.png');
    addHeraldicIcon('Bialystok', '../img/map-regions-icons-middle-ages/Bialystok.png');
    addHeraldicIcon('Sokolka', '../img/map-regions-icons-middle-ages/Sokolka.png');
    addHeraldicIcon('Augustow', '../img/map-regions-icons-middle-ages/Augustow.png');
    addHeraldicIcon('Sejny', '../img/map-regions-icons-middle-ages/Sejny.png');
    addHeraldicIcon('Suwalki', '../img/map-regions-icons-middle-ages/Suwalki.png');
    addHeraldicIcon('Wysokie_Mazowieckie', '../img/map-regions-icons-middle-ages/Wysokie Mazowieckie.png');
    addHeraldicIcon('Monki', '../img/map-regions-icons-middle-ages/Monki.png');
    addHeraldicIcon('Zambrow', '../img/map-regions-icons-middle-ages/Zambrow.png');
    addHeraldicIcon('Lomza', '../img/map-regions-icons-middle-ages/Lomza.png');
    addHeraldicIcon('Grajewo', '../img/map-regions-icons-middle-ages/Grajewo.png');
    addHeraldicIcon('Kolno', '../img/map-regions-icons-middle-ages/Kolno.png');

    addHeraldicIcon('Kovel', '../img/map-regions-icons-middle-ages/Kovel.png');
    addHeraldicIcon('Kamen-Kashirski', '../img/map-regions-icons-middle-ages/Kamen-Kashirski.png');
    addHeraldicIcon('Volodimir', '../img/map-regions-icons-middle-ages/Volodimir.png');
    addHeraldicIcon('Lutsk', '../img/map-regions-icons-middle-ages/Lutsk.png');
    addHeraldicIcon('Varash', '../img/map-regions-icons-middle-ages/Varash.png');
    addHeraldicIcon('Sarni', '../img/map-regions-icons-middle-ages/Sarni.png');
    addHeraldicIcon('Rivne', '../img/map-regions-icons-middle-ages/Rivne.png');
    addHeraldicIcon('Dubno', '../img/map-regions-icons-middle-ages/Dubno.png');
    addHeraldicIcon('Zvyagel', '../img/map-regions-icons-middle-ages/Zvyagel.png');
    addHeraldicIcon('Korosten', '../img/map-regions-icons-middle-ages/Korosten.png');
    addHeraldicIcon('Zhitomir', '../img/map-regions-icons-middle-ages/Zhitomir.png');
    addHeraldicIcon('Berdichiv', '../img/map-regions-icons-middle-ages/Berdichiv.png');
    addHeraldicIcon('Vishgorod', '../img/map-regions-icons-middle-ages/Vishgorod.png');
    addHeraldicIcon('Bucha', '../img/map-regions-icons-middle-ages/Bucha.png')
    addHeraldicIcon('Fastiv', '../img/map-regions-icons-middle-ages/Fastiv.png');
    addHeraldicIcon('Kyiv', '../img/map-regions-icons-middle-ages/Kyiv.png');
    addHeraldicIcon('Brovari', '../img/map-regions-icons-middle-ages/Brovari.png');
    addHeraldicIcon('Borispil', '../img/map-regions-icons-middle-ages/Borispil.png');
    addHeraldicIcon('Obuhiv', '../img/map-regions-icons-middle-ages/Obuhiv.png');
    addHeraldicIcon('Bila_Cerkva', '../img/map-regions-icons-middle-ages/Bila_Cerkva.png');
    addHeraldicIcon('Nizhin', '../img/map-regions-icons-middle-ages/Nizhin.png');
    addHeraldicIcon('Chernigiv', '../img/map-regions-icons-middle-ages/Chernigiv.png');
    addHeraldicIcon('Korukivka', '../img/map-regions-icons-middle-ages/Korukivka.png');
    addHeraldicIcon('Novgorod-Siverskii', '../img/map-regions-icons-middle-ages/Novgorod-Siverskii.png');
    addHeraldicIcon('Priluki', '../img/map-regions-icons-middle-ages/Priluki.png');

    // Добавляйте гербы для других регионов
}

function hideHeraldicIcons() {
    // Логика для скрытия гербов (удаление или скрытие SVG элементов с гербами)
    const heraldicIcons = document.querySelectorAll("#global_region_map svg image");
    heraldicIcons.forEach(icon => icon.remove());
}

function bringToFront(landId) {
    const landPolygon = document.getElementById(landId.replace(' ', '_'));
    landPolygon.parentNode.appendChild(landPolygon);  // Перемещаем элемент в конец
}

function sendToBack(landId) {
    const landPolygon = document.getElementById(landId.replace(' ', '_'));
    const parent = landPolygon.parentNode;
    parent.insertBefore(landPolygon, parent.firstChild);  // Возвращаем элемент в начало
}

document.querySelectorAll('polygon').forEach(polygon => {
    const landId = polygon.id;  // Получаем id региона

    polygon.addEventListener('mouseenter', function () {
        bringToFront(landId);
    });

    polygon.addEventListener('mouseleave', function () {
        sendToBack(landId);
    });
});

// Объект для хранения ссылок для каждого региона
// Объект для хранения ссылок для каждого региона
var regionLinks = {
    'Hrodna': 'https://ru.wikipedia.org/wiki/%D0%98%D1%81%D1%82%D0%BE%D1%80%D0%B8%D1%8F_%D0%93%D1%80%D0%BE%D0%B4%D0%BD%D0%BE',
    'Bierastavica': 'https://ru.wikipedia.org/wiki/%D0%91%D0%BE%D0%BB%D1%8C%D1%88%D0%B0%D1%8F_%D0%91%D0%B5%D1%80%D0%B5%D1%81%D1%82%D0%BE%D0%B2%D0%B8%D1%86%D0%B0',
    'Masty': 'https://ru.wikipedia.org/wiki/%D0%9C%D0%BE%D1%81%D1%82%D1%8B_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4)',
    'Vaukavysk': 'https://ru.wikipedia.org/wiki/%D0%92%D0%BE%D0%BB%D0%BA%D0%BE%D0%B2%D1%8B%D1%81%D0%BA',
    'Zelva': 'https://ru.wikipedia.org/wiki/%D0%97%D0%B5%D0%BB%D1%8C%D0%B2%D0%B0',
    'Svislach': 'https://ru.wikipedia.org/wiki/%D0%A1%D0%B2%D0%B8%D1%81%D0%BB%D0%BE%D1%87%D1%8C_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4)',
    'Slonim': 'https://shtetlroutes.eu/ru/slonim-cultural-heritage-card/',
    'Dzyatlava': 'https://shtetlroutes.eu/ru/dzyatlava-cultural-heritage-card/',
    'Shchuchin': 'https://ru.wikipedia.org/wiki/%D0%A9%D1%83%D1%87%D0%B8%D0%BD_(%D0%91%D0%B5%D0%BB%D0%BE%D1%80%D1%83%D1%81%D1%81%D0%B8%D1%8F)',
    'Voranava': 'https://ru.wikipedia.org/wiki/%D0%92%D0%BE%D1%80%D0%BE%D0%BD%D0%BE%D0%B2%D0%BE_(%D0%93%D1%80%D0%BE%D0%B4%D0%BD%D0%B5%D0%BD%D1%81%D0%BA%D0%B0%D1%8F_%D0%BE%D0%B1%D0%BB%D0%B0%D1%81%D1%82%D1%8C)',
    'Lida': 'https://ru.wikipedia.org/wiki/%D0%9B%D0%B8%D0%B4%D0%B0',
    'Navahrudak': 'https://shtetlroutes.eu/ru/navahrudak-cultural-heritage-card/',
    'Karelichy': 'https://a-taurus.by/regiony/goroda-belarusi/goroda-grodnenskoj-oblasti/istorija-korelichi/',
    'Iue': 'https://shtetlroutes.eu/ru/iwye-cultural-heritage-card/',
    'Ashmjany': 'https://shtetlroutes.eu/ru/ashmyany-cultural-heritage-card/',
    'Smargon': 'https://ru.wikipedia.org/wiki/%D0%A1%D0%BC%D0%BE%D1%80%D0%B3%D0%BE%D0%BD%D1%8C',
    'Astravets': 'https://ru.wikipedia.org/wiki/%D0%9E%D1%81%D1%82%D1%80%D0%BE%D0%B2%D0%B5%D1%86',

    'Pastavy': 'https://ru.wikipedia.org/wiki/%D0%9F%D0%BE%D1%81%D1%82%D0%B0%D0%B2%D1%8B',
    'Braslau': 'https://www.braslaw.by/history',
    'Sharkaushchyna': 'https://ru.wikipedia.org/wiki/%D0%A8%D0%B0%D1%80%D0%BA%D0%BE%D0%B2%D1%89%D0%B8%D0%BD%D0%B0',
    'Dokshytsy': 'https://ru.wikipedia.org/wiki/%D0%94%D0%BE%D0%BA%D1%88%D0%B8%D1%86%D1%8B',
    'Glubokae': 'https://glubmusej.by/ru/nashe-nasledie/istoriya-glubochchiny/2011-07-27-07-34-25',
    'Miyory': 'https://ru.wikipedia.org/wiki/%D0%9C%D0%B8%D0%BE%D1%80%D1%8B',
    'Verhnyadzvinsk': 'https://belta.by/regions/view/dose-k-630-letiju-goroda-verhnedvinska-212100-2016/',
    'Lepel': 'https://ru.wikipedia.org/wiki/%D0%9B%D0%B5%D0%BF%D0%B5%D0%BB%D1%8C',
    'Chashniki': 'https://ru.wikipedia.org/wiki/%D0%A7%D0%B0%D1%88%D0%BD%D0%B8%D0%BA%D0%B8',
    'Talachyn': 'https://ru.wikipedia.org/wiki/%D0%A2%D0%BE%D0%BB%D0%BE%D1%87%D0%B8%D0%BD',
    'Orsha': 'https://ru.wikipedia.org/wiki/%D0%9E%D1%80%D1%88%D0%B0',
    'Dubrouna': 'https://ru.wikipedia.org/wiki/%D0%94%D1%83%D0%B1%D1%80%D0%BE%D0%B2%D0%BD%D0%BE',
    'Lyozna': 'https://liozno.by/history/',
    'Vitebsk': 'https://ru.wikipedia.org/wiki/%D0%98%D1%81%D1%82%D0%BE%D1%80%D0%B8%D1%8F_%D0%92%D0%B8%D1%82%D0%B5%D0%B1%D1%81%D0%BA%D0%B0',
    'Beshankovichy': 'https://ru.wikipedia.org/wiki/%D0%91%D0%B5%D1%88%D0%B5%D0%BD%D0%BA%D0%BE%D0%B2%D0%B8%D1%87%D0%B8',
    'Shumilina': 'https://ru.wikipedia.org/wiki/%D0%A8%D1%83%D0%BC%D0%B8%D0%BB%D0%B8%D0%BD%D0%BE',
    'Ushachi': 'https://ru.wikipedia.org/wiki/%D0%A3%D1%88%D0%B0%D1%87%D0%B8',
    'Polatsk': 'https://ru.wikipedia.org/wiki/%D0%98%D1%81%D1%82%D0%BE%D1%80%D0%B8%D1%8F_%D0%9F%D0%BE%D0%BB%D0%BE%D1%86%D0%BA%D0%B0',
    'Haradok': 'https://ru.wikipedia.org/wiki/%D0%93%D0%BE%D1%80%D0%BE%D0%B4%D0%BE%D0%BA_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4,_%D0%91%D0%B5%D0%BB%D0%BE%D1%80%D1%83%D1%81%D1%81%D0%B8%D1%8F)',
    'Rasony': 'https://ru.wikipedia.org/wiki/%D0%A0%D0%BE%D1%81%D1%81%D0%BE%D0%BD%D1%8B',

    'Horki': 'https://ru.wikipedia.org/wiki/%D0%93%D0%BE%D1%80%D0%BA%D0%B8_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4)',
    'Drybin': 'http://shtetle.com/shtetls_mog/dribin/dribin.html',
    'Mstislaul': 'https://ru.wikipedia.org/wiki/%D0%9C%D1%81%D1%82%D0%B8%D1%81%D0%BB%D0%B0%D0%B2%D0%BB%D1%8C#:~:text=%D0%93%D0%BE%D1%80%D0%BE%D0%B4%20%D0%B1%D1%8B%D0%BB%20%D0%BE%D1%81%D0%BD%D0%BE%D0%B2%D0%B0%D0%BD%20%D0%B2%201135,%D0%B8%20%D0%B2%D0%B0%D0%BB%D0%B0%D0%BC%D0%B8%2C%20%D0%B8%20%D0%BE%D0%BA%D0%BE%D0%BB%D1%8C%D0%BD%D0%BE%D0%B3%D0%BE%20%D0%B3%D0%BE%D1%80%D0%BE%D0%B4%D0%B0.',
    'Shklou': 'https://ru.wikipedia.org/wiki/%D0%A8%D0%BA%D0%BB%D0%BE%D0%B2',
    'Kruhlae': 'https://ru.wikipedia.org/wiki/%D0%9A%D1%80%D1%83%D0%B3%D0%BB%D0%BE%D0%B5_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4)',
    'Byalynichy': 'https://ru.wikipedia.org/wiki/%D0%91%D0%B5%D0%BB%D1%8B%D0%BD%D0%B8%D1%87%D0%B8',
    'Klichau': 'https://ru.wikipedia.org/wiki/%D0%9A%D0%BB%D0%B8%D1%87%D0%B5%D0%B2',
    'Asipovichy': 'https://ru.wikipedia.org/wiki/%D0%9E%D1%81%D0%B8%D0%BF%D0%BE%D0%B2%D0%B8%D1%87%D0%B8',
    'Hlusk': 'https://ru.wikipedia.org/wiki/%D0%93%D0%BB%D1%83%D1%81%D0%BA',
    'Babruisk': 'https://ru.wikipedia.org/wiki/%D0%91%D0%BE%D0%B1%D1%80%D1%83%D0%B9%D1%81%D0%BA',
    'Kirausk': 'https://ru.wikipedia.org/wiki/%D0%9A%D0%B8%D1%80%D0%BE%D0%B2%D1%81%D0%BA_(%D0%91%D0%B5%D0%BB%D0%BE%D1%80%D1%83%D1%81%D1%81%D0%B8%D1%8F)',
    'Byhau': 'https://ru.wikipedia.org/wiki/%D0%91%D1%8B%D1%85%D0%BE%D0%B2',
    'Mahilou': 'https://ru.wikipedia.org/wiki/%D0%98%D1%81%D1%82%D0%BE%D1%80%D0%B8%D1%8F_%D0%9C%D0%BE%D0%B3%D0%B8%D0%BB%D1%91%D0%B2%D0%B0',
    'Chausy': 'https://ru.wikipedia.org/wiki/%D0%A7%D0%B0%D1%83%D1%81%D1%8B',
    'Cherykau': 'https://ru.wikipedia.org/wiki/%D0%A7%D0%B5%D1%80%D0%B8%D0%BA%D0%BE%D0%B2',
    'Krychau': 'https://ru.wikipedia.org/wiki/%D0%9A%D1%80%D0%B8%D1%87%D0%B5%D0%B2',
    'Klimavichy': 'https://ru.wikipedia.org/wiki/%D0%9A%D0%BB%D0%B8%D0%BC%D0%BE%D0%B2%D0%B8%D1%87%D0%B8',
    'Khotimsk': 'https://ru.wikipedia.org/wiki/%D0%A5%D0%BE%D1%82%D0%B8%D0%BC%D1%81%D0%BA',
    'Kastsukovichy': 'https://tutejszy.ru/mogiljovskaya/kostyukovichskij/kostyukovichi/484-istoriya-kostyukovich',
    'Krasnapolle': 'https://krasnopolie.gov.by/region/istoriia',
    'Slauharad': 'https://ru.wikipedia.org/wiki/%D0%A1%D0%BB%D0%B0%D0%B2%D0%B3%D0%BE%D1%80%D0%BE%D0%B4_(%D0%91%D0%B5%D0%BB%D0%BE%D1%80%D1%83%D1%81%D1%81%D0%B8%D1%8F)',

    'Karma': 'https://ru.wikipedia.org/wiki/%D0%9A%D0%BE%D1%80%D0%BC%D0%B0_(%D0%9A%D0%BE%D1%80%D0%BC%D1%8F%D0%BD%D1%81%D0%BA%D0%B8%D0%B9_%D1%80%D0%B0%D0%B9%D0%BE%D0%BD)',
    'Chachersk': 'https://ru.wikipedia.org/wiki/%D0%A7%D0%B5%D1%87%D0%B5%D1%80%D1%81%D0%BA',
    'Rahachou': 'https://ru.wikipedia.org/wiki/%D0%A0%D0%BE%D0%B3%D0%B0%D1%87%D1%91%D0%B2',
    'Zhlobin': 'https://ru.wikipedia.org/wiki/%D0%96%D0%BB%D0%BE%D0%B1%D0%B8%D0%BD',
    'Svetlahorsk': 'https://ru.wikipedia.org/wiki/%D0%A1%D0%B2%D0%B5%D1%82%D0%BB%D0%BE%D0%B3%D0%BE%D1%80%D1%81%D0%BA_(%D0%91%D0%B5%D0%BB%D0%BE%D1%80%D1%83%D1%81%D1%81%D0%B8%D1%8F)',
    'Aktsyabrski': 'https://ru.wikipedia.org/wiki/%D0%9E%D0%BA%D1%82%D1%8F%D0%B1%D1%80%D1%8C%D1%81%D0%BA%D0%B8%D0%B9_(%D0%93%D0%BE%D0%BC%D0%B5%D0%BB%D1%8C%D1%81%D0%BA%D0%B0%D1%8F_%D0%BE%D0%B1%D0%BB%D0%B0%D1%81%D1%82%D1%8C)',
    'Petrykau': 'https://belarus-travel.livejournal.com/214761.html',
    'Zhytkavichy': 'https://www.zhitkovichi.gov.by/ru/history-ru/',
    'Lelchytsy': 'https://ru.wikipedia.org/wiki/%D0%9B%D0%B5%D0%BB%D1%8C%D1%87%D0%B8%D1%86%D1%8B',
    'Kalinkavichy': 'https://ru.wikipedia.org/wiki/%D0%9A%D0%B0%D0%BB%D0%B8%D0%BD%D0%BA%D0%BE%D0%B2%D0%B8%D1%87%D0%B8',
    'Mazyr': 'https://mozyrisp.gov.by/ru/region/istoriya.html',
    'Yelsk': 'https://ru.wikipedia.org/wiki/%D0%95%D0%BB%D1%8C%D1%81%D0%BA',
    'Naroulya': 'https://ru.wikipedia.org/wiki/%D0%9D%D0%B0%D1%80%D0%BE%D0%B2%D0%BB%D1%8F',
    'Khoiniki': 'https://ru.wikipedia.org/wiki/%D0%A5%D0%BE%D0%B9%D0%BD%D0%B8%D0%BA%D0%B8',
    'Brahin': 'https://gp.by/novosti/spetsialno-dlya-gp-/news36000.html',
    'Rechytsa': 'https://ru.wikipedia.org/wiki/%D0%A0%D0%B5%D1%87%D0%B8%D1%86%D0%B0',
    'Loeu': 'https://ru.wikipedia.org/wiki/%D0%9B%D0%BE%D0%B5%D0%B2',
    'Buda-Kashalyova': 'https://ru.wikipedia.org/wiki/%D0%91%D1%83%D0%B4%D0%B0-%D0%9A%D0%BE%D1%88%D0%B5%D0%BB%D1%91%D0%B2%D0%BE',
    'Homel': 'https://ru.wikipedia.org/wiki/%D0%98%D1%81%D1%82%D0%BE%D1%80%D0%B8%D1%8F_%D0%93%D0%BE%D0%BC%D0%B5%D0%BB%D1%8F',
    'Vetka': 'https://ru.wikipedia.org/wiki/%D0%92%D0%B5%D1%82%D0%BA%D0%B0_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4)',
    'Dobrush': 'https://ru.wikipedia.org/wiki/%D0%94%D0%BE%D0%B1%D1%80%D1%83%D1%88',

    'Salihorsk': 'https://ru.wikipedia.org/wiki/%D0%A1%D0%BE%D0%BB%D0%B8%D0%B3%D0%BE%D1%80%D1%81%D0%BA',
    'Liuban': 'https://belta.by/culture/view/dose-k-450-letiju-goroda-ljubani-211145-2016/',
    'Staryja_Darohi': 'https://ru.wikipedia.org/wiki/%D0%A1%D1%82%D0%B0%D1%80%D1%8B%D0%B5_%D0%94%D0%BE%D1%80%D0%BE%D0%B3%D0%B8',
    'Puhavichy': 'https://ru.wikipedia.org/wiki/%D0%9C%D0%B0%D1%80%D1%8C%D0%B8%D0%BD%D0%B0_%D0%93%D0%BE%D1%80%D0%BA%D0%B0',
    'Cherven': 'https://ru.wikipedia.org/wiki/%D0%A7%D0%B5%D1%80%D0%B2%D0%B5%D0%BD%D1%8C',
    'Berazino': 'https://ru.wikipedia.org/wiki/%D0%91%D0%B5%D1%80%D0%B5%D0%B7%D0%B8%D0%BD%D0%BE_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4)',
    'Krupki': 'https://ru.wikipedia.org/wiki/%D0%9A%D1%80%D1%83%D0%BF%D0%BA%D0%B8',
    'Barysau': 'https://ru.wikipedia.org/wiki/%D0%91%D0%BE%D1%80%D0%B8%D1%81%D0%BE%D0%B2_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4)',
    'Smalyavichy': 'https://ru.wikipedia.org/wiki/%D0%A1%D0%BC%D0%BE%D0%BB%D0%B5%D0%B2%D0%B8%D1%87%D0%B8',
    'Lahoisk': 'https://ru.wikipedia.org/wiki/%D0%9B%D0%BE%D0%B3%D0%BE%D0%B9%D1%81%D0%BA',
    'Myadzel': 'https://ru.wikipedia.org/wiki/%D0%9C%D1%8F%D0%B4%D0%B5%D0%BB%D1%8C',
    'Vileika': 'https://ru.wikipedia.org/wiki/%D0%92%D0%B8%D0%BB%D0%B5%D0%B9%D0%BA%D0%B0',
    'Maladzechna': 'https://ru.wikipedia.org/wiki/%D0%9C%D0%BE%D0%BB%D0%BE%D0%B4%D0%B5%D1%87%D0%BD%D0%BE',
    'Minsk': 'https://ru.wikipedia.org/wiki/%D0%98%D1%81%D1%82%D0%BE%D1%80%D0%B8%D1%8F_%D0%9C%D0%B8%D0%BD%D1%81%D0%BA%D0%B0',
    'Valozhyn': 'https://ru.wikipedia.org/wiki/%D0%92%D0%BE%D0%BB%D0%BE%D0%B6%D0%B8%D0%BD',
    'Staubcy': 'https://ru.wikipedia.org/wiki/%D0%A1%D1%82%D0%BE%D0%BB%D0%B1%D1%86%D1%8B',
    'Dzyarzhynsk': 'https://ru.wikipedia.org/wiki/%D0%98%D1%81%D1%82%D0%BE%D1%80%D0%B8%D1%8F_%D0%94%D0%B7%D0%B5%D1%80%D0%B6%D0%B8%D0%BD%D1%81%D0%BA%D0%B0_(%D0%9C%D0%B8%D0%BD%D1%81%D0%BA%D0%B0%D1%8F_%D0%BE%D0%B1%D0%BB%D0%B0%D1%81%D1%82%D1%8C)',
    'Uzda': 'https://ru.wikipedia.org/wiki/%D0%A3%D0%B7%D0%B4%D0%B0_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4)',
    'Slutsk': 'https://ru.wikipedia.org/wiki/%D0%A1%D0%BB%D1%83%D1%86%D0%BA',
    'Kapyl': 'https://ru.wikipedia.org/wiki/%D0%9A%D0%BE%D0%BF%D1%8B%D0%BB%D1%8C',
    'Nyasvizh': 'https://ru.wikipedia.org/wiki/%D0%9D%D0%B5%D1%81%D0%B2%D0%B8%D0%B6',
    'Kletsk': 'https://ru.wikipedia.org/wiki/%D0%9A%D0%BB%D0%B5%D1%86%D0%BA',

    'Baranavichy': 'https://ru.wikipedia.org/wiki/%D0%91%D0%B0%D1%80%D0%B0%D0%BD%D0%BE%D0%B2%D0%B8%D1%87%D0%B8',
    'Lyahavichy': 'https://ru.wikipedia.org/wiki/%D0%9B%D1%8F%D1%85%D0%BE%D0%B2%D0%B8%D1%87%D0%B8',
    'Hantsavichy': 'https://ru.wikipedia.org/wiki/%D0%93%D0%B0%D0%BD%D1%86%D0%B5%D0%B2%D0%B8%D1%87%D0%B8#:~:text=%D0%93%D0%BE%D1%80%D0%BE%D0%B4%20%D0%93%D0%B0%D0%BD%D1%86%D0%B5%D0%B2%D0%B8%D1%87%D0%B8%20%D0%BE%D1%81%D0%BD%D0%BE%D0%B2%D0%B0%D0%BD%20%D0%B2%201898,%E2%80%94%20%D0%A0%D0%B0%D0%B4%D0%B8%D0%BE%D0%BB%D0%BE%D0%BA%D0%B0%D1%86%D0%B8%D0%BE%D0%BD%D0%BD%D0%B0%D1%8F%20%D1%81%D1%82%D0%B0%D0%BD%D1%86%D0%B8%D1%8F%20%C2%AB%D0%92%D0%BE%D0%BB%D0%B3%D0%B0%C2%BB.',
    'Luninec': 'https://ru.wikipedia.org/wiki/%D0%9B%D1%83%D0%BD%D0%B8%D0%BD%D0%B5%D1%86',
    'Stolin': 'https://ru.wikipedia.org/wiki/%D0%A1%D1%82%D0%BE%D0%BB%D0%B8%D0%BD',
    'Pinsk': 'https://ru.wikipedia.org/wiki/%D0%9F%D0%B8%D0%BD%D1%81%D0%BA',
    'Ivatsevichy': 'https://ru.wikipedia.org/wiki/%D0%98%D0%B2%D0%B0%D1%86%D0%B5%D0%B2%D0%B8%D1%87%D0%B8',
    'Ivanava': 'https://ru.wikipedia.org/wiki/%D0%98%D0%B2%D0%B0%D0%BD%D0%BE%D0%B2%D0%BE_(%D0%91%D1%80%D0%B5%D1%81%D1%82%D1%81%D0%BA%D0%B0%D1%8F_%D0%BE%D0%B1%D0%BB%D0%B0%D1%81%D1%82%D1%8C)',
    'Byaroza': 'https://ru.wikipedia.org/wiki/%D0%91%D0%B5%D1%80%D1%91%D0%B7%D0%B0_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4)#:~:text=%D0%9F%D0%B5%D1%80%D0%B2%D0%BE%D0%B5%20%D1%83%D0%BF%D0%BE%D0%BC%D0%B8%D0%BD%D0%B0%D0%BD%D0%B8%D0%B5%20%D0%BE%20%D0%B3%D0%BE%D1%80%D0%BE%D0%B4%D0%B5%20%D0%91%D0%B5%D1%80%D1%91%D0%B7%D0%B0,1600%20%D0%B1%D1%8B%D0%BB%20%D0%BA%D1%80%D1%83%D0%BF%D0%BD%D1%8B%D0%BC%20%D1%86%D0%B5%D0%BD%D1%82%D1%80%D0%BE%D0%BC%20%D0%BA%D0%B0%D0%BB%D1%8C%D0%B2%D0%B8%D0%BD%D0%B8%D0%B7%D0%BC%D0%B0.',
    'Pruzhany': 'https://ru.wikipedia.org/wiki/%D0%9F%D1%80%D1%83%D0%B6%D0%B0%D0%BD%D1%8B',
    'Kamyanec': 'https://ru.wikipedia.org/wiki/%D0%9A%D0%B0%D0%BC%D0%B5%D0%BD%D0%B5%D1%86',
    'Brest': 'https://ru.wikipedia.org/wiki/%D0%91%D1%80%D0%B5%D1%81%D1%82',
    'Zhabinka': 'https://ru.wikipedia.org/wiki/%D0%96%D0%B0%D0%B1%D0%B8%D0%BD%D0%BA%D0%B0',
    'Malaryta': 'https://ru.wikipedia.org/wiki/%D0%9C%D0%B0%D0%BB%D0%BE%D1%80%D0%B8%D1%82%D0%B0',
    'Kobryn': 'https://ru.wikipedia.org/wiki/%D0%9A%D0%BE%D0%B1%D1%80%D0%B8%D0%BD',
    'Drahichyn': 'https://ru.wikipedia.org/wiki/%D0%94%D1%80%D0%BE%D0%B3%D0%B8%D1%87%D0%B8%D0%BD#:~:text=%D0%92%20%D0%BF%D0%B8%D1%81%D1%8C%D0%BC%D0%B5%D0%BD%D0%BD%D1%8B%D1%85%20%D0%B8%D1%81%D1%82%D0%BE%D1%87%D0%BD%D0%B8%D0%BA%D0%B0%D1%85%20%D0%94%D1%80%D0%BE%D0%B3%D0%B8%D1%87%D0%B8%D0%BD%20%D0%B2%D0%BF%D0%B5%D1%80%D0%B2%D1%8B%D0%B5,%D0%BE%D1%84%D0%B8%D1%86%D0%B8%D0%B0%D0%BB%D1%8C%D0%BD%D1%8B%D1%85%20%D0%BD%D0%B0%D0%B7%D0%B2%D0%B0%D0%BD%D0%B8%D1%8F%3A%20%D0%94%D1%80%D0%BE%D0%B3%D0%B8%D1%87%D0%B8%D0%BD%20%D0%B8%20%D0%94%D0%BE%D0%B2%D0%B5%D1%87%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87%D0%B8.',

    'Ershichi': 'https://smolbattle.ru/threads/%D0%95%D1%80%D1%88%D0%B8%D1%87%D0%B8-%D0%BF%D1%83%D1%82%D0%B5%D1%88%D0%B5%D1%81%D1%82%D0%B2%D0%B8%D0%B5-%D0%B2-%D0%BF%D1%80%D0%BE%D1%88%D0%BB%D0%BE%D0%B5.53370/',
    'Shumyachi': 'https://ru.wikipedia.org/wiki/%D0%A8%D1%83%D0%BC%D1%8F%D1%87%D0%B8',
    'Roslavl': 'https://ru.wikipedia.org/wiki/%D0%A0%D0%BE%D1%81%D0%BB%D0%B0%D0%B2%D0%BB%D1%8C',
    'Hislavichi': 'https://ru.wikipedia.org/wiki/%D0%A5%D0%B8%D1%81%D0%BB%D0%B0%D0%B2%D0%B8%D1%87%D0%B8',
    'Monastyrschina': 'https://ru.wikipedia.org/wiki/%D0%9C%D0%BE%D0%BD%D0%B0%D1%81%D1%82%D1%8B%D1%80%D1%89%D0%B8%D0%BD%D0%B0_(%D0%9C%D0%BE%D0%BD%D0%B0%D1%81%D1%82%D1%8B%D1%80%D1%89%D0%B8%D0%BD%D1%81%D0%BA%D0%B8%D0%B9_%D1%80%D0%B0%D0%B9%D0%BE%D0%BD)',
    'Krasny': 'https://ru.wikipedia.org/wiki/%D0%9A%D1%80%D0%B0%D1%81%D0%BD%D1%8B%D0%B9_(%D0%A1%D0%BC%D0%BE%D0%BB%D0%B5%D0%BD%D1%81%D0%BA%D0%B0%D1%8F_%D0%BE%D0%B1%D0%BB%D0%B0%D1%81%D1%82%D1%8C)',
    'Rudnya': 'https://ru.wikipedia.org/wiki/%D0%A0%D1%83%D0%B4%D0%BD%D1%8F_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4)',
    'Pochinok': 'https://ru.wikipedia.org/wiki/%D0%9F%D0%BE%D1%87%D0%B8%D0%BD%D0%BE%D0%BA_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4)',
    'Smolensk': 'https://ru.wikipedia.org/wiki/%D0%98%D1%81%D1%82%D0%BE%D1%80%D0%B8%D1%8F_%D0%A1%D0%BC%D0%BE%D0%BB%D0%B5%D0%BD%D1%81%D0%BA%D0%B0',
    'Demidov': 'https://ru.wikipedia.org/wiki/%D0%94%D0%B5%D0%BC%D0%B8%D0%B4%D0%BE%D0%B2_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4)',
    'Velizh': 'https://ru.wikipedia.org/wiki/%D0%92%D0%B5%D0%BB%D0%B8%D0%B6',
    'Elnya': 'https://ru.wikipedia.org/wiki/%D0%95%D0%BB%D1%8C%D0%BD%D1%8F',
    'Glinka': 'https://ru.wikipedia.org/wiki/%D0%93%D0%BB%D0%B8%D0%BD%D0%BA%D0%B0_(%D0%A1%D0%BC%D0%BE%D0%BB%D0%B5%D0%BD%D1%81%D0%BA%D0%B0%D1%8F_%D0%BE%D0%B1%D0%BB%D0%B0%D1%81%D1%82%D1%8C)',
    'Kardymovo': 'https://ru.wikipedia.org/wiki/%D0%9A%D0%B0%D1%80%D0%B4%D1%8B%D0%BC%D0%BE%D0%B2%D0%BE_(%D0%A1%D0%BC%D0%BE%D0%BB%D0%B5%D0%BD%D1%81%D0%BA%D0%B0%D1%8F_%D0%BE%D0%B1%D0%BB%D0%B0%D1%81%D1%82%D1%8C)',
    'Duhovschina': 'https://ru.wikipedia.org/wiki/%D0%94%D1%83%D1%85%D0%BE%D0%B2%D1%89%D0%B8%D0%BD%D0%B0',
    'Yarcevo': 'https://ru.wikipedia.org/wiki/%D0%AF%D1%80%D1%86%D0%B5%D0%B2%D0%BE',
    'Dorogobuzh': 'https://ru.wikipedia.org/wiki/%D0%94%D0%BE%D1%80%D0%BE%D0%B3%D0%BE%D0%B1%D1%83%D0%B6#:~:text=11%20%D0%A1%D1%81%D1%8B%D0%BB%D0%BA%D0%B8-,%D0%98%D1%81%D1%82%D0%BE%D1%80%D0%B8%D1%8F,%D0%B8%20%D0%BE%D0%BF%D1%83%D1%81%D1%82%D0%BE%D1%88%D0%B8%D0%BB%20%D0%B2%D0%BE%D1%81%D1%82%D0%BE%D1%87%D0%BD%D1%8B%D0%B5%20%D0%BF%D1%80%D0%B5%D0%B4%D0%B5%D0%BB%D1%8B%20%D0%BA%D0%BD%D1%8F%D0%B6%D0%B5%D1%81%D1%82%D0%B2%D0%B0.',
    'Safonovo': 'https://ru.wikipedia.org/wiki/%D0%A1%D0%B0%D1%84%D0%BE%D0%BD%D0%BE%D0%B2%D0%BE_(%D0%B3%D0%BE%D1%80%D0%BE%D0%B4)',
    'Holm-Zhirkovski': 'https://ru.wikipedia.org/wiki/%D0%A5%D0%BE%D0%BB%D0%BC-%D0%96%D0%B8%D1%80%D0%BA%D0%BE%D0%B2%D1%81%D0%BA%D0%B8%D0%B9',
    'Ugra': 'https://ru.wikipedia.org/wiki/%D0%A3%D0%B3%D1%80%D0%B0_(%D0%A1%D0%BC%D0%BE%D0%BB%D0%B5%D0%BD%D1%81%D0%BA%D0%B0%D1%8F_%D0%BE%D0%B1%D0%BB%D0%B0%D1%81%D1%82%D1%8C)',
    'Vyazma': 'https://ru.wikipedia.org/wiki/%D0%92%D1%8F%D0%B7%D1%8C%D0%BC%D0%B0',
    'Tsyomkino': 'https://ru.wikipedia.org/wiki/%D0%A2%D1%91%D0%BC%D0%BA%D0%B8%D0%BD%D0%BE_(%D1%81%D0%B5%D0%BB%D0%BE,_%D0%A1%D0%BC%D0%BE%D0%BB%D0%B5%D0%BD%D1%81%D0%BA%D0%B0%D1%8F_%D0%BE%D0%B1%D0%BB%D0%B0%D1%81%D1%82%D1%8C)#:~:text=%D0%AD%D1%82%D0%BE%20%D1%81%D0%B5%D0%BB%D0%BE%20%D0%B2%D0%BE%D0%B7%D0%BD%D0%B8%D0%BA%D0%BB%D0%BE%20%D0%BA%D0%B0%D0%BA%20%D1%81%D1%82%D0%BE%D1%80%D0%BE%D0%B6%D0%B5%D0%B2%D0%BE%D0%B9,%D0%BD%D0%B0%20%D0%BC%D0%B5%D1%81%D1%82%D0%B5%20%D0%B4%D0%B5%D1%80%D0%B5%D0%B2%D0%BD%D0%B8%20%D0%9F%D0%BE%D0%BB%D1%8F%D0%BD%D0%B0%20%D0%92%D0%BE%D0%B5%D0%B9%D0%BA%D0%BE%D0%B2%D0%B0.',
    'Novodugino': 'https://ru.wikipedia.org/wiki/%D0%9D%D0%BE%D0%B2%D0%BE%D0%B4%D1%83%D0%B3%D0%B8%D0%BD%D0%BE',
    'Sychyovka': 'https://ru.wikipedia.org/wiki/%D0%A1%D1%8B%D1%87%D1%91%D0%B2%D0%BA%D0%B0',
    'Gzhatsk': 'https://ru.wikipedia.org/wiki/%D0%93%D0%B0%D0%B3%D0%B0%D1%80%D0%B8%D0%BD_(%D0%A1%D0%BC%D0%BE%D0%BB%D0%B5%D0%BD%D1%81%D0%BA%D0%B0%D1%8F_%D0%BE%D0%B1%D0%BB%D0%B0%D1%81%D1%82%D1%8C)',

    'Salchininkai': 'https://lt.wikipedia.org/wiki/%C5%A0al%C4%8Dininkai',
    'Varena': 'https://lt.wikipedia.org/wiki/Var%C4%97na',
    'Druskininkai': 'https://lt.wikipedia.org/wiki/Druskininkai',
    'Lazdijai': 'https://lt.wikipedia.org/wiki/Lazdijai',
    'Alytus': 'https://lt.wikipedia.org/wiki/Alytus',
    'Trakai': 'https://lt.wikipedia.org/wiki/Trakai',
    'Vilnius': 'https://lt.wikipedia.org/wiki/Vilnius',
    'Svencionys': 'https://lt.wikipedia.org/wiki/%C5%A0ven%C4%8Dionys',
    'Ignalina': 'https://lt.wikipedia.org/wiki/Ignalina',
    'Zarasai': 'https://lt.wikipedia.org/wiki/Zarasai',
    'Kalvarija': 'https://lt.wikipedia.org/wiki/Kalvarija',
    'Marijampole': 'https://lt.wikipedia.org/wiki/Marijampol%C4%97',
    'Prienai': 'https://lt.wikipedia.org/wiki/Prienai',
    'Elektrenai': 'https://lt.wikipedia.org/wiki/Elektr%C4%97nai',
    'Kaisiadorys': 'https://lt.wikipedia.org/wiki/Kai%C5%A1iadorys',
    'Sirvintos': 'https://lt.wikipedia.org/wiki/%C5%A0irvintos',
    'Moletai': 'https://lt.wikipedia.org/wiki/Mol%C4%97tai',
    'Utena': 'https://lt.wikipedia.org/wiki/Utena',
    'Rokiskis': 'https://lt.wikipedia.org/wiki/Roki%C5%A1kis',
    'Kupiskis': 'https://lt.wikipedia.org/wiki/Kupi%C5%A1kis',
    'Anyksciai': 'https://lt.wikipedia.org/wiki/Anyk%C5%A1%C4%8Diai',
    'Ukmerge': 'https://lt.wikipedia.org/wiki/Ukmerg%C4%97',
    'Ionava': 'https://lt.wikipedia.org/wiki/Jonava',
    'Kaunas': 'https://lt.wikipedia.org/wiki/Kaunas',
    'Kazlu Ruda': 'https://lt.wikipedia.org/wiki/Kazl%C5%B3_R%C5%ABda',
    'Vilaviskis': 'https://lt.wikipedia.org/wiki/Vilkavi%C5%A1kis',
    'Sakiai': 'https://lt.wikipedia.org/wiki/%C5%A0akiai',
    'Jurbarkas': 'https://lt.wikipedia.org/wiki/Jurbarkas',
    'Raseiniai': 'https://lt.wikipedia.org/wiki/Raseiniai',
    'Kedainiai': 'https://lt.wikipedia.org/wiki/K%C4%97dainiai',
    'Panevezys': 'https://lt.wikipedia.org/wiki/Panev%C4%97%C5%BEys',
    'Birzai': 'https://lt.wikipedia.org/wiki/Bir%C5%BEai',
    'Pasvalys': 'https://lt.wikipedia.org/wiki/Pasvalys',
    'Radviliskis': 'https://lt.wikipedia.org/wiki/Radvili%C5%A1kis',
    'Pakruojis': 'https://lt.wikipedia.org/wiki/Pakruojis',
    'Joniskis': 'https://lt.wikipedia.org/wiki/Joni%C5%A1kis',
    'Siauliai': 'https://lt.wikipedia.org/wiki/%C5%A0iauliai',
    'Kelme': 'https://lt.wikipedia.org/wiki/Kelm%C4%97',
    'Silale': 'https://lt.wikipedia.org/wiki/%C5%A0ilal%C4%97',
    'Taurage': 'https://lt.wikipedia.org/wiki/Taurag%C4%97',
    'Pagegiai': 'https://lt.wikipedia.org/wiki/Pag%C4%97giai',
    'Akmene': 'https://lt.wikipedia.org/wiki/Akmen%C4%97',
    'Mazeikiai': 'https://lt.wikipedia.org/wiki/Ma%C5%BEeikiai',
    'Telsiai': 'https://lt.wikipedia.org/wiki/Tel%C5%A1iai',
    'Rietavas': 'https://lt.wikipedia.org/wiki/Rietavas',
    'Plunge': 'https://lt.wikipedia.org/wiki/Plung%C4%97',
    'Skuodas': 'https://lt.wikipedia.org/wiki/Skuodas',
    'Kretinga': 'https://lt.wikipedia.org/wiki/Kretinga',
    'Klaipeda': 'https://lt.wikipedia.org/wiki/Klaip%C4%97da',
    'Silute': 'https://lt.wikipedia.org/wiki/%C5%A0ilut%C4%97',

    'Hajnowka': 'https://pl.wikipedia.org/wiki/Hajn%C3%B3wka',
    'Siemiatycze': 'https://pl.wikipedia.org/wiki/Siemiatycze',
    'Bielsk_Podlaski': 'https://pl.wikipedia.org/wiki/Bielsk_Podlaski',
    'Bialystok': 'https://pl.wikipedia.org/wiki/Bia%C5%82ystok',
    'Sokolka': 'https://pl.wikipedia.org/wiki/Sok%C3%B3%C5%82ka',
    'Augustow': 'https://pl.wikipedia.org/wiki/August%C3%B3w',
    'Sejny': 'https://pl.wikipedia.org/wiki/Sejny',
    'Suwalki': 'https://pl.wikipedia.org/wiki/Suwa%C5%82ki',
    'Wysokie_Mazowieckie': 'https://pl.wikipedia.org/wiki/Wysokie_Mazowieckie',
    'Monki': 'https://pl.wikipedia.org/wiki/Mo%C5%84ki',
    'Zambrow': 'https://pl.wikipedia.org/wiki/Zambr%C3%B3w',
    'Lomza': 'https://pl.wikipedia.org/wiki/%C5%81om%C5%BCa',
    'Grajewo': 'https://pl.wikipedia.org/wiki/Grajewo',
    'Kolno': 'https://pl.wikipedia.org/wiki/Kolno',
    
    // Добавляйте другие регионы здесь
};

// Функция для открытия ссылки на Википедию
function openWikiLink() {
    // Ищем элемент по ID
    var landElement = document.getElementById('selected_land_name');

    if (landElement) {
        // Получаем название региона (или города) из содержимого тега
        var landName = landElement.textContent.trim();

        // Проверяем, есть ли ссылка для данного региона
        if (regionLinks.hasOwnProperty(landName)) {
            // Если есть, открываем ссылку в новой вкладке
            window.open(regionLinks[landName], '_blank');
        } else {
            // Если нет ссылки, можно вывести сообщение или перенаправить на поиск в Википедии
            alert('No Wikipedia page available for this region.');
        }
    } else {
        console.error('Element with ID "selected_land_name" not found');
    }
}
