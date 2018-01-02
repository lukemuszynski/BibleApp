import { CommentDomainObject } from './../models/CommentDomainObject';
import { BookExtendedDomainObject } from './../models/BookExtendedDomainObject';
import { BookService } from './../services/book-service/book.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { Book } from '../models/Book';
import { FormBuilder, FormGroup } from '@angular/forms/forms';
import { MatSnackBar } from '@angular/material';
import { CustomMaterialModule } from '../custom-material/custom-material.module';

@Component({
  selector: 'app-book-content',
  templateUrl: './book-content.component.html',
  styleUrls: ['./book-content.component.scss']
})
export class BookContentComponent implements OnInit {

  constructor(private _bookService: BookService, private route: ActivatedRoute, private sanitizer: DomSanitizer,
    public snackBar: MatSnackBar) { }
  private sub: any;
  private bookGuid: string;
  private book: BookExtendedDomainObject;
  wholeBookComment = false;
  step = 0;
  selectedTabIndex = 0;
  newComment: CommentDomainObject;

  setStep(index: number) {
    this.step = index;
  }

  nextStep() {
    this.step++;
  }

  prevStep() {
    this.step--;
  }


  async ngOnInit() {

    this.book = this.CreateDefault();
    this.cleanComment();


    this.sub = this.route.params.subscribe(async params => {
      this.bookGuid = params['guid'];
      this.book = await this._bookService.getAllBookExtended(this.bookGuid);
      this.book.Passages.sort((a, b) => (a.PassageNumber - b.PassageNumber));
      this.book.Comments.forEach(x => {
        const ownerGuid = localStorage['comment' + x.Guid];

        if (ownerGuid != null) {
          x.ManageCommentKeyGuid = ownerGuid;
        }


      });
      console.log(this.book);
    });

  }

  CreateDefault() {
    const book = new BookExtendedDomainObject();
    book.Guid = '00000000-0000-0000-0000-000000000000';
    book.Passages = [];
    book.BookName = '';
    book.BookFullName = '';
    return book;
  }
  getUrl(comment: CommentDomainObject) {
    return 'https://www.youtube.com/watch?v=' + comment.Url;
  }

  async addComment() {
    console.log(this.newComment);


    this.newComment.BookGuid = this.bookGuid;
    if (this.wholeBookComment) {
      this.newComment.StartIndex = '1';
      this.newComment.EndIndex = this.book.Passages.length.toString();
    }

    const book = await this._bookService.addComment(this.newComment);
    console.log(book);
    this.book.Comments = book.Comments;
    const addedComment = book.Comments.find(x => x.Title === this.newComment.Title);
    localStorage['comment' + addedComment.Guid] = addedComment.ManageCommentKeyGuid;
    this.cleanComment();
    this.snackBar.open('Dodano komentarz', '', { duration: 2000});
  }

  cleanComment() {
    this.newComment = new CommentDomainObject();
    this.newComment.Text = '';
    this.newComment.Url = '';
    this.newComment.Title = '';
    this.newComment.EndIndex = '';
    this.newComment.StartIndex = '';
  }
}
