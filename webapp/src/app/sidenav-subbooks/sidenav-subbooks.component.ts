import { BookDomainObject } from './../models/BookDomainObject';
import { BookService } from './../services/book-service/book.service';
import { Book } from './../models/Book';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { OutputEmitter } from '@angular/compiler/src/output/abstract_emitter';

@Component({
  selector: 'app-sidenav-subbooks',
  templateUrl: './sidenav-subbooks.component.html',
  styleUrls: ['./sidenav-subbooks.component.scss']
})
export class SidenavSubbooksComponent implements OnInit {

  @Input()
  Book: Book;

  @Output()
  subbookChanged: EventEmitter<any> = new EventEmitter<any>();

  constructor(private bookService: BookService) { }

  ngOnInit() {
  }

  selectSubbook(selectedBook: BookDomainObject) {
    if (this.bookService.selectedBook) {
      this.bookService.selectedBook._isSelected = false;
    }
    this.bookService.selectedBook = selectedBook;
    this.bookService.selectedBook._isSelected = true;
    this.subbookChanged.emit(true);
  }
}
