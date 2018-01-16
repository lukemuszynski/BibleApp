import { SidenavSubbooksComponent } from './sidenav-subbooks/sidenav-subbooks.component';
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

  @ViewChild('sidenav')
  private matSidenav: MatSidenav;

  booksObjects: Book[];

  constructor(private bookService: BookService, private route: ActivatedRoute, public authService: AuthService,
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
            this.bookService.selectedBook = foundSubbook;
            foundSubbook._isSelected = true;
            return true;
          }
        });
      }
    });
  }

  selectSubbook() {
    this.matSidenav.close();
  }

  // tslint:disable-next-line:member-ordering
  setStepBook: Book = new Book();
  setBook(book: Book) {
    this.setStepBook = book;
  }

  prepareBooksSidenav(booksObjects: Book[]) {
    this.booksObjects = booksObjects;
    booksObjects.forEach(x => x.Subbooks.sort((y, z) => y.SubbookNumber - z.SubbookNumber));
    booksObjects.sort((x, y) => x.StartGlobalIndex - y.StartGlobalIndex);
    console.log(this.booksObjects);
  }

  // getChipColor(subbook: BookDomainObject): string {
  //   return subbook.Guid === this.selectedSubbook.Guid ? 'warn' : 'secondary';
  // }

  openAuthenticationDialog() {
    const dialogRef = this.dialog.open(AuthenticationComponent, {
      width: '600px',
    });
  }

  async logout() {
    await this.authService.Logout();
  }
}
