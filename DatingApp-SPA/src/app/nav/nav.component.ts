import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { UserService } from '../_services/user.service';

@Component({
  selector: '/app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  name = '';
  Url = '';
  constructor(
    public authService: AuthService,
    private userService: UserService,
     private alertyfiy: AlertifyService,
     private router: Router) {}
  ngOnInit() {
     this.name = this.authService.decodedToken[environment.Name];
      this.authService.photoUrl.subscribe(next => {
       this.Url = next;
     });
  }

  login() {
    this.authService.login(this.model)
        .subscribe(next => {
          this.alertyfiy.success('Logged In Successfully');
        }, (error) => {
          this.alertyfiy.error(error);
        }, () => {
          this.router.navigate(['/members']);

        });
  }

  loggedIn() {
  return this.authService.loggedIn();
  }
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.authService.currentuser = null;
    this.alertyfiy.message('logged out');
    this.router.navigate(['/home']);

  }
}
