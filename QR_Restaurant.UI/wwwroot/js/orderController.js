
$(document).ready(function () {
    $('#btnOrderAlternative').toggleClass('btn-outline');   
    getOrder();
    getCompletedOrders();
});


var connection = new signalR.HubConnectionBuilder().withUrl("/orderHub").build();

connection.on("getNewOrders", function () {   
    getOrder();   
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

const getOrder = () => {
    $('#boxOrder').children('.ibox-content').toggleClass('sk-loading');
    $.ajax({
        type: "get",
        url: "/order/GetActiveOrders",
        success: function (data) {

            let ordersHtml = "";
            if (data.length === 0) {
                ordersHtml += `<tr><th></th><th></th><th></th>
                        <th><br /><br /> ${$('#lblNoOrder').val()}<br /><br /><br /></th>
                        <th><i class="fa fa-bell-o fa-2x"></i></th>
                            <th></th> <th></th> <th></th>
                    </tr>`;
            } else {
                for (var i = 0; i < data.length; i++) {
                    ordersHtml += `<tr><td><div class="col-md-12">
                               <span><strong>${$('#lblTableNo').val()} :</strong> ${data[i].tableNo}</span><br />
                                <span><strong>${$('#lblTableName').val()} : </strong>${data[i].tableName}</span></div></td>
                            <td class="cart-item-upFont"> ${data[i].quantities.map((x) => {
                        return `<p>${x}</p>`;
                    }).join('')}  </td>
                              <td class="cart-item-upFont"> ${data[i].products.map((x, index) => {
                                  return `<p>${x} <span class="text-menu-description text-muted">${data[i].productFeatures[index]}</span></p>`;
                    }).join('')} </td>                              
                            <td> ${data[i].note} </td>
                            <td class="cart-item-upFont"> ${data[i].date} </td>
                                <td>
                                  <span class='badge badge-primary'>${data[i].status}</span>
                                      </td>
                                  <td>
                                 <button onclick="openDeleteModal(${data[i].id})" class="btn btn-danger btn-sm"><i class="fa fa-trash-o"></i></button>
                                     </td>
                                     <td>
                                    ${data[i].staff.length == 0 ? `<span class='text-danger'>${$('#lblNoWaiter').val()}</span>`  : data[i].staff.map((x) => {
                        return `<a onclick="completeOrder(${data[i].id}, '${x.userId}')">
                                        <img src="/images/avatars/${x.avatar}" alt="Merhaba" class="rounded-circle img-sm"> 
                                        </a>`;
                    }).join('')}                                     
                                       </td>
                                  </tr>`;
                }
            }
            document.getElementById('orderRows').innerHTML = ordersHtml;
            $('#boxOrder').children('.ibox-content').toggleClass('sk-loading');
        },
        error: function (error) {
            alert('Check your internet connection.');
        }
    });
}

const getCompletedOrders = () => {
    $('#boxOrder').children('.ibox-content').toggleClass('sk-loading');
    $.ajax({
        type: "get",
        url: "/order/GetCompletedOrders",
        success: function (data) {

            let ordersHtml = "";
            if (data.length === 0) {
                ordersHtml += `<tr><td></td><td></td><td></td>
                        <td><br /><br /> ${$('#lblNoOrder').val()}<br /><br /><br /></td>
                        <td><i class="fa fa-bell-o fa-2x"></i></td>
                            <td></td> <td></td>
                    </tr>`;
            } else {
                for (var i = 0; i < data.length; i++) {
                    ordersHtml += `<tr><td><div class="col-md-12">
                               <span><strong>${$('#lblTableNo').val()} :</strong> ${data[i].tableNo}</span><br />
                                <span><strong>${$('#lblTableName').val()} : </strong>${data[i].tableName}</span></div></td>
                            <td class="cart-item-upFont"> ${data[i].quantities.map((x) => {
                        return `<p>${x}</p>`;
                    }).join('')}  </td>
                              <td class="cart-item-upFont"> ${data[i].products.map((x, index) => {
                        return `<p>${x} <span class="text-menu-description text-muted">${data[i].productFeatures[index]}</span></p>`;
                    }).join('')} </td>                              
                            <td> ${data[i].note} </td>
                            <td class="cart-item-upFont"> ${data[i].date} </td>
                            <td class="cart-item-upFont"> ${data[i].total} ${$('#lblCurrency').val()} </td>
                                <td>
                                 ${data[i].status == $('#lblCompleted').val() ? `<span class='badge badge-primary'>${data[i].status}</span>` : `<span class='badge badge-danger'>${data[i].status}</span>`}
                                      </td>                                 
                                  </tr>
                                </tr>`;
                }
            }
            document.getElementById('completedOrdersRows').innerHTML = ordersHtml;
            $('#boxOrder').children('.ibox-content').toggleClass('sk-loading');
        },
        error: function (error) {
            alert('Check your internet connection.');
        }
    });
}

let selectedOrderId = 0;

const openDeleteModal = (id) => {
    selectedOrderId = id;
    $('#delete-modal').modal();
}

const cancelOrder = () => {
    $('#delete-modal').modal('hide');
    $('#boxOrder').children('.ibox-content').toggleClass('sk-loading');
    $.ajax({
        type: "get",
        url: `/order/CancelOrder?id=${selectedOrderId}`,
        success: function (data) {
            getSuccessNotification(data);
            getOrder();
            getCompletedOrders();
            $('#boxOrder').children('.ibox-content').toggleClass('sk-loading');
        },
        error: function (error) {
            alert('Check your internet connection.');
        }
    });
}

const completeOrder = (orderId, staffId) => {
    event.preventDefault();
    $('#boxOrder').children('.ibox-content').toggleClass('sk-loading');
    
    $.ajax({
        type: "get",
        url: `/order/CompleteOrder?id=${orderId}&staffId=${staffId}`,
        success: function (data) {
            getSuccessNotification(data);
            getOrder();
            getCompletedOrders();
            $('#boxOrder').children('.ibox-content').toggleClass('sk-loading');
        },
        error: function (error) {
            alert('Check your internet connection.');
        }
    });
}