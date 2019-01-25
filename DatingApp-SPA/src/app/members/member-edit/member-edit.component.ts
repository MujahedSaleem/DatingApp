import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { User } from 'src/app/Models/user';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: User;
  @ViewChild('editForm') editForm: NgForm;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }
  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    this.authService.currentUserUrl.subscribe( a => {
      this.user.photosUrl = a;
     });
  }
  updateUsers() {
    this.userService.updateUser(this.authService.decodedToken[environment.NameIdentifier], this.user)
    .subscribe(response => {
      this.alertify.success('Profile Updatet successFuly');

    }, error => {
      this.alertify.error(error);

    });
    this.editForm.reset(this.user);
   }
   setMainPhoto(url: string) {
     this.authService.currentUserUrl.subscribe( a => {
      this.user.photosUrl = a;
     });
   }
}
