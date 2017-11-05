import { CommentDomainObject } from './../models/CommentDomainObject';
import { BookExtendedDomainObject } from './../models/BookExtendedDomainObject';
import { BookService } from './../services/book-service/book.service';
import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { Book } from '../models/Book';
import { FormBuilder, FormGroup } from "@angular/forms/forms";

@Component({
    selector: 'app-comment-section',
    templateUrl: './comment-section.component.html',
    styleUrls: ['./comment-section.component.scss']
})
export class CommentSectionComponent implements OnInit {

    ngOnInit(): void {
        // throw new Error("Method not implemented.");
    }


    constructor(private _bookService: BookService, private route: ActivatedRoute, private sanitizer: DomSanitizer) { }

    step = 0;

    setStep(index: number) {
      this.step = index;
    }
  
    nextStep() {
      this.step++;
    }
  
    prevStep() {
      this.step--;
    }

    @Input("book")
    book: BookExtendedDomainObject;

    getUrl(comment: CommentDomainObject) {
        return 'https://www.youtube.com/watch?v=' + comment.Url;
    }


}