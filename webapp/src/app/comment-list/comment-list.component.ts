import { AuthService } from './../services/auth-service/auth.service';
import { MatSnackBar } from '@angular/material';
import { BookService } from './../services/book-service/book.service';
import { Component, OnInit } from '@angular/core';
import { CommentDomainObject } from '../models/CommentDomainObject';
import { CustomMaterialModule } from '../custom-material/custom-material.module';

@Component({
    selector: 'app-comment-list',
    templateUrl: './comment-list.component.html',
    styleUrls: ['./comment-list.component.scss']
})
export class CommentListComponent implements OnInit {

    constructor(private _bookService: BookService, public snackBar: MatSnackBar, private authService: AuthService) { }
    step = 0;
    comments: CommentDomainObject[] = [];

    async ngOnInit() {

        this.comments = await this._bookService.getComments();

    }
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
            this.comments = this.comments.filter(x => x.Guid !== commentToDelete.Guid);
            this.snackBar.open('Usunięto komentarz', '', { duration: 2000 });
        }
    }
}
