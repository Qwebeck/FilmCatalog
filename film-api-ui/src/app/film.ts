export interface Film {
    filmID: number,
    title: string,
    addedBy: string,
    description: string,
    genre: string,
    director?: string,
    image?: string | ArrayBuffer;
}