import { Component, OnInit, Input } from '@angular/core';
import { UserService } from '../../_services/user.service';
import { User } from '../../Models/user';
import { map } from 'rxjs/operators';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/Models/Pagination';
import { Userparams } from 'src/app/Models/userparams';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  @Input() users: User[];
  @Input()  userParams: Partial<Userparams> = {};
  user: User;
  disabled = false;
  @Input() pagination: Pagination;

  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.data.subscribe(
      data => {
        console.log(data);
        this.users = data['users'].result;
        this.pagination = data['users'].pagination;
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
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
}
