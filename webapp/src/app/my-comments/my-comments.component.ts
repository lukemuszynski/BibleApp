import { CommentDomainObject } from './../models/CommentDomainObject';
import { AuthService } from './../services/auth-service/auth.service';
import { BookService } from './../services/book-service/book.service';
import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-my-comments',
  templateUrl: './my-comments.component.html',
  styleUrls: ['./my-comments.component.scss']
})
export class MyCommentsComponent implements OnInit {

  constructor(private bookService: BookService, public authService: AuthService, private route: ActivatedRoute) { }

  myComments: CommentDomainObject[] = [];

  async ngOnInit() {

    this.myComments = await this.bookService.getMyComments();


  }

}
