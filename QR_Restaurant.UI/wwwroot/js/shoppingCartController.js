
$(document).ready(function () {
    $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
    $('#menuMenuLink').toggleClass('active');  

    $('#txtRestaurantId').val(localStorage.getItem("restaurantId"));
    $('#txtUserId').val(localStorage.getItem("userId"));
    $('#txtTableId').val(localStorage.getItem("tableId"));
  
    getCartList();
});


const changeView = (isList) => {
    if (isList === 1) {
        document.getElementById('isList').style.display = 'block';
        document.getElementById('isNotList').style.display = 'none';
    } else {
        document.getElementById('isList').style.display = 'none';
        document.getElementById('isNotList').style.display = 'block';
    }
}

const getCartNotification = (message) => {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "progressBar": true,
        "preventDuplicates": false,
        "positionClass": "toast-bottom-right",
        "onclick": null,
        "showDuration": "400",
        "hideDuration": "1000",
        "timeOut": "7000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    var messages = message.split('*');
    if (messages[1] === "success") {
        toastr.success(messages[0]);
    } else if (messages[1] === "warning") {
        toastr.warning(messages[0]);
    }
    else {
        toastr.error(messages[0]);
    }

};

const getCartList = () => {
    $.ajax({
        type: "get",
        url: "/shoppingcart/GetCartList",
        success: function (data) {
            if (data !== "404") {
                if (data.cart.total == 0) {
                    document.getElementById('txtOrderIsReady') != null ? document.getElementById('txtOrderIsReady').style.display = 'none' : null;
                    document.getElementById('placeOrder') != null ? document.getElementById('placeOrder').style.display = 'none' : null;
                    $('#shoppingCartNot').text('');
                    $('#shoppingCartNot').removeClass('label label-danger');
                    let cartHtml = "";

                    cartHtml += `<tr><td><img src="/images/empty-cart.png" /></td>
                        <td><b>${$('#txtEmptyCart').val()} </b> </td>
                    </tr>`;
                    document.getElementById('cartContentsHtml').innerHTML = cartHtml;
                } else {
                    document.getElementById('txtOrderIsReady') != null ? document.getElementById('txtOrderIsReady').style.display = 'block' : null;
                    document.getElementById('placeOrder') != null ? document.getElementById('placeOrder').style.display = 'block' : null;
                    $('#shoppingCartNot').addClass('label label-danger');

                    let cartContents = data.cart.cartContents;
                    $('#shoppingCartNot').text(cartContents.length);
                    let currency = document.getElementById('txtCurrency').value;
                    let cartHtml = "";
                    for (var i = 0; i < cartContents.length; i++) {
                        cartHtml += `<tr><td style="width:60%" class="cart-item">${cartContents[i].menuProduct.name} </td>
                                    <td><input onblur="updateQuantity(${cartContents[i].menuProduct.id})" id="cartquantity-${cartContents[i].menuProduct.id}" class="quantity-input" value="${cartContents[i].quantity}" /></td>
                                    <td style="width:28%;" class="cart-item">${cartContents[i].productTotal} ${currency}</td>
                                    <td><a onclick="removeToCart(${cartContents[i].menuProduct.id})" class="float-right"><i class="fa fa-times text-qrRest"></i></a></td>
                                </tr>`;
                    }
                    //document.getElementById('txtCartTotal').innerText = `data.cart.total ${currency}`;
                    cartHtml += `  <tr class="spacer">
                                    <td class="cart-item"><h5>${$('#txtTotal').val()}</h5></td>
                                    <td></td>
                                      <td class="cart-item"> <strong>${data.cart.total} ${currency}</strong></td>
                                    <td></td>
                                    </tr>`;

                    document.getElementById('cartContentsHtml').innerHTML = cartHtml;
                }
            } else {
                alert("Restaurant not found.");
            }
            $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
        },
        error: function (error) {
            alert('Check your internet connection.');
        }
    });

};

const removeToCart = (id) => {
    $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
    $.ajax({
        type: "get",
        url: `/shoppingcart/RemoveToCart?menuProductId=${id}`,
        success: function (data) {
            let dataArray = data.split('%');
            if (dataArray[0] === "200") {
                getCartList();
                getCartNotification(dataArray[1]);
            } else {
                $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
                getCartNotification(dataArray[1]);
            }
        },
        error: function (error) {
            alert('Check your internet connection.');
        }
    });
}

const addTocart = (id, isList) => {
    $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
    let quantity = isList ? $('#txtQuantity-' + id).val() : $('#txtQuantityFloor-' + id).val();
    $.ajax({
        type: "get",
        url: `/shoppingcart/AddToCart?menuProductId=${id}&quantity=${quantity}`,
        success: function (data) {
            let dataArray = data.split('%');
            if (dataArray[0] === "200") {
                $('#txtQuantity-' + id).val(1);
                $('#txtQuantityFloor-' + id).val(1);
                getCartList();
                getCartNotification(dataArray[1]);
            } else {
                $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
                getCartNotification(dataArray[1]);
            }
        },
        error: function (error) {
            alert('Check your internet connection.');
        }
    });
}

const updateQuantity = (id) => {
    let newQuantity = $('#cartquantity-' + id).val();
    $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
    $.ajax({
        type: "get",
        url: `/shoppingcart/UpdateToCart?menuProductId=${id}&quantity=${newQuantity}`,
        success: function (data) {
            let dataArray = data.split('%');
            if (dataArray[0] === "200") {
                getCartList();
                getCartNotification(dataArray[1]);
            } else {
                $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
            }
        },
        error: function (error) {
            alert('Check your internet connection.');
        }
    });
}

const startOrder = () => {
    $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
}

const order = () => {
    $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
    let data = {
        OrderNote: $('#txtOrderNote').val(),
        TableId: $('#txtTableId').val(),
        RestaurantId: $('#txtRestaurantId').val(),
        UserId: ('#txtUserId').val()
    };

    $.ajax({
        type: "post",
        url: '/order/AddOrder',
        data: data,
        success: function (data) {
            console.log('Process is okay.');
        },
        error: function (error) {
            alert('Check your internet connection.');
        }
    });
}