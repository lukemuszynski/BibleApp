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

  async getAllBooks(): Promise<Book[]> {
    const response = await this.http.get(this.booksUrl).toPromise();
    return response.json();
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
    const response = await request.toPromise();
    return response.json();
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
