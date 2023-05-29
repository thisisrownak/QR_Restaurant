

$(document).ready(function () {
    $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
    $('#menuMenuLink').toggleClass('active');
    document.getElementById('isList').style.display = 'block';
    document.getElementById('isNotList').style.display = 'none';
    localStorage.setItem("tableId", $('#txtTableId').val());
    localStorage.setItem("userId", $('#txtUserId').val());
    localStorage.setItem("restaurantId", $('#txtRestaurantId').val());
    if (detectMob()) {
        $('#fboxSf').css('display', 'none');
        $('#welcome-lg').css('display', 'none');
        $('#welcome-sm').css('display', 'block');
    } else {
        $('#welcome-lg').css('display', 'block');
        $('#welcome-sm').css('display', 'none');
    }

    getCartList();

    // Input incrementer
    $(".numbers-row").append('<div class="inc button_inc">+</div><div class="dec button_inc">-</div>');
    $(".button_inc").on('click', function () {

        var $button = $(this);
        var oldValue = $button.parent().find("input").val();

        if ($button.text() == "+") {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            // Don't allow decrementing below zero
            if (oldValue > 1) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 1;
            }
        }
        $button.parent().find("input").val(newVal);
    });
});


const getCartNotification = (message) => {
    if (!message) return;
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


const changeView = (isList) => {
    if (isList === 1) {
        document.getElementById('isList').style.display = 'block';
        document.getElementById('isNotList').style.display = 'none';
    } else {
        document.getElementById('isList').style.display = 'none';
        document.getElementById('isNotList').style.display = 'block';
    }
}

let selectedMenuId = 0;
let quantity = 0;
let featureItemsIds = "";
let hasFeature = false;
let selectedMenuFeatureCount = 0;
let singleCheckBoxIndex = 1;

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
                        cartHtml += `<tr><td style="width:60%" class="cart-item">${cartContents[i].menuProduct.name}<br/> ${cartContents[i].productFeatures != null ? `<span class="text-menu-description text-muted">${cartContents[i].productFeatures}</span>` : ""} </td>
                                    <td><input onblur="updateQuantity(${cartContents[i].menuProduct.id})" id="cartquantity-${cartContents[i].menuProduct.id}" class="quantity-input" value="${cartContents[i].quantity}" /></td>
                                    <td style="width:28%;" class="cart-item">${cartContents[i].productTotal} ${currency}</td>
                                    <td><a onclick="removeToCart(${cartContents[i].menuProduct.id}, '${cartContents[i].productFeatures}')" class="float-right"><i class="fa fa-times text-qrRest"></i></a></td>
                                </tr>`;
                    }
                    //document.getElementById('txtCartTotal').innerText = `data.cart.total ${currency}`;
                    cartHtml += `<tr class="spacer">
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

const removeToCart = (id, featureItems) => {
    $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
    $.ajax({
        type: "get",
        url: `/shoppingcart/RemoveToCart?menuProductId=${id}&featureItems=${featureItems}`,
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
    featureItemsIds = "";

    //if menu has not feature options
    if (id != 0) {
        hasFeature = false
        selectedMenuId = id;
        quantity = isList ? $('#txtQuantity-' + id).val() : $('#txtQuantityFloor-' + id).val();
    } else {
        quantity = $('#qty_1').val()
    }

    if (hasFeature && id == 0) {
        let selectedFeatureCounter = 0;
        //For multiple select we have to increase the counter by one for each header. We will check below by keeping it in this variable.
        let featureTitleForMultipleSelect = ""; 
        if (singleCheckBoxIndex > 1) {
            for (var i = 1; i < singleCheckBoxIndex; i++) {
                if ($(`input[name="options_${i}"]:checked`)[0]) {
                    selectedFeatureCounter++;
                    featureItemsIds += $(`input[name="options_${i}"]:checked`)[0].value + ',';
                }
            }
        }
        if ($('input[name="category_s"]:checked')) {

            $('input[name="category_s"]:checked').each(function () {
                if (featureTitleForMultipleSelect != $(this).attr('title')) {
                    selectedFeatureCounter++;
                }
                featureTitleForMultipleSelect = $(this).attr('title');
                featureItemsIds += `${this.value},`;
            });
        }

        if (selectedFeatureCounter < selectedMenuFeatureCount) {
            document.getElementById('txtFeatureValidation').innerHTML = $('#txtSelectOptions').val();
            $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
            return;
        }
    }

    $.ajax({
        type: "get",
        url: `/shoppingcart/AddToCart?menuProductId=${selectedMenuId}&quantity=${quantity}&productFeaturesIds=${featureItemsIds}`,
        success: function (data) {
            let dataArray = data.split('%');
            if (dataArray[0] === "200") {
                $('#txtQuantity-' + selectedMenuId).val(1);
                $('#txtQuantityFloor-' + selectedMenuId).val(1);
                $('#qty_1').val(1);
                getCartList();
                getCartNotification(dataArray[1]);
            } else {
                $('#fboxSf').children('.ibox-content').toggleClass('sk-loading');
                getCartNotification(dataArray[1]);
            }
            if (id == 0) cancelModal();
        },
        error: function (error) {
            if (id == 0) cancelModal();
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

const cancelModal = () => {
    $('#modal-dialog').modal('hide');
}

const openFeatureDialog = (element, menuId, isList) => {
    let featuresHtml = "";
    selectedMenuFeatureCount = 0;
    selectedMenuId = menuId;
    singleCheckBoxIndex = 1;
    document.getElementById('txtFeatureValidation').innerHTML = '';

    quantity = isList ? $('#txtQuantity-' + menuId).val() : $('#txtQuantityFloor-' + menuId).val();
    $('#qty_1').val(quantity);

    document.getElementById('txtMenuName').innerText = element.children[1].innerText;
    //let featureList = element.parentElement.children[2].innerText.split('#');
    let featureList = element.children[2].innerText.split('#');

    if (featureList.length) {
        selectedMenuFeatureCount = featureList.length - 1;
        hasFeature = true;
        let featuerItemList = [];
        for (var i = 0; i < featureList.length - 1; i++) {
            featuerItemList = featureList[i].split(',');

            if (featuerItemList[2].toString() == "False") { //feature is not multiple select
                let featureItems = featuerItemList[3].split('?');
                featuresHtml += `<h5 class="menu-options-title">${featuerItemList[1]}</h5><ul class="clearfix">`;
                for (let z = 0; z < featureItems.length - 1; z++) {
                    let featureItemsElement = featureItems[z].split('!');
                    featuresHtml += `<li><label class="container_radio menu-options-text"> ${featureItemsElement[1]} <span> ${featureItemsElement[2] != 0 ? `+ ${featureItemsElement[2]} ${document.getElementById('txtCurrency').value}` : ''}</span>
                        <input type="radio" value="${featureItemsElement[0]}" name="options_${singleCheckBoxIndex}">
                        <span class="checkmark"></span> </label></li>
                    `;
                }
                singleCheckBoxIndex++;
            } else {  //feature is multiple select
                let featureItems = featuerItemList[3].split('?');
                featuresHtml += `<h5 class="menu-options-title">${featuerItemList[1]}</h5><ul class="clearfix">`;
                for (let x = 0; x < featureItems.length - 1; x++) {
                    let featureItemsElement = featureItems[x].toString().split('!');
                    featuresHtml += `<li><label class="container_check menu-options-text"> ${featureItemsElement[1]} <span>${featureItemsElement[2] != 0 ? `+ ${featureItemsElement[2]} ${document.getElementById('txtCurrency').value}` : ''}</span>
                        <input type="checkbox" title="${featuerItemList[1]}" name="category_s" value="${featureItemsElement[0]}">
                        <span class="checkmark"></span> </label></li>
                    `;
                }
            }
            featuresHtml += '<br/>'
        }
    } else {
        hasFeature = false;
        selectedMenuFeatureCount = 0;
    }
    document.getElementById('menuFeatureList').innerHTML = featuresHtml;

    setTimeout(() => {
        $('#modal-dialog').modal();
    }, 200);
}

const callTheWaiter = () => {
    $.ajax({
        type: "get",
        url: `/restaurant/callthewaiter?restaurant=${$('#txtRestaurantId').val()}&table=${$('#txtTableId').val()}`,
        success: function (data) {
            if (data === "200") {
                $('#call-done-modal').modal();
            } else {
                alert(data);
            }
        },
        error: function (error) {
            
            alert('Check your internet connection.');
        }
    });
}

const callPayment = () => {
    $.ajax({
        type: "get",
        url: `/restaurant/callthebill?restaurant=${$('#txtRestaurantId').val()}&table=${$('#txtTableId').val()}`,
        success: function (data) {
            if (data === "200") {
                $('#call-done-modal').modal();
            } else {
                alert(data);
            }
        },
        error: function (error) {

            alert('Check your internet connection.');
        }
    });
}