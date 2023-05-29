
$(document).ready(function () {
    $('#btnOrder').toggleClass('btn-outline');    
});

var connection = new signalR.HubConnectionBuilder().withUrl("/orderHub").build();

connection.on("getNewOrders", function (tableNo) {   

    if ($('#table-' + tableNo).hasClass("black-bg")) {
        $('#table-' + tableNo).removeClass('black-bg');
        $('#table-icon' + tableNo).removeClass('fa-minus');
    } else {
        $('#table-' + tableNo).removeClass('open-table-bg');
        $('#table-icon-' + tableNo).removeClass('fa-cutlery');
    }
    $('#table-' + tableNo).addClass('new-order-bg');
    $('#table-icon-' + tableNo).addClass('fa-bell');
    $('#table-notification-' + tableNo).html(`${$('#lblNewOrderNotification').val()}`);

    getSuccessNotification(`${$('#lblNewOrderMessage').val()}*warning`);
    var audio = new Audio('/Audio/Doorbell.mp3');
    audio.play();
});

connection.on("callTheWaiter", function (tableNo, tableName) {
    document.getElementById('txtCallMessage').innerHTML = $('#lblCallWaiter').val();
    document.getElementById("imgCall").src = "/images/call-waiter.png";
    document.getElementById('waiter-tableNo').innerHTML = `<b>${tableNo}</b>`;
    document.getElementById('waiter-tableName').innerHTML = `<b>${tableName}</b>`;
    $('#call-modal').modal();
    var audio = new Audio('/Audio/Doorbell.mp3');
    audio.play();
});

connection.on("callTheBill", function (tableNo, tableName) {
    document.getElementById('txtCallMessage').innerHTML = $('#lblCallBill').val();
    document.getElementById("imgCall").src = "/images/get-money.png";
    document.getElementById('waiter-tableNo').innerHTML = `<b>${tableNo}</b>`;
    document.getElementById('waiter-tableName').innerHTML = `<b>${tableName}</b>`;
    $('#call-modal').modal();
    var audio = new Audio('/Audio/Doorbell.mp3');
    audio.play();
});
connection.start().then(function () {
    let roomId = document.getElementById('lblRestaurantId').value;
    connection.invoke("JoinGroup", roomId);
}).catch(function (err) {
    return console.error(err.toString());
});

