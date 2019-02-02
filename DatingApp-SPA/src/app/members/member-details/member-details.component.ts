import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/Models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import {
  NgxGalleryOptions,
  NgxGalleryImage,
  NgxGalleryAnimation
} from 'ngx-gallery';
import { TabsetComponent } from 'ngx-bootstrap';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css']
})
export class MemberDetailsComponent implements OnInit {
  @ViewChild('memberTabs') memberTabs: TabsetComponent;

  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  Action = true;
 recipientId: string;
  userId: any;
  done: any = true;
  constructor(
    private userService: UserService,
    private alertFiy: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {

    this.route.data.subscribe(
      data => {
        this.user = data['user'];
      },
      error => {
        this.alertFiy.error(error);
      });
      this.route.queryParams.subscribe(params => {
const selectedTab = params['tab'];
   this.selectTab(selectedTab > 0 ? selectedTab : 0);
      });
    this.recipientId = '' + this.route.snapshot.params.id;
    this.userId = JSON.parse(localStorage.getItem('user'));
   if (this.recipientId === this.userId.id) {
     this.Action = false;
   }
    this.loadImage();
  }
  loadImage() {
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: true
      }
    ];
    this.galleryImages = [];
    this.galleryImages = this.getImages();
  }
  getImages() {
    const imageUrls = [];
    for (let i = 0; i < this.user.photos.length; i++) {
      imageUrls.push({
        small: this.user.photos[i].url,
        medium: this.user.photos[i].url,
        big: this.user.photos[i].url,
        description: this.user.photos[i].description
      });
    }

    return imageUrls;
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
  selectTab(typeNumber: number) {
    this.memberTabs.tabs[typeNumber].active = true;
  }
}
