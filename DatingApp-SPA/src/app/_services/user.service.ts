import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../Models/user';
import { Observable } from 'rxjs';
import { UserForUpdate } from '../Models/userForUpdate';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiurl + 'Users/';

  constructor(private http: HttpClient) {}
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl);
  }
  getUser(id: string): Observable<User> {
    return this.http.get<User>(this.baseUrl + id);
  }
  updateUser(id: string, data: UserForUpdate): Observable<UserForUpdate> {
   return this.http.put<UserForUpdate>(this.baseUrl + id, data);
  }
}


