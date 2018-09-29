
import { Component } from '@angular/core';
import { fadeInOut } from '../../services/animations';


@Component({
    selector: 'tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css'],
    animations: [fadeInOut]
})
export class TasksComponent {

}
