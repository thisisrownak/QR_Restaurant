

$(document).ready(function () {
    $('#btnOrder').toggleClass('btn-outline');
});

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
            $('#order-' + selectedOrderId).css("display", "none");
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

            $('#badge-' + orderId).html(`<span class='badge badge-primary'> ${$('#lblCompleted').val()}</span>`);
            $('#staff-' + orderId).html(`<p>${$('#lblCompleted').val()}</p>`);
            getSuccessNotification(data);          
            $('#boxOrder').children('.ibox-content').toggleClass('sk-loading');
        },
        error: function (error) {
            alert('Check your internet connection.');
        }
    });
}


const openPayModal = () => {
    $('#pay-modal').modal();
}

const payOrder = () => {
    $('#boxOrder').children('.ibox-content').toggleClass('sk-loading');
    $('#pay-modal').modal('hide');
    $.ajax({
        type: "get",
        url: `/order/payorders?tableId=${$('#txtTableId').val()}`,
        success: function (data) {
            let responses = data.split('%');
            if (responses[0] === "200") {
                window.open(`/report/tablepayment?tableId=${$('#txtTableId').val()}&paymentId=${responses[1]}`, '_blank');
                //manuple
                getSuccessNotification(`${responses[2]}*success`);
                $('#nonPayOrder').html(`<h4 class='text-center'>${responses[2]}</h4><br/><h4 class='text-center text-success'><i class="fa fa-check fa-2x"><i/></h4>`);
            }          
            $('#boxOrder').children('.ibox-content').toggleClass('sk-loading');
        },
        error: function (error) {
            alert('Check your internet connection.');
        }
    });
}