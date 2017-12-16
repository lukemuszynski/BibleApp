import { BookService } from './services/book-service/book.service';
import { CustomMaterialModule } from './custom-material/custom-material.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { HttpModule } from '@angular/http';
import { BookContentComponent } from './book-content/book-content.component';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommentSectionComponent } from './comment-section/comment-section.component';
import { ClipboardModule } from 'ngx-clipboard';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { CommentListComponent } from './comment-list/comment-list.component';
const appRoutes: Routes = [
  {
    path: 'Book/:guid',
    component: BookContentComponent
  },
  {
    path: '',
    component: CommentListComponent,
    pathMatch: 'full'
  }

];

@NgModule({
  declarations: [
    AppComponent,
    BookContentComponent,
    CommentSectionComponent,
    CommentListComponent
  ],
  imports: [
    CustomMaterialModule,
    BrowserModule,
    HttpModule,
    FormsModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true, useHash: true } // <-- debugging purposes only
    ),
    ClipboardModule,
    Ng2SearchPipeModule
  ],
  providers: [BookService],
  bootstrap: [AppComponent]
})
export class AppModule { }
