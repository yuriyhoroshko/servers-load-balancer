import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TasksComponent } from "./tasks/tasks.component";
import { AddtaskComponent } from "./addtask/addtask.component";
import { LoginComponent } from "./login/login.component";

const routes: Routes = [
    { path: 'tasks', component: TasksComponent },
    { path: 'addtask', component: AddtaskComponent },
    { path: 'login', component: LoginComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
