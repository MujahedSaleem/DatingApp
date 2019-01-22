import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: '/app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  name = '';
  constructor(
    public authService: AuthService,
     private alertyfiy: AlertifyService,
     private router: Router) {}
  ngOnInit() {
     this.name = this.authService.decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
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
    this.alertyfiy.message('logged out');
    this.router.navigate(['/home']);

  }
}
