import { Categoria } from "./categoria.models";

export interface Tarefa{
    tarefaId? : number;
    titulo : string;
    descricao : string;
    criadoEm? : string;
    categoriaId : number;
    categoria ?: Categoria;
    status? : string;
}