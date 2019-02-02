
import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../Models/message';
import { AuthService } from '../_services/auth.service';
import { environment } from 'src/environments/environment';
@Injectable()
export class MessagesResolver implements Resolve<Message[]> {
  pageNumber = 1;
  pageSize = 5;
  messagesContainer = 'Unread';
  constructor(private userService: UserService,
     private router: Router,
      private alertify: AlertifyService,
       private authService: AuthService) { }
  resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
    const userId = this.authService.decodedToken[environment.NameIdentifier];
    return this.userService.getMessages(userId, this.pageNumber, this.pageSize, this.messagesContainer).pipe(catchError(error => {
      this.alertify.error('Problem Retreving Messages');
      this.router.navigate(['/home']);
      return of(null);
    }));
  }
}
