import { AuthenticationComponent } from './../authentication/authentication.component';
import { RegisterUserData } from './../models/RegisterUserData';
import { AuthService } from './../services/auth-service/auth.service';
import { Component, OnInit } from '@angular/core';
import { CustomMaterialModule } from '../custom-material/custom-material.module';
import { Validators, FormControl, FormGroupDirective, NgForm, FormGroup } from '@angular/forms';

import { ErrorStateMatcher } from '@angular/material/core';
import { MatDialogRef, MatSnackBar } from '@angular/material';

/** Error when invalid control is dirty, touched, or submitted. */
export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(public authService: AuthService, private dialogRef: MatDialogRef<AuthenticationComponent>, private snackBar: MatSnackBar) { }
  hash = '';

  // https://www.jokecamp.com/blog/angular-whitespace-validator-directive/
  // email form control
  matcher = new MyErrorStateMatcher();
  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);
  // login form control
  loginFormControl = new FormControl('',
    [Validators.required]
  );
  // Password form control

  passwordFormControl = new FormControl('', [
    Validators.required,
    Validators.minLength(6)
  ]);

  passwordConfirmFormControl = new FormControl('', [
    Validators.required
  ]);

  passwordFormGroup = new FormGroup({
    'passwordFormControl': this.passwordFormControl,
    'passwordConfirmFormControl': this.passwordConfirmFormControl,
    'loginFormControl': this.loginFormControl,
    'emailFormControl': this.emailFormControl
  },
    x => {
      if (x.value['passwordFormControl'] === x.value['passwordConfirmFormControl']) { return null; }
      // tslint:disable-next-line:one-line
      else {
        this.passwordConfirmFormControl.setErrors({ 'passNotMatch': 'Password do not match' });
        return { 'passNotMatch': 'Password do not match' };
      }
    }
  );



  async ngOnInit() {
    this.hash = await this.authService.hashPassword('passwordForMyMom');

  }

  async sendRegistrationForm() {
    const model = new RegisterUserData();
    model.EmailAddress = this.emailFormControl.value;
    model.Login = this.loginFormControl.value;
    model.Password = await this.authService.hashPassword(this.passwordFormControl.value);
    console.log(model);

    const response = await this.authService.Register(model);
    if (response.Ok) {
      this.authService.SaveToken(response.Value);
      this.snackBar.open('Zarejestrowano pomyślnie', '', { duration: 2000 });
      this.dialogRef.close();
    } else if (response.StatusMessage === 'EMAIL_IS_USED') {
      this.snackBar.open('Ten adres email jest już zarejestrowany', '', { duration: 2000 });
    } else if (response.StatusMessage === 'LOGIN_IS_USED') {
      this.snackBar.open('Ten login jest już zarejestrowany', '', { duration: 2000 });
    } else if (response.StatusMessage === 'EMAIL_AND_LOGIN_IS_USED') {
      this.snackBar.open('Ten adres email i login jest już zarejestrowany', '', { duration: 2000 });
    } else {
      this.snackBar.open('Wystąpił nieznany błąd', '', { duration: 2000 });
    }
  }

}
