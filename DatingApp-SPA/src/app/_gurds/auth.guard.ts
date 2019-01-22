import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { flatMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private atuhService: AuthService, private router: Router, private alertfy: AlertifyService) {}
  canActivate(): boolean {
    if (this.atuhService.loggedIn()) {
      return true;
    }

    this.alertfy.error('You shall not Pass !!!!');
    this.router.navigate(['/home']);
    return false;
  }
}
