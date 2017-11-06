import { CommentDomainObject } from './CommentDomainObject';
import { PassageDomainObject } from './PassageDomainObject';

export class BookExtendedDomainObject {
      Guid: String;
      BookName: String;
      BookFullName: String;
      Passages: PassageDomainObject[];
      SubbookNumber: number;
      BookGlobalNumber: number;
      NextBookGuid: string;
      PreviousBookGuid: string;
      Comments: CommentDomainObject[];
}
