import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { task } from 'src/app/models/model';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  // Properties
  @Input('tasksFromRoot') tasks?:task[];
  @Output() changeStatusEvent = new EventEmitter<task>();
  @Output() removeTaskEvent = new EventEmitter<task>();

  // Functions
  removeTask(task:task) {
    return this.removeTaskEvent.emit(task);
  }

  changeStatus(task:task) {
    return this.changeStatusEvent.emit(task);
  }
}
