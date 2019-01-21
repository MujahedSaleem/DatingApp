import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerModel = false;
  constructor(private http: HttpClient) { }

  ngOnInit() {
  }
  cancelRegisterMode (registerMode: boolean) {
   this.registerModel = registerMode;
  }
  rigsterToggle() {
this.registerModel = true;
  }


}
