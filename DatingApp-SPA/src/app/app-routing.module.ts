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

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: '', component: HomeComponent },
  { path: '*', redirectTo: '' , pathMatch: 'full'},
  {path: '',
   runGuardsAndResolvers: 'always',
  canActivate: [AuthGuard],
children: [
  { path: 'messages', component: MessagesComponent },
  { path: 'lists', component: ListsComponent },
  { path: 'members', component: MemberListComponent, resolve: {users: MemberListResolver}},
  { path: 'members/:id', component: MemberDetailsComponent, resolve: {user: MemberDetailResolver} }

]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
