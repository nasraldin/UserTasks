
import { Component, Inject } from "@angular/core";
import { ConfigurationService } from "../../services/configuration.service";
import { TaskService } from "../../services/TaskService";
import { AuthService } from "../../services/auth.service";
import { error } from "util";
import { OnInit, OnDestroy, Input, TemplateRef, ViewChild } from "@angular/core";
import { ModalDirective } from "ngx-bootstrap/modal";
import { AlertService, MessageSeverity, DialogType } from "../../services/alert.service";

@Component({
  selector: "home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
})

export class HomeComponent {

  public taskItems: ITaskData[];
  _currentUserId: number;
  formResetToggle: boolean = true;
  taskEdit = {};

  constructor(public configurations: ConfigurationService, private alertService: AlertService, private taskService: TaskService, private authService: AuthService) {
    this.currentUserId();
    this.getTasks();
  }

  currentUserId() {
    if (this.authService.currentUser)
      this._currentUserId = Number(this.authService.currentUser.id);

    return this._currentUserId;
  }

  getTasks() {
    this.taskService.getTasks().subscribe(
      data => this.taskItems = data.filter(task => task.userOwnerId === this._currentUserId)
    );
  }

  getTask(taskId) {
    this.taskService.getTaskById(taskId).subscribe((data) => {
        this.getTasks();
        alert(data);
      },
      error => console.error(error));
  }

  assignTask(id, userId) {
    this.taskService.assignTask(id, userId).subscribe((data) => {
      this.getTasks();
      alert(data);
    },
      error => console.error(error));
  }

  taskDone(id) {
    this.taskService.taskDone(id).subscribe((data) => {
      this.getTasks();
      alert(data);
    },
      error => console.error(error));
  }

  delete(taskId) {
    const ans = confirm("Do you want to delete task: " + taskId);
    if (ans) {
      this.taskService.deleteTasksItem(taskId).subscribe((data) => {
          this.getTasks();
        },
        //error => {
        //  if (error => error.status == 404)
        //    alert("Not Found");
        //}
        error => console.error(error));
    }
  }

  @ViewChild("editorModal")
  editorModal: ModalDirective;

  addTask() {
    this.formResetToggle = false;

    setTimeout(() => {
      this.formResetToggle = true;

      this.taskEdit = {};
      this.editorModal.show();
    });
  }

  showErrorAlert(caption: string, message: string) {
    this.alertService.showMessage(caption, message, MessageSeverity.error);
  }

}

interface ITaskData {
  id: number;
  task: string;
  isDone: boolean;
  userOwnerId: number;
}
