import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() valuesFromHome: any;
  @Output() cancelRegsiter = new EventEmitter();
  model: any = {};
  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }
  register() {
    this.authService.register(this.model).subscribe(_ => {
      this.alertify.success('Regstriation done successfuly');
    } , error => {
     this.alertify.error(error);
    });


  }

  calcel() {
    this.cancelRegsiter.emit(false);
  }

}
