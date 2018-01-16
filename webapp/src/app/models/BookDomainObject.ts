export class BookDomainObject {
    Guid: string;
    BookName: string;
    BookFullName: string;
    SubbookNumber: number;
    BookGlobalNumber: number;

    _isSelected: Boolean = false;
}
