import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { task } from '../models/model';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
  @Input('tasksFromRoot') tasks?:task[];

  @Output() addNewTaskEvent = new EventEmitter<task>();
  @Output() changeStatusEvent = new EventEmitter<task>();
  @Output() removeTaskEvent = new EventEmitter<task>();

  addNewTask(task: task) {
    return this.addNewTaskEvent.emit(task);
  }

  removeTask(task:task) {
    return this.removeTaskEvent.emit(task);
  }

  changeStatus(task:task) {
    return this.changeStatusEvent.emit(task);
  }
}
