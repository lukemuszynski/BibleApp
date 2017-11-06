import { BookDomainObject } from './../../models/BookDomainObject';
import { BookExtendedDomainObject } from './../../models/BookExtendedDomainObject';
import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { Book } from '../../models/Book';
import { CommentDomainObject } from '../../models/CommentDomainObject';

@Injectable()
export class BookService {

  constructor(private http: Http) { }

  booksUrl = 'http://localhost:5544/api/Books/GetBooks';
  bookUrl = 'http://localhost:5544/api/Books/GetBook/';
  addCommentUrl = 'http://localhost:5544/api/Books/AddComment';
  deleteCommentUrl = 'http://localhost:5544/api/Books/DeleteComment';

  async getAllBooks(): Promise<Book[]> {
    const response = await this.http.get(this.booksUrl).toPromise();
    return response.json();
  }

  async getAllBookExtended(guid: string): Promise<BookExtendedDomainObject> {
    const response = await this.http.get(this.bookUrl + guid).toPromise();
    return response.json();
  }

  async addComment(comment: CommentDomainObject): Promise<BookExtendedDomainObject> {

    const headers = new Headers({ 'Content-Type': 'application/json' });
    const options = new RequestOptions({ headers: headers });
    const response = await this.http.post(this.addCommentUrl, JSON.stringify(comment), options).toPromise();
    return response.json();
  }

  async deleteComment(comment: CommentDomainObject): Promise<boolean> {
    const headers = new Headers({ 'Content-Type': 'application/json' });
    const options = new RequestOptions({ headers: headers });
    const response = await this.http.post(this.deleteCommentUrl, JSON.stringify(comment), options).toPromise();
    if (response.status === 200) {
      console.log(response);
      return true;
    } else {
      return false;
    }
  }
}
