var basesBusca = new Array<BaseBusca>();

$("#filtrosModal").on('show.bs.modal', function () {
    montarModals("filtrosBody", "filter");
});

$("#filtrosModal").on('hidden.bs.modal', function () {
    unmountModal("filtrosBody");
});

$("#selectsModal").on('show.bs.modal', function () {
    montarModals("selectsBody", "select");
});

$("#selectsModal").on('hidden.bs.modal', function () {
    unmountModal("selectsBody");
});

setInterval(function () {
    var element = $("#baseArea");

    for (var z = 0; z < basesBusca.length; z++) {
        var children = element.children("#" + basesBusca[z].name);

        if (children.length == 0)
            element.append(`<span id="${basesBusca[z].name}" class="badge badge-info">${basesBusca[z].name}</span>`);
    }

    var childrens = element.children();

    for (var z = 0; z < childrens.length; z++) {

        var base = basesBusca.findIndex(function (value) {
            return value.name == childrens[z].id;
        });

        if (base == -1)
            element[0].removeChild(childrens[z]);
    }

}, 500);

setInterval(function () { updateModalSelect(); }, 500);

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

$("#BaseTable tbody").on("click", "button.dt-view", function () {
    var tr = $(this).closest('tr');
    var row = baseTable.row(tr);

    var dataRow = row.data() as BaseBusca;
    var baseName = dataRow.name;

    var colunas = getColunas(baseName);

    var isNaBusca = basesBusca.findIndex(function (value) {
        return value.name == baseName;
    });

    if (isNaBusca != -1) {
        basesBusca.splice(isNaBusca, 1);
        return;
    }

    basesBusca.push(new BaseBusca(baseName));

});

