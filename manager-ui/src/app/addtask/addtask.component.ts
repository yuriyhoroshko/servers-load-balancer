import { Component, OnInit } from '@angular/core';
import TaskService from "../tasks/taskService";
import { Router } from '@angular/router';

@Component({
  selector: 'app-addtask',
  templateUrl: './addtask.component.html',
  styleUrls: ['./addtask.component.scss']
})
export class AddtaskComponent implements OnInit {

    constructor(private taskService: TaskService, private router: Router) { }
    size: number;
    ngOnInit() {
      if (localStorage.getItem('JWT') === null) {
        this.router.navigate(['login']);
      }
    }

    storeTask() {
      if (this.size < 50 && this.size > 0) {
          this.taskService.sendSize(this.size).subscribe(data => {
            this.router.navigate(["tasks"]);
          });
      }
    }

    updateSize(inpt) {
        this.size = parseInt(inpt);
    }
}
