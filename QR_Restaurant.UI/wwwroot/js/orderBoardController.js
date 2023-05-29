

$(document).ready(function () {
    $('#btnOrderBoard').toggleClass('btn-outline');
    getOrders();
    $("#todo, #inprogress, #completed").sortable({
        connectWith: ".connectList",
        update: function (event, ui) {
            var todo = $("#todo").sortable("toArray");
            var inprogress = $("#inprogress").sortable("toArray");
            var completed = $("#completed").sortable("toArray");
            $('.output').html("ToDo: " + window.JSON.stringify(todo) + "<br/>" + "In Progress: " + window.JSON.stringify(inprogress) + "<br/>" + "Completed: " + window.JSON.stringify(completed));
        },
        start: function (event, ui) {

        },
        stop: function (event, ui) {
            //siparis evden siparisse get paiment ac
            //if ($(ui.item[0].parentElement).attr('id').toString() == "completed") {
            //    $(ui.item[0]).removeClass(`${ui.item[0].className}`);
            //    $(ui.item[0]).addClass('success-element');
            //    payOrder($(ui.item[0]).attr('table'));        
            //    return;
            //}

            $.ajax({
                type: "get",
                url: `/order/updatekanbanorder?id=${$(ui.item).attr('id')}&status=${$(ui.item[0].parentElement).attr('id')}`,
                success: function (data) {
                    if (data !== "100" && data !== "500") {
                        getSuccessNotification(data);
                    }

                    switch ($(ui.item[0].parentElement).attr('id').toString()) {
                        case "todo":
                            $(ui.item[0]).removeClass(`${ui.item[0].className}`);
                            $(ui.item[0]).addClass('warning-element');
                            if ($(ui.item[0]).children()[4]) {
                                $(ui.item[0]).children()[4].remove();
                            }
                            return;
                        case "inprogress":
                            $(ui.item[0]).removeClass(`${ui.item[0].className}`);
                            $(ui.item[0]).addClass('info-element');
                            if ($(ui.item[0]).children()[4]) {
                                $(ui.item[0]).children()[4].remove();
                            }
                            return;
                        case "completed":
                            $(ui.item[0]).removeClass(`${ui.item[0].className}`);
                            $(ui.item[0]).addClass('success-element');
                            $(ui.item[0]).append(`<button onclick="openPaymentPage(${$(ui.item).attr('tid')})"                            
                            class="btn btn-outline btn-primary btn-block btn-sm mt-3"><i class="fa fa-credit-card"></i> - ${$('#lblPaid').val()} </button>`)
                            return;
                        default:
                            $(ui.item[0]).toggleClass('warning-element');
                            return;
                    }
                },
                error: function (error) {
                    alert('Check your internet connection.');
                }
            });
        }
    }).disableSelection();
});

var connection = new signalR.HubConnectionBuilder().withUrl("/orderHub").build();
var allBoardOrders = [];

