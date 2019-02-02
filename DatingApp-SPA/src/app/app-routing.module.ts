import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './Messages/Messages.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { AuthGuard } from './_gurds/auth.guard';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { MemberDetailResolver } from './_resolver/member-detail.resolver';
import { MemberListResolver } from './_resolver/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolver/member-edit.resolver';
import { PreventUnsavedChanges } from './_gurds/prevent-Unsaved-Changes.guard';
import { MemberFilterComponent } from './members/member-filter/member-filter.component';
import { ListLikerLikeeResolver } from './_resolver/list-liker-likee.resolver';
import { MessagesResolver } from './_resolver/messages.resolver';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: '', component: HomeComponent },
  { path: '*', redirectTo: '' , pathMatch: 'full'},
  {path: '',
   runGuardsAndResolvers: 'always',
  canActivate: [AuthGuard],
children: [
  { path: 'members/edit', component: MemberEditComponent,
    resolve: {user: MemberEditResolver}, canDeactivate: [PreventUnsavedChanges] }
,
  { path: 'messages', component: MessagesComponent, resolve: {messages: MessagesResolver} },
  { path: 'lists', component: ListsComponent, resolve: {users: ListLikerLikeeResolver} },
  { path: 'members', component: MemberFilterComponent, resolve: {users: MemberListResolver}},
  { path: 'members/:id', component: MemberDetailsComponent, resolve: {user: MemberDetailResolver} },

]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
