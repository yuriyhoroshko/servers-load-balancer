import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TasksComponent } from './tasks/tasks.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import TaskService from "./tasks/taskService";
import { AddtaskComponent } from './addtask/addtask.component';
import { LoginComponent } from './login/login.component';
import UserService from "./login/userService";

interface IAppState {

}

@NgModule({
  declarations: [
    AppComponent,
    TasksComponent,
    AddtaskComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule
  ],
    providers: [TaskService,
      UserService],
  bootstrap: [AppComponent]
})
export class AppModule {}
