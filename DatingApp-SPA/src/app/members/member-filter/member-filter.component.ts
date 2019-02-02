import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/Models/user';
import { Userparams } from 'src/app/Models/userparams';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/Models/Pagination';
import { HubConnection, ILogger } from '@aspnet/signalr';
import { Message } from 'src/app/Models/message';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-member-filter',
  templateUrl: './member-filter.component.html',
  styleUrls: ['./member-filter.component.css']
})
export class MemberFilterComponent implements OnInit {
  private _hubConnection: HubConnection;
  msgs: Partial<Message[]> = [];
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  userParams: Partial<Userparams> = {};
  pagination: Pagination;
  params: signalR.IHttpConnectionOptions;


  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {

    


    this.route.data.subscribe(
      data => {
        this.users = data['users'].result;
        this.pagination = data['users'].pagination;
      },
      error => {
        this.alertify.error(error);
      }
    );
    this.userParams.maxAge = '99';
    this.userParams.minAge = '18';
    this.userParams.orderBy = 'lastActive';
    this.loadUser();
  }
  loadUser() {
    this.userService.getUsers( this.pagination.currentPage, this.pagination.itemPerPage, this.userParams)
    .subscribe((user: PaginatedResult<User[]>) => {
    this.users = user.result;
    this.pagination = user.pagination;
    }, erro => {
      this.alertify.error(erro);
    });

  }

  resetFilter() {
    this.userParams.gender = 'all';
    this.userParams.maxAge = '99';
    this.userParams.minAge = '18';
    this.userParams.orderBy = 'lastActive';
    this.userParams.name = 'all';

    this.loadUser();
  }
  filter(event) {
    if ( event.keyCode == 13) {
   this.loadUser();

    }

  }
  clicks() {

    this.loadUser();

  }
}
