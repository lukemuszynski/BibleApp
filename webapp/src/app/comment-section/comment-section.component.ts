import { MatSnackBar } from '@angular/material';
import { CommentDomainObject } from './../models/CommentDomainObject';
import { BookExtendedDomainObject } from './../models/BookExtendedDomainObject';
import { BookService } from './../services/book-service/book.service';
import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { Book } from '../models/Book';
import { FormBuilder, FormGroup } from '@angular/forms/forms';

@Component({
    selector: 'app-comment-section',
    templateUrl: './comment-section.component.html',
    styleUrls: ['./comment-section.component.scss']
})
export class CommentSectionComponent implements OnInit {

    step = 0;

    @Input('book')
    book: BookExtendedDomainObject;

    ngOnInit(): void {
        // throw new Error("Method not implemented.");
    }


    constructor(private _bookService: BookService, private route: ActivatedRoute,
        private sanitizer: DomSanitizer, public snackBar: MatSnackBar) { }



    setStep(index: number) {
        this.step = index;
    }

    nextStep() {
        this.step++;
    }

    prevStep() {
        this.step--;
    }



    getUrl(comment: CommentDomainObject) {
        // tslint:disable-next-line:curly
        if (comment.IsYoutubeVideo)
            return 'https://www.youtube.com/watch?v=' + comment.Url;
        else {
            return comment.Url;
        }
    }

    async deleteComment(comment: CommentDomainObject) {
        const commentToDelete = new CommentDomainObject();
        commentToDelete.Guid = comment.Guid;
        commentToDelete.ManageCommentKeyGuid = comment.ManageCommentKeyGuid;
        const response = await this._bookService.deleteComment(commentToDelete);
        if (response) {
            this.book.Comments = this.book.Comments.filter(x => x.Guid !== commentToDelete.Guid);
            this.snackBar.open('Usunięto komentarz', '', { duration: 2000 });
        }
    }
}
