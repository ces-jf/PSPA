var basesBusca = new Array<BaseBusca>();

//Funcions Definitions
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

        var legend = `<legend>${basesBusca[l].name} <button id="add-button" type="button" data-type="${modalType}" data-id-selectlist="${selectListId}" class="btn bt-sm btn-primary btn-add-query"><i class="fas fa-plus-square"></i> Add</button> ${select} </legend>`;
        field += legend;
        field += `<ul class="list-group list-group-flush" id="${basesBusca[l].name}-ulFiltro">`;
        field += '</ul></fieldset>';

        $("#" + idBody).append(field);

        updateClickAddFeature();

        if (modalType == "runGraphics") {
            var buttonAdd = document.getElementById("add-button") as HTMLButtonElement;
            buttonAdd.parentElement.removeChild(buttonAdd);
        }
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

function updateModalFilter() {

    var fieldsets = Array.from(document.getElementById("filtrosBody").children);

    fieldsets.forEach(function (value) {

        for (var b = 0; b < basesBusca.length; b++) {

            if (value.id != basesBusca[b].name)
                continue;

            var ul = document.getElementById(`${value.id}-ulFiltro`) as HTMLUListElement;

            var ulArray = Array.from(ul.children);

            if (basesBusca[b].columnsFilter.length == 0) {
                ulArray.forEach(function (value) {
                    ul.removeChild(value);
                });
            }

            for (var c = 0; c < basesBusca[b].columnsFilter.length; c++) {

                var column = basesBusca[b].columnsFilter[c];
                var liIndex = ulArray.findIndex(function (value) { return value.id == `${column.descricao}-${column.filterType}`; });

                if (liIndex != -1)
                    continue;

                var selectList = document.createElement('select');
                selectList.id = `${column.descricao}-${column.filterType}-selectFilterType`;
                selectList.classList.add("form-control", "form-control-sm");

                var options = '';
                options += '<option value="">Select a filter...</option>';
                options += '<option value=">">Greather than</option>';
                options += '<option value=">=">Greather or equals than</option>';
                options += '<option value="<=">Less or equals than</option>';
                options += '<option value="<">Less than</option>';
                options += '<option value="=">Equals than</option>';

                selectList.innerHTML = options;

                var indexSelected = Array.from(selectList.options).findIndex(function (value) { return value.value == column.filterType });
                if (indexSelected == -1)
                    indexSelected = 0;

                selectList.value = column.filterType;

                var liElement = document.createElement("li");
                liElement.classList.add("list-group-item", "d-flex", "justify-content-between", "align-items-center");
                liElement.id = `${column.descricao}-${column.filterType}`;
                liElement.innerHTML = `<div>${column.descricao}</div>&nbsp; ${selectList.outerHTML} &nbsp; <input id="${column.descricao}-${column.filterType}-formValue" value="${column.valueFilter}" class="form-control form-control-sm" type="text" placeholder="Value..."> &nbsp; <span class="badge badge-danger badge-pill">X</span>`;
                ul.append(liElement);
                updateClickDeleteFilter();

                (document.getElementById(selectList.id) as HTMLSelectElement).selectedIndex = indexSelected;
            }

            for (var c = 0; c < ulArray.length; c++) {
                var li = ulArray[c];
                var liIndex = basesBusca[b].columnsFilter.findIndex(function (value) { return `${value.descricao}-${value.filterType}` == li.id });

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
            case "filter":
                updateFilterList(element);
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

function updateClickDeleteFilter() {
    $("span.badge-danger").on("click", function (value) {

        var element = value.currentTarget;
        var parent = element.parentElement;

        basesBusca.forEach(function value(value) {
            value.columnsFilter.forEach(function (value, index, array) {
                if (`${value.descricao}-${value.filterType}` != parent.id)
                    return;

                array.splice(index, 1);
            });
        });

    });
    $("span.badge-danger").css("cursor", "pointer");
}

function updateFilterList(element: Element) {

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

        var indexColunaExists = value.columnsFilter.findIndex(function (value) { return `${value.descricao}-${value.filterType}` == `${selectList.value}-`; });

        if (indexColunaExists != -1)
            return;

        value.columnsFilter.push(new ColunaBase(selectList.value));
    });
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
    base.allEntries = (document.getElementById("checkboxEntries") as HTMLInputElement).checked;
    base.numberEntries = (document.getElementById("formEntries") as HTMLInputElement).valueAsNumber;

    $.blockUI();

    $.ajax({
        url: '/Query/Run',
        method: 'POST',
        data: base,
        async: false,
        success: function (data) {
            window.location.href = `/Query/DownloadFile/?guid=${data.fileGuid}`;
            $.unblockUI();
        },
        error: function (data) {
            $.unblockUI();
            throw data.responseJSON;
        }
    });
}

function runGraphicsBases() {

    var base = basesBusca[0];
    base.allEntries = (document.getElementById("checkboxEntries") as HTMLInputElement).checked;
    base.numberEntries = (document.getElementById("formEntries") as HTMLInputElement).valueAsNumber;

    var graphicType = (document.getElementById("graphicSelectTypeList") as HTMLSelectElement).value;

    if (graphicType == undefined || graphicType == "" || graphicType == null) {
        alert("Need to select ONE Graphic Type!");
        return;
    }

    if (base.columnsSelect.length != 1) {
        alert("Only need ONE select field!");
        return;
    }

    $.blockUI();

    $.ajax({
        url: '/Query/RunGraphics',
        method: 'POST',
        data: base,
        async: false,
        success: function (data) {
            var selectColumn = base.columnsSelect[0].descricao;
            drawChart(data, selectColumn, base.columnsFilter, graphicType);
            $("#graphicsConfigModal").modal('toggle');
            $.unblockUI();
        },
        error: function (data) {
            $.unblockUI();
            throw data.responseJSON;
        }
    });
}

function drawChart(elements: Array<object>, groupColumn: string, filterColumns: Array<ColunaBase>, graphicType: string) {
    var chartDiv = document.getElementById("chartDiv");
    var data = new google.visualization.DataTable();
    data.addColumn('string', groupColumn);
    data.addColumn('number', `Number of ${groupColumn}`);

    elements.forEach(function (value, index, array) {
        var columnName = Object.keys(array[index])[0];
        var valueNumber = parseInt(value[columnName] as string);
        data.addRow([columnName, valueNumber]);
    });

    var filterText = "";

    filterColumns.forEach(function (value, index) {
        if (index == 0)
            filterText = "and filtered by ";

        if (index > 0)
            filterText += ` and `;

        filterText += `${value.descricao} ${value.filterType} ${value.valueFilter}`;
    });

    var options = { "title": `Graphic based on ${groupColumn} ${filterText}` };

    var chart = null;

    switch (graphicType) {
        case "pie":
            chart = new google.visualization.PieChart(chartDiv);
            break;
        case "bar":
            chart = new google.visualization.BarChart(chartDiv);
            break;
        case "column":
            chart = new google.visualization.ColumnChart(chartDiv);
            break;
        case "histogram":
            chart = new google.visualization.Histogram(chartDiv);
            break;
        case "table":
            chart = new google.visualization.Table(chartDiv);
            break;
        default:
            break;
    }

    chart.draw(data, options);
}
