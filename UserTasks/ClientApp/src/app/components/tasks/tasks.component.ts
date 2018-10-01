
import { Component, Inject } from "@angular/core";
import { ConfigurationService } from "../../services/configuration.service";
import { TaskService } from "../../services/TaskService";
import { AuthService } from "../../services/auth.service";
import { error } from "util";
import { OnInit, OnDestroy, Input, TemplateRef, ViewChild } from "@angular/core";
import { ModalDirective } from "ngx-bootstrap/modal";
import { AlertService, MessageSeverity, DialogType } from "../../services/alert.service";
import { Router } from '@angular/router';


@Component({
    selector: 'tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css']
})
export class TasksComponent {
  public taskItems: ITaskData[];
  _currentUserId: number;
  user_permissions: any;

  constructor(public configurations: ConfigurationService, private router: Router, private alertService: AlertService, private taskService: TaskService, private authService: AuthService) {
    this.currentUserId();

    if (this._currentUserId !== 1) {
      this.router.navigateByUrl('/');
    }

    this.getTasks();
  }

  currentUserId() {
    if (this.authService.currentUser)
      this._currentUserId = Number(this.authService.currentUser.id);
    this.user_permissions = this.authService.userPermissions;
    return this._currentUserId;
  }

  getTasks() {
    this.taskService.getTasks().subscribe(
      data => this.taskItems = data as ITaskData[]
    );
  }
}

interface ITaskData {
  id: number;
  task: string;
  isDone: boolean;
  userOwnerId: number;
}
