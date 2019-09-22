/// <reference path="index.ts" />

$("#filtrosModal").on('show.bs.modal', function () {
    montarModals("filtrosBody", "filter");
});

$("#filtrosModal").on('hidden.bs.modal', function() {
    unmountModal("filtrosBody");
});

$("#selectsModal").on('show.bs.modal', function() {
    montarModals("selectsBody", "select");
});

$("#selectsModal").on('hidden.bs.modal', function() {
    unmountModal("selectsBody");
});

setInterval(function() {
    var element = $("#baseArea");
    for (var z = 0; z < basesBusca.length; z++) {
        var children = element.children("#" + basesBusca[z].name);
        if (children.length == 0)
            element.append(`<span id="${basesBusca[z].name}" class="badge badge-info">${basesBusca[z].name}</span>`);
    }
    var childrens = element.children();
    for (var z = 0; z < childrens.length; z++) {
        var base = basesBusca.findIndex(function(value) {
            return value.name == childrens[z].id;
        });
        if (base == -1)
            element[0].removeChild(childrens[z]);
    }
}, 500);

setInterval(function() { updateModalSelect(); updateModalFilter(); }, 500);
var baseTable = $("#BaseTable")
    .DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/api/basedados",
            "method": "GET"
        },
        "columns": [
            { "data": null },
            { "data": "name" },
            { "data": null }
        ],
        "columnDefs": [
            {
                "targets": -3,
                "data": null,
                "defaultContent": '<div class="btn-group"> <button type="button" class="btn btn-info dt-expand" style="margin-right:16px;"><i class="fas fa-angle-right"></i></button></div>'
            },
            {
                "targets": -1,
                "data": null,
                "defaultContent": '<div class="btn-group"> <button type="button" class="btn btn-info dt-view" style="margin-right:16px;"><i class="fas fa-plus-square"></i></button></div>'
            }
        ],
        "ordering": true,
        "paging": true,
        "pagingType": "full_numbers",
        "pageLength": 5
    });

$("#BaseTable tbody").on("click", "button.dt-view", function() {
    var tr = $(this).closest('tr');
    var row = baseTable.row(tr);
    var dataRow = row.data() as BaseBusca;
    var baseName = dataRow.name;
    var isNaBusca = basesBusca.findIndex(function(value) {
        return value.name == baseName;
    });
    if (isNaBusca != -1) {
        basesBusca.splice(isNaBusca, 1);
        return;
    }
    basesBusca.push(new BaseBusca(baseName));
});

$("#checkboxEntries").on("change", function() {
    if ($(this).is(":checked"))
        $("#formEntries").prop("disabled", true);
    else
        $("#formEntries").prop("disabled", false);
});

$("#BaseTable tbody").on("click", "button.dt-expand", function() {
    var tr = $(this).closest('tr');
    var row = baseTable.row(tr);
    if (row.child.isShown()) {
        // This row is already open - close it
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        var dataRow = row.data() as BaseBusca;
        var data = getColunas(dataRow.name);
        row.child(formatColunas(data)).show();
        tr.addClass('shown');
    }
});

$("#BtnQuery").on("click", function() {
    queryBases();
});

$("#btnSaveFilter").on("click", function() {
    var fieldsets = Array.from(document.getElementById("filtrosBody").children);
    fieldsets.forEach(function(value) {
        for (var b = 0; b < basesBusca.length; b++) {
            if (value.id != basesBusca[b].name)
                continue;
            var ul = document.getElementById(`${value.id}-ulFiltro`) as HTMLUListElement;
            var ulArray = Array.from(ul.children);
            if (basesBusca[b].columnsFilter.length == 0) {
                return;
            }
            for (var c = 0; c < ulArray.length; c++) {
                var li = ulArray[c];
                basesBusca[b].columnsFilter.forEach(function(value) {
                    if (`${value.descricao}-${value.filterType}` != li.id)
                        return;
                    value.filterType = (document.getElementById(`${value.descricao}-${value.filterType}-selectFilterType`) as HTMLSelectElement).value;
                    value.valueFilter = (document.getElementById(`${li.id}-formValue`) as HTMLInputElement).value;
                });
            }
        }
    });

    $('#filtrosModal').modal('toggle');
});
