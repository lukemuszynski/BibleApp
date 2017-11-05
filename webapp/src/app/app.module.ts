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

const appRoutes: Routes = [
  { path: 'Book/:guid',      component: BookContentComponent },
  { path: '',
    redirectTo: '/',
    pathMatch: 'full'
  }
];

@NgModule({
  declarations: [
    AppComponent,
    BookContentComponent,
    CommentSectionComponent
  ],
  imports: [
    CustomMaterialModule,
    BrowserModule,
    HttpModule,
    FormsModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true } // <-- debugging purposes only
    )
  ],
  providers: [BookService],
  bootstrap: [AppComponent]
})
export class AppModule { }
