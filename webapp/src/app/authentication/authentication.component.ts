import { MatSnackBar } from '@angular/material';
import { Component, OnInit } from '@angular/core';
import { CustomMaterialModule } from '../custom-material/custom-material.module';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss']
})
export class AuthenticationComponent implements OnInit {

  constructor() { }

  loginPage = true;

  ngOnInit() {
  }

  switchPage() {
    this.loginPage = !this.loginPage;
  }

}
