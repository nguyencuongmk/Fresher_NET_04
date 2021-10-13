import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { task } from 'src/app/models/model';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  // Properties
  @Input('tasksFromRoot') tasks?: task[];
  @Output() changeStatusEvent = new EventEmitter<task>();
  @Output() removeTaskEvent = new EventEmitter<task>();

  completedStatus = { background: 'rgba(166, 223, 182, 0.4)', cursor: 'pointer' };
  incompletedStatus = { background: 'rgba(166, 223, 255, 0.4)', cursor: 'pointer' };
  newTask: task = { id: 0, name: '', isCompleted: false };

  // Functions
  removeTask(task: task) {
    return this.removeTaskEvent.emit(task);
  }

  changeStatus(task: task) {
    return this.changeStatusEvent.emit(task);
  }
}
