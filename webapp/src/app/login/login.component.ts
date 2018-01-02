import { AuthService } from './../services/auth-service/auth.service';
import { Component, OnInit } from '@angular/core';
import { CustomMaterialModule } from '../custom-material/custom-material.module';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.hashPassword('passwordForMyMom');
  }

}
