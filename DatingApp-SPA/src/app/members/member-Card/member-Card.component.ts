import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/Models/user';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-Card.component.html',
  styleUrls: ['./member-Card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user: User;
  constructor() { }

  ngOnInit() {
  }

}
