class BaseBusca {
    name: string;
    columnsFilter: Array<ColunaBase>;
    columnsSelect: Array<ColunaBase>;

    constructor(name: string = "", columnsFilter: Array<ColunaBase> = new Array<ColunaBase>(), columnsSelect: Array<ColunaBase> = new Array <ColunaBase>()) {
        this.name = name;
        this.columnsFilter = columnsFilter;
        this.columnsSelect = columnsSelect;
    }
}