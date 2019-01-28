import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { User } from '../Models/user';
import { Observable } from 'rxjs';
import { UserForUpdate } from '../Models/userForUpdate';
import { PaginatedResult } from '../Models/Pagination';
import { map, tap } from 'rxjs/operators';
import { Userparams } from '../Models/userparams';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiurl + 'Users/';

  constructor(private http: HttpClient) {}
  userParams: Partial<Userparams>;

  getUsers(page?, itemsPerPage?, userparams?: Partial<Userparams>): Observable<PaginatedResult<User[]>> {
    const paginationResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('Pagenumber', page);
      params = params.append('PageSize', itemsPerPage);
      if (userparams != null) {
        params = params.append('Gender', userparams.gender == null ? '' : userparams.gender );
        params = params.append('maxAge', userparams.maxAge == null ? '99' :   userparams.maxAge);
        params = params.append('minAge',  userparams.minAge == null ? '18' : userparams.minAge);
        params = params.append('name',  userparams.name == null ? 'all' : userparams.name);
        if (userparams.orderBy != null) {
          params = params.append('orderBy',  userparams.orderBy);

        }
      }
    }
    return this.http.get<User[]>(this.baseUrl, {observe: 'response', params})
    .pipe(
      map( response => {
        paginationResult.result = response.body;


        if (response.headers.get('Pagination') != null) {
          paginationResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginationResult;
      })
    );
  }
  getUser(id: string): Observable<User> {
    return this.http.get<User>(this.baseUrl + id);
  }
  updateUser(id: string, data: UserForUpdate): Observable<UserForUpdate> {
   return this.http.put<UserForUpdate>(this.baseUrl + id, data);
  }
  setMainPhoto(id: number, userId: string) {
    return this.http.post(this.baseUrl + userId + '/photo/' + id + '/setmain', {} );
  }
  deletePhoto(id: number, userId: string) {
    return this.http.post(this.baseUrl + userId + '/photo/' + id + '/delete', {} );
  }
}


