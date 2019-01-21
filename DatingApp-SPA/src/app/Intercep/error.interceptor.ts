import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
  HTTP_INTERCEPTORS,
  HttpResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(err => {
        if (err instanceof HttpErrorResponse) {
          if (err.status === 401) {
            return throwError(err.statusText);
          }
          const applicationErro = err.headers.get('Application-Error');
          if (applicationErro) {
            console.log('this should print your error!', applicationErro);
            return throwError(err);
          }
          const serverError = err.error;
          let modelStateError = '';
          if (serverError && typeof serverError === 'object') {
            for (const key in serverError) {
              if (serverError[key]) {
                modelStateError += serverError[key] + '\n';
              }
            }
          }
          return throwError(modelStateError || serverError || 'Server Error');
        }
      })
    );
  }
}

/** Http interceptor providers in outside-in order */
export const httpInterceptorProviders = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};
