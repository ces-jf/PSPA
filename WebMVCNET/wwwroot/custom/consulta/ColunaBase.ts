class ColunaBase {
    descricao: string;
    filterType: string;
    valueFilter: string;

    constructor(descricao: string) {
        this.descricao = descricao;
        this.filterType = "";
        this.valueFilter = "";
    }
}