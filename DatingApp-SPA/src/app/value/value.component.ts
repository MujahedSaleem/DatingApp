import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { error } from '@angular/compiler/src/util';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})

export class ValueComponent implements OnInit {

  values: any;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getValue();
  }
  getValue(): void {
    this.http.get(hostURL, httpOptions)
      .subscribe(
        values => { this.values = values; },
        // tslint:disable-next-line:no-shadowed-variable
        (error: any) => { console.log(error); }
    );
  }

}
const hostURL = 'http://localhost:5000/api/values/';
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
