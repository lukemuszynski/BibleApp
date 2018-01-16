import { element } from 'protractor';
import { AuthService } from './../auth-service/auth.service';
import { environment } from './../../../environments/environment';
import { BookDomainObject } from './../../models/BookDomainObject';
import { BookExtendedDomainObject } from './../../models/BookExtendedDomainObject';
import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { Book } from '../../models/Book';
import { CommentDomainObject } from '../../models/CommentDomainObject';

@Injectable()
export class BookService {

  constructor(private http: Http, private authService: AuthService) { }

  booksUrl = environment.apiUrl + 'api/Books/GetBooks';
  bookUrl = environment.apiUrl + 'api/Books/GetBook/';
  addCommentUrl = environment.apiUrl + 'api/Comments/AddComment';
  deleteCommentUrl = environment.apiUrl + 'api/Comments/DeleteComment';
  getCommentsListUrl = environment.apiUrl + 'api/Comments/GetComments';
  getMyCommentsListUrl = environment.apiUrl + 'api/Comments/GetMyComments';

  private books: Book[] = [];

  async getAllBooks(): Promise<Book[]> {
    const response = await this.http.get(this.booksUrl).toPromise();
    this.books = response.json();
    return this.books;
  }

  async getComments(): Promise<CommentDomainObject[]> {
    let request;
    if (this.authService.IsAuthorized) {
      const bearer = 'Bearer ' + this.authService.GetToken();
      const headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': bearer });
      const options = new RequestOptions({ headers: headers });
      request = this.http.get(this.getCommentsListUrl, options);
    } else {
      request = this.http.get(this.getCommentsListUrl);
    }
    const response = (await request.toPromise()).json();
    // tslint:disable-next-line:no-shadowed-variable
    response.forEach(element => {
      this.books.find(x => {
        const subBook = x.Subbooks.find(y => y.Guid === element.BookGuid);
        if (subBook) {
          element.BookName = subBook.BookName;
          return true;
        }
        return false;
      });
    });
    return response;
  }

  async getMyComments(): Promise<CommentDomainObject[]> {
    let request;
    const bearer = 'Bearer ' + this.authService.GetToken();
    const headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': bearer });
    const options = new RequestOptions({ headers: headers });
    request = this.http.get(this.getMyCommentsListUrl, options);

    const response = (await request.toPromise()).json();
    // tslint:disable-next-line:no-shadowed-variable
    response.forEach(element => {
      this.books.find(x => {
        const subBook = x.Subbooks.find(y => y.Guid === element.BookGuid);
        if (subBook) {
          element.BookName = subBook.BookName;
          return true;
        }
        return false;
      });
    });
    return response;
  }

  async getAllBookExtended(guid: string): Promise<BookExtendedDomainObject> {
    let request;
    if (this.authService.IsAuthorized) {
      const bearer = 'Bearer ' + this.authService.GetToken();
      const headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': bearer });
      const options = new RequestOptions({ headers: headers });
      request = this.http.get(this.bookUrl + guid, options);
    } else {
      request = this.http.get(this.bookUrl + guid);
    }
    const response = await request.toPromise();
    return response.json();
  }

  async addComment(comment: CommentDomainObject): Promise<BookExtendedDomainObject> {
    const bearer = 'Bearer ' + this.authService.GetToken();
    const headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': bearer });
    const options = new RequestOptions({ headers: headers });
    const response = await this.http.post(this.addCommentUrl, JSON.stringify(comment), options).toPromise();
    return response.json();
  }

  async deleteComment(comment: CommentDomainObject): Promise<boolean> {
    const bearer = 'Bearer ' + this.authService.GetToken();
    const headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': bearer });
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
