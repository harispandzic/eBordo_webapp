function animateValue(obj, start, end, duration) {
    let startTimestamp = null;
    const step = (timestamp) => {
        if (!startTimestamp) startTimestamp = timestamp;
        const progress = Math.min((timestamp - startTimestamp) / duration, 1);
        obj.innerHTML = Math.floor(progress * (end - start) + start);
        if (progress < 1) {
            window.requestAnimationFrame(step);
        }
    };
    window.requestAnimationFrame(step);
}
$(document).ready(function () {
    var x = document.getElementsByClassName("spanStatistika");
    var i;
    for (i = 0; i < x.length; i++) {
        var broj = x[i].innerText;
        animateValue(x[i], 0, broj, 800);
    }
});
$("#btnCollapseDown").click(function () {
    $(".collapse").collapse('show');
    var x = document.getElementsByClassName("spanStatistikaShow");
    var i;
    for (i = 0; i < x.length; i++) {
        var broj = x[i].innerText;
        animateValue(x[i], 0, broj, 800);
    }
});
$("#btnCollapseUp").click(function () {
    $(".collapse").collapse('hide');
});
