import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { baseURL } from '../../baseUrl';
import { JsonPipe } from '@angular/common';

@Injectable()
export default class UserService {
  constructor(private http: HttpClient) { }
  public registerUser(user) {
    const httpBody = {
      UserName : user.UserName,
      Password : user.Password,
    };
      JSON.stringify(httpBody);

      console.log(httpBody);
    return this.http.post(baseURL + 'register', httpBody);
  }
  public loginUser( userName, password ) {
    const httpBody = {
      userName,
      password
    };
    JSON.stringify(httpBody);
    return this.http.post(baseURL + 'login', httpBody);
  }
}
