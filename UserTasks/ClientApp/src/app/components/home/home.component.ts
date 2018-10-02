
import { Component, Inject } from "@angular/core";
import { ConfigurationService } from "../../services/configuration.service";
import { TaskService } from "../../services/TaskService";
import { AuthService } from "../../services/auth.service";
import { error } from "util";
import { OnInit, OnDestroy, Input, TemplateRef, ViewChild } from "@angular/core";
import { ModalDirective } from "ngx-bootstrap/modal";
import { AlertService, MessageSeverity, DialogType } from "../../services/alert.service";
import { Router } from '@angular/router';
import { NgForm, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';

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
  addTaskForm= [];
  errorMessage: any;
  complexForm: FormGroup;


  constructor(public configurations: ConfigurationService, private router: Router, private alertService: AlertService, private taskService: TaskService, private authService: AuthService, fb: FormBuilder) {
    this.currentUserId();

    if (this._currentUserId === 1) {
      this.router.navigateByUrl('tasks');
    }

    this.getTasks();

    this.complexForm = fb.group({
      'task': [null, Validators.required],
      'userOwnerId': this._currentUserId,
    });
  }

  addNewTask(model: ITaskData) {
    this.taskService.saveTask(model).subscribe((data) => {
        this.getTasks();
      this.editorModal.hide();
      this.showSuccessAlert('','added new task item successfully');
      },
      error => this.errorMessage = error);
  }

  currentUserId() {
    if (this.authService.currentUser)
      this._currentUserId = Number(this.authService.currentUser.id);
    return this._currentUserId;
  }

  getTasks() {
    this.taskService.userTasks(this._currentUserId).subscribe(
      data => this.taskItems = data as ITaskData[]
    );
  }

  //getTasks() {
  //  this.taskService.getTasks().subscribe(
  //    data => this.taskItems = data.filter(task => task.userOwnerId === this._currentUserId)
  //  );
  //}

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
      this.showSuccessAlert('', 'task is assigned!');
    },
      error => console.error(error));
  }

  taskDone(id) {
    this.taskService.taskDone(id).subscribe((data) => {
      this.getTasks();
        this.showSuccessAlert('', 'task is Done!');
    },
      error => console.error(error));
  }

  delete(taskId) {
    const ans = confirm("Do you want to delete task: " + taskId);
    if (ans) {
      this.taskService.deleteTasksItem(taskId).subscribe((data) => {
        this.getTasks();
          this.showSuccessAlert('', 'task is deleted!');
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

  save() {

    this.taskService.saveTask(this.taskEdit)
      .subscribe((data) => {
        this.getTasks();
        this.editorModal.hide();
      },
        error => this.errorMessage = error);
  }

  showErrorAlert(caption: string, message: string) {
    this.alertService.showMessage(caption, message, MessageSeverity.error);
  }

  showSuccessAlert(caption: string, message: string) {
    this.alertService.showMessage(caption, message, MessageSeverity.success);
  }

}

interface ITaskData {
  id: number;
  task: string;
  isDone: boolean;
  userOwnerId: number;
}
