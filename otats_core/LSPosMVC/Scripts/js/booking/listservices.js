$(document).ready(function () {
    $("#datepicker").kendoDatePicker({
        value: new Date(),
        format: "{0:dd/MM/yyyy}",
        change: function (e) {
            getListServicesByDate()
        }
    })

    $("#listview").kendoListView({
        height: function () {
            var heightGrid = $(window).height() - ($(".navbar").height() + $(".sitemap").height());
            return heightGrid - 80;
        },
        dataSource: {
            schema: {
                model: { id: "ProductID" },
                fields: [
                    { UnitQty: "number" },
                    { Price: "number" }
                ]
            },
            aggregate: [
                { field: "Price", aggregate: "sum" },
            ],
            data: []
        },
        selectable: true,
        pageable: false,
        //change: onlistviewchange,
        template: kendo.template($("#templateOrder").html())
    })
})


function getListServicesByDate() {
    kendo.ui.progress($("body"), true);
    $.ajax({
        url: '/Booking/GetServicesByDate?date=' + kendo.toString($("#datepicker").data("kendoDatePicker").value(), "yyyy-MM-dd"),
        type: 'GET',
    }).done(function successCallback(response) {
        console.log(response);
        var dataSource = new kendo.data.DataSource({
            data: response,
            schema: {
                model: {
                    id: "ServiceRateID",
                    fields: {
                        SellPrice: { type: 'number' }
                    }
                }
            },
            pageSize: 200
        });
        $("#listview").data('kendoListView').setDataSource(dataSource);
        kendo.ui.progress($("body"), false);
    });
}