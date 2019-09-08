class BaseBusca {
    name: string;
    columns: Array<ColunaBase>;

    constructor(name: string = "", columns: Array<ColunaBase> = new Array<ColunaBase>()) {
        this.name = name;
        this.columns = columns;
    }
}