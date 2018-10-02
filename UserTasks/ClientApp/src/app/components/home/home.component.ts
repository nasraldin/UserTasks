
import { Component, Inject } from "@angular/core";
import { ConfigurationService } from "../../services/configuration.service";
import { TaskService } from "../../services/TaskService";
import { AuthService } from "../../services/auth.service";
import { ViewChild } from "@angular/core";
import { ModalDirective } from "ngx-bootstrap/modal";
import { AlertService, MessageSeverity } from "../../services/alert.service";
import { ITaskData } from "../../models/ITaskData";
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: "home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
})

export class HomeComponent {

  public taskItems: ITaskData[];
  _currentUserId: number;
  complexForm: FormGroup;
  complexForm2: FormGroup;
  formResetToggle: boolean = true;

  constructor(public configurations: ConfigurationService, private router: Router, private alertService: AlertService, private taskService: TaskService, private authService: AuthService, private fb: FormBuilder) {

    this.currentUserId();

    if (this._currentUserId === 1) {
      this.router.navigateByUrl('tasks');
    }

    this.getTasks();

    this.complexForm = fb.group({
      'task': [null, Validators.required],
      'userOwnerId': this._currentUserId,
    });

    this.complexForm2 = fb.group({
      'taskId': [null, Validators.required],
      'userOwnerId': [null, Validators.required],
    });
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

  addTask() {
    this.formResetToggle = false;

    setTimeout(() => {
      this.formResetToggle = true;

      this.complexForm = this.fb.group({
        'task': [null, Validators.required],
        'userOwnerId': this._currentUserId,
      });

      this.editorModal.show();
    });
  }

  assignNTask() {
    this.formResetToggle = false;
    setTimeout(() => {
      this.formResetToggle = true;
      //this.complexForm2 = this.fb.group({
      //  'taskId': [null, Validators.required],
      //  'userOwnerId': [null, Validators.required],
      //});
      this.assignTaskModal.show();
    });
  }


  addNewTask(model: ITaskData) {
    this.taskService.saveTask(model).subscribe((data) => {
      this.getTasks();
      this.editorModal.hide();
      this.showSuccessAlert('', 'added new task item successfully');
    },
      error => this.showErrorAlert('', error));
  }

  assignTask(id, userId) {
    this.taskService.assignTask(id, userId).subscribe((data) => {
      this.getTasks();
      this.showSuccessAlert('', 'task is assigned!');
    },
      error => this.showErrorAlert('', error));
  }

  taskDone(id) {
    this.taskService.taskDone(id).subscribe((data) => {
      this.getTasks();
      this.showSuccessAlert('', 'task is Done!');
    },
      error => this.showErrorAlert('', error));
  }

  delete(taskId) {
    const ans = confirm("Do you want to delete task: " + taskId);
    if (ans) {
      this.taskService.deleteTasksItem(taskId).subscribe((data) => {
        this.getTasks();
        this.showSuccessAlert('', 'task is deleted!');
      },
        error => this.showErrorAlert('', error));
    }
  }

  @ViewChild("editorModal")
  editorModal: ModalDirective;

  @ViewChild("assignTaskModal")
  assignTaskModal: ModalDirective;

  showErrorAlert(caption: string, message: string) {
    this.alertService.showMessage(caption, message, MessageSeverity.error);
  }

  showSuccessAlert(caption: string, message: string) {
    this.alertService.showMessage(caption, message, MessageSeverity.success);
  }

}
