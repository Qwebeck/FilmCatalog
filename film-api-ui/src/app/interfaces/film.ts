import { FilmComment } from './film-comment';

export class Film {
    filmID: number;
    title: string;
    addedBy: string;
    description: string;
    genre: string;
    director?: string;
    images?: (string | ArrayBuffer)[];
    comments?: FilmComment[];
    averageMark?: number;

    constructor() {
        this.filmID = 0;
        this.title = '';
        this.addedBy = '';
        this.description = '';
        this.genre = '';
    }
}