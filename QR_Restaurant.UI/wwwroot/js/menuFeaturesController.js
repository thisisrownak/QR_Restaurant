$(document).ready(function () {
    $('#btnMenuFeatures').toggleClass('btn-outline');   
    $('.dataTables-list').DataTable({
        pageLength: 10,
        responsive: true,
        dom: '<"html5buttons"B>lTfgitp',
        buttons: [
            { extend: 'copy' },
            { extend: 'csv' },
            { extend: 'excel', title: 'Excel List' },
            { extend: 'pdf', title: 'Pdf' },

            {
                extend: 'print',
                customize: function (win) {
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');

                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            }
        ]

    });
});

let selectedId = 0;

openDeleteModal = (id) => {
    selectedId = id;
    $('#delete-modal').modal();
}

deleteProduct = () => {
    location.href = "/MenuProductFeature/Passive/" + selectedId;
};