$("#BaseTable tbody").on("click", "button.dt-expand", function () {
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

$("#BtnQuery").on("click", function () {
    queryBases();
});

//Funcions Definitions
function montarFiltro(selectName: string) {
    var idBase = selectName;
    selectName += "-selectListColunas";
    var ulFiltro = idBase + "-ulFiltro";

    var selectList = document.createElement('select');
    var options = '';
    options += '<option value="[data]>">[Data] - ></option>';
    options += '<option value="[data]>=">[Data] - >=</option>';
    options += '<option value="[data]<=">[Data] - <=</option>';
    options += '<option value="[data]<">[Data] - <</option>';

    selectList.append(options);
}

function formatColunas(elements: Array<ColunaBase>): string {

    var colunas = '';

    for (var i = 0; i < elements.length; i++) {
        colunas += '<tr><td>' + elements[i].descricao + '</td></tr>'
    }


    return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' + colunas + '</table>';
}

function getColunas(indexName: string): Array<ColunaBase> {
    var result = new Array<ColunaBase>();

    $.ajax({
        url: '/api/basedados/Colunas/' + indexName,
        method: 'GET',
        async: false,
        success: function (data) {
            result = data.data as Array<ColunaBase>;
        },
        error: function (data) {
            throw data.responseJSON;
        }
    });

    return result;
}

function addFilter(idSelectList: string) {
    var selectList = document.getElementById(idSelectList) as HTMLSelectElement;

}

function montarModals(idBody: string, modalType: string) {
    for (var l = 0; l < basesBusca.length; l++) {
        var field = `<fieldset id="${basesBusca[l].name}">`;

        var select = '';
        var selectListId = `${basesBusca[l].name}-selectListColunas`;

        select += `<select class="custom-select custom-select-sm" id="${selectListId}">`;

        var columns = getColunas(basesBusca[l].name);

        for (var j = 0; j < columns.length; j++) {
            select += '<option value="' + columns[j].descricao + '">' + columns[j].descricao + '</option>';
        }

        select += '</select>';

        var legend = `<legend>${basesBusca[l].name} <button type="button" data-type="${modalType}" data-id-selectlist="${selectListId}" class="btn bt-sm btn-primary btn-add-query"><i class="fas fa-plus-square"></i> Add</button> ${select} </legend>`;
        field += legend;
        field += `<ul class="list-group list-group-flush" id="${basesBusca[l].name}-ulFiltro">`;
        field += '</ul></fieldset>';

        $("#" + idBody).append(field);

        updateClickAddFeature();
    }
}

function unmountModal(idBody: string) {
    var filtrosBody = $("#" + idBody)[0];
    var children = filtrosBody.children;

    for (var i = 0; i < children.length; i++) {
        filtrosBody.removeChild(children[i]);
    }
}

function addSelectItem(idSelectList: string, baseName: string) {
    var selectList = document.getElementById(idSelectList) as HTMLSelectElement;

    if (selectList.value == null || selectList.value == undefined)
        return;

    var baseIndex = basesBusca.findIndex(function (value) {
        return value.name == baseName;
    });

    if (baseIndex == -1) {
        return;
    }

    var base = basesBusca[baseIndex];

    var columnIndex = base.columnsSelect.findIndex(function (value) {
        return value.descricao == selectList.value;
    });

    if (columnIndex != -1)
        return;

    base.columnsSelect.push(new ColunaBase(selectList.value));


}

function updateModalSelect() {

    var fieldsets = Array.from(document.getElementById("selectsBody").children);

    fieldsets.forEach(function (value) {

        for (var b = 0; b < basesBusca.length; b++) {

            if (value.id != basesBusca[b].name)
                continue;

            var ul = document.getElementById(`${value.id}-ulFiltro`) as HTMLUListElement;

            var ulArray = Array.from(ul.children);

            if (basesBusca[b].columnsSelect.length == 0) {
                ulArray.forEach(function (value) {
                    ul.removeChild(value);
                });
            }

            for (var c = 0; c < basesBusca[b].columnsSelect.length; c++) {

                var column = basesBusca[b].columnsSelect[c];
                var liIndex = ulArray.findIndex(function (value) { return value.id == column.descricao; });

                if (liIndex != -1)
                    continue;

                var liElement = document.createElement("li");
                liElement.classList.add("list-group-item", "d-flex", "justify-content-between", "align-items-center");
                liElement.id = column.descricao;
                liElement.innerHTML = `${column.descricao}<span class="badge badge-danger badge-pill">X</span>`;
                ul.append(liElement);
                updateClickDeleteSelect();
            }

            for (var c = 0; c < ulArray.length; c++) {
                var li = ulArray[c];
                var liIndex = basesBusca[b].columnsSelect.findIndex(function (value) { return value.descricao == li.id });

                if (liIndex == -1)
                    ul.removeChild(li);
            }
        }

    });
}

function updateClickAddFeature() {
    $("button.btn-add-query").on("click", function (value) {

        var element = value.currentTarget;
        var dataType = element.getAttribute("data-type");

        switch (dataType) {
            case "select":
                updateSelectList(element);
                break;
            default:
                return;
        }

    });
}

function updateClickDeleteSelect() {
    $("span.badge-danger").on("click", function (value) {

        var element = value.currentTarget;
        var parent = element.parentElement;

        basesBusca.forEach(function value(value) {
            value.columnsSelect.forEach(function (value, index, array) {
                if (value.descricao != parent.id)
                    return;

                array.splice(index, 1);
            });
        });

    });
    $("span.badge-danger").css("cursor", "pointer");
}

function updateSelectList(element: Element) {

    var selectListId = element.getAttribute("data-id-selectlist");

    if (selectListId == null)
        throw `Attribute data-id-selectlist in ${element.id} not found!`;

    var selectList = document.getElementById(selectListId) as HTMLSelectElement;

    if (selectList == null || selectList == undefined)
        throw `SelectList ${selectListId} not found!`;

    var baseBuscaName = selectListId.replace("-selectListColunas", "");

    basesBusca.forEach(function (value) {

        if (value.name != baseBuscaName)
            return;

        var indexColunaExists = value.columnsSelect.findIndex(function (value) { return value.descricao == selectList.value; });

        if (indexColunaExists != -1)
            return;

        value.columnsSelect.push(new ColunaBase(selectList.value));
    });
}

function queryBases() {

    var base = basesBusca[0];

    $.blockUI();

    $.ajax({
        url: '/Consulta/Consultar',
        method: 'POST',
        data: base,
        async: false,
        success: function (data) {
            window.location.href = `/Consulta/DownloadFile/?guid=${data.fileGuid}`;
            $.unblockUI();
        },
        error: function (data) {
            $.unblockUI();
            throw data.responseJSON;
        }
    });
}

