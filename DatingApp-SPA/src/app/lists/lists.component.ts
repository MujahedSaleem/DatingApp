import { Component, OnInit } from '@angular/core';
import { User } from '../Models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Pagination, PaginatedResult } from '../Models/Pagination';
import { Userparams } from '../Models/userparams';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
users: User[];
pagination: Pagination;
userParams: Partial<Userparams> = {};
model: any;
disabled = false;

  constructor(    private route: ActivatedRoute,      private userService: UserService,
    private alertify: AlertifyService

    ) { }

  ngOnInit() {
    this.route.data.subscribe(
      data => {
        this.users = data['users'].result;
        this.pagination = data['users'].pagination;
      },
      error => {
        this.alertify.error(error);
      });
      this.model = 'likers';
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUser();
  }
  loadUser() {
    if (this.model === 'likee') {
      this.userParams.likee = true;
      this.userParams.liker = false;

    } else {
      this.userParams.liker = true;
      this.userParams.likee = false;

    }
    this.userService.getUsers( this.pagination.currentPage, this.pagination.itemPerPage, this.userParams)
    .subscribe((user: PaginatedResult<User[]>) => {
    this.users = user.result;
    this.pagination = user.pagination;
    }, erro => {
      this.alertify.error(erro);
    });

  }

}
