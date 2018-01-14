import { AuthenticationComponent } from './authentication/authentication.component';
import { AuthService } from './services/auth-service/auth.service';
import { BookDomainObject } from './models/BookDomainObject';
import { BookService } from './services/book-service/book.service';
import { MatSidenavModule, MatSidenav } from '@angular/material';
import { CustomMaterialModule } from './custom-material/custom-material.module';
import { Component, ViewChild, OnInit, Inject } from '@angular/core';
import { Book } from './models/Book';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

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
  term = '';
  @ViewChild('sidenav')
  private matSidenav: MatSidenav;

  booksObjects: Book[];
  selectedBook: Book;
  selectedSubbook: BookDomainObject;

  constructor(private bookService: BookService, private route: ActivatedRoute, private authService: AuthService,
    public dialog: MatDialog
  ) {

  }

  async ngOnInit() {
    this.prepareBooksSidenav(await this.bookService.getAllBooks());

    await this.route.params.subscribe(async params => {
      const subbookGuid = params['guid'];
      if (subbookGuid) {
        this.booksObjects.find(x => {
          const foundSubbook = x.Subbooks.find(y => y.Guid === subbookGuid);
          if (foundSubbook !== undefined) {
            this.selectedSubbook = foundSubbook;
            this.selectedBook = x;
            return true;
          }
        });
      } else {
        this.selectedSubbook = new BookDomainObject();
        this.selectedSubbook.Guid = '0';
      }
    });
  }

  selectSubbook(book, subbook) {
    this.selectedBook = book;
    this.selectedSubbook = subbook;
    this.matSidenav.close();
    console.log(subbook);
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
    booksObjects.forEach(x => x.Subbooks.sort((y, z) => y.SubbookNumber - z.SubbookNumber));
    booksObjects.sort((x, y) => x.StartGlobalIndex - y.StartGlobalIndex);
    console.log(this.booksObjects);
  }

  getChipColor(subbook: BookDomainObject): string {
    return subbook.Guid === this.selectedSubbook.Guid ? 'warn' : 'secondary';
  }

  getChipSelected(subbook: BookDomainObject): boolean {
    return subbook.Guid === this.selectedSubbook.Guid;
  }

  openAuthenticationDialog() {
    const dialogRef = this.dialog.open(AuthenticationComponent, {
      width: '600px',
    });
  }

  async logout() {
    await this.authService.Logout();
  }
}
