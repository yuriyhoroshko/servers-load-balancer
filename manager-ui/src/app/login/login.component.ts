import { Component, OnInit } from '@angular/core';
import UserService from "./userService";
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private userService: UserService, private router: Router) { }

  private userName: string;
  private password: string;

  public loginFailed = false;


    ngOnInit() {
      if (localStorage.getItem('JWT') != null) {
        this.router.navigate(['tasks']);
      }
    }
  updateUserName = (event: any) => {
    this.userName = event.target.value;
  }

  updatePassword = (event: any) => {
    this.password = event.target.value;
  }

  Send = () => {
    this.userService.loginUser(this.userName, this.password).subscribe((data: any) => {
      data = JSON.parse(data);
      localStorage.setItem('userName', data.UserName);
      localStorage.setItem('userId', data.UserId);
      localStorage.setItem('JWT', data.Token);
      this.loginFailed = false;
      this.router.navigate(['tasks']);
    }, error => this.loginFailed = true);
  }

  SendRegister = () => {
      this.userService.registerUser({ UserName: this.userName, Password: this.password}).subscribe((data) => this.router.navigate(['login']), error => console.log(error));
    }
}
