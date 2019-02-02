import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { User } from '../Models/user';
import { Observable } from 'rxjs';
import { UserForUpdate } from '../Models/userForUpdate';
import { PaginatedResult } from '../Models/Pagination';
import { map, tap } from 'rxjs/operators';
import { Userparams } from '../Models/userparams';
import { Message } from '../Models/message';

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
        params = params.append('liker',  userparams.liker === true ? 'true' : 'false');
        params = params.append('likee',  userparams.likee === true ? 'true' : 'false');

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
  like(userId: string, recipientId: string): Observable<any> {
    return this.http.post(this.baseUrl + userId + '/like/' + recipientId , {} );
  }

  getMessages(userId: string, page?, itemsPerPage?, messagesContainer?): Observable<PaginatedResult<Message[]>> {
    const paginationResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('Pagenumber', page);
      params = params.append('PageSize', itemsPerPage);
      if (messagesContainer != null) {
        params = params.append('MessageContainer', messagesContainer);
      }
    }
    return this.http.get<Message[]>(this.baseUrl + userId + '/Message/', {observe: 'response', params}).pipe(
      map( response => {
        paginationResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginationResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginationResult;
      })
    );
  }

  getMessageThred(userId: string, recipientId: string) {
    return this.http.get<Message[]>(this.baseUrl  + userId + '/Message/thread/' + recipientId);
  }
  sendMessage(userId: string, message: Message) {
    return this.http.post(this.baseUrl + userId + '/Message', message);
  }
  sendrealTimeMessage(userId: string, message: Message) {
    return this.http.post(this.baseUrl + userId + '/Message/signalR', message);
  }
  deleterealTimeMessage(userId: string, id: number) {
    return this.http.post(this.baseUrl + userId + '/Message/signalR/delete/' + id, {} );
  }
  ReadRealTimeMessage(userId: string, recipientId: string) {
    return this.http.post(this.baseUrl + userId + '/Message/signalR/read/' + recipientId, {} );
  }

}