connection.on("getNewOrders", function () {
    getOrders();
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

connection.on("listenPayOrder", function (tid) {
    if ($(`li[tid="${tid}"]`)) {
        $(`li[tid="${tid}"]`).remove();
    }
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

const getOrders = () => {
    $('#boxOrder').children('.ibox-content').toggleClass('sk-loading');
    $.ajax({
        type: "get",
        url: "/order/GetKanbanOrders",
        success: function (data) {
            let todoOrdersHtml = "";
            let inProgressOrdersHtml = "";
            let completedOrdersHtml = "";
            let orderItems = "";
            if (data.length !== 0) {
                let todoOrders = data.filter(x => x.statusNo == 1);
                for (var i = 0; i < todoOrders.length; i++) {
                    orderItems = "";
                    orderItems += todoOrders[i].quantities.map((x, index) => {
                        return `<b>${x} ${todoOrders[i].products[index]}</b> <span class="board-item-text">${todoOrders[i].productFeatures[index]}</span> <br/>`;
                    }).join('');

                    todoOrdersHtml += `<li class="warning-element" id="${todoOrders[i].id}" tid="${todoOrders[i].tableId}">
                       ${orderItems}
                    <div class="agile-detail">
                      <span class="board-item-text"><b class="text-danger">${$('#lblNote').val()}</b> : ${todoOrders[i].note} </span>
                      <h5 class="mt-3"> <span class="date-block"><i class="fa fa-clock-o"></i> ${todoOrders[i].date}</span>
                        <span class='badge badge-pill badge-warning float-right'> ${$('#lblTableNo').val()} : ${todoOrders[i].tableNo} </span>
                        </h5>
                     </div></li>`;
                }

                document.getElementById('todo').innerHTML = todoOrdersHtml;
                let inProgressOrders = data.filter(x => x.statusNo == 2);
                for (var i = 0; i < inProgressOrders.length; i++) {
                    orderItems = "";
                    orderItems += inProgressOrders[i].quantities.map((x, index) => {
                        return `<b> ${x} ${inProgressOrders[i].products[index]} </b> <span class="board-item-text">${inProgressOrders[i].productFeatures[index]}</span> <br/>`;
                    }).join('');

                    inProgressOrdersHtml += `<li class="info-element" id="${inProgressOrders[i].id}" tid="${inProgressOrders[i].tableId}">
                       ${orderItems}
                    <div class="agile-detail">
                      <span class="board-item-text"><b class="text-danger">${$('#lblNote').val()}</b> : ${inProgressOrders[i].note} </span>
                      <h5 class="mt-3">
                        <span class="date-block"><i class="fa fa-clock-o"></i> ${inProgressOrders[i].date}</span>
                        <span class='badge badge-pill badge-warning float-right'>${$('#lblTableNo').val()} : ${inProgressOrders[i].tableNo}</span>
                        </h5>
                     </div></li>`;
                }

                document.getElementById('inprogress').innerHTML = inProgressOrdersHtml;
                let completedOrders = data.filter(x => x.statusNo == 3);
                for (var i = 0; i < completedOrders.length; i++) {
                    orderItems = "";
                    orderItems += completedOrders[i].quantities.map((x, index) => {
                        return `<b> ${x} ${completedOrders[i].products[index]} </b><span class="board-item-text">${completedOrders[i].productFeatures[index]}</span> <br/>`;
                    }).join('');

                    completedOrdersHtml += `<li class="success-element" id="${completedOrders[i].id}" tid="${completedOrders[i].tableId}">
                       ${orderItems}
                    <div class="agile-detail">
                       <span class="board-item-text"><b class="text-danger">${$('#lblNote').val()}</b> : ${completedOrders[i].note} </span>
                       <h5 class="mt-3">
                        <span class="date-block"><i class="fa fa-clock-o"></i> ${completedOrders[i].date}</span>
                        <span class='badge badge-pill badge-warning float-right'>${$('#lblTableNo').val()} : ${completedOrders[i].tableNo}</span>
                        </h5>
                        <button onclick="openPaymentPage(${completedOrders[i].tableId})" class="btn btn-outline btn-primary btn-block btn-sm mt-3"><i class="fa fa-credit-card"></i> - ${$('#lblPaid').val()} </button>
                     </div></li>`;
                }
                document.getElementById('completed').innerHTML = completedOrdersHtml;
            }
        },
        error: function (error) {
            alert('Check your internet connection.');
        }
    });
}

const openPaymentPage = (tableId) => {
    window.open(`/restaurant/OrdersOfTable/${tableId}`, '_blank');
}

const payOrder = () => {
    $.ajax({
        type: "get",
        url: `/order/payorders?tableId=${tableId}`,
        success: function (data) {
            let responses = data.split('%');
            if (responses[0] === "200") {
                window.open(`/report/tablepayment?tableId=${tableId}&paymentId=${responses[1]}`, '_blank');
                getSuccessNotification(`${responses[2]}*success`);
            }
        },
        error: function (error) {
            alert('Check your internet connection and refresh page.');
        }
    });
}