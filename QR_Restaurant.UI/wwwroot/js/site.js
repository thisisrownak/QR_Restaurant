
const getSuccessNotification = (message) => {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "progressBar": true,
        "preventDuplicates": false,
        "positionClass": "toast-top-right",
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

function detectMob() {
    const toMatch = [
        /Android/i,
        /webOS/i,
        /iPhone/i,
        /iPad/i,
        /iPod/i,
        /BlackBerry/i,
        /Windows Phone/i
    ];

    return toMatch.some((toMatchItem) => {
        return navigator.userAgent.match(toMatchItem);
    });
}

if (detectMob()) {
    $('#lg-footer').css('display', 'none');
    $('#sm-footer').css('display', 'block');
} else {
    $('#lg-footer').css('display', 'block');
    $('#sm-footer').css('display', 'none');
}

const goToCart = () => {
    if (localStorage.getItem("userId") != null && localStorage.getItem("userId") != "") {
        window.location.href = `/shoppingcart/index/${localStorage.getItem("restaurantId")}`;
    } else {
        getSuccessNotification("Please scan the qr code of the table you are at.*danger");
    }
}

const goToMenu = () => {
    if (localStorage.getItem("userId") != null && localStorage.getItem("userId") != "") {
        window.location.href = `/restaurant/menu?id=${localStorage.getItem("restaurantId")}&tableNo=${localStorage.getItem("tableId")}&userId=${localStorage.getItem("userId")}`;
    } else {
        getSuccessNotification("Please scan the qr code of the table you are at.*danger");
    }
}

const goToPanel = () => {
    window.location.href = `/restaurant/RestaurantManager`;
}