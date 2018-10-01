import { Injectable, Inject } from "@angular/core";
import { Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";
import "rxjs/add/observable/throw";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class TaskService {
  appUrl: string = null;

  constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.appUrl = baseUrl;
  }

  getTasks() {
    return this.http.get(this.appUrl + "/api/tasks/Get")
      .map(response => response)
      .catch(this.errorHandler);
  }

  getTaskById(id: number) {
    return this.http.get(this.appUrl + "api/tasks/" + id)
      .map(response => response)
      .catch(this.errorHandler);
  }

  saveTask(taskItem) {
    return this.http.post(this.appUrl + "api/tasks/Post", taskItem)
      .map(response => response)
      .catch(this.errorHandler);
  }

  //updateTaskItem(taskItem) {
  //  return this.http.put(this.appUrl + "api/tasks/", taskItem)
  //    .map(response => response)
  //    .catch(this.errorHandler);
  //}

  assignTask(id, userId) {
    const prams = [id = id, userId = userId];
    return this.http.put(this.appUrl + "api/tasks/AssignTask", prams)
      .map(response => response)
      .catch(this.errorHandler);
  }

  taskDone(id) {
    return this.http.put(this.appUrl + "api/tasks/TaskDone", id)
      .map(response => response)
      .catch(this.errorHandler);
  }

  deleteTasksItem(id) {
    return this.http.delete(this.appUrl + "api/tasks/Delete/" + id)
      .map(response => response)
      .catch(this.errorHandler);
  }

  errorHandler(error: Response) {
    console.log(error);
    return Observable.throw(error);
  }
}
