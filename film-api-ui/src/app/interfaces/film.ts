import { FilmComment } from './film-comment';

export interface Film {
    filmID: number,
    title: string,
    addedBy: string,
    description: string,
    genre: string,
    director?: string,
    images?: (string | ArrayBuffer)[];
    comments?: FilmComment[],
    averageMark?: number
}