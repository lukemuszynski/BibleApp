import { AuthenticationComponent } from './../authentication/authentication.component';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar, MatDialogRef } from '@angular/material';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { AuthService } from './../services/auth-service/auth.service';
import { Component, OnInit } from '@angular/core';
import { CustomMaterialModule } from '../custom-material/custom-material.module';
import { Credentials } from '../models/Credentials';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent implements OnInit {

  constructor(private authService: AuthService, private snackBar: MatSnackBar, private route: ActivatedRoute, private router: Router,
    private dialogRef: MatDialogRef<AuthenticationComponent>) { }
  loginFormControl = new FormControl('',
    [Validators.required]
  );

  passwordFormControl = new FormControl('', [
    Validators.required,
    Validators.minLength(6)
  ]);

  loginFormGroup = new FormGroup({
    'passwordFormControl': this.passwordFormControl,
    'loginFormControl': this.loginFormControl
  });

  ngOnInit() {
    // this.authService.hashPassword('passwordForMyMom');
    console.log('started login');
  }

  async login() {
    const model = new Credentials();
    model.Login = this.loginFormControl.value;
    model.Password = await this.authService.hashPassword(this.passwordFormControl.value);
    console.log(model);

    const response = await this.authService.Login(model);
    if (response.Ok) {
      this.authService.SaveToken(response.Value);
      this.snackBar.open('Zalogowano pomyślnie', '', { duration: 2000 });
      this.dialogRef.close();
    } else {
      if (response.StatusCode === 403) {
        this.snackBar.open('Niepoprawny login lub hasło!', '', { duration: 2000 });
      }
    }
  }

}
