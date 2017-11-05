import { BookDomainObject } from './models/BookDomainObject';
import { BookService } from './services/book-service/book.service';
import { MatSidenavModule, MatSidenav } from '@angular/material';
import { CustomMaterialModule } from './custom-material/custom-material.module';
import { Component, ViewChild, OnInit } from '@angular/core';
import { Book } from './models/Book';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  calc2Cols = '2 2 calc(10em + 10px);';
  /** 10px is the missing margin of the missing box */
  calc3Cols = '3 3 calc(15em + 20px)';
  title = 'Bible explorer';

  currentStep: String = 'book';
  step = 0;

  bookService: BookService;
  @ViewChild('sidenav')
  private matSidenav: MatSidenav;

  booksObjects: Book[];
  selectedBook: Book;
  selectedSubbook: BookDomainObject;

  constructor(_bookService: BookService) {
    this.bookService = _bookService;
  }

  async ngOnInit() {
    this.prepareBooksSidenav(await this.bookService.getAllBooks()); // .then( x => this.prepareBooksSidenav(x));
  }

  selectSubbook(book, subbook) {
    this.selectedBook = book;
    this.selectedSubbook = subbook;
    this.matSidenav.close();
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

  prepareBooksSidenav(booksObjects: Book[]) {
    this.booksObjects = booksObjects;
    booksObjects.forEach(x => x.Subbooks.sort((y, z) => y.BookGlobalNumber - z.BookGlobalNumber));
    booksObjects.sort((x, y) => x.StartGlobalIndex - y.StartGlobalIndex);
    console.log(this.booksObjects);
  }

}
