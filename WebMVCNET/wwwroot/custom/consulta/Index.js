var basesBusca = new Array();
$("#filtrosModal").on('show.bs.modal', function () {
    montarModals("filtrosBody");
});
$("#filtrosModal").on('hidden.bs.modal', function () {
    unmountModal("filtrosBody");
});
$("#selectsModal").on('show.bs.modal', function () {
    montarModals("selectsBody");
});
$("#selectsModal").on('hidden.bs.modal', function () {
    unmountModal("selectsBody");
});
setInterval(function () {
    var element = $("#baseArea");
    for (var z = 0; z < basesBusca.length; z++) {
        var children = element.children("#" + basesBusca[z].name);
        if (children.length == 0)
            element.append('<span id=' + basesBusca[z].name + ' class="badge badge-info">' + basesBusca[z].name + '</span>');
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
    var dataRow = row.data();
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
        var dataRow = row.data();
        var data = getColunas(dataRow.name);
        row.child(formatColunas(data)).show();
        tr.addClass('shown');
    }
});
//Funcions Definitions
function montarFiltro(selectName) {
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
function formatColunas(elements) {
    var colunas = '';
    for (var i = 0; i < elements.length; i++) {
        colunas += '<tr><td>' + elements[i].descricao + '</td></tr>';
    }
    return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' + colunas + '</table>';
}
function getColunas(indexName) {
    var result = new Array();
    $.ajax({
        url: '/api/basedados/Colunas/' + indexName,
        method: 'GET',
        async: false,
        success: function (data) {
            result = data.data;
        },
        error: function (data) {
            throw data.responseJSON;
        }
    });
    return result;
}
function addFilter(idSelectList) {
    var selectList = document.getElementById(idSelectList);
}
function montarModals(idBody) {
    for (var l = 0; l < basesBusca.length; l++) {
        var field = '<fieldset id="' + basesBusca[l].name + '-fieldset">';
        var select = '';
        select += '<select class="custom-select custom-select-sm" id="' + basesBusca[l].name + '-selectListColunas">';
        var columns = getColunas(basesBusca[l].name);
        for (var j = 0; j < columns.length; j++) {
            select += '<option value="' + columns[j].descricao + '">' + columns[j].descricao + '</option>';
        }
        select += '</select>';
        var legend = '<legend>' + basesBusca[l].name + ' <button type="button" data-id-seleclist="' + basesBusca[l].name + '-selectListColunas" class="btn bt-sm btn-primary"><i class="fas fa-plus-square"></i> Add</button>' + select + '</legend>';
        field += legend;
        field += '<ul class="list-group list-group-flush" id="' + basesBusca[l].name + '-ulFiltro">';
        field += '</ul></fieldset>';
        $("#" + idBody).append(field);
    }
}
function unmountModal(idBody) {
    var filtrosBody = $("#" + idBody)[0];
    var children = filtrosBody.children;
    for (var i = 0; i < children.length; i++) {
        filtrosBody.removeChild(children[i]);
    }
}
function addSelectItem(idSelectList, baseName) {
    var selectList = document.getElementById(idSelectList);
    if (selectList.value == null || selectList.value == undefined)
        return;
    var baseIndex = basesBusca.findIndex(function (value) {
        value.name == baseName;
    });
    if (baseIndex == -1) {
        return;
    }
    var base = basesBusca[baseIndex];
    var columnIndex = base.columnsSelect.findIndex(function (value) {
        value.descricao = selectList.value;
    });
    if (columnIndex != -1)
        return;
    base.columnsSelect.push(new ColunaBase(selectList.value));
}
function updateModalSelect() {
    var fieldsets = document.getElementById("selectsBody").children;
    for (var s = 0; s < basesBusca.length; s++) {
        for (var p = 0; p < fieldsets.length; p++) {
            if (basesBusca[s].name != fieldsets[p].id)
                return;
        }
    }
}
//# sourceMappingURL=Index.js.map