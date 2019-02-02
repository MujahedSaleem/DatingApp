import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { NavComponent } from './nav/nav.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { httpInterceptorProviders } from './Intercep/error.interceptor';
import { AlertifyService } from './_services/alertify.service';
import { BsDropdownModule, TabsModule } from 'ngx-bootstrap';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './Messages/Messages.component';
import { RouterModule } from '@angular/router';
import { AuthGuard } from './_gurds/auth.guard';
import { UserService } from './_services/user.service';
import { MemberCardComponent } from './members/member-Card/member-Card.component';
import { JwtModule } from '@auth0/angular-jwt';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { MemberDetailResolver } from './_resolver/member-detail.resolver';
import { MemberListResolver } from './_resolver/member-list.resolver';
import { NgxGalleryModule } from 'ngx-gallery';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolver/member-edit.resolver';
import { PreventUnsavedChanges } from './_gurds/prevent-Unsaved-Changes.guard';
import { PhotoEditorComponent } from './members/photoEditor/photoEditor.component';
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule } from 'ngx-bootstrap';
import {TimeAgoPipe} from 'time-ago-pipe';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MemberFilterComponent } from './members/member-filter/member-filter.component';
import { SidebarJSModule } from 'ng-sidebarjs';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { ListLikerLikeeResolver } from './_resolver/list-liker-likee.resolver';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { MessagesResolver } from './_resolver/messages.resolver';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import {ScrollDispatchModule} from '@angular/cdk/scrolling';


export function tokenGetter() {
   return localStorage.getItem('token');
 }
@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      ListsComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailsComponent,
      MemberEditComponent,
      PhotoEditorComponent,
      TimeAgoPipe,
      MemberFilterComponent,
      MemberMessagesComponent

   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      ScrollDispatchModule,
      FormsModule,
      ReactiveFormsModule,
      NgbModule,
      TabsModule.forRoot(),
      SidebarJSModule.forRoot(),
      PaginationModule.forRoot(),
      BsDatepickerModule.forRoot(),
      BsDropdownModule.forRoot(),
      ButtonsModule.forRoot(),
      JwtModule.forRoot({
        config: {
          tokenGetter: tokenGetter,
          /*send Token*/
          whitelistedDomains: ['localhost:5000'],
          /*don't send Token*/
          blacklistedRoutes: ['localhost:5000/api/auth']
        }
      }),
      NgxGalleryModule,
      FileUploadModule

      ],
   providers: [
      AuthService,
      httpInterceptorProviders,
      AlertifyService,
      AuthGuard,
      UserService,
      MemberDetailResolver,
      MemberListResolver,
      MemberEditResolver,
      PreventUnsavedChanges,
      ListLikerLikeeResolver,
      MessagesResolver,
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
