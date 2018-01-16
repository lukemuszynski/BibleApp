import { ServiceResponse } from './../../models/ServiceResponse';
import { BearerToken } from './../../models/BearerToken';
import { RegisterUserData } from './../../models/RegisterUserData';
import { environment } from './../../../environments/environment';
import { Injectable, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Credentials } from '../../models/Credentials';


@Injectable()
export class AuthService {

  constructor(private http: Http) {
    console.log('AuthService ctor');
    this._token = localStorage.getItem('token');
    this._userGuid = localStorage.getItem('userGuid');
   }

  private _token: string = null;
  private _userGuid: string = null;

  IsAuthorized(): Boolean {
    if (this._token) {
      return true;
    }
    return false;
  }

  GetToken(): String {
    return this._token;
  }

  GetUserGuid(): String {
    return this._userGuid;
  }

  SaveToken(bearerToken: BearerToken) {
    localStorage.setItem('token', bearerToken.Token);
    localStorage.setItem('userGuid', bearerToken.Guid);
    this._token = bearerToken.Token;
    this._userGuid = bearerToken.Guid;
  }

  async Register(userData: RegisterUserData): Promise<ServiceResponse<BearerToken>> {
    const registerUrl = environment.apiUrl + 'api/Auth/Register';
    const res = this.http.post(registerUrl, userData).toPromise();
    return await ServiceResponse.CreateServiceResponse<BearerToken>(res);
  }

  async Login(credentials: Credentials): Promise<ServiceResponse<BearerToken>> {
    const loginUrl = environment.apiUrl + 'api/Auth/Login';
    const res = this.http.post(loginUrl, credentials).toPromise();
    return await ServiceResponse.CreateServiceResponse<BearerToken>(res);
  }

  async Logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userGuid');
    this._token = null;
    this._userGuid = null;
  }

  // https://stackoverflow.com/questions/40459020/angular-js-cryptography-pbkdf2-and-iteration/40468218#40468218
  async hashPassword(password): Promise<string> {

    // let booksUrl = environment.apiUrl + 'api/Books/GetBooks';
    const salt = environment.passwordSalt;
    const iterations = environment.passwordIterations;
    const hash = environment.passwordHash;
    // First, create a PBKDF2 "key" containing the password

    const baseKey = await window.crypto.subtle.importKey(
      'raw',
      this.stringToArrayBuffer(password),
      { 'name': 'PBKDF2' },
      false,
      ['deriveKey']
    );

    // Derive a key from the password
    const aesKey = await window.crypto.subtle.deriveKey(
      {
        'name': 'PBKDF2',
        'salt': this.stringToArrayBuffer(salt),
        'iterations': iterations,
        'hash': hash
      },
      baseKey,
      { 'name': 'AES-CBC', 'length': 256 },
      // Key we want.Can be any AES algorithm ("AES-CTR",
      //  "AES-CBC", "AES-CMAC", "AES-GCM", "AES-CFB", "AES-KW", "ECDH", "DH", or "HMAC")
      true,                               // Extractable
      ['encrypt', 'decrypt']              // For new key
    );

    // Export it so we can display it
    const keyBytes = await window.crypto.subtle.exportKey('raw', aesKey);

    // Display key in Base64 format
    const keyS = this.arrayBufferToString(keyBytes);
    const keyB64 = btoa(keyS);
    return keyB64;
  }

  // Utility functions
  private stringToArrayBuffer(byteString) {
    const byteArray = new Uint8Array(byteString.length);
    for (let i = 0; i < byteString.length; i++) {
      byteArray[i] = byteString.codePointAt(i);
    }
    return byteArray;
  }

  private arrayBufferToString(buffer) {
    const byteArray = new Uint8Array(buffer);
    let byteString = '';
    for (let i = 0; i < byteArray.byteLength; i++) {
      byteString += String.fromCodePoint(byteArray[i]);
    }
    return byteString;
  }
}
