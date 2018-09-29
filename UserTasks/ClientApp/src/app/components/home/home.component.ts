
import { Component, Inject } from '@angular/core';
import { fadeInOut } from '../../services/animations';
import { ConfigurationService } from '../../services/configuration.service';
import { HttpClient } from '@angular/common/http';
import { TaskService } from "../../services/TaskService";

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    animations: [fadeInOut]
})
export class HomeComponent {

  public taskItems: ITaskData[];

  //constructor(public configurations: ConfigurationService, http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
  //  http.get<ITaskData[]>(baseUrl + 'api/tasks').subscribe(result => {
  //    this.taskItems = result;
  //    console.log(result);
  //  }, error => console.error(error));
  //}

  constructor(public configurations: ConfigurationService, private taskService: TaskService) {
    this.getTasks();
  }

  getTasks() {
    this.taskService.getTasks().subscribe(
      data => this.taskItems = data
    );
  }
  
  delete(taskId) {
    const ans = confirm("Do you want to delete task: " + taskId);
    if (ans) {
      this.taskService.deleteTasksItem(taskId).subscribe((data) => {
        this.getTasks();
      },
        error => console.error(error));
    }
  }

  getTask(taskId) {
    this.taskService.getTaskById(taskId).subscribe((data) => {
      this.getTasks();
        alert(data);
      },
      error => console.error(error));
  }
}

interface ITaskData {
  id: number;
  task: string;
  isDone: boolean;
  userOwnerId: number;
}
