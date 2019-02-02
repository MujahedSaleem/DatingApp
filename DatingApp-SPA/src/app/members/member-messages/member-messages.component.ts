import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { Message } from 'src/app/Models/message';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { environment } from 'src/environments/environment';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
   @Input() recipientId: string;
   @ViewChild('messageContainer') public panel: ElementRef<any>;
   @ViewChild('scrollMe') comment: ElementRef;

   scrolltop: number = null;
   userId: string = this.authservice.decodedToken[environment.NameIdentifier];
   messages: Message[];
   photoUrl: any;
   newMessage: any = {};
  _hubConnection: signalR.HubConnection;
  container: HTMLElement;

  constructor(private userService: UserService,
    private authservice: AuthService,
     private alertifyService: AlertifyService) { }

  ngOnInit() {
    this._hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:5000/notify',
     {accessTokenFactory : () => {
       return localStorage.getItem('token');
     }})
    .build();
    this._hubConnection.start().then( a => {
      console.log(a);
    }).catch(err => console.log(err));

    this._hubConnection.on('BroadcastMessage', (message: Message) => {
      this.loadMessages();

      this.messages.unshift(message);
     this.newMessage = { };
     this.scrollToBottom();
    });
    this._hubConnection.on('DeleteMessage', (id: number) => {
      this.loadMessages();

     let m = this.messages.find(a => a.id === id);
     const index: number = this.messages.indexOf(m);
     if (index !== -1) {
      this.messages.splice(index, 1);
  }
     this.newMessage = { };
     this.scrollToBottom();
    });
    this._hubConnection.on('MarkMessages', id => {
      for (let index = 0; index < id.length; index++) {

        this.messages.find(a => a.id === id[index]).isRead = true;
        }
    });
    this.loadMessages();
  }
  loadMessages() {
    this.userService.getMessageThred(this.authservice.decodedToken[environment.NameIdentifier], this.recipientId)
       .subscribe( a => {
      this.messages = a;
    }, error => {
      this.alertifyService.error(error);
    }, () => {
      this.userService.ReadRealTimeMessage(this.authservice.decodedToken[environment.NameIdentifier], this.recipientId).subscribe();
    });
    this.scrollToBottom();
  }
  sendMessage() {
    this.newMessage.recipientId = this.recipientId;
    this.userService.sendrealTimeMessage(this.authservice.decodedToken[environment.NameIdentifier], this.newMessage)
    .subscribe();


   /*  this.newMessage.recipientId = this.recipientId;
    this.userService.sendMessage(this.authservice.decodedToken[environment.NameIdentifier], this.newMessage)
    .subscribe((message: Message) => {
      this.messages.push(message);
      this.newMessage.content = '';
    }, error => {
      this.alertifyService.error(error);
    }); */
  }
  delteMessage(id: number, userid ) {
    if (userid !== this.userId) {
      return ;
    }
    this.alertifyService.confirm('Are you sure you want to remove this message permetaily !!!', () => {
      this.userService.deleterealTimeMessage(this.authservice.decodedToken[environment.NameIdentifier], id).subscribe();

    });
  }
  scrollToBottom(): void {
    try {
        this.panel.nativeElement.scrollTop = this.panel.nativeElement.scrollHeight;
    } catch (err) { }
}
}
