import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/Models/Photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { User } from 'src/app/Models/user';

const URL = environment.apiurl;

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photoEditor.component.html',
  styleUrls: ['./photoEditor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Output() user = new EventEmitter();

  public uploader: FileUploader;
  public hasBaseDropZoneOver: Boolean = false;
  userID: string = this.authServic.decodedToken[environment.NameIdentifier];

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initilizeUploder() {
    this.uploader = new FileUploader({
      url: URL + 'users/' + this.userID + '/photo',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = file => {
      file.withCredentials = false;
    };
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        this.photos.push(photo);
      }
    };
  }

  constructor(
    private authServic: AuthService,
    private userService: UserService,
    private alertify: AlertifyService
  ) {}
  isMainPhoto(id: number) {
    return this.photos[id].isMain;
  }
  ngOnInit() {
    this.initilizeUploder();
  }
  setMainPhoto(photo: Photo) {
    this.userService.setMainPhoto(photo.id, this.userID).subscribe(
      next => {
        this.photos.find(a => a.isMain === true).isMain = false;
        photo.isMain = true;
        this.alertify.success('Photo Chancged successFully');
        this.authServic.changeMemberPhoto(photo.url);
        this.authServic.currentuser.photosUrl = photo.url;
        localStorage.setItem(
          'user',
          JSON.stringify(this.authServic.currentuser)
        );
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  deletePhoto(photo: Photo) {
    this.alertify.confirm('Are you Want To remove This picture', () => {
      this.userService.deletePhoto(photo.id, this.userID).subscribe(
        next => {
          const id = this.photos.findIndex(a => a.id === photo.id);
          this.photos.splice(id, 1);
          this.alertify.success('Photo Removed successFully');
        },
        error => {
          this.alertify.error(error);
        }
      );
    });
  }
}
