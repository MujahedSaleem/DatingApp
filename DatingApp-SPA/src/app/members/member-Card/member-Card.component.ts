import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/Models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-Card.component.html',
  styleUrls: ['./member-Card.component.css']
})
export class MemberCardComponent implements OnInit {
  recipientId: string;
  userId: any;
  done: any = true;
  @Input() user: User;
  constructor(
    private userService: UserService,
    private alertFiy: AlertifyService,
    private route: ActivatedRoute
  ) {}
  ngOnInit() {
    this.userId = JSON.parse(localStorage.getItem('user'));
    this.recipientId = this.user.id;
  }
  like() {
    this.done = false;
   this.userService.like(this.userId.id, this.recipientId)
  .subscribe(a => {
    this.alertFiy.success(JSON.parse(a) + this.user.userName);
  } , error => {
  this.alertFiy.error(error);
  }, () => {
   this.done = true;
  });
}
}
