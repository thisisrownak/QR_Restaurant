$(document).ready(function () {
    $('#btnMenuFeatures').toggleClass('btn-outline');
    $('input').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        increaseArea: '20%' /* optional */
    });
});

const createLoad = () => {
    $('#qrTableForm').children('.ibox-content').toggleClass('sk-loading');
    itemHtml = ""; //we use for hidden input via posted item list

    for (var i = 0; i < items.length; i++) {
        if (i == 0) {
            itemHtml += `${items[i].name}?${items[i].price}`;
        } else {
            itemHtml += `#${items[i].name}?${items[i].price}`;
        }
    }
    $('#txtAddedItemList').val(itemHtml);
    document.getElementById('btnSubmitForm').click();
}

let itemId = 0;
let items = [];
let itemHtml = "";
let newItem = {};
let selectedItemId = 0;

const addItem = () => {
    $('#txtValidation').val('');
    itemHtml = "";
    if ($('#txtItemName').val()) {
        if ($('#txtItemName').val().includes('?') || $('#txtItemName').val().includes('#') || $('#txtItemPrice').val().includes('?') || $('#txtItemPrice').val().includes('#')) {
            alert("You cant use special char.");
            return;
        }
        itemId += 1;
        //items += `${$('#txtItemName').val()}?${$('#txtItemPrice').val()}#`;
        //$('#txtAddedItemList').val(items);

        newItem = {
            id: itemId,
            name: $('#txtItemName').val(),
            price: $('#txtItemPrice').val() ? $('#txtItemPrice').val() : 0
        };
        items.push(newItem);
    } else {
        document.getElementById('txtValid').innerHTML = $('#txtFillField').val();
        return;
    }

    itemHtml = `<tr id='item-${itemId}'><td>${newItem.name} </td>
                                                     <td>${newItem.price}$ </td>
                                                     <td><a onclick="deleteItem(${itemId})" class="btn  btn-primary text-white"><i class='fa fa-close'></i> </a> </td>
                                                   </tr>`;

    $('#txtItemName').val("");
    $('#txtItemPrice').val("");
    $('#lstFeatureItem').append(itemHtml);
};

const deleteItem = (itemId) => {
    let index = items.findIndex(x => x.id == itemId);
    if (index > -1) {
        items.splice(index, 1);
        document.getElementById(`item-${itemId}`).style.display = 'none';
    }
};

const addItemDirect = () => {
    itemHtml = "";

    if ($('#txtNewItemName').val() && $('#txtNewPrice').val()) {
        if ($('#txtNewItemName').val().includes('?') || $('#txtNewItemName').val().includes('#') || $('#txtNewPrice').val().includes('?') || $('#txtNewPrice').val().includes('#')) {
            alert("You cant use special char.");
            return;
        }

        $.ajax({
            type: "get",
            url: `/MenuProductFeature/AddFeatureItem?featureId=${$('#txtId').val()}&items=${$('#txtNewItemName').val()}?${$('#txtNewPrice').val()}`,
            success: function (data) {
                if (data !== "500") {
                    getSuccessNotification($('#txtSuccessMessage').val() + "*success");

                    itemHtml = `<tr id='item-${data}'><td>${$('#txtNewItemName').val()} </td>
                                                     <td>${$('#txtNewPrice').val()} $ </td>
                                                     <td><a onclick="deleteItemDirect(${data})" class="btn btn-sm btn-primary text-white"><i class='fa fa-close'></i> </a> </td>
                                                   </tr>`;
                    $('#lstItemForUpdate').append(itemHtml);
                    $('#txtNewItemName').val("");
                    $('#txtNewPrice').val("");
                } else {
                    alert('Check your field. We didnt add the feature item.');
                }
            },
            error: function (error) {
                console.log(error);
                alert('Check your internet connection.');
            }
        });
    } else {
        alert("Please fill the field.");
        return;
    }
}

const deleteItemDirect = (itemId) => {
    selectedItemId = itemId;
    $('#delete-modal').modal();
}

const deleteForUp = () => {
    $.ajax({
        type: "get",
        url: `/MenuProductFeature/DeleteFeatureItem?featureId=${selectedItemId}`,
        success: function (data) {
            if (data === "200") {
                document.getElementById(`item-${selectedItemId}`).style.display = 'none';
                $('#delete-modal').modal("hide");
                getSuccessNotification($('#txtSuccessMessage').val() + "*success");
            } else {
                $('#delete-modal').modal("hide");
                alert('Check your field. We didnt add the feature item.');
            }
        },
        error: function (error) {
            console.log(error);
            alert('Check your internet connection.');
        }
    });
}