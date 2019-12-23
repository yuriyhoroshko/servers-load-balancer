import { Component, OnInit } from '@angular/core';
import Task from '../models/taskModel';
import TaskService from "./taskService";
import { interval, Subscription } from 'rxjs';
import { Router } from '@angular/router';



@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss'],
  providers:[TaskService]
})
export class TasksComponent implements OnInit {
    constructor(private taskService: TaskService, private router: Router) {}
    public tasks: Task[];
    public source = interval(5000);
    ngOnInit() {
      if (localStorage.getItem('JWT') === null) {
        this.router.navigate(['login']);
      }
      this.pollTasks();
        this.repeater();

  }
    pollTasks() {
      this.taskService.getItems().subscribe((data: any) => {
        this.tasks = data.value;
      });
    }
    repeater = () => {
      this.source.subscribe(a => this.pollTasks());
    }

    cancel = (taskId) => {
      this.taskService.cancelTask(taskId);
    }

    getResult(taskId) {
        this.taskService.getResult(taskId).subscribe((data: any) => {
          console.log(data);
          this.downloadFile(data)
        });
    }

    downloadFile(data) {
      console.log(data);
    }
}
