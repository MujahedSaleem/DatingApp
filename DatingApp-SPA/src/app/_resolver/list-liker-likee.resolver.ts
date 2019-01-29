import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../Models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Userparams } from '../Models/userparams';

@Injectable()
export class ListLikerLikeeResolver implements Resolve<User> {
    userparams: Partial<Userparams> = {};
  constructor(
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<User> {
      this.userparams.likee = true;
    return this.userService.getUsers(1, 5, this.userparams).pipe(
      catchError(error => {
        this.alertify.error('Problem Retreving Data');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
