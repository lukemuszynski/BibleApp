import { BookDomainObject } from './BookDomainObject';
export class Book {
    BookFullName: String;
    Subbooks: BookDomainObject[];
    StartGlobalIndex: number;
}