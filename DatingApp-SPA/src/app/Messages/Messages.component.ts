import { Component, OnInit, NgZone } from '@angular/core';
import { Message } from '../Models/message';
import { Pagination } from '../Models/Pagination';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { User } from '../Models/user';
import { AuthService } from '../_services/auth.service';
import { environment } from 'src/environments/environment';
import { catchError } from 'rxjs/operators';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-messages',
  templateUrl: './Messages.component.html',
  styleUrls: ['./Messages.component.css']
})
export class MessagesComponent implements OnInit {
  userId: string;
  messages: Message[];
  pagination: Pagination;
  messagesContainer: 'Unread';
  currentRoom: string;
  _hubConnection: signalR.HubConnection;

  constructor(
    private userService: UserService,
    private atuhservice: AuthService,
    private rotue: ActivatedRoute,
    private alertify: AlertifyService
  ) {
  }

  ngOnInit() {
    this._hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:5000/notify',
     {accessTokenFactory : () => {
       return localStorage.getItem('token');
     }})
    .build();
    this._hubConnection.start().then( a => {
      console.log(a);
    }).catch(err => document.write(err));
    this.rotue.data.subscribe(
      data => {
        this.messages = data['messages'].result;
        this.pagination = data['messages'].pagination;
      },
      error => {
        this.alertify.error(error);
      }
    );
    this.userId = this.atuhservice.decodedToken[environment.NameIdentifier];
    this._hubConnection.on('DeleteMessage', (id: number) => {
      this.loadMessages();

     let m = this.messages.find(a => a.id === id);
     const index: number = this.messages.indexOf(m);
     if (index !== -1) {
      this.messages.splice(index, 1);
  }});
  }

  loadMessages() {
    this.userService
      .getMessages(
        this.userId,
        this.pagination.currentPage,
        this.pagination.itemPerPage,
        this.messagesContainer
      )
      .subscribe(
        a => {
          this.messages = a.result;
          this.pagination = a.pagination;
        },
        error => {
          this.alertify.error(error);
        }
      );
  }
  pageChanged(event: any) {
    ;
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }
  delteMessage(id: number, userid ) {
    if (userid !== this.userId) {
      return ;
    }
    this.alertify.confirm('Are you sure you want to remove this message permetaily !!!', () => {
      this.userService.deleterealTimeMessage(this.atuhservice.decodedToken[environment.NameIdentifier], id).subscribe();

    });
  }
}
