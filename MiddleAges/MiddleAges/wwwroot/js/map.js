// We select the SVG into the page
var svg = document.querySelector('svg');

// If browser supports pointer events
if (window.PointerEvent) {
    svg.addEventListener('pointerdown', onPointerDown); // Pointer is pressed
    svg.addEventListener('pointerup', onPointerUp); // Releasing the pointer
    svg.addEventListener('pointerleave', onPointerUp); // Pointer gets out of the SVG area
    svg.addEventListener('pointermove', onPointerMove); // Pointer is moving
} else {
    // Add all mouse events listeners fallback
    svg.addEventListener('mousedown', onPointerDown); // Pressing the mouse
    svg.addEventListener('mouseup', onPointerUp); // Releasing the mouse
    svg.addEventListener('mouseleave', onPointerUp); // Mouse gets out of the SVG area
    svg.addEventListener('mousemove', onPointerMove); // Mouse is moving
    // Add all touch events listeners fallback
    svg.addEventListener('touchstart', onPointerDown); // Finger is touching the screen
    svg.addEventListener('touchend', onPointerUp); // Finger is no longer touching the screen
    svg.addEventListener('touchmove', onPointerMove); // Finger is moving
}

// This function returns an object with X & Y values from the pointer event
function getPointFromEvent(event) {
    var point = { x: 0, y: 0 };
    // If even is triggered by a touch event, we get the position of the first finger
    if (event.targetTouches) {
        point.x = event.targetTouches[0].clientX;
        point.y = event.targetTouches[0].clientY;
    } else {
        point.x = event.clientX;
        point.y = event.clientY;
    }
    return point;
}

// This variable will be used later for move events to check if pointer is down or not
var isPointerDown = false;

// This variable will contain the original coordinates when the user start pressing the mouse or touching the screen
var pointerOrigin = {
    x: 0,
    y: 0
};

// Function called by the event listeners when user start pressing/touching
function onPointerDown(event) {
    isPointerDown = true; // We set the pointer as down
    // We get the pointer position on click/touchdown so we can get the value once the user starts to drag
    var pointerPosition = getPointFromEvent(event);
    pointerOrigin.x = pointerPosition.x;
    pointerOrigin.y = pointerPosition.y;
}

// We save the original values from the viewBox
var viewBox = {
    x: 0,
    y: 0,
    width: 1000,
    height: 890
};

// The distances calculated from the pointer will be stored here
var newViewBox = {
    x: 0,
    y: 0
};

// Function called by the event listeners when user start moving/dragging
function onPointerMove(event) {
    // Only run this function if the pointer is down
    if (!isPointerDown) {
        return;
    }
    // This prevent user to do a selection on the page
    event.preventDefault();
    // Get the pointer position
    var pointerPosition = getPointFromEvent(event);
    let [x, y, width, height] = svg.getAttribute('viewBox').split(' ').map(Number);
    // We calculate the distance between the pointer origin and the current position
    // The viewBox x & y values must be calculated from the original values and the distances
    newViewBox.x = viewBox.x - (pointerPosition.x - pointerOrigin.x) / (viewBox.width / width);
    newViewBox.y = viewBox.y - (pointerPosition.y - pointerOrigin.y) / (viewBox.height / height);
    // We create a string with the new viewBox values
    // The X & Y values are equal to the current viewBox minus the calculated distances
    var viewBoxString = `${newViewBox.x} ${newViewBox.y} ${width} ${height}`;
    // We apply the new viewBox values onto the SVG
    svg.setAttribute('viewBox', viewBoxString);
}

function onPointerUp() {
    // The pointer is no longer considered as down
    isPointerDown = false;
    // We save the viewBox coordinates based on the last pointer offsets
    viewBox.x = newViewBox.x;
    viewBox.y = newViewBox.y;
}

window.addEventListener("DOMContentLoaded", (event) => {
    const svg = document.querySelector('#lhst_svg_map');
    // zooming
    svg.onwheel = function (event) {
        event.preventDefault();
        // set the scaling factor (and make sure it's at least 10%)
        let scale = event.deltaY / 1000;
        scale = Math.abs(scale) < .1 ? .1 * event.deltaY / Math.abs(event.deltaY) : scale;
        // get point in SVG space
        let pt = new DOMPoint(event.clientX, event.clientY);
        pt = pt.matrixTransform(svg.getScreenCTM().inverse());
        // get viewbox transform
        let [x, y, width, height] = svg.getAttribute('viewBox').split(' ').map(Number);
        // get pt.x as a proportion of width and pt.y as proportion of height
        let [xPropW, yPropH] = [(pt.x - x) / width, (pt.y - y) / height];
        // calc new width and height, new x2, y2 (using proportions and new width and height)
        let [width2, height2] = [width + width * scale, height + height * scale];
        let x2 = pt.x - xPropW * width2;
        let y2 = pt.y - yPropH * height2;
        svg.setAttribute('viewBox', `${x2} ${y2} ${width2} ${height2}`);
        viewBox.x = x2;
        viewBox.y = y2;
    }
})
