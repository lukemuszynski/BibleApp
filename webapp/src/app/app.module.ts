import { MatDialogRef, MatDialogModule, MatDialog } from '@angular/material';
import { BookService } from './services/book-service/book.service';
import { CustomMaterialModule } from './custom-material/custom-material.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { HttpModule } from '@angular/http';
import { BookContentComponent } from './book-content/book-content.component';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommentSectionComponent } from './comment-section/comment-section.component';
import { ClipboardModule } from 'ngx-clipboard';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { CommentListComponent } from './comment-list/comment-list.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { AuthService } from './services/auth-service/auth.service';
import { AuthenticationComponent } from './authentication/authentication.component';
import { NotificationsService } from './services/notifications-service/notifications.service';
import { MyCommentsComponent } from './my-comments/my-comments.component';
import { SidenavSubbooksComponent } from './sidenav-subbooks/sidenav-subbooks.component';

const appRoutes: Routes = [
  {
    path: 'Book/:guid',
    component: BookContentComponent
  },
  {
    path: '',
    component: CommentListComponent,
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'Register',
    component: RegisterComponent
  },
  {
    path: 'Authentication',
    component: AuthenticationComponent
  },
  {
    path: 'MyComments',
    component: MyCommentsComponent
  }
];

@NgModule({
  declarations: [
    AppComponent,
    BookContentComponent,
    CommentSectionComponent,
    CommentListComponent,
    RegisterComponent,
    LoginComponent,
    AuthenticationComponent,
    MyCommentsComponent,
    SidenavSubbooksComponent
  ],
  imports: [
    CustomMaterialModule,
    ReactiveFormsModule,
    BrowserModule,
    HttpModule,
    FormsModule,
    // MatDialogModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true, useHash: true } // <-- debugging purposes only
    ),
    ClipboardModule,
    Ng2SearchPipeModule
  ],
  providers: [BookService, AuthService, NotificationsService, MatDialog],
  bootstrap: [AppComponent],
  // entryComponents: [AuthenticationComponent]
})
export class AppModule { }
