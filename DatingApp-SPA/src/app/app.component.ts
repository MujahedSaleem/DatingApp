import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from './Models/user';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();
  _hubConnection: signalR.HubConnection;
  ngOnInit(): void {


    const token = localStorage.getItem('token');
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    if (user) {
      this.authService.currentuser = user ;
      this.authService.changeMemberPhoto(user.photosUrl) ;
    }

  }
  constructor(private authService: AuthService) {}
}
