import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class TaskService {
  appUrl: string = null;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.appUrl = baseUrl;
  }
  
  getTasks() {
    return this.http.get(this.appUrl + '/api/tasks')
      .map((response: Response) => response)
      .catch(this.errorHandler);
  }

  getTaskById(id: number) {
    return this.http.get(this.appUrl + "api/Tasks/Get/" + id)
      .map((response: Response) => response.json())
      .catch(this.errorHandler);
  }

  saveTask(employee) {
    return this.http.post(this.appUrl + 'api/Tasks/Create', employee)
      .map((response: Response) => response.json())
      .catch(this.errorHandler);
  }

  updateTaskItem(employee) {
    return this.http.put(this.appUrl + 'api/Tasks/Edit', employee)
      .map((response: Response) => response.json())
      .catch(this.errorHandler);
  }

  deleteTasksItem(id) {
    return this.http.delete(this.appUrl + "api/Tasks/Delete/" + id)
      .map((response: Response) => response)
      .catch(this.errorHandler);
  }

  errorHandler(error: Response) {
    console.log(error);
    return Observable.throw(error);
  }
}
